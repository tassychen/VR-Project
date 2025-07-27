using UnityEngine;

public class TriggerGrabStarter : MonoBehaviour
{
    public GameObject grabbableEndEffectorPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand")) // Replace with your actual tag or hand logic
        {
            Transform hand = other.transform;

            GameObject grabbed = Instantiate(
                grabbableEndEffectorPrefab,
                transform.position,
                transform.rotation
            );

            grabbed.transform.SetParent(hand); // So it moves with the hand/controller

            Debug.Log("[TriggerGrabStarter] Spawning GrabbableEndEffector on grab!");

            Destroy(gameObject); // Remove the fake placeholder
        }
    }
}
