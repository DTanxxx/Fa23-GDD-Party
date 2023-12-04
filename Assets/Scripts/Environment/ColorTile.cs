using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TileColor
{
    White, //Nothing
    Red, //Deadly
    Cyan, //Swap red+white
    Black, //up+down, step to change state
    Green, //Lower all green
    Blue, //Slide through
    Yellow, //Lift only this tile
    Magenta, //Swap red+white in row and column
    Gray, //Wall
    LightGray, //Normal Ground
    Purple //Something else that isn't part of color puzzle
}
[RequireComponent(typeof(Collider))]
public class ColorTile : MonoBehaviour
{
    [SerializeField] private TileColor tileColor;
    [SerializeField] private Sprite whiteSprite;
    [SerializeField] private Sprite redSprite;
    [SerializeField] private Sprite cyanSprite;
    [SerializeField] private Sprite blackSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite yellowSprite;
    [SerializeField] private Sprite magentaSprite;
    [SerializeField] private Sprite purpleSprite;

    private ColorTileManager tileManager;
    private Collider _collider;
    private SpriteRenderer[] spriteRenderers;
    private bool offTile = false;
    private bool onTile = false;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        offTile = false;
        tileManager = FindObjectOfType<ColorTileManager>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        switch (tileColor)
        {
            case TileColor.White:
                SetSprite(whiteSprite);
                break;
            case TileColor.Red: 
                SetSprite(redSprite);
                break;
            case TileColor.Cyan:
                SetSprite(cyanSprite);
                break;
            case TileColor.Black:
                SetSprite(blackSprite);
                break;
            case TileColor.Green:
                SetSprite(greenSprite);
                break;
            case TileColor.Blue:
                SetSprite(blueSprite);
                break;
            case TileColor.Yellow:
                SetSprite(yellowSprite);
                break;
            case TileColor.Magenta:
                SetSprite(magentaSprite);
                break;
            case TileColor.Purple:
                SetSprite(purpleSprite);
                break;
        }
    }

    public void SetColor(TileColor c)
    {
        tileColor = c;
    }

    private void OnTriggerEnter(Collider collision)
    { 

        GameObject player = collision.transform.parent.parent.gameObject;
        PlayerMovement pm = player.GetComponent<PlayerMovement>();

        if (!player.gameObject.CompareTag("Player"))
        {
            return;
        }


        Animator animator = player.GetComponentInChildren<Animator>();
        string enter = EnterDirection(collision);

        switch (tileColor)
        {
            case TileColor.White:
                Debug.Log("white");
                Debug.Log(collision.GetInstanceID());
                break;

            case TileColor.Red:
                Debug.Log("red");
                player.GetComponent<PlayerMovement>().DeathViaRed();
                break;

            case TileColor.Cyan:
                Debug.Log("cyan");
                tileManager.ActivateCyan();
                break;

            case TileColor.Black:
                tileManager.ActivateBlack();
                break;

            case TileColor.Green:
                tileManager.ActivateGreen();
                break;

            case TileColor.Blue:
                pm.Immobile(true);
                StartCoroutine(Mover(player, enter));
                pm.Immobile(false);
                break;

            case TileColor.Yellow:
                StartCoroutine(Mover(gameObject, "raise"));
                break;

            case TileColor.Magenta:
                tileManager.ActivateMagenta(gameObject);
                break;
        }
    }
    public TileColor GetTileColor() { return tileColor; }

    private void SetSprite(Sprite sp)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sprite = sp;
        }
        Debug.Log(sp);
    }

    //Cyan Tile
    public void TurnRed()
    {
        tileColor = TileColor.Red;
        SetSprite(redSprite);
        Debug.Log(tileColor);
    }
    public void TurnWhite()
    {
        tileColor = TileColor.White;
        SetSprite(whiteSprite);
        Debug.Log(tileColor);
    }

    //Blue Tile
    private string EnterDirection(Collider collision)
    {
        var dir = collision.transform.position - transform.position;
        var frw = transform.TransformDirection(Vector3.forward);
        var ri = transform.TransformDirection(Vector3.right);

        string direction;

        if (Vector3.Dot(frw, dir) <= 2.1)
        {
            direction = "bottom";
            //Debug.Log("bottom");
        }
        else if (Vector3.Dot(frw, dir) >= 4.6)
        {
            direction = "top";
            //Debug.Log("top");
        }
        else
        {
            if (Vector3.Dot(ri, dir) > 0)
            {
                direction = "right";
                //Debug.Log("right");
            }
            else
            {
                direction = "left";
                //Debug.Log("Left");
            }
        }
        return direction;
    }

    //Move Player or Raise+lower Tile
    public IEnumerator Mover(GameObject go, string state)
    {
        var endPos = go.transform.position;

        switch (state)
        {
            case ("right"):
                endPos = go.transform.position + Vector3.left * 8.94f;
                break;
            case ("left"):
                endPos = go.transform.position + Vector3.right * 8.94f;
                break;
            case ("top"):
                endPos = go.transform.position + Vector3.back * 8.94f;
                break;
            case ("bottom"):
                endPos = go.transform.position + Vector3.forward * 8.94f;
                break;

            case "lower":
                endPos = new Vector3(go.transform.position.x, 0.11f, go.transform.position.z);
                break;
            case "raise":
                endPos = new Vector3(go.transform.position.x, 2.5f, go.transform.position.z);
                break;
        }
        
        float elapsedTime = 0;
        float waitTime = 1.0f;

        var currentPos = go.transform.position;
        
        if (onTile)
        {
            if (state == "lower" | state == "raise")
            {
                Debug.Log(offTile);
                yield return new WaitUntil(() => offTile);
            }
        }

        while (elapsedTime < waitTime)
        {
            go.transform.position = Vector3.Lerp(currentPos, endPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        go.transform.position = endPos;
        offTile = false;
        onTile = false;

        yield return null;
    }

    //Check exit
    public void ExitTile()
    {
        offTile = true;
    }

    public void EnterTile()
    {
        onTile = true;
    }
}
