using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lurkers.Control;  // TODO this dependency should be removed to prevent cyclic dependency
using Lurkers.Audio;  // TODO this dependency should be removed to prevent cyclic dependency, use C# event listened by TileAudioSources

namespace Lurkers.Environment.Vision
{
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
        [SerializeField] private Sprite whiteSideSprite;
        [SerializeField] private Sprite redSideSprite;
        [SerializeField] private Sprite cyanSideSprite;
        [SerializeField] private Sprite blackSideSprite;
        [SerializeField] private Sprite greenSideSprite;
        [SerializeField] private Sprite blueSideSprite;
        [SerializeField] private Sprite yellowSideSprite;
        [SerializeField] private Sprite magentaSideSprite;
        [SerializeField] private Sprite purpleSideSprite;
        [SerializeField] private string tileFlatLayer;
        [SerializeField] private string tileRaisedLayer;
        [SerializeField] private Transform leftSide = null;
        [SerializeField] private Transform rightSide = null;
        [SerializeField] private Transform topSide = null;
        [SerializeField] private Transform botSide = null;
        [SerializeField] private float blueTileSlideDuration = 0.5f;
        [SerializeField] private float tileRaiseDuration = 1.0f;
        [SerializeField] private SpriteRenderer[] topSpriteRends;
        [SerializeField] private SpriteRenderer[] sideSpriteRends;

        private SpriteRenderer[] allSpriteRends;
        private ColorTileManager tileManager;
        private Collider _collider;
        private bool offTile = false;
        private bool onTile = false;
        private Coroutine slideCoroutine;
        private bool isRaised = false;
        private bool playerColliding = false;
        private bool hasTriggered = false;
        private Collider savedEnterCollider;
        private GameObject playerObject;
        private bool checker = false;
        
        private enum TileActivation
        {
            MOVE = 0,
        }

        public static Action onIncinerate;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            allSpriteRends = GetComponentsInChildren<SpriteRenderer>();
            _collider.isTrigger = true;
            offTile = false;
        }

        public void SetData(TileColor c, ColorTileManager manager, bool raised)
        {
            tileColor = c;
            tileManager = manager;
            isRaised = raised;

            if (isRaised)
            {
                // set top surface and sides of tile's layer to tileRaised
                foreach (SpriteRenderer rend in allSpriteRends)
                {
                    rend.sortingLayerName = tileRaisedLayer;
                }
            }
            else
            {
                // set top surface and sides of tile's layer to tileFlat
                foreach (SpriteRenderer rend in allSpriteRends)
                {
                    rend.sortingLayerName = tileFlatLayer;
                }
            }

            switch (tileColor)
            {
                case TileColor.White:
                    SetSprite(whiteSprite, whiteSideSprite);
                    break;
                case TileColor.Red:
                    SetSprite(redSprite, redSideSprite);
                    break;
                case TileColor.Cyan:
                    SetSprite(cyanSprite, cyanSideSprite);
                    break;
                case TileColor.Black:
                    SetSprite(blackSprite, blackSideSprite);
                    break;
                case TileColor.Green:
                    SetSprite(greenSprite, greenSideSprite);
                    break;
                case TileColor.Blue:
                    SetSprite(blueSprite, blueSideSprite);
                    break;
                case TileColor.Yellow:
                    SetSprite(yellowSprite, yellowSideSprite);
                    break;
                case TileColor.Magenta:
                    SetSprite(magentaSprite, magentaSideSprite);
                    break;
                case TileColor.Purple:
                    SetSprite(purpleSprite, purpleSideSprite);
                    break;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            // TODO remove all instances of Lurkers.Control classes, and use C# event instead
            playerObject = collision.transform.parent.parent.gameObject;
            PlayerController pm = playerObject.GetComponent<PlayerController>();
            PlayerHealth health = playerObject.GetComponent<PlayerHealth>();

            if (!playerObject.gameObject.CompareTag("Player") || health.GetIsPlayerDead())
            {
                return;
            }
            
            savedEnterCollider = collision;
            checker = false;
            
        }

        void Update() 
        {
            if (playerObject != null)
            {
                PlayerController pm = playerObject.GetComponent<PlayerController>();
                if (!checker && (pm.tileTriggerCounter == 0) )
                {
                    runTile(playerObject, savedEnterCollider);
                    checker = true;
                    pm.tileTriggerCounter += 1;
                }
            }
            
            
        }

        private void runTile(GameObject player, Collider collision)
        {
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            Animator animator = player.GetComponentInChildren<Animator>();
            string enter = EnterDirection(collision);

            switch (tileColor)
            {
                case TileColor.White:
                    break;

                case TileColor.Red:
                    health.SetIsPlayerDead();
                    onIncinerate?.Invoke();
                    break;

                case TileColor.Cyan:
                    tileManager.ActivateCyan();
                    break;

                case TileColor.Green:
                    if (tileManager.IsAllGreenLowered())
                    {
                        return;
                    }
                    AudioManager.instance.SetPlayOneShot(FMODEvents.instance.tileActivation, transform, "TileActivation", (float)TileActivation.MOVE);
                    tileManager.ActivateGreen();
                    break;

                case TileColor.Blue:
                    tileManager.ResetBlueTileCoroutines();
                    RestorePlayerState(player);
                    slideCoroutine = StartCoroutine(Mover(player, enter));
                    break;
                case TileColor.Magenta:
                    tileManager.ActivateMagenta(gameObject);
                    break;
            }
        }


        private void OnTriggerExit(Collider collision)
        {
            GameObject player = collision.transform.parent.parent.gameObject;

            if (!player.gameObject.CompareTag("Player")) 
            { 
                return;
            }
            PlayerController pm = player.GetComponent<PlayerController>();
            pm.tileTriggerCounter = 0;
            

            if (tileColor == TileColor.Yellow)
            {
                // TODO remove all instances of AudioManager... and fire a C# event instead
                AudioManager.instance.SetPlayOneShot(FMODEvents.instance.tileActivation, transform, "TileActivation", (float)TileActivation.MOVE);
                StartCoroutine(Mover(gameObject, "raise"));
            }
            else if (tileColor == TileColor.Black)
            {
                AudioManager.instance.SetPlayOneShot(FMODEvents.instance.tileActivation, transform, "TileActivation", (float)TileActivation.MOVE);
                tileManager.ActivateBlack();
            }
        }

        public TileColor GetTileColor() { return tileColor; }

        private void SetSprite(Sprite sp, Sprite sideSp)
        {
            foreach (SpriteRenderer spriteRenderer in topSpriteRends)
            {
                spriteRenderer.sprite = sp;
            }

            foreach (SpriteRenderer spriteRenderer in sideSpriteRends)
            {
                spriteRenderer.sprite = sideSp;
            }
        }

        //Cyan Tile
        public void TurnRed()
        {
            TurnColor(TileColor.Red);
        }

        public void TurnWhite()
        {
            TurnColor(TileColor.White);
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

        private void RestorePlayerState(GameObject player)
        {
            offTile = false;
            onTile = false;
            //player.GetComponent<PlayerController>().Immobile(false);
        }

        private void SetSprite(Sprite sp)
        {
            foreach (SpriteRenderer spriteRenderer in allSpriteRends)
            {
                spriteRenderer.sprite = sp;
            }
        }
       
        public bool GetIsRaised()
        {
            return isRaised;
        }

        //Move Player or Raise+lower Tile
        public IEnumerator Mover(GameObject go, string state)
        {
            // the only case where go is the player game object is when we step on blue tile
            PlayerController controller;
            if (go.TryGetComponent<PlayerController>(out controller))
            {
                // make player immobile
                controller.Immobile(true);
            }

            var endPos = go.transform.position;

            switch (state)
            {
                case ("right"):
                    endPos = new Vector3(leftSide.position.x, go.transform.position.y, go.transform.position.z);
                    break;
                case ("left"):
                    endPos = new Vector3(rightSide.position.x, go.transform.position.y, go.transform.position.z);
                    break;
                case ("top"):
                    endPos = new Vector3(go.transform.position.x, go.transform.position.y, botSide.position.z);
                    break;
                case ("bottom"):
                    endPos = new Vector3(go.transform.position.x, go.transform.position.y, topSide.position.z);
                    break;

                case "lower":
                    // check if it's already lowered
                    if (!isRaised)
                    {
                        yield break;
                    }
                    isRaised = false;
                    endPos = new Vector3(go.transform.position.x, 0.11f, go.transform.position.z);
                    break;
                case "raise":
                    // check if it's already raised
                    if (isRaised)
                    {
                        yield break;
                    }
                    isRaised = true;
                    endPos = new Vector3(go.transform.position.x, 2.5f, go.transform.position.z);
                    break;
            }

            float elapsedTime = 0;

            var currentPos = go.transform.position;

            if (state == "raise")
            {
                // set top surface and sides of tile's layer to tileRaised
                foreach (SpriteRenderer rend in allSpriteRends)
                {
                    rend.sortingLayerName = tileRaisedLayer;
                }

            }

            if (onTile)
            {
                if (state == "lower" | state == "raise")
                {
                    yield return new WaitUntil(() => offTile);
                }
            }

            if (controller != null)
            {
                // blue tile slide
                while (elapsedTime < blueTileSlideDuration)
                {
                    // directly update player position
                    go.transform.position = Vector3.Lerp(currentPos, endPos, (elapsedTime / blueTileSlideDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                // tile raise/lower
                while (elapsedTime < tileRaiseDuration)
                {
                    // directly update tile position
                    go.transform.position = Vector3.Lerp(currentPos, endPos, (elapsedTime / tileRaiseDuration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }

            if (state == "lower")
            {
                // set top surface and sides of tile's layer to tileFlat
                foreach (SpriteRenderer rend in allSpriteRends)
                {
                    rend.sortingLayerName = tileFlatLayer;
                }
            }

            go.transform.position = endPos;
            offTile = false;
            onTile = false;

            yield return null;

            if (controller != null)
            {
                // mobilise player
                controller.Immobile(false);
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

        public void StopSlideCoroutine()
        {
            if (slideCoroutine != null)
            {
                StopCoroutine(slideCoroutine);
                slideCoroutine = null;
            }
        }

        // Color changes
        public void TurnColor(TileColor newColor)
        {
            tileColor = newColor;
            switch (newColor)
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
    }
}
