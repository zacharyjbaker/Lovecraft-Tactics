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
    public int remainingMovePts = 4;


    // Start is called before the first frame update
    void Start()
    {
        continueState = false;
        state = BattleState.START;
        StartCoroutine(SetupBattle());
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

    void Update()
    {
        if (isPlayerTurn)
        {
            movePointsText.SetText("Move " + remainingMovePts + "/4");
            shadowMovePointsText.SetText("Move " + remainingMovePts + "/4");
            var endTurn = Input.GetKey(KeyCode.Q);
            if (endTurn)
            {
                isPlayerTurn = false;
                state = BattleState.ENEMYTURN;
                Debug.Log("ENEMYTURN");
                EnemyTurn();
            }

            if (isAbilitySelected)
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
