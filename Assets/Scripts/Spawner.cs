using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject[] enemyTypes;
    [SerializeField] private GameObject EnemyList;

    public void SpawnEnemy(int option, Transform spawnLoc)
    {
        Debug.Log("Spawn Enemy:" + option);
        Instantiate(enemyTypes[option], spawnLoc.position, spawnLoc.rotation, EnemyList.transform);
    }
}
