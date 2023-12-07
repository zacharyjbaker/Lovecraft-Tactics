using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ArrowTranslator;
using UnityEngine.TextCore.Text;
using Unity.Burst.CompilerServices;
using System.Linq;
using System.Security.Cryptography;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float startingHitPoints = 0f;
    [SerializeField] private float hitPoints = 0f;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject OverlayTileContainer;
    [SerializeField] private OverlayTile characterTile;
    [SerializeField] private TurnManager turnManager;
    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private List<OverlayTile> path;
    private List<OverlayTile> rangeFinderTiles;
    Animator animator;
    public OverlayTile enemyTile;
    private bool isMoving;
    public bool fireDamage = false;
    public float speed;
   

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hitPoints = startingHitPoints;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }

    IEnumerator Shake(int length)
    {
        for (int i = 0; i < length; i++)
        {
            transform.localPosition += new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
            transform.localPosition -= new Vector3(0.06f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void TakeDamage(int dmgValue)
    {
        hitPoints -= dmgValue;
        Debug.Log("Remaining HP" + hitPoints);

        if (hitPoints <= (startingHitPoints * 0.25f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f); }
        else if (hitPoints <= (startingHitPoints * 0.50f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.65f, 0.65f, 1f); }
        else if (hitPoints <= (startingHitPoints * 0.75f)) { this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f, 1f); }

        StartCoroutine(Shake(12));
        //this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

        if (hitPoints <= 0f)
        {
            StartCoroutine(Shake(36));
            Destroy(gameObject, 0.8f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        if (other.CompareTag("Fire") && fireDamage == false)
        {
            Debug.Log("Fire Hit");
            TakeDamage(1);
            fireDamage = true;
        }

    }

    // Update is called once per frame
    /*
    void LateUpdate()
    {
        //var enemyTrigger = Input.GetKey(KeyCode.H);
        if (turnManager.isEnemyTurn)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);

            if (hit.HasValue)
            {
                OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
                character = GameObject.Find("hamilton(Clone)");
            }
            characterTile = character.GetComponent<CharacterInfo>().getCharTile();
            enemyTile = OverlayTileContainer.transform.GetChild(8).GetComponent<OverlayTile>();
            MoveTowardPlayer();
        }
    }
    private void PositionEnemyOnLine(OverlayTile tile)
    {
        this.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        this.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        enemyTile = tile;
    }

    public void MoveTowardPlayer()
    {
        if (rangeFinderTiles.Contains(characterTile))
        {
            path = pathFinder.FindPath(enemyTile, characterTile, rangeFinderTiles);
        }

        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        Vector3 temp = this.transform.position;
        this.transform.position = Vector2.MoveTowards(this.transform.position, path[0].transform.position, step);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, zIndex);

        if (temp.x < path[0].transform.position.x)
        {
            this.GetComponent<PlayerMove>().FlipDirection(1);
        }
        else if (temp.x > path[0].transform.position.x)
        {
            this.GetComponent<PlayerMove>().FlipDirection(-1);
        }
        else
        {
            this.GetComponent<PlayerMove>().FlipDirection(0);
        }

        // When character reaches destination
        if (Vector2.Distance(this.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionEnemyOnLine(path[0]);
            path.RemoveAt(0); // delete path
        }

        if (path.Count == 0)
        {
            GetInRangeTiles();
            isMoving = false;
        }
    }

    public void GetInRangeTiles()
    {
        //Debug.Log(turnManager.remainingMovePts);
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(enemyTile.gridLocation.x, enemyTile.gridLocation.y), 3);

        foreach (var item in rangeFinderTiles)
        {

            Debug.Log("Reveal Tile");
            item.ShowTile();
        }
    }
    */
}
