using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    Text ScoreText;
    Rigidbody2D rigid;
    BoxCollider2D box;
    SpriteRenderer spriteRenderer;
    GameManager gameManager;

    public GameObject enemyBullet;
    public float attackDelay;
    public float bulletSpeed;
    public GameObject player;

    public int hp;

    float speed;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        speed = Random.Range(gameManager.enemyMinSpeed, gameManager.enemyMaxSpeed);
        Invoke("Attack", attackDelay);

    }

    void Update()
    {
        Vector3 curPos = transform.position;
        transform.position = curPos + Vector3.left * speed * Time.deltaTime;
    }


    void Attack() {
        GameObject bullet = Instantiate(enemyBullet, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector3 direction = player.transform.position - transform.position;
        rigid.AddForce(direction.normalized * bulletSpeed, ForceMode2D.Impulse);

        Invoke("Attack", attackDelay);
    }

    // Update is called once per frame
    

    void OnHit(int dmg)
    {
        hp -= dmg;

        if (hp == 0)
        {
            Destroy(gameObject);
            Score.score += 100;

            ScoreText = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<Text>();
            ScoreText.text = "Score: " + Score.score;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            OnHit(1);
        }
    }

}
