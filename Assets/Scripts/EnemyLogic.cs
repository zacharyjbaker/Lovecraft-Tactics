using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ArrowTranslator;
using UnityEngine.TextCore.Text;
using Unity.Burst.CompilerServices;
using System.Linq;
using System.Security.Cryptography;
using static UnityEngine.GraphicsBuffer;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float startingHitPoints = 0f;
    [SerializeField] private float hitPoints = 0f;
    [SerializeField] private float movementSpeed = 0f;
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
    public bool isMoving = false;
    public bool fireDamage = false;
    public bool inPlayerRange = false;
    public bool angleSet = false;
    public float speed;
    public GameObject player;
    private Vector3 target;
   

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        hitPoints = startingHitPoints;
    }

    IEnumerator MoveToward(Vector3 target)
    {
        if (target != null)
        {
            yield return new WaitForSeconds(1f);
            isMoving = false;
        }
        
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
        //Debug.Log("Collision");
        if (other.CompareTag("Fire") && fireDamage == false)
        {
            Debug.Log("Fire Hit");
            TakeDamage(1);
            fireDamage = true;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Reached Player");
            isMoving = false;
            Attack();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, 0.004f);
        }
    }

    public void Move()
    {
        isMoving = true;
        target = SetAngle();
        StartCoroutine(MoveToward(target));
    }

    private Vector3 SetAngle()
    {
        Debug.Log("Set Angle of " + this.name);
        player = GameObject.Find("hamilton(Clone)");
        float angleToPlayer = Mathf.Atan2(player.transform.position.y - this.transform.position.y, player.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        Debug.Log(this.name + ": Angle: " + angleToPlayer);

        Debug.DrawLine(this.transform.position, player.transform.position, Color.red, 1);

        Vector3 target = this.transform.position;

        if (angleToPlayer >= -135 && angleToPlayer < -45)
        {
            target.y -= 1;
        }
        else if (angleToPlayer >= 135 || angleToPlayer < -135)
        {
            target.x -= 1;
        }
        else if ((angleToPlayer < 45 && angleToPlayer >= 0) || (angleToPlayer > -45 && angleToPlayer <= 0))
        {
            target.x += 1;
        }
        else if (angleToPlayer >= 45 && angleToPlayer < 135)
        {
            target.y += 1;
        }

        return target;
    }

    private void Attack()
    {

    }
}
