using UnityEngine;

public class ProcessingLineManager : MonoBehaviour
{
    public enum LineState { Idle, Running, EmergencyStopped, Resetting }

    public static ProcessingLineManager Instance { get; private set; }

    public LineState currentState = LineState.Idle;

    [Header("Tub Settings")]
    public ChocolateTub currentTub; // The intial active tub

    [Header("Initial Spawn Setup")]
    public GameObject coolerPrefab;
    public GameObject pickAndPlacePrefab;
    public GameObject stampPrefab;

    public Transform leftSnapZone;
    public Transform centerSnapZone;
    public Transform rightSnapZone;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log("[ProcessingLine] System auto-starting in Running state.");
        ConveyorParent.Instance.ResumeAll();
        Spawner.Instance.StartSpawning();
        currentState = LineState.Running;

        if (!AllRoboticArmsHaveEffectors())
        {
            Debug.LogWarning("[ProcessingLine] Not all arms have effectors.");
            return;
        }
    }


    public void OnRunPressed()
    {
        if (currentState != LineState.Resetting)
        {
            Debug.LogWarning("[ProcessingLine] Run can only start after reset.");
            return;
        }
        if (currentState == LineState.Running)
        {
            Debug.Log("[ProcessingLine] Already running.");
            return;
        }

        if (!AllRoboticArmsHaveEffectors())
        {
            Debug.LogWarning("[ProcessingLine] Not all arms have effectors.");
            return;
        }
        Debug.Log("[Run Button] Pressed!");
        //ConveyorParent.Instance.ResumeAll();
        ConveyorParent.Instance.ForceResumeAll();

        Spawner.Instance.StartSpawning();

        currentState = LineState.Running;
        Debug.Log("[ProcessingLine] Line started.");
    }

    public void OnEmergencyStopPressed()
    {

        if (currentState == LineState.Running)
        {
            //ConveyorParent.Instance.PauseAll();
            ConveyorParent.Instance.ForcePauseAll();
            Spawner.Instance.StopSpawning();

            currentState = LineState.EmergencyStopped;
            Debug.Log("[ProcessingLine] Emergency stop activated.");

        }
        else
        {
            Debug.LogWarning("[ProcessingLine] Emergency stop only works when running.");
            return;  
        }

        
    }

    public void OnResetPressed()
    {

        if (currentState == LineState.Running)
        {
            Debug.Log("[ProcessingLine] Already running.");
            return;
        }

        if (currentState == LineState.EmergencyStopped)
        {
            ChocoBox.DestroyAllBoxes();
            ChocoMold.DestroyAllMolds();

            // destroy end-effectors from robotic arms
            ResetAllRoboticArms();

            // reset beeping sound for this new round

            RoverFollower rover = FindObjectOfType<RoverFollower>();
            if (rover != null)
            {
                rover.ResetBeepState();
            }

            // Spawn new end-effectors on rover
            SpawnInitialEndEffectors();

            // Play beeping sound once
            if (rover != null)
            {
                rover.PlayBeepOnce();
            }


            //reset triggercolliders

            foreach (var trigger in FindObjectsOfType<TriggerCollider>())
            {
                trigger.ResetTrigger();
                Debug.Log("[ProcessingLine] Resetted triggercollider from EE1 and EE3");

            }

            foreach (var trigger in FindObjectsOfType<SecondTriggerCollider>())
            {
                trigger.ResetTrigger();
                Debug.Log("[ProcessingLine] Resetted triggercollider from EE2");

            }

            foreach (var trigger in FindObjectsOfType<DispenserTrigger>())
            {
                trigger.ResetTrigger();
                Debug.Log("[ProcessingLine] Resetted triggercollider from dispenser");

            }


            //reset snapzones isoccupied state
            ClearAllSnapZones();

            //reset glowing effect on ee snapzones
            foreach (var snapZone in FindObjectsOfType<SnapZoneTrigger>())
            {
                snapZone.ClearZone();
                Debug.Log("[ProcessingLine] Reset snap zone and reactivated guide visual");
            }

            // reset chocolate tub
            ResetChocolateTub();

            //reset conffeti
            ChocoBox.isConfettied = false;
            Debug.Log("[ProcessingLine] Confetti state reset.");
            Debug.Log("[ProcessingLine] Confetti state should be false and now is " + ChocoBox.isConfettied);



            currentState = LineState.Resetting;

            Debug.Log("[ProcessingLine] Reset complete. Ready to run.");
 
        }
        else
        {
            Debug.LogWarning("[ProcessingLine] Reset only allowed after emergency stop.");
            return;
        }
       

       
    }


    //HELPER FUNCTIONS

    //check if all robotic arms have end-effector attached
    public bool AllRoboticArmsHaveEffectors()
    {
        RoboticSimulator[] allArms = FindObjectsOfType<RoboticSimulator>();

        foreach (var arm in allArms)
        {
            if (arm.effectorType == null || arm.effectorType.currentState == EndEffectorType.EffectorState.Empty)
            {
                Debug.LogWarning($"[ProcessingLineManager] Missing or empty end-effector on: {arm.name}");
                return false;
            }
        }

        return true;
    }

    //destroy all end-effector attached to the robotic arms
    public void ResetAllRoboticArms()
    {
        RoboticSimulator[] allArms = FindObjectsOfType<RoboticSimulator>();

        foreach (var arm in allArms)
        {
            if (arm.effectorType != null)
            {
                Destroy(arm.effectorType.gameObject);
                arm.SetEffectorReference(null);
                Debug.Log($"[ProcessingLineManager] Destroyed end-effector on {arm.name}");
            }
        }
    }

    // spawn new end-effectors on rover
    public void SpawnInitialEndEffectors()
    {
        if (coolerPrefab != null && leftSnapZone != null)
            Instantiate(coolerPrefab, leftSnapZone.position, leftSnapZone.rotation, leftSnapZone);

        if (pickAndPlacePrefab != null && centerSnapZone != null)
            Instantiate(pickAndPlacePrefab, centerSnapZone.position, centerSnapZone.rotation, centerSnapZone);

        if (stampPrefab != null && rightSnapZone != null)
            Instantiate(stampPrefab, rightSnapZone.position, rightSnapZone.rotation, rightSnapZone);

        Debug.Log("[ProcessingLineManager] Initial end-effectors spawned on rover.");
    }

    //reset a new tub
    public void ResetChocolateTub()
    {
        if (currentTub == null)
        {
            Debug.LogWarning("[ProcessingLine] No ChocolateTub reference assigned.");
            return;
        }

        currentTub.ForceDropNow();
        Debug.Log("[ProcessingLineManager] trying to reset tub.");

    }

    private void ClearAllSnapZones()
    {
        SnapZoneTrigger[] zones = FindObjectsOfType<SnapZoneTrigger>();
        foreach (var zone in zones)
        {
            zone.ClearZone();
        }
        Debug.Log("[ProcessingLineManager] All snap zones cleared.");
    }




}
