using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDirection : MonoBehaviour
{
    [SerializeField] private LayerMask weepingAngelLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject lightContainer;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float damping = 20.0f;

    private Light2D lightComponent;
    private Vector3 currDirection;
    private Vector3 tempDirection;
    private float sphereCastRadius;
    private ClueGlow clueGlow;

    // for Gizmos
    private RaycastHit[] sphereCastHits;
    private float sphereCastHitDistance;

    private void Start()
    {
        lightContainer.transform.rotation = Quaternion.Euler(0, 90, 0);
        lightComponent = GetComponent<Light2D>();

        sphereCastRadius = Mathf.Atan(lightComponent.pointLightOuterAngle / 2.0f * Mathf.Deg2Rad) * lightComponent.pointLightOuterRadius;

        clueGlow = GameObject.Find("EventSystem").GetComponent<ClueGlow>();

    }

    private void FixedUpdate()
    {
        currDirection = playerMovement.getDir();
        Quaternion smoothing = Quaternion.LookRotation(currDirection);
        lightContainer.transform.rotation = Quaternion.Lerp(lightContainer.transform.rotation, smoothing,
            Time.fixedDeltaTime * damping);

        tempDirection = lightContainer.transform.rotation * Vector3.forward;

        sphereCastHits = Physics.SphereCastAll(transform.position, sphereCastRadius,
            tempDirection, lightComponent.pointLightOuterRadius, weepingAngelLayer.value, QueryTriggerInteraction.Ignore);
        
        // for Gizmos ========================================
        if (sphereCastHits.Length > 0)
        {
            sphereCastHitDistance = sphereCastHits[0].distance;
        }
        else
        {
            sphereCastHitDistance = lightComponent.pointLightOuterRadius;
        }
        // ===================================================

        foreach (var hit in sphereCastHits)
        {
            RaycastHit hitInfo;
            // check for light of sight
            if (Physics.Raycast(playerMovement.transform.position, (hit.transform.position - playerMovement.transform.position).normalized,
                out hitInfo, lightComponent.pointLightOuterRadius, ~playerLayer, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log(hitInfo.transform.gameObject.layer);
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
                {
                    // flashlight hits enemy
                    Vector3 hitDir = new Vector3(hit.transform.position.x - transform.position.x,
                        0, hit.transform.position.z - transform.position.z);
                    float cosTheta = Vector3.Dot(tempDirection.normalized, hitDir.normalized);
                    float deg = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
                    if (Mathf.Abs(deg) <= lightComponent.pointLightOuterAngle / 2.0f)
                    {
                        // check if both hitDir and actual direction from player to enemy match
                        Vector3 dirFromPlayerToEnemy = new Vector3(hit.transform.position.x - playerMovement.transform.position.x,
                            0, hit.transform.position.z - playerMovement.transform.position.z);
                        if (dirFromPlayerToEnemy.x * hitDir.x >= 0 && dirFromPlayerToEnemy.z * hitDir.z >= 0)
                        {
                            hit.transform.GetComponentInParent<WeepingAngelMovement>().Freeze();
                        }
                    }
                }
            }
        }
        ClueSpot();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(lightContainer.transform.position, lightContainer.transform.position + tempDirection * sphereCastHitDistance);
        Gizmos.DrawWireSphere(lightContainer.transform.position + tempDirection * sphereCastHitDistance, sphereCastRadius);
    }

    public void ClueSpot()
    {
        clueGlow.ClueSpot(transform.position, sphereCastRadius, tempDirection, lightComponent, 
            ~playerLayer, playerMovement);
    }

}
