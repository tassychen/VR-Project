//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.Interactables;

//[RequireComponent(typeof(XRGrabInteractable))]
//public class DraggableEndEffector : MonoBehaviour
//{
//    [Header("Snap Settings")]
//    public float snapRadius = 0.1f;
//    private SnapZone previousSnapZone;

//    private XRGrabInteractable grabInteractable;
//    void Start()
//    {
//        // iniitial checking: Auto-detect SnapZone if end-effector is placed as a child of one
//        SnapZone initialZone = GetComponentInParent<SnapZone>();

//        if (initialZone != null)
//        {
//            previousSnapZone = initialZone;
//            initialZone.AssignOccupant(this);
//            Debug.Log("[DraggableEndEffector] Initial end-effector detected in the snapzone.");
//        }
//    }

//    void Awake()
//    {
//        grabInteractable = GetComponent<XRGrabInteractable>();

//        // Subscribe to grab/drop events
//        grabInteractable.selectEntered.AddListener(OnGrabbed);
//        grabInteractable.selectExited.AddListener(OnReleased);
//    }

//    private void OnGrabbed(SelectEnterEventArgs args)
//    {

//        // Free the snap zone this came from
//        if (previousSnapZone != null && previousSnapZone.currentOccupant == this)
//        {
//            previousSnapZone.currentOccupant = null;
//            Debug.Log("[DraggableEndEffector] end-effector got grabbed.");
//        }
//    }

//    private void OnReleased(SelectExitEventArgs args)
//    {
//        SnapZone targetZone = SnapZoneCollideDetection.GetAvailableSnapZone(transform.position, snapRadius);

//        if (targetZone != null)
//        {
//            SnapToZone(targetZone);
//            Debug.Log("[DraggableEndEffector] end-effector got dropped to new snapzone.");

//        }
//        else
//        {
//            ReturnToPrevious();
//            Debug.Log("[DraggableEndEffector] end-effector got back to old snapzone.");

//        }
//    }

//    // snap end-effector to a snap zone
//    private void SnapToZone(SnapZone zone)
//    {
//        transform.position = zone.transform.position;
//        transform.rotation = zone.transform.rotation;
//        transform.SetParent(zone.transform);  // Optional: for hierarchy clarity

//        zone.AssignOccupant(this);
//        previousSnapZone = zone;

//        Debug.Log("[DraggableEndEffector] Snapped to zone: " + zone.name);
//    }
//    private void ReturnToPrevious()
//    {
//        if (previousSnapZone != null)
//        {
//            transform.position = previousSnapZone.transform.position;
//            transform.rotation = previousSnapZone.transform.rotation;
//            transform.SetParent(previousSnapZone.transform);
//            previousSnapZone.AssignOccupant(this);

//            Debug.Log("[DraggableEndEffector] Returned to previous zone.");
//        }
//        else
//        {
//            Debug.LogWarning("[DraggableEndEffector] No previous snap zone. Consider implementing a home fallback.");
//        }
//    }
//}
