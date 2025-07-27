using UnityEngine;

public class EndEffectorType : MonoBehaviour
{
    public enum EffectorState { Empty, Cooler, PickAndPlace, Stamp }
    public enum StainState { NotStained, Stained }

    public EffectorState currentState = EffectorState.Empty;
    public StainState currentStainState = StainState.NotStained;

    [Header("Stain Setup")]
    public GameObject stainPrefab;
    public Transform stainSpawnPoint;

    private GameObject stainInstance;

    [Header("Sticker Setup")]
    public GameObject stickerPrefab;
    public Transform stickerSpawnPoint;

    public void ApplyStain()
    {
        if (currentStainState == StainState.NotStained && stainPrefab != null && stainSpawnPoint != null)
        {
            stainInstance = Instantiate(stainPrefab, stainSpawnPoint.position, stainSpawnPoint.rotation, stainSpawnPoint);
            currentStainState = StainState.Stained;
            Debug.Log("[EndEffectorType] Stain applied.");
        }
    }

    public void RemoveStain()
    {
        if (stainInstance != null)
        {
            Destroy(stainInstance);
            stainInstance = null;
            currentStainState = StainState.NotStained;
            Debug.Log("[EndEffectorType] Stain removed.");
        }
    }

    public void ApplySticker(GameObject target, Transform spawnPoint)
    {
        if (stickerPrefab != null && spawnPoint != null && target != null)
        {
            Instantiate(stickerPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            Debug.Log("[EndEffectorType] Sticker applied.");
        }
    }

}

