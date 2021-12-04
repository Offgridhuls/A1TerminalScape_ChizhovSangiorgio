using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{

    public Transform[] randomSpawns;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRandomEnemyStats();
    }

    public void GenerateRandomEnemyStats()
    {
        for(int i = 0; i < (int)Enemystats.STATCOUNT; i++)
        {
            Enemy.stats[i] = Random.Range(0, 100);
        }
    }

    // Update is called once per frame
    public void SpawnEnemyAtRandomLocation()
    {

    }
}
