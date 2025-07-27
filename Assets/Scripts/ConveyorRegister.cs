using UnityEngine;
using PCS;
//Attach this script to all conveyor gameobjects to be managed by ConveyorParent
public class ConveyorRegister : MonoBehaviour
{
    void Start()
    {
        PCSConfig config = GetComponent<PCSConfig>();
         if (ConveyorParent.Instance != null && config != null)
        {
            ConveyorParent.Instance.RegisterConveyor(config);
        }
        else
        {
            Debug.LogWarning("ConveyorParent or PCSConfig is not attached to " + gameObject.name);
        }
    }
}

