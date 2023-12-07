using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using static ArrowTranslator;
using System.Collections;
using JetBrains.Annotations;
using Unity.Burst.CompilerServices;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    public float speed;
    public GameObject characterPrefab;
    private CharacterInfo character;
    public GameObject firePrefab;
    [SerializeField] public Sprite x1Cursor;
    [SerializeField] public Sprite x3Cursor;
    [SerializeField] private int gunRange = 0;
    [SerializeField] GameObject EnemyList;
    //[SerializeField] private LayerMask test;

    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    [SerializeField] private TurnManager turnManager;
    private ArrowTranslator arrowTranslator;
    private List<OverlayTile> path;
    private List<OverlayTile> rangeFinderTiles;
    private bool isMoving;
    private bool isShot;

    private void Start()
    {
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
        arrowTranslator = new ArrowTranslator();

        path = new List<OverlayTile>();
        isMoving = false;
        rangeFinderTiles = new List<OverlayTile>();
    }

    IEnumerator DealDamage(float delay, GameObject target)
    {
        yield return new WaitForSeconds(delay);
        target.GetComponent<EnemyLogic>().TakeDamage(1);
        print("hit");
    }

    IEnumerator ResetShotState()
    {
        yield return new WaitForSeconds(3);
        isShot = false;
    }

    void LateUpdate()
    {
        // Movement selection
        if ((turnManager.isPlayerTurn || turnManager.isStart) && !turnManager.isAbilitySelected && !turnManager.menu)
        {
            cursor.GetComponent<SpriteRenderer>().sprite = x1Cursor;
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = tile.transform.position;
                cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

                if (rangeFinderTiles.Contains(tile) && !isMoving)
                {
                    path = pathFinder.FindPath(character.standingOnTile, tile, rangeFinderTiles);

                    foreach (var item in rangeFinderTiles)
                    {
                        MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowDirection.None);
                    }

                    for (int i = 0; i < path.Count; i++)
                    {
                        var previousTile = i > 0 ? path[i - 1] : character.standingOnTile;
                        var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                        var arrow = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                        path[i].SetSprite(arrow);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    tile.ShowTile();

                    if (character == null)
                    {
                        character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                        PositionCharacterOnLine(tile);
                        GetInRangeTiles();
                    }
                    else
                    {
                        isMoving = true;
                        tile.gameObject.GetComponent<OverlayTile>().HideTile();
                    }
                }
            }

            if (path.Count > 0 && isMoving)
            {
                MoveAlongPath();
                HideAllArrows();
            }
        }

        // Shooting selection
        else if (turnManager.isPlayerTurn && turnManager.isAbility1Selected && !turnManager.menu)
        {
            cursor.GetComponent<SpriteRenderer>().sprite = x1Cursor;
            if (isShot || turnManager.outOfAmmo)
            {
                HideInRangeTilesShooting();
            }
            else
            {
                GetInRangeTilesShooting();
            }
            //Debug.Log(isShot);
            //Debug.Log("Ability");
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = tile.transform.position;
                cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

                if (rangeFinderTiles.Contains(tile))
                {
                    path = pathFinder.FindPath(character.standingOnTile, tile, rangeFinderTiles);
                }

                if (Input.GetMouseButtonDown(0) && turnManager.outOfAmmo)
                {
                    Debug.Log("Out of shots for this turn!");
                }
                else if (Input.GetMouseButtonDown(0) && !turnManager.outOfAmmo)
                {
                    isShot = true;
                    HideInRangeTilesShooting();
                    turnManager.GetComponent<TurnManager>().UseBullet();
                    //tile.ShowTile();
                    character.GetComponent<PlayerMove>().ShootAnimation();
                    //tile.gameObject.GetComponent<OverlayTile>().HideTile();

                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                    //RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

                    RaycastHit2D shotHit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                    if (shotHit.collider.gameObject.tag == "Enemy")
                    {
                        StartCoroutine(DealDamage(2f, shotHit.collider.gameObject));
                    }

                    StartCoroutine(ResetShotState());
                }
            }
        }

        // Molotov selection
        else if (turnManager.isPlayerTurn && turnManager.isAbility2Selected && !turnManager.menu)
        {
            //Debug.Log("Molotov");
            cursor.GetComponent<SpriteRenderer>().sprite = x3Cursor;
            GetInRangeTilesShooting();
            RaycastHit2D? hit = GetFocusedOnTile();
            if (hit.HasValue)
            {
                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                cursor.transform.position = tile.transform.position;
                cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;
         
                if (Input.GetMouseButtonDown(0) && !turnManager.outOfMolotov)
                {
                    turnManager.GetComponent<TurnManager>().UseMolotov();
                    Debug.Log("Spawn Fire");
                    GameObject fire = Instantiate(firePrefab, tile.transform.position, tile.transform.rotation, EnemyList.transform);
                    fire.GetComponent<SpriteRenderer>().sortingOrder = 7;
                }
            }
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        Vector3 temp = character.transform.position;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (temp.x < path[0].transform.position.x)
        {
            character.GetComponent<PlayerMove>().FlipDirection(1);
        }
        else if (temp.x > path[0].transform.position.x)
        {
            character.GetComponent<PlayerMove>().FlipDirection(-1);
        }
        else
        {
            character.GetComponent<PlayerMove>().FlipDirection(0);
        }

        // When character reaches destination
        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0); // delete path

        }

        if (path.Count == 0)
        {
            GetInRangeTiles();
            isMoving = false;
        }
    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.standingOnTile = tile;
        turnManager.SubMovePoints(1);
        //print("RemainMove: " + turnManager.SubMovePoints(1));
    }

    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    public void GetInRangeTiles()
    {
        //Debug.Log(turnManager.remainingMovePts);
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), turnManager.remainingMovePts);

        if (turnManager.isPlayerTurn || turnManager.isStart)
        {
            foreach (var item in rangeFinderTiles)
            {
                //Debug.Log("Reveal Tile");
                item.ShowTile();
            }
        }
    }
     
    public void GetInRangeTilesShooting()
    {
        //Debug.Log("Show Shots");
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), gunRange);

        if (turnManager.isPlayerTurn || turnManager.isStart)
        {
            foreach (var item in rangeFinderTiles)
            {
                //Debug.Log("Reveal Tile");
                item.ShowTileShooting();
            }
        }
    }

    public void HideInRangeTiles()
    {
        //Debug.Log(turnManager.remainingMovePts);
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), turnManager.remainingMovePts);

        if ((turnManager.isEnemyTurn || turnManager.isStart) && !turnManager.isAbilitySelected)
        {
            foreach (var item in rangeFinderTiles)
            {
                item.HideTile();
            }
        }
    }

    public void HideInRangeTilesShooting()
    {
        //Debug.Log("Hide Shots");
        //Debug.Log(turnManager.remainingMovePts);
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), gunRange);

        if (turnManager.isPlayerTurn || turnManager.isStart)
        {
            foreach (var item in rangeFinderTiles)
            {
                item.HideTile();
            }
        }
    }

    public void HideAllArrows()
    {
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), turnManager.remainingMovePts);

        if ((turnManager.isPlayerTurn || turnManager.isStart) && !turnManager.isAbilitySelected)
        {
            foreach (var item in rangeFinderTiles)
            {
                item.HideArrow();
            }
        }
    }
}