using System.Collections;
using UnityEngine;
using Obi;

public class ChocoDispenser : MonoBehaviour
{
    [Header("Chocolate Pouring Setup")]
    public ObiEmitter chocolateEmitter;
    public float pourDuration = 5f;

    private bool isRunning = false;
    public bool lastActionDone = true;
    private ChocoMold currentMold = null;


    void Start()
    {
        if (chocolateEmitter != null)
        {
            chocolateEmitter.speed = 0f; 
        }
    }
    public void StartPouring(ChocoMold mold)
    {
        if (!isRunning && lastActionDone)
        {
            currentMold = mold;
            StartCoroutine(PourRoutine());
        }
    }

    private IEnumerator PourRoutine()
    {
        isRunning = true;
        lastActionDone = false;

        Debug.Log("[ChocoDispenser] Pouring chocolate...");

        if (chocolateEmitter != null)
            chocolateEmitter.speed = 1f;

        yield return new WaitForSeconds(pourDuration);

        if (chocolateEmitter != null)
        {
            chocolateEmitter.speed = 0f;
            chocolateEmitter.KillAll();  // Clear all particles
        }

        lastActionDone = true;
        isRunning = false;

        Debug.Log("[ChocoDispenser] Pour complete. doneAction = true.");
        if (currentMold != null)
        {
            currentMold.SetToLiquid(); //change the state of chocolate in the mold to liquid
            currentMold = null; 
        }
    }
}
