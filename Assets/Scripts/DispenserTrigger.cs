using UnityEngine;

public class DispenserTrigger : MonoBehaviour
{

    public ChocoDispenser dispenserObject;

    public bool collided = false;
    private bool hasResumed = false; //conveyor belt resume

    // Update is called once per frame
    void Update()
    {
        if(collided && dispenserObject != null && dispenserObject.lastActionDone && !hasResumed)
        {
            Debug.Log("[DispenserTrigger] pouring done. Resuming conveyors.");
            ConveyorParent.Instance.ResumeAll();
            hasResumed = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!collided && dispenserObject != null && dispenserObject.lastActionDone)
        {
            ChocoMold mold = other.GetComponent<ChocoMold>();
            if (mold != null && mold.currentState == ChocoMold.ChocolateState.Empty)
            {
                Debug.Log("[DispenserTrigger] Trigger entered. Starting chocolate pour.");
                ConveyorParent.Instance.PauseAll();

                dispenserObject.StartPouring(mold); 
                collided = true;
                hasResumed = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collided = false;
        Debug.Log("[DispenserTrigger] Trigger exited, collided = false");
    }


    //reset collided to false when reset button is pressed
    public void ResetTrigger()
    {
        collided = false;
        hasResumed = false;

        Debug.Log("[DispenserTrigger] Reset called. State cleared.");
    }

}

