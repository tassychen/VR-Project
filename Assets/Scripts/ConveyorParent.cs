using UnityEngine;
using System.Collections.Generic;
using PCS;

public class ConveyorParent : MonoBehaviour
{
    public static ConveyorParent Instance { get; private set; }
    public float globalSpeed = 0.6f;
    public bool isPaused = false;
    public bool forcePaused = false;
    private List<PCSConfig> allConveyors = new List<PCSConfig>();

    void Awake()
    {
        //to make sure there's only one conveyorparent
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterConveyor(PCSConfig config)
    {
        if (!allConveyors.Contains(config))
            allConveyors.Add(config);
        // Debug.Log("Number of registered conveyors is " + allConveyors.Count);
    }
    void Update()
    {
        if (forcePaused)
        {
            foreach (var conveyor in allConveyors)
            {
                if (conveyor.speed != 0f)
                    conveyor.SetSpeed(0f);
            }
            return; 
        }

        if (!isPaused)
        {
            foreach (var conveyor in allConveyors)
            {
                if (Mathf.Abs(conveyor.speed - globalSpeed) > 0.01f)
                    conveyor.SetSpeed(globalSpeed);                
            }
        }
        
    }

    public void PauseAll()
    {
        if (forcePaused)
        {
            Debug.LogWarning("[ConveyorParent] Cannot pause because forcePaused is active.");
            return;
        }

        if (isPaused) return;
        isPaused = true;
        
        Debug.Log("[ConveyorParent] Pausing all conveyors");

        foreach (var conveyor in allConveyors)
        {
            conveyor.SetSpeed(0f);
        }
        Debug.Log("[ConveyorParent - PAUSEALL] isPaused should be true and now it is " + isPaused);
        Debug.Log("[ConveyorParent - PAUSEALL] global speed is " + globalSpeed);
    }

    public void ResumeAll()
    {
        if (forcePaused)
        {
            Debug.LogWarning("[ConveyorParent] Cannot resume because forcePaused is active.");
            return;
        }

        if (!isPaused) return;
        isPaused = false;

        Debug.Log("[ConveyorParent] Resuming all conveyors");
        foreach (var conveyor in allConveyors)
        {
            //globalSpeed = 0.6f;
            conveyor.SetSpeed(globalSpeed);

            Debug.Log("[ConveyorParent - RESUMEALL] isPaused should be false and now it is " + isPaused);
            Debug.Log("[ConveyorParent - RESUMEALL] global speed is " + globalSpeed);



        }
    }
    public void ForcePauseAll()
    {
        forcePaused = true;
        isPaused = true;
        Debug.Log("[ConveyorParent] Force pausing all conveyors.");
        foreach (var conveyor in allConveyors)
        {
            conveyor.SetSpeed(0f);
        }
    }

    public void ForceResumeAll()
    {
        forcePaused = false;
        isPaused = false;
        Debug.Log("[ConveyorParent] Force resuming all conveyors.");
        foreach (var conveyor in allConveyors)
        {
            conveyor.SetSpeed(globalSpeed);
        }
        Debug.Log("[ConveyorParent - FORCERESUMEALL] global speed is " + globalSpeed);

    }

}

