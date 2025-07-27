using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;

    //public AudioClip backgroundMusic;
    public AudioSource audioSource;

    void Awake()
    {
        // Ensure only one instance exists (singleton)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes

        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound (non-directional)
        audioSource.Play();
    }
}
