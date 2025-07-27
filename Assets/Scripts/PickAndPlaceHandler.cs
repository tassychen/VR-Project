using UnityEngine;

public class PickAndPlaceHandler : MonoBehaviour
{
    [Header("Chocolate Transfer Setup")]
    public GameObject carriedChocolatePrefab;
    public Transform chocolateSpawnPoint;

    private GameObject spawnedChocolate;

    [Header("ChocoBox Transfer Setup")]
    public GameObject carriedBoxPrefab;
    public Transform boxSpawnPoint;
    public Transform boxDropPoint;

    private GameObject spawnedBox;

    // Called when robot arm begins lowering to grab chocolate
    public void SpawnCarriedChocolate()
    {
        if (spawnedChocolate == null && carriedChocolatePrefab != null && chocolateSpawnPoint != null)
        {
            spawnedChocolate = Instantiate(carriedChocolatePrefab, chocolateSpawnPoint.position, chocolateSpawnPoint.rotation, transform);
            Debug.Log("[PickAndPlaceHandler] Chocolate spawned on arm.");
        }
    }

    // Called when robot "drops" chocolate into the box
    public void DestroyCarriedChocolate()
    {
        if (spawnedChocolate != null)
        {
            Destroy(spawnedChocolate);
            spawnedChocolate = null;
            Debug.Log("[PickAndPlaceHandler] Chocolate removed from arm.");
        }
    }


    // Called when robot arm begins lowering to grab chocolate box
    public void SpawnCarriedBox()
    {
        if (spawnedBox == null && carriedBoxPrefab != null && boxSpawnPoint != null)
        {
            //spawnedBox = Instantiate(carriedBoxPrefab, boxSpawnPoint.position, boxSpawnPoint.rotation, transform);

            spawnedBox = Instantiate(
            carriedBoxPrefab,
            boxSpawnPoint.position,
            boxSpawnPoint.rotation,
            boxSpawnPoint //parent to spawn point
        );

            spawnedBox.transform.localPosition = Vector3.zero;
            spawnedBox.transform.localRotation = Quaternion.identity;
            spawnedBox.transform.localScale = Vector3.one;


            ChocoBox boxScript = spawnedBox.GetComponent<ChocoBox>();
            if (boxScript != null)
            {
                boxScript.currentFoldingState = ChocoBox.BoxFoldingState.Folded;

                // should already be closed
                Animator anim = boxScript.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.Play("BoxClose", 0, 1f); 
                }

                Debug.Log("[PickAndPlaceHandler] Carried box spawned already closed.");
            }

            Rigidbody rb = spawnedBox.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            Debug.Log("[PickAndPlaceHandler] Folded box spawned on arm.");
        }
    }

    // Called when robot "drops" chocolate box to the ground
    public void DestroyCarriedBox()
    {
        if (spawnedBox != null)
        {
            Destroy(spawnedBox);
            spawnedBox = null;
            Debug.Log("[PickAndPlaceHandler] Folded box removed from arm.");
        }
    }

}
