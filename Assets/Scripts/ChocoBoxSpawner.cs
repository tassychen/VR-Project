using UnityEngine;

public class ChocoBoxSpawner : MonoBehaviour
{
    [Header("Spawn Setup")]
    public GameObject ChocoBoxPrefab;           
    public Transform spawnLocation;         

    public GameObject Spawn()
    {
        if (ChocoBoxPrefab == null || spawnLocation == null)
        {
            Debug.LogWarning("[ChocoBoxSpawner] Missing prefab or spawn location. Cannot spawn.");
            return null;
        }

        GameObject spawnedObject = Instantiate(ChocoBoxPrefab, spawnLocation.position, spawnLocation.rotation);
        Debug.Log("[ChocoBoxSpawner] Object spawned at location: " + spawnLocation.position);
        return spawnedObject;
    }
}
