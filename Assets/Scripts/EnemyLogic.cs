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
    [SerializeField] private int damage = 0;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject OverlayTileContainer;
    [SerializeField] private OverlayTile characterTile;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private AudioClip attackClip;
    private AudioSource audioSource;
    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private List<OverlayTile> path;
    private List<OverlayTile> rangeFinderTiles;
    Animator animator;
    public OverlayTile enemyTile;
    public bool isMoving = false;
    public bool fireDamage = false;
    public bool inPlayerRange = false;
    public bool movedTwice = false;
    public bool movedThrice = false;
    public bool reachedPlayer = false;
    public bool isEnemyTurn = false;
    public bool hasAttacked = false;
    public float speed;
    public GameObject player;
    private Vector3 target;
   

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        hitPoints = startingHitPoints;
    }

    IEnumerator MoveToward(Vector3 target)
    {
        if (target != null)
        {
            // Note: Framerate dependent for MacOS/Windows. Use 0.7f on Windows and 1f on MacOS.
            yield return new WaitForSeconds(0.7f);
            isMoving = false;
            Debug.Log(this.name + " finished a move");
            if (movementSpeed >= 2 && movedTwice == false)
            {
                movedTwice = true;
                Move();
                Debug.Log(this.name + " finished move 2");
            }
            else if (movementSpeed >= 3 && movedTwice == true && movedThrice == false)
            {
                movedThrice = true;
                Move();
                Debug.Log(this.name + " finished move 3");
            }
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

    IEnumerator PlaySFX()
    {
        yield return new WaitForSeconds(0.8f);
        audioSource.PlayOneShot(attackClip);
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

        //if (other.CompareTag("Enemy"))
        //{
        //    isMoving = false;
        //}

        if (other.CompareTag("Player"))
        {
            Debug.Log("Reached Player");
            isMoving = false;
            reachedPlayer = true;
            Attack();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exited Player");
            reachedPlayer = false;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            // Note: Framerate dependent for MacOS/Windows. Use 0.004f on Windows and 0.008f on MacOS.
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, 0.004f);
        }
    }

    public void Move()
    {
        if (!reachedPlayer)
        {
            Debug.Log(this.name + " begins move");
            isMoving = true;
            target = SetAngle();
            StartCoroutine(MoveToward(target));
        }
    }

    private Vector3 SetAngle()
    {
        //Debug.Log("Set Angle of " + this.name);
        player = GameObject.Find("hamilton(Clone)");
        float angleToPlayer = Mathf.Atan2(player.transform.position.y - this.transform.position.y, player.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        //Debug.Log(this.name + ": Angle: " + angleToPlayer);

        Debug.DrawLine(this.transform.position, player.transform.position, Color.red, 1);

        Vector3 target = this.transform.position;

        if (angleToPlayer >= -135 && angleToPlayer < -45)
        {
            target.y -= 1;
        }
        else if (angleToPlayer >= 135 || angleToPlayer < -135)
        {
            target.x -= 1;
            FlipDirection(-1);
        }
        else if ((angleToPlayer < 45 && angleToPlayer >= 0) || (angleToPlayer > -45 && angleToPlayer <= 0))
        {
            target.x += 1;
            FlipDirection(1);
        }
        else if (angleToPlayer >= 45 && angleToPlayer < 135)
        {
            target.y += 1;
        }

        return target;
    }

    private void Attack()
    {
        if (isEnemyTurn == true && !hasAttacked)
        {
            Debug.Log("Attack");
            StartCoroutine(PlaySFX());
            animator.SetTrigger("Attack");
            player = GameObject.Find("hamilton(Clone)");
            player.GetComponent<CharacterInfo>().TakeHit(damage);
            hasAttacked = true;
            
        }
    }

    public void FlipDirection(int direction)
    {
        if (this.name == "mi-go(Clone)" || this.name == "yith(Clone)" || this.name == "ghoul(Clone)")
        {
            if (direction == -1)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (direction == 1)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            if (direction == -1)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (direction == 1)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}
