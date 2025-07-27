using UnityEngine;

public class ConfettiTesting : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update is running");

            ConfettiManager.Instance.PlayConfetti();
    }


}
