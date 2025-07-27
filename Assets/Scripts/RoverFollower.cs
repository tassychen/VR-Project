using UnityEngine;

public class RoverFollower : MonoBehaviour
{
    public Transform userHead; 
    public float followSpeed = 3f;
    public float followDistance = 1.5f;
    public float heightOffset = 0.07f;

    [Header("Avoidance")]
    public LayerMask obstacleLayers;     // static obstacles rover needs to avoid
    public float avoidRadius = 0.5f;       // How far to check for collisions

    [Header("Beep Audio")]
    public AudioSource beepAudioSource;
    private bool hasBeepedThisRound = false;

    void Update()
    {
        if (userHead == null) return;

        Vector3 targetPosition = userHead.position - userHead.forward * followDistance;
        targetPosition.y += heightOffset;

        // Prevent intersecting with obstacle
        if (!Physics.CheckSphere(targetPosition, avoidRadius, obstacleLayers))
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
        //// Optional: rotate to always face user
        //Vector3 lookDirection = userHead.position - transform.position;
        //lookDirection.y = 0;
        //if (lookDirection.magnitude > 0.1f)
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * followSpeed);
    }
    public void PlayBeepOnce()
    {
        if (hasBeepedThisRound) return;

        if (beepAudioSource != null)
        {
            beepAudioSource.Play();
            Debug.Log("[RoverFollower] Beep sound played.");
        }

        hasBeepedThisRound = true;
    }

    public void ResetBeepState()
    {
        hasBeepedThisRound = false;
        Debug.Log("[RoverFollower] Beep state reset.");
    }

    // Debugging purpose
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (userHead != null)
        {
            Vector3 targetPosition = userHead.position - userHead.forward * followDistance;
            Gizmos.DrawWireSphere(targetPosition, avoidRadius);
        }
    }
}
