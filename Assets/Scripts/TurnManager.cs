using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class TurnManager : MonoBehaviour
{
    //public GameObject playerPrefab;
    //public GameObject enemyPrefab;

    [SerializeField] TMP_Text stateText;
    [SerializeField] TMP_Text shadowStateText;
    [SerializeField] TMP_Text movePointsText;
    [SerializeField] TMP_Text shadowMovePointsText;
    [SerializeField] MouseController mouseController;
    [SerializeField] GameObject UIContainer;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bullet1;
    [SerializeField] private GameObject OverlayTileContainer;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] spawners;
    [SerializeField] GameObject enemyList;
    [SerializeField] GameObject objectList;

    public BattleState state;
    private bool continueState;
    public bool isPlayerTurn = false;
    public bool isStart = true;
    public bool isEnemyTurn = false;
    public bool isAbilitySelected = false;
    public bool isAbility1Selected = false;
    public bool isAbility2Selected = false;
    public bool menu = false;
    public bool delay = false;
    public float decay = 1f;
    public int remainingMovePts = 5;
    public bool isFirstShot = true;
    public bool outOfAmmo = false;
    public bool outOfMolotov = false;
    private int rand;
    private List<int> tempRand = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        continueState = false;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        movePointsText.transform.position = new Vector3(6.63f, -4.7f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.82f, 0.0f);

        //Instantiate(enemyPrefab, this.transform);
        //enemyPrefab.GetComponent<EnemyLogic>().enemyTile = OverlayTileContainer.transform.GetChild(23).GetComponent<OverlayTile>();

        Debug.Log("START");

        SpawnHorde();
    }

    public void SpawnHorde()
    {
        for (int i = 0; i < (spawners.Length / 2); i++)
        {
            rand = Random.Range(0, spawners.Length);

            while (tempRand.Contains(rand))
            {
                rand = Random.Range(0, spawners.Length);
            }
            tempRand.Add(rand);
            Debug.Log("Activate Spawner: " + rand);
            spawners[rand].GetComponent<Spawner>().SpawnEnemy(Random.Range(0, 5), spawners[rand].transform);
        }
        tempRand.Clear();
    }

    IEnumerator SetupBattle()
    {
        //Instantiate(playerPrefab);

        //yield return new WaitForSeconds(2f);
        while (continueState == false)
        {
            //Debug.Log("while");
            if (GameObject.Find("hamilton(Clone)") != null)
            {
                continueState = true;
            }
            yield return null;
        }
        state = BattleState.PLAYERTURN;
        isStart = false;
        PlayerTurn();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }

    public void UseBullet()
    {
        if (isFirstShot) 
        {
            bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            isFirstShot = false;
        }
        else 
        { 
            bullet1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            outOfAmmo = true;
        }
    }

    public void UseMolotov()
    {
        outOfMolotov = true;
    }

    public int SubMovePoints(int subtraction)
    {
        remainingMovePts -= subtraction;
        return remainingMovePts;
    }

    private void PlayerTurn()
    {
        isPlayerTurn = true;
        
        remainingMovePts = 4;

        isAbilitySelected = false;
        isAbility1Selected = false;
        isAbility2Selected = false;
        isFirstShot = true;
        outOfAmmo = false;
        outOfMolotov = false;

        mouseController.HideInRangeTilesShooting();
        mouseController.GetInRangeTiles();

        movePointsText.transform.position = new Vector3(6.63f, -4.7f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.82f, 0.0f);
        stateText.SetText("Player Turn");
        shadowStateText.SetText("Player Turn");
        movePointsText.SetText("Move " + remainingMovePts + "/4");
        shadowMovePointsText.SetText("Move " + remainingMovePts + "/4");
    }

    private void EnemyTurn()
    {
        isEnemyTurn = true;
        menu = false;

        mouseController.HideInRangeTiles();
        mouseController.HideInRangeTilesShooting();

        stateText.SetText("Enemy Turn");
        shadowStateText.SetText("Enemy Turn");
        movePointsText.SetText("");
        shadowMovePointsText.SetText("");

        UIContainer.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(1, 1, 1, 0);
        UIContainer.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);

        foreach (Transform child in enemyList.transform)
        {
            if (child.gameObject != null && child.gameObject.tag != "Fire")
            {
                child.gameObject.GetComponent<EnemyLogic>().Move();
            }
        }
    }
    public void Ability1Selected()
    {
        Debug.Log("AB1");

        mouseController.HideInRangeTiles();
        mouseController.HideInRangeTilesShooting();
        mouseController.HideAllArrows();

        isAbilitySelected = true;
        isAbility1Selected = true;
        isAbility2Selected = false;
        menu = false;

        movePointsText.transform.position = new Vector3(6.63f, -4f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.12f, 0.0f);
        movePointsText.SetText("Colt .32 Revolver");
        shadowMovePointsText.SetText("Colt .32 Revolver");
        
        UIContainer.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(1, 1, 1, 0);
        UIContainer.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        if (isFirstShot)
        { 
            bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            bullet1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        else if (!isFirstShot && !outOfAmmo)
        {
            bullet1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    public void Ability2Selected()
    {
        Debug.Log("AB2");

        mouseController.HideInRangeTiles();
        mouseController.HideInRangeTilesShooting();
        mouseController.HideAllArrows();

        isAbilitySelected = true;
        isAbility1Selected = false;
        isAbility2Selected = true;
        menu = false;

        movePointsText.transform.position = new Vector3(6.63f, -4f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.12f, 0.0f);
        movePointsText.SetText("Molotov Cocktail");
        shadowMovePointsText.SetText("Molotov Cocktail");

        UIContainer.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(1, 1, 1, 0);
        UIContainer.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        bullet1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
    public void MoveSelected()
    {
        Debug.Log("MOV");

        mouseController.GetInRangeTiles();

        isAbilitySelected = false;
        isAbility1Selected = false;
        isAbility2Selected = false;
        menu = false;

        movePointsText.transform.position = new Vector3(6.63f, -4.7f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.82f, 0.0f);

        UIContainer.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(1, 1, 1, 0);
        UIContainer.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        bullet.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        bullet1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void Menu()
    {
        if (menu == false)
        {
            menu = true;
            mouseController.HideInRangeTiles();
            mouseController.HideInRangeTilesShooting();
            mouseController.HideAllArrows();
            //var step = 3f * Time.deltaTime;
            UIContainer.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(1, 1, 1, 1);
            UIContainer.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
        }
        else if (menu == true)
        {
            mouseController.GetInRangeTiles();
            //UIContainer.transform.position = Vector2.Lerp(UIContainer.transform.position, new Vector3(2.12f, -1.34f, 800f), 1f * Time.deltaTime);
            menu = false;
            UIContainer.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(1, 1, 1, 0);
            UIContainer.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        }
    }

    void Update()
    {
        if (isPlayerTurn)
        {
            Delay();
            var endTurn = Input.GetKey(KeyCode.Q);
            var menu = Input.GetKey(KeyCode.Tab);

            if (endTurn)
            {
                isPlayerTurn = false;
                state = BattleState.ENEMYTURN;
                Debug.Log("ENEMYTURN");
                EnemyTurn();
            }

            else if (menu && !delay)
            {   
                Debug.Log("MENU");
                decay = 0.5f;
                delay = true;
                Menu();
            }

            if (!isAbilitySelected)
            {
                movePointsText.SetText("Move " + remainingMovePts + "/4");
                shadowMovePointsText.SetText("Move " + remainingMovePts + "/4");
            }

            if (isAbility1Selected)
            {

            }

            if (isAbility2Selected)
            {

            }
        }

        if (isEnemyTurn)
        {
            var endTurn = Input.GetKey(KeyCode.E);
            if (endTurn)
            {
                isEnemyTurn = false;
                state = BattleState.PLAYERTURN;
                Debug.Log("PLAYERTURN");
                PlayerTurn();
                foreach (Transform child in enemyList.transform)
                {
                    Debug.Log(child.gameObject.name);
                    if (child.gameObject.tag == "Fire")
                    {
                        child.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                        child.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                        Debug.Log("Fire");
                        if (child.gameObject.GetComponent<Fire>().timer == 0)
                        {
                            Destroy(child.gameObject);
                        }
                        else { child.gameObject.GetComponent<Fire>().timer--; }

                    }
                    else if (child.gameObject != null)
                    {
                        child.gameObject.GetComponent<EnemyLogic>().fireDamage = false;
                        child.gameObject.GetComponent<EnemyLogic>().movedTwice = false;
                        child.gameObject.GetComponent<EnemyLogic>().movedThrice = false;

                        //StartCoroutine(Wait());
                        //child.gameObject.GetComponent<EnemyLogic>().isMoving = false;
                    }
                    else { continue; }
                }
                SpawnHorde();
            }
        }
    }

    private void Delay()
    {
        if (delay && decay > 0)
        {
            decay -= Time.deltaTime;
        }
        if (decay < 0)
        {
            decay = 0;
            delay = false;
        }
    }
}