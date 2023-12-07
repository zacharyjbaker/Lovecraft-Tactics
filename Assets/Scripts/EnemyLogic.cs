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

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Collision");
        if (other.CompareTag("Fire") && fireDamage == false)
        {
            Debug.Log("Fire Hit");
            TakeDamage(1);
            fireDamage = true;
        }

    }
}
