using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Light targetLight;
    public float blinkInterval = 0.5f;

    private bool isBlinking = false;
    private float timer = 0f;
    private bool isOn = false;

    void Update()
    {
        if (!isBlinking || targetLight == null) return;

        timer += Time.deltaTime;
        if (timer >= blinkInterval)
        {
            timer = 0f;
            isOn = !isOn;
            targetLight.enabled = isOn;
            Debug.Log("[BlinkingLight] light blinking");
        }
    }

    public void StartBlinking()
    {
        isBlinking = true;
        timer = 0f;
        isOn = true;
        Debug.Log("[BlinkingLight] startblinking function triggered");
        if (targetLight != null)
            targetLight.enabled = true;
    }

    public void StopBlinking()
    {
        isBlinking = false;
        isOn = false;
        if (targetLight != null)
            targetLight.enabled = false;
    }
}
