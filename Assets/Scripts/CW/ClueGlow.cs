using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGlow : MonoBehaviour
{
    [SerializeField] private string clueTile = "clueTile";
    [SerializeField] private float _revealTime = 3f;
    [SerializeField] private LayerMask clueTrailLayerMask;

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

    public void ClueSpot(Vector3 currPos, float sphereCastRadius, Vector3 currDir, Light light, int excludePlayerLayerMask, PlayerMovement playerMovement)
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
            currDir, light.range, clueTrailLayerMask.value);

        foreach (var hit in sphereHitsClue)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(playerMovement.transform.position, (hit.transform.position - playerMovement.transform.position).normalized,
                out hitInfo, light.range, excludePlayerLayerMask))
            {
                if (hitInfo.transform.CompareTag(clueTile))
                {
                    Vector3 hitDir = new Vector3(hit.transform.position.x - currPos.x, 0, hit.transform.position.z - currPos.z);
                    float cosTheta = Vector3.Dot(currDir.normalized, hitDir.normalized);
                    float deg = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
                    if (Mathf.Abs(deg) <= light.spotAngle / 2.0f)
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
                    // start this coroutine
                    Coroutine a = StartCoroutine(Glower(clue));
                    activeCoroutines.Add(clue, a);
                }
            }

            foreach (var clue in prevClue)
            {
                // check if this clue is in uniqueClue
                if (!uniqueClue.Contains(clue))
                {
                    // stop this coroutine
                    StopCoroutine(activeCoroutines[clue]);
                    activeCoroutines.Remove(clue);
                    Faderr(clue);
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
        float fadeDuration = 1f;
        SpriteRenderer rend = clue.GetComponent<SpriteRenderer>();
        Color normclue = rend.color;
        Color glowclue = new Color(normclue.r, normclue.g, normclue.b, 1f);
        float elapsedTime = 0f;
        yield return _renderChangeTime;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            rend.color = Color.Lerp(normclue, glowclue, elapsedTime / fadeDuration);
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
        float fadeDuration = 1f;
        SpriteRenderer rend = clue.GetComponent<SpriteRenderer>();
        Color normclue = rend.color;
        Color glowclue = new Color(normclue.r, normclue.g, normclue.b, 0f);
        float elapsedTime = 0f;
        yield return _renderChangeTime;

        while (elapsedTime < fadeDuration)
        {
            Debug.Log(elapsedTime);
            elapsedTime += Time.deltaTime;
            rend.color = Color.Lerp(normclue, glowclue, elapsedTime / fadeDuration);
            yield return null;
        }
    }
}
