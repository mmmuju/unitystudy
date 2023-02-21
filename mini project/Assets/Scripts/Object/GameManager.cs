using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float environmentSpeed;

    [Header("Obstacle Setting")]
    public GameObject spike;
    public GameObject icicle;

    public float obstacleDelay;

    public float spikeHeight; // -3
    public float icicleHeight; // 2.7
    public float obstacleDelayMargin;

    [Header("Enemy Setting")]
    public GameObject[] enemy;

    public float enemyDelay;
    public float enemyDelayMargin;

    // Start is called before the first frame update
    void Start()
    {
        spawnObstacle();
        Invoke("spawnObstacle", obstacleDelay);
        Invoke("spawnEnemy", enemyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Score.isRunning) CancelInvoke();
    }

    void spawnObstacle() {
        int choose = Random.Range(0, 2);
        if(choose == 0) {
            int amount;
            amount = Random.Range(2, 5);
            for (int i=1; i<amount; i++)
                Instantiate<GameObject>(spike, new Vector2(13+(1*i), spikeHeight), Quaternion.identity);
        }
        else
            Instantiate<GameObject>(icicle, new Vector2(13, icicleHeight), Quaternion.identity);
        
        float margin = Random.Range(0, obstacleDelayMargin);
        Invoke("spawnObstacle", obstacleDelay + obstacleDelayMargin);
    }

    void spawnEnemy()
    {
        int enemyNumber = Random.Range(0, enemy.Length);

        switch(enemyNumber) {
            case 0: // cat
                Instantiate<GameObject>(enemy[0], new Vector2(13, -2.5f), Quaternion.identity);
                EnemyControl enemyControl = enemy[0].GetComponent<EnemyControl>();
                enemyControl.player = player;
            break;

            case 1: // crow
                float spawnHeight = Random.Range(-1.5f, 3);
                Instantiate<GameObject>(enemy[1], new Vector2(13, spawnHeight), Quaternion.identity);
                CrowControl crowControl = enemy[1].GetComponent<CrowControl>();
                crowControl.player = player;
            break;

        }
        

        float margin = Random.Range(0, enemyDelayMargin);
        Invoke("spawnEnemy", enemyDelay + enemyDelayMargin);
    }
}
