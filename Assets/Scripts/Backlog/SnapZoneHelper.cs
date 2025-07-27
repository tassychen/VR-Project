//using UnityEngine;

//public static class SnapZoneHelper
//{
//    public static SnapZone GetAvailableSnapZone(Vector3 position, float radius)
//    {
//        Collider[] colliders = Physics.OverlapSphere(position, radius);
//        foreach (Collider col in colliders)
//        {
//            SnapZone zone = col.GetComponent<SnapZone>();
//            if (zone != null && !zone.IsOccupied)
//            {
//                return zone;
//            }
//        }
//        return null;
//    }
//}
