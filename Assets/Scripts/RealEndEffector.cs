using UnityEngine;

public class RealEndEffector : MonoBehaviour
{
    private SnapZone assignedZone;

    public void Initialize(SnapZone zone)
    {
        assignedZone = zone;
        if (zone != null) zone.isOccupied = true;
    }

    private void OnDestroy()
    {
        if (assignedZone != null)
        {
            assignedZone.isOccupied = false;
        }
    }
}
