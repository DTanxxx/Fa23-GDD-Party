using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGlow : MonoBehaviour
{
    [SerializeField] private string clueTile = "clueTile";
    [SerializeField] public Material clueGlow;
    [SerializeField] public Material clueNorm;

    private Transform _selection;
    private Movement parentScript;

    private void Start()
    {
        parentScript = GetComponentInParent<Movement>();
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
        
        Vector3 flash_dir = parentScript.getDir();
        var ray = new Ray(gameObject.transform.position, flash_dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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
