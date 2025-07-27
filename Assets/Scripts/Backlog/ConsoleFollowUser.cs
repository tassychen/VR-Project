using UnityEngine;

public class ConsoleFollowUser : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0, 0, 2);

    void Update()
    {
        if (cameraTransform == null) return;

        transform.position = cameraTransform.position + cameraTransform.forward * offset.z;
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}

