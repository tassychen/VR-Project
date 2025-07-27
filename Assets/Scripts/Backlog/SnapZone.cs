using UnityEngine;

public class SnapZone : MonoBehaviour
{
    public bool isOccupied = false;

    [Tooltip("Where the end-effector should be spawned")]
    public Transform snapParent;  //  This will be the robotic arm or rover slot transform
}

