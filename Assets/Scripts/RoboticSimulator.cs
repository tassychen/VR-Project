using System.Collections;
using UnityEditor;
using UnityEngine;
using System;
using static EndEffectorType;

public class RoboticSimulator : MonoBehaviour
{
    [Header("Only Accept One TriggerCollider Reference")]
    public TriggerCollider triggerReference;
    public SecondTriggerCollider secondTriggerReference;

    private bool isRunning = false;
    public bool doneAction = true;

    public EndEffectorType effectorType;
    private Animator animator;
    public event Action OnArmLowered;


    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("[RoboticSimulator] No Animator found on this GameObject!");
    }

    public virtual void StartRobotAction()
    {
        bool hasValidTrigger = (triggerReference != null || secondTriggerReference != null);
        if (hasValidTrigger && !isRunning && doneAction)
        {
            StartCoroutine(RobotActionSimulation());
        }
    }
    IEnumerator RobotActionSimulation()
    {
        isRunning = true;
        doneAction = false;
        if (effectorType == null || effectorType.currentState == EffectorState.Empty)
        {
            Debug.LogWarning("[RoboticSimulator] No end-effector assigned!");
            doneAction = true;
            isRunning = false;
            yield break;
        }
        Debug.Log("[RoboticSimulator] Started robotic action");


        switch (effectorType.currentState)
        {
            case EndEffectorType.EffectorState.Cooler:
                animator.SetTrigger("CoolerTrigger");
                Debug.Log("[RoboticSimulator] Cooler being triggered");
                break;

            case EndEffectorType.EffectorState.PickAndPlace:
                animator.SetTrigger("PickTrigger");
                Debug.Log("[RoboticSimulator] PickAndPlace being triggered");
                break;

            case EndEffectorType.EffectorState.Stamp:
                animator.SetTrigger("StampTrigger");
                Debug.Log("[RoboticSimulator] Stamp being triggered");
                break;

        }
        yield return null; // wait one frame so the animator gets to animation state
        
        while (animator.IsInTransition(0))
            yield return null;

        float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log($"[RoboticSimulator] Playing state: {animator.GetCurrentAnimatorStateInfo(0).fullPathHash}, duration: {animLength}s");
        yield return new WaitForSeconds(animLength);

        doneAction = true;
        Debug.Log("[RoboticSimulator] Trying to set doneAction to true!");

        isRunning = false;
    }
    public void FanBlowNow()
    {
        if (effectorType == null)
        {
            Debug.LogWarning("[RoboticSimulator] FanBlowNow called but effectorType is null!");
            return;
        }
        if (effectorType.currentState == EndEffectorType.EffectorState.Cooler)
        {
            // Trigger wind logic
            WindForceApplier wind = GetComponentInChildren<WindForceApplier>();
            if (wind != null && triggerReference != null)
            {
                ChocoBox box = triggerReference.GetCurrentBox();
                if (box != null && box.currentState == ChocoBox.BoxState.Empty)
                {
                    wind.Blow(box);
                    Debug.Log("[RoboticSimulator] Wind applied to empty box.");
                }
                else
                {
                    Debug.Log("[RoboticSimulator] Box is not empty. Wind not applied.");
                }
            }

            // Trigger fan blade spin inside the current end-effector
            FanBladeRotator blade = effectorType.GetComponentInChildren<FanBladeRotator>();
            if (blade != null)
            {
                blade.StartSpin();
                StartCoroutine(StopFanAfterDelay(blade, 1.3f));
                Debug.Log("[RoboticSimulator] Fan blade started spinning.");
            }
            else
            {
                Debug.LogWarning("[RoboticSimulator] No FanBladeRotator found in end-effector.");
            }
        }
    }
    private IEnumerator StopFanAfterDelay(FanBladeRotator blades, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (blades != null)
        {
            blades.StopSpin();
            Debug.Log("[RoboticSimulator] Fan blade stopped.");
        }
    }


    //public void FanBlowNow()
    //{
    //    if (effectorType.currentState == EndEffectorType.EffectorState.Cooler)
    //    {
    //        WindForceApplier wind = GetComponentInChildren<WindForceApplier>();
    //        if (wind != null && triggerReference != null)
    //        {
    //            ChocoBox box = triggerReference.GetCurrentBox();
    //            if (box != null && box.currentState == ChocoBox.BoxState.Empty)
    //            {
    //                wind.Blow(box);
    //                Debug.Log("[FanBlowNow] Wind applied to empty box.");
    //            }
    //            else
    //            {
    //                Debug.Log("[FanBlowNow] Box is not empty. Wind not applied.");
    //            }
    //        }
    //    }
    //}
    //helper function to notify when apply chocolate stain on end-effector
    public void ArmLoweredEvent()
    {
        Debug.Log("[RoboticSimulator] Arm lowering complete.");
        OnArmLowered?.Invoke();
    }

    // Called by SnapZoneTrigger when a new effector is spawned
    // to get reference of end-effector
    public void SetEffectorReference(EndEffectorType newEffector)
    {
        if (newEffector == null)
        {
            Debug.LogWarning("[RoboticSimulator] Received null EndEffectorType reference.");
            return;
        }

        effectorType = newEffector;
        Debug.Log("[RoboticSimulator] Effector reference updated: " + newEffector.currentState);
    }
}

