using UnityEngine;

public class ChocoBoxFillerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ChocoBox box = other.GetComponent<ChocoBox>();

        if (box != null && box.currentState == ChocoBox.BoxState.Empty)
        {
            ChocoMold mold = ConveyorManager.Instance.LastProcessedMold;
            RoboticSimulator robot = ConveyorManager.Instance.LastUsedRobot;

            if (mold == null || robot == null) return;

            EndEffectorType effector = robot.GetComponentInChildren<EndEffectorType>();
            if (effector != null &&
                effector.currentState == EndEffectorType.EffectorState.PickAndPlace &&
                mold.currentState == ChocoMold.ChocolateState.Solid)
            {
                Debug.Log("[ChocoBoxFillerTrigger] Spawning chocolate into box.");
                box.FillBox();
                ConveyorParent.Instance.PauseAll();
            }
            else
            {
                Debug.Log("[ChocoBoxFillerTrigger] Conditions not met. No spawn.");
            }
        }
    }
}
