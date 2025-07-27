using UnityEngine;
using System.Collections;

public class GrabbableHandler : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject realEndEffectorPrefab;

    [Header("Snap Zone Detection")]
    public float snapRadius = 0.1f;
    private bool isReleased = false;
    private bool allowSnapCheck = false;

    [Tooltip("Cached snap zone to return to if needed")]
    public Transform previousSnapZone; // will auto-populate at runtime if not assigned

    private void Start()
    {
        StartCoroutine(DelayedSnapActivation());

        // Try to auto-find current snap zone on startup
        if (previousSnapZone == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, snapRadius);
            foreach (var hit in hits)
            {
                SnapZone zone = hit.GetComponent<SnapZone>();
                if (zone != null && !zone.isOccupied)
                {
                    previousSnapZone = zone.transform;
                    zone.isOccupied = true;
                    Debug.Log("[GrabbableHandler] Initial end-effector detected in snap zone: " + zone.name);
                    break;
                }
            }
        }
    }

    private IEnumerator DelayedSnapActivation()
    {
        yield return new WaitForSeconds(0.2f); // let everything initialize
        allowSnapCheck = true;
    }

    private void Update()
    {
        if (!allowSnapCheck) return;

        if (!IsGrabbed() && !isReleased)
        {
            isReleased = true;
            TrySnapOrReturn();
        }
    }

    private bool IsGrabbed()
    {
        return transform.parent != null &&
               (transform.parent.name.Contains("Hand") ||
                transform.parent.GetComponentInParent<Camera>() != null);
    }

    private void TrySnapOrReturn()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, snapRadius);
        SnapZone bestZone = null;

        foreach (var hit in hits)
        {
            SnapZone zone = hit.GetComponent<SnapZone>();
            if (zone != null && !zone.isOccupied)
            {
                bestZone = zone;
                break;
            }
        }

        if (bestZone != null)
        {
            SpawnRealEndEffector(bestZone.transform, bestZone);
        }
        else
        {
            Debug.LogWarning("[GrabbableHandler] No valid snap zone. Returning to previous.");
            SpawnRealEndEffector(previousSnapZone, null);
        }

        Destroy(gameObject); // Destroy the fake grabbable
    }

    private void SpawnRealEndEffector(Transform target, SnapZone zone)
    {
        Transform parent = (zone != null && zone.snapParent != null) ? zone.snapParent : target;

        GameObject spawned = Instantiate(realEndEffectorPrefab, parent.position, parent.rotation, parent);

        Debug.LogWarning("[GrabbableHandler] just spawned an end-effector.");
        if (zone != null)
        {
            zone.isOccupied = true;
        }

        RealEndEffector effector = spawned.GetComponent<RealEndEffector>();
        if (effector != null)
        {
            effector.Initialize(zone);
        }

        Debug.Log("[GrabbableHandler] Spawned real end-effector at: " + parent.name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, snapRadius);
    }
}
//using UnityEngine;

//public class GrabbableHandler : MonoBehaviour
//{
//    [Header("Spawning")]
//    public GameObject realEndEffectorPrefab;

//    [Tooltip("Fallback position if snap zone not found")]
//    public Transform previousSnapZone;

//    [Header("Snap Zone Detection")]
//    public float snapRadius = 0.1f;

//    private bool isReleased = false;
//    //checker to switch when user just let go of the end-effector
//    //but release behavior has not done

//    private void Start()
//    {
//        // Detect initial SnapZone and cache it
//        Collider[] hits = Physics.OverlapSphere(transform.position, 0.05f); 
//        foreach (var hit in hits)
//        {
//            SnapZone zone = hit.GetComponent<SnapZone>();
//            if (zone != null)
//            {
//                previousSnapZone = zone.transform;
//                zone.isOccupied = true;
//                Debug.Log("[GrabbableHandler] Initial end-effector detected in snap zone: " + zone.name);
//                break;
//            }
//        }
//    }
//    private void Update()
//    {
//        // Listen for release — could be customized based on trigger or grab state
//        if (!IsGrabbed() && !isReleased)
//        {
//            isReleased = true;
//            TrySnapOrReturn();
//        }
//    }

//    // This checks if it's still held by a grabber (you can replace this with your own logic)
//    private bool IsGrabbed()
//    {
//        return transform.parent != null &&
//               (transform.parent.name.Contains("Hand") || transform.parent.GetComponentInParent<Camera>() != null);
//    }

//    private void TrySnapOrReturn()
//    {
//        Collider[] hits = Physics.OverlapSphere(transform.position, snapRadius);
//        SnapZone bestZone = null;

//        foreach (var hit in hits)
//        {
//            SnapZone zone = hit.GetComponent<SnapZone>();
//            if (zone != null && !zone.isOccupied)
//            {
//                bestZone = zone;
//                break;
//            }
//        }

//        if (bestZone != null)
//        {
//            SpawnRealEndEffector(bestZone.transform, bestZone);
//        }
//        else
//        {
//            Debug.LogWarning("[GrabbableHandler] No valid snap zone. Returning to previous.");
//            SpawnRealEndEffector(previousSnapZone, null);
//        }

//        Destroy(gameObject); // remove the released end-effector
//    }

//    private void SpawnRealEndEffector(Transform target, SnapZone zone)
//    {
//        Transform parent = (zone != null && zone.snapParent != null) ? zone.snapParent : target;

//        GameObject spawned = Instantiate(realEndEffectorPrefab, parent.position, parent.rotation, parent);

//        Debug.LogWarning("[GrabbableHandler] just spawned an end-effector.");
//        if (zone != null)
//        {
//            zone.isOccupied = true;
//        }

//        RealEndEffector effector = spawned.GetComponent<RealEndEffector>();
//        if (effector != null)
//        {
//            effector.Initialize(zone);
//        }

//        Debug.Log("[GrabbableHandler] Spawned real end-effector at: " + parent.name);
//    }


//    // Optional: Draw detection area in scene
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.cyan;
//        Gizmos.DrawWireSphere(transform.position, snapRadius);
//    }
//}
