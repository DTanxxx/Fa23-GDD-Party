using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGlow : MonoBehaviour
{
    [SerializeField] private string clueTile = "clueTile";
    [SerializeField] public Material clueGlow;
    [SerializeField] public Material clueNorm;
    [SerializeField] public LightDirection flashlight;

    private Transform _selection;
    private Movement parentScript;
    private RaycastHit[] inFlashlight;

    private void Start()
    {
        parentScript = GetComponentInParent<Movement>();
        inFlashlight = flashlight.GetInRange();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = clueNorm;
            _selection = null;
        }

        foreach (RaycastHit hit in inFlashlight)
        {
            var clue = hit.transform;

            if (clue.CompareTag(clueTile))
            {
                var clueRenderer = clue.GetComponent<Renderer>();
                if (clueRenderer != null)
                {
                    clueRenderer.material = clueGlow;
                }

                _selection = clue;
            }
        }
    }
}
