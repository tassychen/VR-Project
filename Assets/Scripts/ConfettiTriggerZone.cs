using UnityEngine;

public class ConfettiTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ChocoBox box = other.GetComponent<ChocoBox>();
        if (box == null) return;

        if (!ChocoBox.isConfettied &&
            box.currentState == ChocoBox.BoxState.Filled &&
            box.currentFoldingState == ChocoBox.BoxFoldingState.Folded)
        {
            ChocoBox.isConfettied = true;
            ConfettiManager.Instance.PlayConfetti();
            Debug.Log("[ConfettiTriggerZone] Confetti triggered! Perfect box entered final drop zone.");
        }
    }
}
