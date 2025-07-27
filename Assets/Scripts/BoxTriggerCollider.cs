using UnityEngine;

public class BoxTriggerCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        ChocoBox box = other.GetComponent<ChocoBox>();
        if (box != null && box.currentFoldingState != ChocoBox.BoxFoldingState.Folded)
        {
            box.CloseBox();
        }
    }

}
