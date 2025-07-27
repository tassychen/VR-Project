using UnityEngine;

public class FanBladeRotator : MonoBehaviour
{
    public float spinSpeed = 720f;
    private bool spinning = false;

    void Update()
    {
        if (spinning)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void StartSpin() => spinning = true;
    public void StopSpin() => spinning = false;
}
