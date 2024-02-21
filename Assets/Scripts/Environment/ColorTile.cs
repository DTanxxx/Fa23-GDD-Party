using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

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
    [SerializeField] private string tileFlatLayer;
    [SerializeField] private string tileRaisedLayer;
    [SerializeField] private Transform leftSide = null;
    [SerializeField] private Transform rightSide = null;
    [SerializeField] private Transform topSide = null;
    [SerializeField] private Transform botSide = null;

    private ColorTileManager tileManager;
    private Collider _collider;
    private SpriteRenderer[] spriteRenderers;
    private bool offTile = false;
    private bool onTile = false;

    public static Action onIncinerate;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        offTile = false;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void SetColor(TileColor c, ColorTileManager manager)
    {
        tileColor = c;
        tileManager = manager;

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

    private void OnTriggerEnter(Collider collision)
    { 
        GameObject player = collision.transform.parent.parent.gameObject;
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (!player.gameObject.CompareTag("Player") || health.GetIsPlayerDead())
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
                health.SetIsPlayerDead();
                onIncinerate?.Invoke();
                break;

            case TileColor.Cyan:
                Debug.Log("cyan");
                tileManager.ActivateCyan();
                break;

            case TileColor.Green:
                tileManager.ActivateGreen();
                break;

            case TileColor.Blue:
                StartCoroutine(Mover(player, enter));
                break;
            case TileColor.Magenta:
                tileManager.ActivateMagenta(gameObject);
                break;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (tileColor == TileColor.Yellow)
        {
            StartCoroutine(Mover(gameObject, "raise"));
        }
        else if (tileColor == TileColor.Black)
        {
            tileManager.ActivateBlack();
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
        string direction;

        if (Mathf.Abs(dir.x) >= Mathf.Abs(dir.z))
        {
            // player entered from left or right
            if (dir.x < 0)
            {
                // left side
                direction = "left";
            }
            else
            {
                // right side
                direction = "right";
            }
        }
        else
        {
            // player entered from top or bottom
            if (dir.z < 0)
            {
                // bottom side
                direction = "bottom";
            }
            else
            {
                // top side
                direction = "top";
            }
        }
        
        return direction;
    }

    //Move Player or Raise+lower Tile
    public IEnumerator Mover(GameObject go, string state)
    {
        // the only case where go is the player game object is when we step on blue tile
        PlayerMovement pm;
        if (go.TryGetComponent<PlayerMovement>(out pm))
        {
            // make player immobile
            pm.Immobile(true);
        }

        var endPos = go.transform.position;

        switch (state)
        {
            case ("right"):
                Debug.Log("ENTERED FROM RIGHT");
                endPos = new Vector3(leftSide.position.x, go.transform.position.y, go.transform.position.z);
                break;
            case ("left"):
                Debug.Log("ENTERED FROM LEFT");
                endPos = new Vector3(rightSide.position.x, go.transform.position.y, go.transform.position.z);
                break;
            case ("top"):
                Debug.Log("ENTERED FROM TOP");
                endPos = new Vector3(go.transform.position.x, go.transform.position.y, botSide.position.z);
                break;
            case ("bottom"):
                Debug.Log("ENTERED FROM BOTTOM");
                endPos = new Vector3(go.transform.position.x, go.transform.position.y, topSide.position.z);
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

        if (state == "raise")
        {
            // set top surface and sides of tile's layer to tileRaised
            foreach (SpriteRenderer rend in spriteRenderers)
            {
                rend.sortingLayerName = tileRaisedLayer;
            }
        }
        else if (state == "lower")
        {
            // set top surface and sides of tile's layer to tileFlat
            foreach (SpriteRenderer rend in spriteRenderers)
            {
                rend.sortingLayerName = tileFlatLayer;
            }
        }
        
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
            // directly update player position
            go.transform.position = Vector3.Lerp(currentPos, endPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        go.transform.position = endPos;
        offTile = false;
        onTile = false;

        yield return null;

        if (pm != null)
        {
            // mobilise player
            pm.Immobile(false);
        }
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
