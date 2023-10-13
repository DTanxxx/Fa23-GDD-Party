using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Timeline;

public class LightDirection : MonoBehaviour
{
    [SerializeField] private LayerMask weepingAngelLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject lightContainer;
    [SerializeField] private PlayerMovement playerMovement;

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
        lightContainer.transform.rotation = Quaternion.Euler(0, 90, 0);
        lightComponent = GetComponent<Light>();

        sphereCastRadius = Mathf.Atan(lightComponent.spotAngle / 2.0f * Mathf.Deg2Rad) * lightComponent.range;
    }

    private void FixedUpdate()
    {
        currDir = playerMovement.getDir();
        Quaternion smoothing = Quaternion.LookRotation(currDir);
        lightContainer.transform.rotation = Quaternion.Lerp(lightContainer.transform.rotation, smoothing,
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
            // check for light of sight
            if (Physics.Raycast(playerMovement.transform.position, (hit.transform.position - playerMovement.transform.position).normalized,
                out hitInfo, lightComponent.range, ~playerLayer, QueryTriggerInteraction.Ignore))
            {
                inFlashlight += hitInfo;

                Debug.Log(hitInfo.transform.gameObject.layer);
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
                {
                    // flashlight hits enemy
                    Vector3 hitDir = new Vector3(hit.transform.position.x - transform.position.x,
                        0, hit.transform.position.z - transform.position.z);
                    float cosTheta = Vector3.Dot(currDir.normalized, hitDir.normalized);
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
        Debug.DrawLine(lightContainer.transform.position, lightContainer.transform.position + currDir * sphereCastHitDistance);
        Gizmos.DrawWireSphere(lightContainer.transform.position + currDir * sphereCastHitDistance, sphereCastRadius);
    }

    public GetInRange()
    {
        return inFlashlight;
    }
}
