using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    //this script keeps track of the current state of chocolate in the mold below second robotic arm
    //and end-effector type for second robotic arm
    public static ConveyorManager Instance;

    public ChocoMold LastProcessedMold;
    public RoboticSimulator LastUsedRobot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
