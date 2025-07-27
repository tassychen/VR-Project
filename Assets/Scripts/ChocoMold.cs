using UnityEngine;
using System.Collections.Generic;

public class ChocoMold : MonoBehaviour
{
    public enum ChocolateState { Empty, Liquid, Solid }

    [Header("Chocolate Status")]
    public ChocolateState currentState = ChocolateState.Empty;

    [Header("Chocolate Spawn Setup")]
    public GameObject fakeLiquidPrefab;
    public GameObject solidChocolatePrefab;
    public Transform spawnPoint;

    private GameObject spawnedLiquidChocolate;
    private GameObject spawnedSolidChocolate;

    // Chocolate mold manager
    public static List<ChocoMold> allMolds = new List<ChocoMold>();

    void Awake()
    {
        allMolds.Add(this);
    }

    void OnDestroy()
    {
        allMolds.Remove(this);
    }

    // Called by the dispenser after pouring
    public void SetToLiquid()
    {
        if (currentState == ChocolateState.Empty)
        {
            currentState = ChocolateState.Liquid;
            SpawnFakeChocolate();
            Debug.Log("[ChocoMold]: Current chocolate state should be Liquid " + currentState);
        }
    }

    private void SpawnFakeChocolate()
    {
        if (spawnPoint != null && fakeLiquidPrefab != null && spawnedLiquidChocolate == null)
        {
            spawnedLiquidChocolate = Instantiate(
                fakeLiquidPrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                transform  
            );
            Debug.Log("[ChocoMold]: Fake liquid chocolate is spawned");

        }
    }

    public void SetToSolid()
    {
        if (currentState == ChocolateState.Liquid)
        {
            currentState = ChocolateState.Solid;
            SpawnSolidChocolate();
            Debug.Log("[ChocoMold]: Current chocolate state should be Solid " + currentState);
        }

    }
    private void SpawnSolidChocolate()
    {
        if(spawnPoint != null && solidChocolatePrefab != null && spawnedSolidChocolate == null)
        {
            DestroyFakeChocolate();

            spawnedSolidChocolate = Instantiate(
                solidChocolatePrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                transform
            );
            Debug.Log("[ChocoMold]: Solid chocolate is spawned");
        }
    }

    public void DestroyFakeChocolate()
    {
        if (spawnedLiquidChocolate != null)
        {
            Destroy(spawnedLiquidChocolate);
        }
    }

    public void DestroySolidChocolate()
    {
        if (spawnedSolidChocolate != null)
        {
            Destroy(spawnedSolidChocolate);
            spawnedSolidChocolate = null;
            Debug.Log("[ChocoMold]: Solid chocolate removed for pick-and-place.");
        }
    }


    public void ForceDestroyMold()
    {
        DestroyFakeChocolate();
        Destroy(gameObject);
    }

    public static void DestroyAllMolds()
    {
        foreach (var mold in new List<ChocoMold>(allMolds))
        {
            if (mold != null)
                mold.ForceDestroyMold();
        }
        allMolds.Clear();
        Debug.Log("[ChocoMold] All molds destroyed");
    }

}
