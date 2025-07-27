using System.Collections.Generic;
using UnityEngine;

public class ChocoBox : MonoBehaviour
{
    public enum BoxState { Empty, Filled }

    public enum BoxFoldingState { Unfolded, Folded }

    public BoxState currentState = BoxState.Empty;
    public BoxFoldingState currentFoldingState = BoxFoldingState.Unfolded;
    public static bool isConfettied = false;


    [Header("Chocolate Bar Setup")]
    public GameObject chocolateBarPrefab;
    public Transform spawnPoint;
    private GameObject spawnedSolidChocolate;

    [Header("Animation")]
    private Animator animator;

    public static List<ChocoBox> allBoxes = new List<ChocoBox>();

    void Awake()
    {
        allBoxes.Add(this);
    }

    void OnDestroy()
    {
        allBoxes.Remove(this);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("[ChocoBox] Total boxes in list: " + ChocoBox.allBoxes.Count);
    }

    public void FillBox()
    {
        if (currentState == BoxState.Empty && chocolateBarPrefab != null && spawnPoint != null)
        {
            spawnedSolidChocolate = Instantiate(chocolateBarPrefab, spawnPoint.position, spawnPoint.rotation, transform);
            currentState = BoxState.Filled;
            Debug.Log("[ChocoBox] Chocolate bar spawned. State: Filled");
        }
    }

    public void CloseBox()
    {
        if (currentFoldingState == BoxFoldingState.Unfolded && animator != null)
        {
            animator.SetTrigger("Close");
            currentFoldingState = BoxFoldingState.Folded;
            Debug.Log("[ChocoBox] Animation triggered. State: Folded");
        }
    }

    public void ForceDestroyBox()
    {
        if (spawnedSolidChocolate != null)
        {
            Destroy(spawnedSolidChocolate);
        }
        Destroy(gameObject);
    }

    // Optional helper to destroy all boxes
    public static void DestroyAllBoxes()
    {
        foreach (var box in new List<ChocoBox>(allBoxes))
        {
            if (box != null)
                box.ForceDestroyBox();
        }
        allBoxes.Clear();
        Debug.Log("[ChocoBox] All boxes destroyed");
    }
}




//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChocoBox : MonoBehaviour
//{

//    public bool isClosed = false;
//    public bool isStamped;
//    private Animator animator;

//    void Start()
//    {
//        animator = GetComponent<Animator>();

//    }

//    public void CloseBox()
//    {
//        if (!isClosed)
//        {
//            Debug.Log("[ChocoBox] isClosed should be false and testing result is " + isClosed);
//            animator.SetTrigger("Close");
//            isClosed = true;
//            Debug.Log("[ChocoBox] isClosed should be true and testing result is " + isClosed);
//        }
//    }

//}
