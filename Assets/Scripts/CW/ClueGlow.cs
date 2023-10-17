using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class ClueGlow : MonoBehaviour
{
    [SerializeField] private string clueTile = "clueTile";
    [SerializeField] public Sprite clueGlow;
    [SerializeField] public Sprite clueNorm;
    [SerializeField] public float _revealTime = 3f;

    [SerializeField] public LightDirection flashlight;

    private RaycastHit[] sphereHitsClue;
    private HashSet<Transform> uniqueClue;
    private HashSet<Transform> prevClue;
    private List<Coroutine> activeCoroutines;


    private WaitForSecondsRealtime _renderChangeTime;

    private bool faceClue;

    private void Start()
    {
        _renderChangeTime = new WaitForSecondsRealtime(_revealTime);
        prevClue = new HashSet<Transform>();
        activeCoroutines = new List<Coroutine>();
    }
    public void clueSpot(Vector3 currPos, float sphereCastRadius, Vector3 currDir, Light light, int layer, PlayerMovement playermovement)
    {
        uniqueClue = new HashSet<Transform>();
        faceClue = false;

        //if (faceClue && _selection == null)
        //{
        //    //need code to stop glow and fade
        //    //this is called even when facing
        //    Debug.Log('p');
        //    Debug.Log(faceClue);
        //}
        
        sphereHitsClue = Physics.SphereCastAll(transform.position, sphereCastRadius,
            currDir, light.range, layer, QueryTriggerInteraction.Ignore);

        foreach (var hit in sphereHitsClue)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(playermovement.transform.position, (hit.transform.position - playermovement.transform.position).normalized,
                out hitInfo, light.range, layer, QueryTriggerInteraction.Ignore))
            {
                Vector3 hitDir = new Vector3(hit.transform.position.x - transform.position.x, 0, hit.transform.position.z - transform.position.z);
                float cosTheta = Vector3.Dot(currDir.normalized, hitDir.normalized);
                float deg = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
                if (Mathf.Abs(deg) <= light.spotAngle / 2.0f)
                {
                    var clue = hitInfo.transform;
                    if (clue.CompareTag(clueTile))
                    {
                        faceClue = true;
                        if (!uniqueClue.Contains(clue))
                        {
                            uniqueClue.Add(clue);
                        }
                    }
                }
            }
        }

        if (faceClue)
        {
            Debug.Log("clue");           

            if (!prevClue.SetEquals(uniqueClue))
            {
                Debug.Log("stoped");
                //foreach (var clue in prevClue)
                //{
                //    StopCoroutine(glower(clue));
                //    faderr(clue);
                //}

                foreach (var clue in uniqueClue)
                {
                    Coroutine a = StartCoroutine(glower(clue));
                    activeCoroutines.Add(a);
                }
            }
            prevClue = uniqueClue;
        }
        else
        {
            Debug.Log("no clue");
            
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
                    StopCoroutine(glower(clue));
                    faderr(clue);
                }

                prevClue = new HashSet<Transform>();
            }
        }
    }

    public IEnumerator glower(Transform clue)
    {
        float fadeDuration = 1f;
        SpriteRenderer rend = clue.GetComponent<SpriteRenderer>();
        Color normclue = rend.color;
        Color glowclue = new Color(normclue.r, normclue.g, normclue.b, 1f);
        float elapsedTime = 0f;
        yield return _renderChangeTime;

        while (elapsedTime < fadeDuration)
        {
            //Debug.Log(elapsedTime);
            elapsedTime += Time.deltaTime;
            rend.color = Color.Lerp(normclue, glowclue, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    public void faderr(Transform clue)
    {
        SpriteRenderer rend = clue.GetComponent<SpriteRenderer>();
        Color normclue = rend.color;
        Color unglowclue = new Color(normclue.r, normclue.g, normclue.b, 0f);
        rend.color = unglowclue;
    }
    public IEnumerator fader(Transform clue)
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
