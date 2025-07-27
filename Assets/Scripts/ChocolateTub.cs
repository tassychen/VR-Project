using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YAProgressBar;

public class ChocolateTub : MonoBehaviour
{
    public int moldCounter = 0;
    public bool hasLiquid = false;
    public int maxCapacity = 5;

    [Header("Blinking Light")]
    public BlinkingLight blinkingLight;

    [Header("Progress Bar")]
    public LinearProgressBar progressBar;

    [Header("Respawn Settings")]
    public GameObject tubPrefab;         
    public float dropSpeed = 2.0f;       // Speed of drop
    public float destroyY = -1.0f;       // Y value under the floor
    public float respawnDelay = 1.5f;

    private Boolean isDropping = false;

    private HashSet<ChocoMold> registeredMolds = new HashSet<ChocoMold>();
    private List<ChocoMold> fallenMolds = new List<ChocoMold>(); // track all molds in tub, needs to be destroyed when tub is emptied

    private void Awake()
    {
        // Ensure the newly spawned tub registers itself
        if (ProcessingLineManager.Instance != null)
        {
            ProcessingLineManager.Instance.currentTub = this;
            Debug.Log("[ChocolateTub] Registered self as currentTub in manager.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ChocoMold mold = other.GetComponentInParent<ChocoMold>();
        if (mold == null) return;

        if (registeredMolds.Contains(mold)) return;

        registeredMolds.Add(mold);
        fallenMolds.Add(mold);
        moldCounter++;

        UpdateProgressBar();

        if (mold.currentState == ChocoMold.ChocolateState.Liquid)
        {
            hasLiquid = true;
            if (blinkingLight != null)
            {
                blinkingLight.StartBlinking();
                Debug.Log("[ChocolateTub] light blinking");
            }
            Debug.Log("[ChocolateTub] liquid detected");
        }

        Debug.Log($"[ChocolateTub] Mold entered. Count: {moldCounter}, Has Liquid: {hasLiquid}");

        if (moldCounter >= maxCapacity && !isDropping)
        {
            StartCoroutine(DropAndReplaceTub());
            Debug.Log("[ChocolateTub] tub full, try to dorp and spawn new");
        }
    }

    private void UpdateProgressBar()
    {
        if (progressBar == null) return;

        float progress = Mathf.Clamp01((float)moldCounter / maxCapacity);
        progressBar.FillAmount = progress;
        Debug.Log($"[ChocolateTub] Progress updated to: {progress * 100}%");
    }
    private void ResetProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.FillAmount = 0f;
            Debug.Log("[ChocolateTub] Progress bar reset to 0");
        }
    }
    private IEnumerator DropAndReplaceTub()
    {
        isDropping = true;


        // disable colliders in mold so it pass through floor 
        foreach (var mold in fallenMolds)
        {
            if (mold != null)
            {
                Collider[] colliders = mold.GetComponentsInChildren<Collider>();
                foreach (var col in colliders)
                {
                    col.enabled = false;
                    Debug.Log("[ChocolateTub] All molds' colliders disabled");

                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        // Drop down over time
        while (transform.position.y > destroyY)
        {
            transform.parent.position += Vector3.down * dropSpeed * Time.deltaTime;
            yield return null;
        }

        // Destroy all molds inside
        DestroyAllFallenMolds();
        Debug.Log("[ChocolateTub] All molds in full tub are destroyed");


        // Wait a moment, then spawn a new tub
        yield return new WaitForSeconds(respawnDelay);

        GameObject newTub = Instantiate(tubPrefab, transform.position + Vector3.up * 1.45f, Quaternion.identity);
        Debug.Log("[ChocolateTub] empty tub spawned");

        ChocolateTub tubComponent = newTub.GetComponentInChildren<ChocolateTub>();

        if (tubComponent != null)
        {
            ProcessingLineManager.Instance.currentTub = tubComponent;
        }


        // Finally destroy this tub
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        Debug.Log("[ChocolateTub] full tub destroyed");

    }

    // destroy all molds that fell into this tub
    public void DestroyAllFallenMolds()
    {
        foreach (var mold in fallenMolds)
        {
            if (mold != null)
            {
                Destroy(mold.gameObject);
            }
        }

        fallenMolds.Clear();
        registeredMolds.Clear();
        moldCounter = 0;
        ResetProgressBar();
        hasLiquid = false;

        Debug.Log("[ChocolateTub] All fallen molds destroyed.");
    }

    //only for resetting state
    public void ForceDropNow()
    {
        if (!isDropping)
        {
            StartCoroutine(DropAndReplaceTub());
        }
    }

}

