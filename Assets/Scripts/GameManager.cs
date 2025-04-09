using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float finalScore = 0f;

    public GameObject player;
    public float spawnTerm = 5;
    public float fasterEverySpawn = 0.05f;
    public float minSpawnTerm = 1;
    public TextMeshProUGUI scoreText;
    float timeAfterLastSpawn;
    float score;

    // Start is called before the first frame update
    void Start()
    {
        timeAfterLastSpawn = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterLastSpawn += Time.deltaTime;
        score += Time.deltaTime;

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

        scoreText.text = ((int)score).ToString();
    }


    void SpawnEnemy()
    {
        float x = Random.Range(-10f, 10f);
        float y = Random.Range(-5f, 5f);

        GameObject obj = GetComponent<ObjectPool>().Get();
        obj.transform.position = new Vector3(x, y, 0);
        obj.GetComponent<EnemyController>().Spawn(player);
    }

    public void SaveFinalScore()
    {
        finalScore = score;  // 현재 점수를 static 변수에 저장
    }

}
