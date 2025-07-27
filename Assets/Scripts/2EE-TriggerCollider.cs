using System;
using UnityEngine;
using System.Collections;


public class SecondTriggerCollider : MonoBehaviour
{
    public RoboticSimulator robotReference;
    public ChocoBoxSpawner boxSpawner;

    public bool collided = false;
    private bool hasResumed = false;

    private ChocoMold moldInZone;
    private ChocoBox spawnedBox;


    void Update()
    {
        if (robotReference != null && robotReference.doneAction && !hasResumed)
        {
            Debug.Log("[SecondTriggerCollider] Robot done. Resuming conveyors.");
            EndEffectorType effector = robotReference.GetComponentInChildren<EndEffectorType>();

            // check if effector is attached to robot
            if (effector == null)
            {
                Debug.LogWarning("[SecondTriggerCollider] No end-effector found on robot!");
                // TODO: Show warning UI to user here
                hasResumed = true;
                ConveyorParent.Instance.ResumeAll();
                return;
            }

            //Check when effector is Cooler
            if (effector.currentState == EndEffectorType.EffectorState.Cooler)
            {
                if(moldInZone != null && moldInZone.currentState == ChocoMold.ChocolateState.Liquid)
                {
                    moldInZone.SetToSolid();
                    Debug.Log("[SecondTriggerCollider] Robot finished. Chocolate turned solid");
                }
            }
            //check when effector is PickAndPlace
            else if (effector.currentState == EndEffectorType.EffectorState.PickAndPlace)
            {
                //check if chocolate is solid
                if (moldInZone != null && moldInZone.currentState == ChocoMold.ChocolateState.Solid)
                {
                    //check if there's an empty box to place solid chocolate in
                    if (spawnedBox != null && spawnedBox.currentState == ChocoBox.BoxState.Empty)
                    {
                        spawnedBox.FillBox();
                        Debug.Log("[SecondTriggerCollider] Chocolate bar placed in ChocoBox.");
                    }
                }
                else
                {
                    Debug.Log("[SecondTriggerCollider] Chocolate not solid. No bar spawned in ChocoBox.");
                }
            }
            else
            {
                Debug.Log("[SecondTriggerCollider] Not a PickAndPlace effector. ChocoBox stays empty.");
            }

            ConveyorParent.Instance.ResumeAll();
            hasResumed = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (robotReference != null && !collided)
        {
            collided = true;
            hasResumed = false;
            Debug.Log("[SecondTriggerCollider] Collision detected, setting Collided = true");

            ConveyorParent.Instance.PauseAll();
            moldInZone = other.GetComponent<ChocoMold>();

            //immidiately spawn an empty chocobox
            if (boxSpawner != null)
            {
                GameObject newBoxGO = boxSpawner.Spawn();
                spawnedBox = newBoxGO?.GetComponent<ChocoBox>();

                if (spawnedBox != null)
                    Debug.Log("[SecondTriggerCollider] New ChocoBox spawned and cached.");
                else
                    Debug.LogWarning("[SecondTriggerCollider] Spawned box is missing ChocoBox component.");
            }

            robotReference.OnArmLowered += HandleArmLowered;
            robotReference.StartRobotAction();
            //spawn chocolate in box before the robot action is done -> for accuracy of animation
            StartCoroutine(SpawnChocolateEarly(1.5f));

        }
    }
    private void OnTriggerExit(Collider other)
    {
        collided = false;
        moldInZone = null;
        spawnedBox = null;

        //stop listening to futhure OnArmLowered events
        if (robotReference != null)
        {
            robotReference.OnArmLowered -= HandleArmLowered;
        }

        Debug.Log("[SecondTriggerCollider] Trigger exited, collided = false");
    }

    // this helper function is to fake pickandplace of solid chocolate from mold to box
    private IEnumerator SpawnChocolateEarly(float early)
    {
        EndEffectorType effector = robotReference.GetComponentInChildren<EndEffectorType>();
        PickAndPlaceHandler pickHandler = robotReference.GetComponentInChildren<PickAndPlaceHandler>();

        yield return new WaitForSeconds(0.5f);
        // destroy solid chocolate in mold
        if (effector != null && effector.currentState == EndEffectorType.EffectorState.PickAndPlace)
        {
            if (moldInZone != null && moldInZone.currentState == ChocoMold.ChocolateState.Solid)
            {
                moldInZone.DestroySolidChocolate();
                Debug.Log("[SecondTriggerCollider] Chocolate removed from mold at start of PickAndPlace.");

                //spawn chocolate on end-effector
                if (pickHandler != null)
                {
                    pickHandler.SpawnCarriedChocolate();
                }
            }
        }


        yield return new WaitForSeconds(early);

        if (effector != null && effector.currentState == EndEffectorType.EffectorState.PickAndPlace)
        {
            if (moldInZone != null && moldInZone.currentState == ChocoMold.ChocolateState.Solid)
            {
                moldInZone.DestroySolidChocolate();

                if (spawnedBox != null && spawnedBox.currentState == ChocoBox.BoxState.Empty)
                {
                    // destroy solid chocolate from end-effector
                    if (pickHandler != null)
                    {
                        pickHandler.DestroyCarriedChocolate();
                    }

                    // spawn solid chocolate in box
                    spawnedBox.FillBox();
                    Debug.Log("[SecondTriggerCollider] Chocolate bar spawned early via coroutine delay.");
                }
            }
        }
    }
    //reset collided when reset button is pressed
    public void ResetTrigger()
    {
        collided = false;
        hasResumed = false;
        moldInZone = null;

        if (robotReference != null)
        {
            robotReference.OnArmLowered -= HandleArmLowered;
        }

        Debug.Log("[SecondTriggerCollider] Reset called. State cleared.");
    }


    //helper function to apply stain on end-effector & apply sticker on chocolate & chocobox
    private void HandleArmLowered()
    {
        EndEffectorType effector = robotReference.GetComponentInChildren<EndEffectorType>();

        if (moldInZone != null &&
            moldInZone.currentState == ChocoMold.ChocolateState.Liquid &&
            effector != null &&
            (effector.currentState == EndEffectorType.EffectorState.Stamp ||
             effector.currentState == EndEffectorType.EffectorState.PickAndPlace))
        {
            if (effector.currentStainState == EndEffectorType.StainState.NotStained) // prevent re-staining
            {
                effector.ApplyStain();
                Debug.Log("[SecondTriggerCollider] Stain applied during arm lowering.");
            }
        }

        //apply sticker
        if (effector.currentState == EndEffectorType.EffectorState.Stamp)
        {
            if (moldInZone != null && moldInZone.currentState == ChocoMold.ChocolateState.Solid)
            {
                Transform spawnPoint = moldInZone.transform.Find("StickerSpawnPoint");
                if (spawnPoint != null)
                {
                    effector.ApplySticker(moldInZone.gameObject, spawnPoint);
                    Debug.Log("[SecondTriggerCollider] Sticker applied to chocolate.");
                }
            }
        }
    }

}
