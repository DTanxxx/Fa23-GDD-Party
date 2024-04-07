using Lurkers.Environment.Vision;
using Lurkers.UI;
using Lurkers.Vision;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Lurkers.Control.Vision
{

    public class LightDirection : MonoBehaviour
    {
        [SerializeField] private LayerMask weepingAngelLayer;
        [SerializeField] private LayerMask faceHuggerLayer;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private GameObject lightContainer;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Transform lightTransform;
        [SerializeField] private float damping = 20.0f;
        [SerializeField] private float dialogueTriggerRange = 5f;

        private Light2D lightComponent;
        private Vector3 currDirection;
        private Vector3 tempDirection;
        private float sphereCastRadius;

        private static bool firstTime;
        private bool firstTimeAfter;

        public static Action onFirstEnemyEncounter;
        public static Action onFirstFreezeEnemy;

        //private ClueGlow clueGlow;

        // for Gizmos
        private RaycastHit[] sphereCastHits;
        private float sphereCastHitDistance;

        private void Start()
        {
            lightContainer.transform.rotation = Quaternion.Euler(0, 90, 0);
            lightComponent = GetComponent<Light2D>();

            sphereCastRadius = Mathf.Atan(lightComponent.pointLightOuterAngle / 2.0f * Mathf.Deg2Rad) * lightComponent.pointLightOuterRadius;
            firstTime = true;
            firstTimeAfter = true;

            //clueGlow = GameObject.Find("EventSystem").GetComponent<ClueGlow>();
        }

        private void FixedUpdate()
        {
            currDirection = playerController.GetDir();
            Quaternion smoothing = Quaternion.LookRotation(currDirection);
            lightContainer.transform.rotation = Quaternion.Lerp(lightContainer.transform.rotation, smoothing,
                Time.fixedDeltaTime * damping);

            tempDirection = lightContainer.transform.rotation * Vector3.forward;

            sphereCastHits = Physics.SphereCastAll(transform.position, sphereCastRadius,
                tempDirection, lightComponent.pointLightOuterRadius, (weepingAngelLayer | faceHuggerLayer), QueryTriggerInteraction.Ignore);

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
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("WeepingAngel") ||
                        hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("FaceHugger"))
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
                                hit.transform.GetComponentInParent<IFlashable>().OnFlash();

                                // ensure these dialogues only get triggered when enemy is closer
                                if (firstTime && !FindObjectOfType<EnemyActivate>().IsActive() && 
                                    Vector3.Distance(hit.transform.position, lightTransform.position) <= dialogueTriggerRange)
                                {
                                    onFirstEnemyEncounter?.Invoke();
                                    firstTime = false;
                                }

                                if (FindObjectOfType<EnemyActivate>().IsActive() && firstTimeAfter &&
                                    Vector3.Distance(hit.transform.position, lightTransform.position) <= dialogueTriggerRange)
                                {
                                    onFirstFreezeEnemy?.Invoke();
                                    firstTimeAfter = false;
                                }
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
            //clueGlow.ClueSpot(transform.position, sphereCastRadius, tempDirection, lightComponent, ~playerLayer, playerMovement.gameObject);
        }

    }
}
