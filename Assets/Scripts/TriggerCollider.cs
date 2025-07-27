using System;
using UnityEngine;
using System.Collections;


public class TriggerCollider : MonoBehaviour
{
    public RoboticSimulator robotReference;

    public bool collided = false;

    private bool hasResumed = false;

    private ChocoMold moldInZone;

    private ChocoBox boxInZone;

    void Update()
    {
        if ( robotReference != null && robotReference.doneAction && !hasResumed)
        {
            EndEffectorType effector = robotReference.GetComponentInChildren<EndEffectorType>();

            if (effector == null)
            {
                Debug.LogWarning("[TriggerCollider] No end-effector attached to robot.");
                ConveyorParent.Instance.ResumeAll();
                // TODO: Show warning UI to user here
                hasResumed = true;
                return;
            }

            //Check when effector is Cooler
            if (effector.currentState == EndEffectorType.EffectorState.Cooler)
            {
                if(moldInZone != null && moldInZone.currentState == ChocoMold.ChocolateState.Liquid)
                {
                    moldInZone.SetToSolid();
                    Debug.Log("[TriggerCollider] Robot finished. Chocolate turned solid");
                }
            }

            ConveyorParent.Instance.ResumeAll();
            Debug.Log("[TriggerCollider] Robot done. Resumed conveyors.");
            hasResumed = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (robotReference != null && !collided)
        {
            collided = true;
            hasResumed = false;
            Debug.Log("[TriggerCollider] Collision detected, setting Collided = true");

            ConveyorParent.Instance.PauseAll();
            moldInZone = other.GetComponent<ChocoMold>();
            boxInZone = other.GetComponent<ChocoBox>();

            robotReference.OnArmLowered += HandleArmLowered;
            robotReference.StartRobotAction();

            //special case: only for conveyor 3 pickandplace chocobox effect
            PickAndPlaceHandler handler = robotReference.GetComponentInChildren<PickAndPlaceHandler>();
            if (boxInZone != null && handler != null)
            {
                StartCoroutine(DropBoxAfterDelay(1.5f, handler, boxInZone));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        collided = false;
        moldInZone = null;
        boxInZone = null;

        //stop listening to futhure OnArmLowered events
        if (robotReference != null)
        {
            robotReference.OnArmLowered -= HandleArmLowered;
        }

        Debug.Log("[TriggerCollider] Trigger exited, collided = false");
    }

    public ChocoBox GetCurrentBox()
    {
        return boxInZone;
    }

    private IEnumerator DropBoxAfterDelay(float delay, PickAndPlaceHandler handler, ChocoBox box)
    {
        EndEffectorType effector = robotReference.GetComponentInChildren<EndEffectorType>();

        if (effector == null || effector.currentState != EndEffectorType.EffectorState.PickAndPlace)
        {
            yield break;
        }

        if (box.currentFoldingState != ChocoBox.BoxFoldingState.Folded)
        {
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        // Fake pick: destroy from conveyor
        box.ForceDestroyBox();
        Debug.Log("[TriggerCollider] Chocolate box destroyed (picked)");

        handler.SpawnCarriedBox();
        Debug.Log("[TriggerCollider] Chocolate box being carried");

        yield return new WaitForSeconds(delay);

        // Drop from robot arm
        handler.DestroyCarriedBox();

        // Drop new box at target
        if (handler.boxDropPoint != null)
        {
            GameObject droppedBox = Instantiate(handler.carriedBoxPrefab, handler.boxDropPoint.position, handler.boxDropPoint.rotation);

            ChocoBox boxScript = droppedBox.GetComponent<ChocoBox>();
            if (boxScript != null)
            {
                boxScript.currentFoldingState = ChocoBox.BoxFoldingState.Folded;

                // should already be in closed state
                Animator anim = boxScript.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.Play("BoxClose", 0, 1f);
                }

                Debug.Log("[PickAndPlaceHandler] Carried box spawned already closed.");
            }

            Rigidbody rb = droppedBox.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            Debug.Log("[TriggerCollider] Chocolate box dropped at drop point");
        }
        else
        {
            Debug.LogWarning("[TriggerCollider] boxDropPoint is not assigned!");
        }
    }
    //reset collided to false when reset button is pressed
    public void ResetTrigger()
    {
        collided = false;
        hasResumed = false;
        moldInZone = null;
        boxInZone = null;

        if (robotReference != null)
        {
            robotReference.OnArmLowered -= HandleArmLowered;
        }

        Debug.Log("[TriggerCollider] Reset called. State cleared.");
    }


    //helper function to apply stain on end-effector & apply sticker on chocolate & chocobox
    private void HandleArmLowered()
    {
        EndEffectorType effector = robotReference.GetComponentInChildren<EndEffectorType>();
        //apply stain
        if (moldInZone != null &&
            moldInZone.currentState == ChocoMold.ChocolateState.Liquid &&
            effector != null &&
            (effector.currentState == EndEffectorType.EffectorState.Stamp ||
             effector.currentState == EndEffectorType.EffectorState.PickAndPlace))
        {
            if (effector.currentStainState == EndEffectorType.StainState.NotStained) // prevent re-staining
            {
                effector.ApplyStain();
                Debug.Log("[TriggerCollider] Stain applied during arm lowering.");
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
                    Debug.Log("[TriggerCollider] Sticker applied to closed box.");
                }
            }

            if (boxInZone != null && boxInZone.currentFoldingState == ChocoBox.BoxFoldingState.Folded)
            {
                Transform spawnPoint = boxInZone.transform.Find("StickerSpawnPoint");
                if (spawnPoint != null)
                {
                    effector.ApplySticker(boxInZone.gameObject, spawnPoint);
                    Debug.Log("[TriggerCollider] Sticker applied to solid chocolate.");
                }
            }
        }
    }

}
