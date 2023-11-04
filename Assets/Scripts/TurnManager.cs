using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class TurnManager : MonoBehaviour
{
    //public GameObject playerPrefab;
    //public GameObject enemyPrefab;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        //Instantiate(playerPrefab);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        //PlayerTurn();
    }

    /*
    void PlayerTurn()
    {
        
    }*/
}
