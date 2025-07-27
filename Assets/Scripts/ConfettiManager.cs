using UnityEngine;

public class ConfettiManager : MonoBehaviour
{
    public static ConfettiManager Instance { get; private set; }

    [Header("Assign your confetti particle system here")]
    public ParticleSystem confettiFx;

    public AudioSource confettiAudioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayConfetti()
    {
        if (confettiFx != null && !confettiFx.isPlaying)
        {
            confettiFx.Play();
            Debug.Log("[ConfettiManager] Confetti played!");
        }

        if (confettiAudioSource != null && !confettiAudioSource.isPlaying)
        {
            confettiAudioSource.Play();
            Debug.Log("[ConfettiManager] Confetti sound played!");
        }

    }

    public void StopConfetti()
    {
        if (confettiFx != null && confettiFx.isPlaying)
        {
            confettiFx.Stop();
            Debug.Log("[ConfettiManager] Confetti stopped.");
        }
    }

    public void ResetConfettiState()
    {
        ChocoBox.isConfettied = false;
        StopConfetti();
        Debug.Log("[ConfettiManager] Confetti state reset.");
    }
}
