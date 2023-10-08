using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Timeline;

public class LightDirection : MonoBehaviour
{
    [SerializeField] private LayerMask weepingAngelLayer;
    [SerializeField] private LayerMask playerLayer;

    private PlayerMovement parentScript;
    private Light lightComponent;
    private Vector3 currDir;
    private float damping = 20.0f;
    private float sphereCastRadius;


    // for Gizmos
    private RaycastHit[] sphereCastHits;
    private float sphereCastHitDistance;
    private RaycastHit[] inFlashlight;


    private void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        parentScript = GetComponentInParent<PlayerMovement>();
        lightComponent = GetComponent<Light>();

        sphereCastRadius = Mathf.Atan(lightComponent.spotAngle / 2.0f * Mathf.Deg2Rad) * lightComponent.range;
    }

    private void FixedUpdate()
    {
        currDir = parentScript.getDir();
        Quaternion smoothing = Quaternion.LookRotation(currDir);
        gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, smoothing,
            Time.fixedDeltaTime * damping);

        sphereCastHits = Physics.SphereCastAll(transform.position, sphereCastRadius,
            currDir, lightComponent.range, weepingAngelLayer.value, QueryTriggerInteraction.Ignore);
        
        // for Gizmos ========================================
        if (sphereCastHits.Length > 0)
        {
            sphereCastHitDistance = sphereCastHits[0].distance;
        }
        else
        {
            sphereCastHitDistance = lightComponent.range;
        }
        // ===================================================

        foreach (var hit in sphereCastHits)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, (hit.transform.position - transform.position).normalized,
                out hitInfo, lightComponent.range, ~playerLayer, QueryTriggerInteraction.Ignore))
            {
                inFlashlight += hitInfo;

                Debug.Log(hitInfo.transform.gameObject.layer);
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
                {
                    // flashlight hits enemy
                    float cosTheta = Vector3.Dot(currDir.normalized,
                    (hit.transform.position - transform.position).normalized);
                    float deg = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
                    if (Mathf.Abs(deg) <= lightComponent.spotAngle / 2.0f)
                    {
                        hit.transform.GetComponentInParent<WeepingAngelMovement>().Freeze();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(transform.position, transform.position + currDir * sphereCastHitDistance);
        Gizmos.DrawWireSphere(transform.position + currDir * sphereCastHitDistance, sphereCastRadius);
    }

    public GetInRange()
    {
        return inFlashlight;
    }
}
