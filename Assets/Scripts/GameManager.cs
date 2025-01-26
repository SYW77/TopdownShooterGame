using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float spawnTerm = 5;
    public float fasterEverySpawn = 0.05f;
    public float minSpawnTerm = 1;
    float timeAfterLastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timeAfterLastSpawn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterLastSpawn += Time.deltaTime;

        if(timeAfterLastSpawn > spawnTerm)
        {
            timeAfterLastSpawn -= spawnTerm;

            SpawnEnemy();

            spawnTerm -= fasterEverySpawn;

            if (spawnTerm < minSpawnTerm)
            {
                spawnTerm = minSpawnTerm;
            }
        }
    }

    void SpawnEnemy()
    {
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-5f, 5f);

        GameObject obj = GetComponent<ObjectPool>().Get();
        obj.transform.position = new Vector3(x, y, 0);
        obj.GetComponent<EnemyController>().Spawn(player);
    }
}
