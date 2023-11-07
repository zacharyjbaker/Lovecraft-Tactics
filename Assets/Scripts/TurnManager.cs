using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public BattleState state;
    private bool continueState;
    public bool isPlayerTurn = false;
    public bool isStart = true;
    public bool isEnemyTurn = false;
    public bool isAbilitySelected = false;
    public bool isAbility1Selected = false;
    public bool isAbility2Selected = false;
    public int remainingMovePts = 5;


    // Start is called before the first frame update
    void Start()
    {
        continueState = false;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        movePointsText.transform.position = new Vector3(6.63f, -4.7f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.82f, 0.0f);
        Debug.Log("START");
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

    public int subMovePoints(int subtraction)
    {
        remainingMovePts -= subtraction;
        return remainingMovePts;
    }

    private void PlayerTurn()
    {
        isPlayerTurn = true;
        remainingMovePts = 4;
        mouseController.GetInRangeTiles();
        stateText.SetText("Player Turn");
        shadowStateText.SetText("Player Turn");
        movePointsText.SetText("Move " + remainingMovePts + "/4");
        shadowMovePointsText.SetText("Move " + remainingMovePts + "/4");
    }

    private void EnemyTurn()
    {
        isEnemyTurn = true;
        mouseController.HideInRangeTiles();
        stateText.SetText("Enemy Turn");
        shadowStateText.SetText("Enemy Turn");
        movePointsText.SetText("");
        shadowMovePointsText.SetText("");
    }
    public void Ability1Selected()
    {
        Debug.Log("AB1");
        mouseController.HideInRangeTiles();
        mouseController.HideAllArrows();
        isAbilitySelected = true;
        isAbility1Selected = true;
        isAbility2Selected = false;
        movePointsText.transform.position = new Vector3(6.63f, -4f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.12f, 0.0f);
        movePointsText.SetText("Colt .32 Revolver");
        shadowMovePointsText.SetText("Colt .32 Revolver");
    }

    public void Ability2Selected()
    {
        Debug.Log("AB2");
        mouseController.HideInRangeTiles();
        mouseController.HideAllArrows();
        isAbilitySelected = true;
        isAbility1Selected = false;
        isAbility2Selected = true;
        movePointsText.transform.position = new Vector3(6.63f, -4f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.12f, 0.0f);
        movePointsText.SetText("Molotov Cocktail");
        shadowMovePointsText.SetText("Molotov Cocktail");
    }
    public void MoveSelected()
    {
        Debug.Log("MOV");
        mouseController.GetInRangeTiles();
        isAbilitySelected = false;
        isAbility1Selected = false;
        isAbility2Selected = false;
        movePointsText.transform.position = new Vector3(6.63f, -4.7f, 0.0f);
        shadowMovePointsText.transform.position = new Vector3(6.63f, -4.82f, 0.0f);
    }

    void Update()
    {
        if (isPlayerTurn)
        {
            var endTurn = Input.GetKey(KeyCode.Q);
            if (endTurn)
            {
                isPlayerTurn = false;
                state = BattleState.ENEMYTURN;
                Debug.Log("ENEMYTURN");
                EnemyTurn();
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
            }
        }
    }
}
