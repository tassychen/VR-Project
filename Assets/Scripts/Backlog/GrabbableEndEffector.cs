//using UnityEngine;
//using Oculus.Interaction;

//[RequireComponent(typeof(Grabbable))]
//public class GrabbableEndEffector : MonoBehaviour, 
//{
//    [Header("Snap Settings")]
//    public float snapRadius = 0.1f;

//    private SnapZone previousSnapZone;
//    private Vector3 originalPosition;
//    private Quaternion originalRotation;
//    private Transform originalParent;
//    private Grabbable grabbable;

//    private void Awake()
//    {
//        grabbable = GetComponent<Grabbable>();
//        grabbable.InjectOptionalRigidbody(GetComponent<Rigidbody>());

//        // Register this object to receive grab state events
//        grabbable.InjectOptionalKinematicWhileSelected(true);
//        grabbable.WhenPointerEventRaised += OnPointerEvent;
//    }

//    private void Start()
//    {
//        // Store original transform for reset
//        originalParent = transform.parent;
//        originalPosition = transform.position;
//        originalRotation = transform.rotation;

//        // If already placed in a snap zone at start
//        SnapZone initialZone = GetComponentInParent<SnapZone>();
//        if (initialZone != null)
//        {
//            previousSnapZone = initialZone;
//            initialZone.AssignOccupant(this);
//            Debug.Log("[GrabbableEndEffector] Initial end-effector detected in snap zone.");
//        }
//    }

//    // Required by IGrabStateResponder but unused
//    public void UpdateGrab(Pose worldGrabPose, Pose localGrabPose) { }

//    public void BeginGrab() => OnGrabStart();

//    public void EndGrab() => OnGrabEnd();

//    private void OnGrabStart()
//    {
//        Debug.Log("[GrabbableEndEffector] Grab started");

//        if (previousSnapZone != null)
//        {
//            previousSnapZone.ClearOccupant();
//            previousSnapZone = null;
//        }
//    }

//    private void OnGrabEnd()
//    {
//        Debug.Log("[GrabbableEndEffector] Grab ended");

//        SnapZone targetZone = SnapZoneHelper.GetAvailableSnapZone(transform.position, snapRadius);

//        if (targetZone != null)
//        {
//            SnapToZone(targetZone);
//        }
//        else
//        {
//            ReturnToPrevious();
//        }
//    }

//    private void SnapToZone(SnapZone zone)
//    {
//        transform.SetParent(zone.transform);
//        transform.localPosition = Vector3.zero;
//        transform.localRotation = Quaternion.identity;
//        transform.localScale = Vector3.one;

//        zone.AssignOccupant(this);
//        previousSnapZone = zone;

//        Debug.Log("[GrabbableEndEffector] Snapped to zone: " + zone.name);
//    }

//    private void ReturnToPrevious()
//    {
//        if (previousSnapZone != null)
//        {
//            transform.SetParent(previousSnapZone.transform);
//            transform.localPosition = Vector3.zero;
//            transform.localRotation = Quaternion.identity;
//            transform.localScale = Vector3.one;

//            previousSnapZone.AssignOccupant(this);

//            Debug.Log("[GrabbableEndEffector] Returned to previous snap zone.");
//        }
//        else
//        {
//            // Fall back to original
//            transform.SetParent(originalParent);
//            transform.position = originalPosition;
//            transform.rotation = originalRotation;
//            transform.localScale = Vector3.one;

//            Debug.LogWarning("[GrabbableEndEffector] No valid snap zone. Returned to original position.");
//        }
//    }

//    // Optional helper to listen to low-level events (if needed)
//    private void OnPointerEvent(PointerEvent evt)
//    {
//        // Could use this for debugging or tracking grab state transitions
//    }
//}
////using UnityEngine;

////public class GrabbableEndEffector : MonoBehaviour
////{
////    [Header("Snap Settings")]
////    public float snapRadius = 0.1f;

////    private SnapZone previousSnapZone;
////    private Transform originalParent;
////    private Vector3 originalPosition;
////    private Quaternion originalRotation;

////    void Start()
////    {
////        // Cache original position in case of reset
////        originalParent = transform.parent;
////        originalPosition = transform.position;
////        originalRotation = transform.rotation;

////        // Initialize previousSnapZone if already placed in one
////        SnapZone zone = GetComponentInParent<SnapZone>();
////        if (zone != null)
////        {
////            previousSnapZone = zone;
////            zone.AssignOccupant(this);
////            Debug.Log("[GrabbableEndEffector] Initial end-effector detected in snap zone.");
////        }
////    }

////    public void OnGrabStart()
////    {
////        if (previousSnapZone != null)
////        {
////            previousSnapZone.ClearOccupant();
////            previousSnapZone = null;
////            Debug.Log("[GrabbableEndEffector] end-effector is grabbed");
////        }
////    }

////    public void OnGrabEnd()
////    {
////        // Try snapping to nearest available zone
////        SnapZone zone = SnapZoneHelper.GetAvailableSnapZone(transform.position, snapRadius);

////        if (zone != null && !zone.IsOccupied)
////        {
////            SnapToZone(zone);
////            Debug.Log("[GrabbableEndEffector] end-effector snapped to new snapzone");

////        }
////        else
////        {
////            ReturnToPrevious();
////            Debug.Log("[GrabbableEndEffector] end-effector return to previous snapzone");

////        }
////    }

////    private void SnapToZone(SnapZone zone)
////    {
////        transform.position = zone.transform.position;
////        transform.rotation = zone.transform.rotation;
////        transform.SetParent(zone.transform); // Optional for organization

////        zone.AssignOccupant(this);
////        previousSnapZone = zone;

////        Debug.Log("[GrabbableEndEffector] Snapped to zone: " + zone.name);
////    }

////    private void ReturnToPrevious()
////    {
////        if (previousSnapZone != null)
////        {
////            transform.position = previousSnapZone.transform.position;
////            transform.rotation = previousSnapZone.transform.rotation;
////            transform.SetParent(previousSnapZone.transform);
////            previousSnapZone.AssignOccupant(this);

////            Debug.Log("[GrabbableEndEffector] Returned to previous zone.");
////        }
////        else
////        {
////            transform.position = originalPosition;
////            transform.rotation = originalRotation;
////            transform.SetParent(originalParent);
////            Debug.LogWarning("[GrabbableEndEffector] No previous snap zone. Reverting to original location.");
////        }
////    }
////}

