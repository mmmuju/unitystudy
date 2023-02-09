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

    public float spikeHeight; // -3.8
    public float icicleHeight; // 1.3
    public float obstacleDelayMargin;

    [Header("Enemy Setting")]
    public GameObject enemy;

    public float enemyDelay;
    public float enemyMinSpeed;
    public float enemyMaxSpeed;
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
        
        
    }

    void spawnObstacle() {
        int choose = Random.Range(0, 2);
        if(choose == 0) {
            int amount;
            amount = Random.Range(2, 5);
            for (int i=1; i<amount; i++)
                Instantiate<GameObject>(spike, new Vector2(15+(1*i), spikeHeight), Quaternion.identity);
        }
        else
            Instantiate<GameObject>(icicle, new Vector2(15, icicleHeight), Quaternion.identity);
        
        float margin = Random.Range(0, obstacleDelayMargin);
        Invoke("spawnObstacle", obstacleDelay + obstacleDelayMargin);
    }

    void spawnEnemy()
    {
        Instantiate<GameObject>(enemy, new Vector2(15, -3f), Quaternion.identity);
        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
        enemyControl.player = player;

        float margin = Random.Range(0, enemyDelayMargin);
        Invoke("spawnEnemy", enemyDelay + enemyDelayMargin);
    }
}
