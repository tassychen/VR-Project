using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class SimpleVRButton : MonoBehaviour
{
    public enum ButtonType { Run, EmergencyStop, Reset }
    public ButtonType buttonType;

    [Header("Visual Feedback")]
    public Renderer buttonRenderer;               // Drag the mesh renderer here
    public Color activeColor = Color.white;       // Default color when pressable
    public Color inactiveColor = Color.gray;      // Gray when not pressable

    [Header("Audio")]
    public AudioSource buttonAudioSource; // assign in Inspector

    public Animator buttonAnimator;                  // assign via inspector
    public string pressTrigger = "Press";            // name of the animation trigger
    public UnityEvent onButtonPressed;               // assign functions in Inspector
    public ProcessingLineManager processingLineManager; // reference to ProcessingLineManager to check state

    private bool isPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isPressed) return;

        // Ensure the collision is with the hand or controller
        if (other.CompareTag("PlayerHand") || other.name.Contains("Grab"))
        {
            // Check the current state of the processing line manager
            if (processingLineManager != null)
            {
                if (!CanPressButton())
                {
                    Debug.Log("[SimpleVRButton] Button cannot be pressed in the current state.");
                    return;
                }
            }

            // Perform the button press animation
            if (buttonAnimator != null && !string.IsNullOrEmpty(pressTrigger))
            {
                buttonAnimator.Play("ButtonPressed");
                Debug.Log($"[SimpleVRButton] [{gameObject.name}] Press animation triggered.");
            }


            onButtonPressed?.Invoke();
            Debug.Log($"[SimpleVRButton] [{gameObject.name}] Button action invoked.");

            //play button press sound
            if (buttonAudioSource != null)
            {
                buttonAudioSource.Play();
                Debug.Log("[SimpleVRButton] Button press sound played.");
            }


            isPressed = true;

            // Optional: reset button press after a delay
            ResetButton(1f);  // Reset after 1 second
            Debug.Log("[SimpleVRButton] Button is reset");
        }
        else
        {
            Debug.Log("[SimpleVRButton] Not detecting the controller. Collided with: " + other.name + " (Tag: " + other.tag + ")");
        }
    }

    private void Update()
    {
        if (processingLineManager == null || buttonRenderer == null)
            return;

        bool canPress = CanPressButton();
        Color targetColor = canPress ? activeColor : inactiveColor;

        if (buttonRenderer.material.color != targetColor)
        {
            buttonRenderer.material.color = targetColor;
        }
    }


    public void ResetButton(float delay = 1f)
    {
        Invoke(nameof(EnablePress), delay);
    }

    private void EnablePress()
    {
        isPressed = false;
    }

    private bool CanPressButton()
    {
        bool allowed = false;

        switch (processingLineManager.currentState)
        {
            case ProcessingLineManager.LineState.Running:
                allowed = buttonType == ButtonType.EmergencyStop;
                break;
            case ProcessingLineManager.LineState.EmergencyStopped:
                allowed = buttonType == ButtonType.Reset;
                break;
            case ProcessingLineManager.LineState.Resetting:
                allowed = buttonType == ButtonType.Run;
                break;
        }

        Debug.Log($"[SimpleVRButton] CanPressButton result: {allowed} for {buttonType} in state {processingLineManager.currentState}");
        return allowed;
    }

    //private bool CanPressButton()
    //{
    //    switch (processingLineManager.currentState)
    //    {
    //        case ProcessingLineManager.LineState.Running:
    //            return buttonType == ButtonType.EmergencyStop;
    //        case ProcessingLineManager.LineState.EmergencyStopped:
    //            return buttonType == ButtonType.Reset;
    //        case ProcessingLineManager.LineState.Resetting:
    //            return buttonType == ButtonType.Run;
    //        default:
    //            return false;
    //    }

    //}


}
//using UnityEngine;
//using UnityEngine.Events;

//[RequireComponent(typeof(Collider))]
//public class SimpleVRButton : MonoBehaviour
//{
//    public Animator buttonAnimator;                  // assign via inspector
//    public string pressTrigger = "Press";            // name of the animation trigger
//    public UnityEvent onButtonPressed;               // assign functions in Inspector

//    private bool isPressed = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (isPressed) return;

//        if (other.CompareTag("PlayerHand") || other.name.Contains("Grab"))
//        {
//            Debug.Log("[SimpleVRButton] Button trigger collider triggered by hand.");
//            isPressed = true;

//            if (buttonAnimator != null && !string.IsNullOrEmpty(pressTrigger))
//            {
//                buttonAnimator.Play("ButtonPressed");
//                //buttonAnimator.SetTrigger(pressTrigger);
//                Debug.Log($"[SimpleVRButton] [{gameObject.name}] Press animation triggered.");
//            }

//            onButtonPressed?.Invoke();
//            Debug.Log($"[SimpleVRButton] [{gameObject.name}] Button action invoked.");
//        }
//        else
//        {
//            Debug.Log("[SimpleVRButton] Not detecting the controller. Collided with: " + other.name + " (Tag: " + other.tag + ")");
//        }
//    }

//    // Optional: allow button to be pressed again after delay
//    public void ResetButton(float delay = 1f)
//    {
//        Invoke(nameof(EnablePress), delay);
//    }

//    private void EnablePress()
//    {
//        isPressed = false;
//    }
//}
