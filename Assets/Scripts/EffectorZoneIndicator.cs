using System;
using UnityEngine;

public class EffectorZoneIndicator : MonoBehaviour
{
    public enum ZoneID { Left, Center, Right }
    public ZoneID zoneID;

    [Tooltip("The light Inditator")]
    public Renderer indicatorRenderer;

    [Header("Colors")]
    public Color placedColor = Color.red;
    public Color grabbedColor = Color.green;

    private bool isOccupied = false;  // Flag to track if the zone is occupied
    private Collider currentEndEffector = null;  // Track the current end-effector in this zone

    private void OnTriggerEnter(Collider other)
    {
        // Ensure we only interact with end-effectors
        if (other.CompareTag("EndEffector"))
        {
            // If the zone is not occupied, place the first end-effector
            if (!isOccupied)
            {
                SetColor(placedColor);
                isOccupied = true;
                currentEndEffector = other;  // Store the reference to the occupied end-effector
                Debug.Log($"[{zoneID}] End-effector placed.");
            }
            else
            {
                Debug.Log($"[{zoneID}] Snapzone is already occupied. No color change.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If the end-effector that is leaving is the one currently occupying the zone, reset
        if (other.CompareTag("EndEffector") && other == currentEndEffector)
        {
            SetColor(grabbedColor);  // Change color back to grabbed
            isOccupied = false;
            currentEndEffector = null;  // Clear the current end-effector reference
            Debug.Log($"[{zoneID}] End-effector removed.");
        }
    }

    private void SetColor(Color color)
    {
        if (indicatorRenderer != null)
        {
            indicatorRenderer.material.color = color;
        }
    }
}
//using System;
//using UnityEngine;

//public class EffectorZoneIndicator : MonoBehaviour
//{
//    public enum ZoneID { Left, Center, Right }
//    public ZoneID zoneID;

//    [Tooltip("The light Inditator")]
//    public Renderer indicatorRenderer;

//    [Header("Colors")]
//    public Color placedColor = Color.red;
//    public Color grabbedColor = Color.green;

//    private Boolean isOccupied = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (isOccupied)
//        {
//            Debug.Log($"[EffectorZoneIndicator] [{zoneID}] Snapzone is already occupied");
//            return;
//        }
//        if (other.CompareTag("EndEffector") && !isOccupied)
//        {

//            SetColor(placedColor);
//            isOccupied = true;
//            Debug.Log($"[{zoneID}] End-effector placed.");
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("EndEffector") && isOccupied)
//        {
//            SetColor(grabbedColor);
//            isOccupied = false;
//            Debug.Log($"[{zoneID}] End-effector removed.");
//        }
//    }

//    private void SetColor(Color color)
//    {
//        if (indicatorRenderer != null)
//        {
//            indicatorRenderer.material.color = color;
//        }
//    }
//}
