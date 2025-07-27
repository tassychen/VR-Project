using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject objectToSpawn;
    public Transform spawnLocation;
    public float spawnInterval = 3.5f;

    private float timer = 0f;
    private bool isSpawning = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartSpawning()
    {
        isSpawning = true;
        timer = 0f;
        Debug.Log("[Spawner] Started spawning.");
    }

    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log("[Spawner] Stopped spawning.");
    }

    void Update()
    {
        if (!isSpawning) return;
        if (ConveyorParent.Instance != null && ConveyorParent.Instance.isPaused)
        {
            //Debug.Log("[Spawner] conveyor is paused, stop spawnning!");
            //timer = 0f;
            //Debug.Log("[Spawner] reset timmer to 0, and now timer is " + timer);
            return;
        }
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        Instantiate(objectToSpawn, spawnLocation.position, spawnLocation.rotation);
        Debug.Log("[Spawner] one mold is spawned!");
    }
}

//using UnityEngine;

//public class Spawner : MonoBehaviour
//{
//    public GameObject objectToSpawn;
//    public Transform spawnLocation;
//    public float spawnInterval = 3.5f;   //in seconds
//    private float timer = 0f; //accumulator that counts how much time has been past from past spwan
//    void Update()
//    {
//        if (ConveyorParent.Instance != null && ConveyorParent.Instance.isPaused) return;

//        timer += Time.deltaTime;
//        if (timer >= spawnInterval)
//        {
//            Spawn();
//            timer = 0f;
//        }
//    }

//    void Spawn()
//    {
//        Instantiate(objectToSpawn, spawnLocation.position, spawnLocation.rotation);
//    }

//}