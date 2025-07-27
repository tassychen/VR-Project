using UnityEngine;

public class WindForceApplier : MonoBehaviour
{
    public float windForce = 15f;
    public Transform windDirectionPoint;

    public void Blow(ChocoBox box)
    {
        if (box != null && box.currentState == ChocoBox.BoxState.Empty)
        {
            Rigidbody rb = box.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = windDirectionPoint.forward;
                rb.AddForce(direction * windForce, ForceMode.Impulse);
                Debug.Log("[WindForceApplier] Wind force applied to empty box!");
            }
        }
    }
}

