using UnityEngine;

public class SnapZoneTrigger : MonoBehaviour
{
    [Header("Spawn Prefabs by Type")]
    public GameObject coolerPrefab;
    public GameObject pickAndPlacePrefab;
    public GameObject stampPrefab;

    [Tooltip("The parent object to attach the spawned end-effector to")]
    public Transform snapParent;

    [Tooltip("Glowing Visual")]
    public GameObject guideVisual;

    [Header("Offset Overrides")]
    public Vector3 coolerRotationOffset;
    public Vector3 pickAndPlaceRotationOffset;
    public Vector3 stampRotationOffset;

    public Vector3 coolerPositionOffset;
    public Vector3 pickAndPlacePositionOffset;
    public Vector3 stampPositionOffset;

    private bool isOccupied = false;
    private bool glowWhenSnapped = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied)
        {
            Debug.Log("[SnapZoneTrigger] Snapzone occupied");
            return;
        }
        if (!other.CompareTag("EndEffector")) return;

        EndEffectorType type = other.GetComponentInChildren<EndEffectorType>();
        if (type == null)
        {
            Debug.LogWarning("[SnapZoneTrigger] EndEffectorType missing on: " + other.name);
            return;
        }

        GameObject prefabToSpawn = GetPrefabFromState(type.currentState);
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("[SnapZoneTrigger] No prefab assigned for state: " + type.currentState);
            return;
        }

        isOccupied = true;

        if (guideVisual != null)
        {
            guideVisual.SetActive(false);
            Debug.Log("[SnapZoneTrigger] glowing effect should be invisible");

        }
        glowWhenSnapped = true;

        Destroy(other.gameObject);
        Debug.Log("[SnapZoneTrigger] Destroyed carried end-effector.");

        // Calculate final spawn position and rotation
        Vector3 localOffset = GetPositionOffset(type.currentState);
        Vector3 worldPosition = snapParent.position + snapParent.TransformVector(localOffset);
        Quaternion spawnRotation = snapParent.rotation * Quaternion.Euler(GetRotationOffset(type.currentState));

        GameObject newEffector = Instantiate(prefabToSpawn, worldPosition, spawnRotation, snapParent);

        EndEffectorType newType = newEffector.GetComponentInChildren<EndEffectorType>();

        // Notify RoboticSimulator if it exists on the same parent
        RoboticSimulator arm = snapParent.GetComponentInParent<RoboticSimulator>();
        if (arm != null && newType != null)
        {
            arm.SetEffectorReference(newType);
            Debug.Log($"[SnapZoneTrigger] found new end-effector reference.");
        }
        Debug.Log($"[SnapZoneTrigger] Spawned new {type.currentState} at snap zone.");
    }

    private GameObject GetPrefabFromState(EndEffectorType.EffectorState state)
    {
        return state switch
        {
            EndEffectorType.EffectorState.Cooler => coolerPrefab,
            EndEffectorType.EffectorState.PickAndPlace => pickAndPlacePrefab,
            EndEffectorType.EffectorState.Stamp => stampPrefab,
            _ => null
        };
    }

    private Vector3 GetRotationOffset(EndEffectorType.EffectorState state)
    {
        return state switch
        {
            EndEffectorType.EffectorState.Cooler => coolerRotationOffset,
            EndEffectorType.EffectorState.PickAndPlace => pickAndPlaceRotationOffset,
            EndEffectorType.EffectorState.Stamp => stampRotationOffset,
            _ => Vector3.zero
        };
    }

    private Vector3 GetPositionOffset(EndEffectorType.EffectorState state)
    {
        return state switch
        {
            EndEffectorType.EffectorState.Cooler => coolerPositionOffset,
            EndEffectorType.EffectorState.PickAndPlace => pickAndPlacePositionOffset,
            EndEffectorType.EffectorState.Stamp => stampPositionOffset,
            _ => Vector3.zero
        };
    }
    public void ClearZone()
    {
        isOccupied = false;
        glowWhenSnapped = false;

        if (guideVisual != null)
            guideVisual.SetActive(true); // show only after reset
        Debug.Log("[SnapZoneTrigger] ClearZone is called");

    }

    //public void ClearZone()
    //{
    //    isOccupied = false;
    //}
}
//using UnityEngine;
//public class SnapZoneTrigger : MonoBehaviour
//{
//    [Header("Spawn Prefabs by Type")]
//    public GameObject coolerPrefab;
//    public GameObject pickAndPlacePrefab;
//    public GameObject stampPrefab;

//    [Tooltip("new end-effector spawnPoint")]
//    public Transform snapParent;

//    [Header("Optional Rotation/Location Overrides")]
//    public Vector3 coolerRotationOffset;
//    public Vector3 pickAndPlaceRotationOffset;
//    public Vector3 stampRotationOffset;
//    public Vector3 positionOffset = Vector3.zero;

//    private bool isOccupied = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (isOccupied)
//        {
//            Debug.Log("[SnapZoneTrigger] snapzone already occupied");

//            return;
//        }
//        if (!other.CompareTag("EndEffector")) return;

//        EndEffectorType type = other.GetComponentInChildren<EndEffectorType>();
//        if (type == null)
//        {
//            Debug.LogWarning("[SnapZoneTrigger] EndEffectorType missing on: " + other.name);
//            return;
//        }

//        GameObject prefabToSpawn = GetPrefabFromState(type.currentState);
//        if (prefabToSpawn == null)
//        {
//            Debug.LogWarning("[SnapZoneTrigger] No prefab assigned for state: " + type.currentState);
//            return;
//        }

//        isOccupied = true;
//        // Destroy carried end-effector
//        Destroy(other.gameObject);
//        Debug.LogWarning("[SnapZoneTrigger] carried end-effector destroyed");

//        // Spawn end-effector on robotic arm
//        Quaternion spawnRotation = snapParent.rotation * Quaternion.Euler(GetRotationOffset(type.currentState));
//        GameObject newEffector = Instantiate(prefabToSpawn, snapParent.position, spawnRotation, snapParent);

//        Debug.Log("[SnapZoneTrigger] Spawned: " + type.currentState);
//    }

//    private GameObject GetPrefabFromState(EndEffectorType.EffectorState state)
//    {
//        switch (state)
//        {
//            case EndEffectorType.EffectorState.Cooler: return coolerPrefab;
//            case EndEffectorType.EffectorState.PickAndPlace: return pickAndPlacePrefab;
//            case EndEffectorType.EffectorState.Stamp: return stampPrefab;
//            default: return null;
//        }
//    }

//    private Vector3 GetRotationOffset(EndEffectorType.EffectorState state)
//    {
//        switch (state)
//        {
//            case EndEffectorType.EffectorState.Cooler:
//                return coolerRotationOffset;
//            case EndEffectorType.EffectorState.PickAndPlace:
//                return pickAndPlaceRotationOffset;
//            case EndEffectorType.EffectorState.Stamp:
//                return stampRotationOffset;
//            default:
//                return Vector3.zero;
//        }
//    }


//    //for resetting the scene
//    public void ClearZone()
//    {
//        isOccupied = false;
//    }
//}

