using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrowControl : MonoBehaviour
{
    Text ScoreText;
    Rigidbody2D rigid;
    BoxCollider2D box;
    SpriteRenderer spriteRenderer;
    GameManager gameManager;
    public GameObject player;

    public int hp;

    public float speed;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 direction = Vector3.zero;
        if(player != null)
            direction = player.transform.position - transform.position;
        transform.position = curPos + direction.normalized * speed * Time.deltaTime; 
    }

    void OnHit(int dmg)
    {
        hp -= dmg;

        if (hp == 0)
            Destroyed();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            OnHit(1);
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroyed();
        }
    }

    void Destroyed()
    {
        Destroy(gameObject);
        Score.score += 100;

        ScoreText = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<Text>();
        ScoreText.text = "Score: " + Score.score;
    }

}
