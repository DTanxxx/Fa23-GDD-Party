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
    [SerializeField] private Transform lightTransform;
    [SerializeField] private float damping = 20.0f;

    private Light2D lightComponent;
    private Vector3 currDirection;
    private Vector3 tempDirection;
    private string currColor;
    private Color defaultColor;
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

        defaultColor = lightComponent.color;
        currColor = "none";

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
            // check for line of sight
            if (Physics.Raycast(lightTransform.position, (hit.transform.position - lightTransform.position).normalized,
                out hitInfo, lightComponent.pointLightOuterRadius, ~playerLayer, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log(hitInfo.transform.gameObject.layer);
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("WeepingAngel"))
                {
                    // flashlight hits enemy
                    Vector3 hitDir = new Vector3(hit.transform.position.x - lightTransform.position.x,
                        0, hit.transform.position.z - lightTransform.position.z);
                    float cosTheta = Vector3.Dot(tempDirection.normalized, hitDir.normalized);
                    float deg = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
                    if (Mathf.Abs(deg) <= lightComponent.pointLightOuterAngle / 2.0f)
                    {
                        // check if both hitDir and actual direction from player to enemy match
                        Vector3 dirFromPlayerToEnemy = new Vector3(hit.transform.position.x - lightTransform.position.x,
                            0, hit.transform.position.z - lightTransform.position.z);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            lightComponent.color = defaultColor;
            currColor = "none";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            lightComponent.color = Color.red;
            currColor = "red";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            lightComponent.color = Color.green;
            currColor = "green";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            lightComponent.color = Color.blue;
            currColor = "blue";
        }
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

    public string GetColor()
    {
        return currColor;
    }

}
