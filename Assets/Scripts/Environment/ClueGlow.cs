using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ClueGlow : MonoBehaviour
{
    [SerializeField] private string clueTile = "clueTile";
    [SerializeField] private float _revealTime = 3f;
    [SerializeField] private LayerMask clueTrailLayerMask;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 0.1f;

    private RaycastHit[] sphereHitsClue;
    private HashSet<Transform> uniqueClue;
    private HashSet<Transform> prevClue;
    private Dictionary<Transform, Coroutine> activeCoroutines;

    private WaitForSeconds _renderChangeTime;

    private void Start()
    {
        _renderChangeTime = new WaitForSeconds(_revealTime);
        prevClue = new HashSet<Transform>();
        activeCoroutines = new Dictionary<Transform, Coroutine>();
    }

    public void ClueSpot(Vector3 currPos, float sphereCastRadius, Vector3 currDir, Light2D light, int excludePlayerLayerMask, PlayerMovement playerMovement)
    {
        uniqueClue = new HashSet<Transform>();

        //if (faceClue && _selection == null)
        //{
        //    //need code to stop glow and fade
        //    //this is called even when facing
        //    Debug.Log('p');
        //    Debug.Log(faceClue);
        //}
        
        sphereHitsClue = Physics.SphereCastAll(currPos, sphereCastRadius, 
            currDir, light.pointLightOuterRadius, clueTrailLayerMask.value);

        foreach (var hit in sphereHitsClue)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(playerMovement.transform.position, (hit.transform.position - playerMovement.transform.position).normalized,
                out hitInfo, light.pointLightOuterRadius, excludePlayerLayerMask))
            {
                if (hitInfo.transform.CompareTag(clueTile))
                {
                    Vector3 hitDir = new Vector3(hit.transform.position.x - currPos.x, 0, hit.transform.position.z - currPos.z);
                    float cosTheta = Vector3.Dot(currDir.normalized, hitDir.normalized);
                    float deg = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
                    if (Mathf.Abs(deg) <= light.pointLightOuterAngle / 2.0f)
                    {
                        // check if both hitDir and actual direction from player to clue match
                        Vector3 dirFromPlayerToEnemy = new Vector3(hit.transform.position.x - playerMovement.transform.position.x,
                            0, hit.transform.position.z - playerMovement.transform.position.z);
                        if (dirFromPlayerToEnemy.x * hitDir.x >= 0 && dirFromPlayerToEnemy.z * hitDir.z >= 0)
                        {
                            if (!uniqueClue.Contains(hitInfo.transform))
                            {
                                uniqueClue.Add(hitInfo.transform);
                            }
                        }
                    }
                }
            }
        }

        if (!prevClue.SetEquals(uniqueClue))
        {
            //Debug.Log("stoped");
            //foreach (var clue in prevClue)
            //{
            //    StopCoroutine(glower(clue));
            //    faderr(clue);
            //}

            foreach (var clue in uniqueClue)
            {
                // check if this clue is in prevClue
                if (!prevClue.Contains(clue))
                {
                    // stop fade out coroutine
                    if (activeCoroutines.ContainsKey(clue))
                    {
                        StopCoroutine(activeCoroutines[clue]);
                        activeCoroutines.Remove(clue);
                    }
                    // then start fade in coroutine
                    Coroutine a = StartCoroutine(Glower(clue));
                    activeCoroutines.Add(clue, a);
                }
            }

            foreach (var clue in prevClue)
            {
                // check if this clue is in uniqueClue
                if (!uniqueClue.Contains(clue))
                {
                    // stop fade in coroutine
                    StopCoroutine(activeCoroutines[clue]);
                    activeCoroutines.Remove(clue);
                    // then start fade out coroutine
                    //Faderr(clue);
                    Coroutine a = StartCoroutine(Fader(clue));
                    activeCoroutines.Add(clue, a);
                }
            }
        }
        prevClue = uniqueClue;

        /*if (faceClue)
        {
            if (!prevClue.SetEquals(uniqueClue))
            {
                //Debug.Log("stoped");
                //foreach (var clue in prevClue)
                //{
                //    StopCoroutine(glower(clue));
                //    faderr(clue);
                //}

                foreach (var clue in uniqueClue)
                {
                    // check if this clue is in prevClue
                    if (!prevClue.Contains(clue))
                    {
                        // start this coroutine
                        Debug.Log("COROUTINE STARTS FOR: " + clue.gameObject.name);
                        Coroutine a = StartCoroutine(Glower(clue));
                        activeCoroutines.Add(a);
                    }
                }
            }
            prevClue = uniqueClue;
        }
        else
        {            
            if (activeCoroutines.Count > 0)
            {
                foreach (Coroutine a in activeCoroutines)
                {
                    StopCoroutine(a);
                }
                activeCoroutines = new List<Coroutine>();
            }

            
            if (prevClue.Count != 0)
            {
                foreach (var clue in prevClue)
                {
                    StopCoroutine(Glower(clue));
                    Faderr(clue);
                }

                prevClue = new HashSet<Transform>();
            }
        }*/
    }

    public IEnumerator Glower(Transform clue)
    {
        SpriteRenderer rend = clue.GetComponent<SpriteRenderer>();
        Color normclue = rend.color;
        Color glowclue = new Color(normclue.r, normclue.g, normclue.b, 1f);
        float elapsedTime = 0f;
        yield return _renderChangeTime;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            rend.color = Color.Lerp(normclue, glowclue, elapsedTime / fadeInDuration);
            yield return null;
        }
    }

    public void Faderr(Transform clue)
    {
        SpriteRenderer rend = clue.GetComponent<SpriteRenderer>();
        Color normclue = rend.color;
        Color unglowclue = new Color(normclue.r, normclue.g, normclue.b, 0f);
        rend.color = unglowclue;
    }
    public IEnumerator Fader(Transform clue)
    {
        SpriteRenderer rend = clue.GetComponent<SpriteRenderer>();
        Color normclue = rend.color;
        Color glowclue = new Color(normclue.r, normclue.g, normclue.b, 0f);
        float elapsedTime = 0f;
        //yield return _renderChangeTime;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            rend.color = Color.Lerp(normclue, glowclue, elapsedTime / fadeOutDuration);
            yield return null;
        }
    }
}
