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
    Animator anim;
    GameManager gameManager;
    public GameObject effect;
    public GameObject player;

    public int hp;

    public float speed;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void Update()
    {
        if (!Score.isRunning) return;

        if (player != null) {
            Vector3 curPos = transform.position;
            Vector3 direction = player.transform.position - transform.position;
            transform.position = curPos + direction.normalized * speed * Time.deltaTime;
        }

        else
            Destroy(gameObject);
            
        
    }

    void OnHit(int dmg)
    {
        hp -= dmg;
        anim.SetBool("isHit", true);
        Invoke("ReturnSprite", 0.1f);

        if (hp < 1)
            Destroyed();

    }

    void ReturnSprite()
    {
        anim.SetBool("isHit", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            OnHit(other.gameObject.GetComponent<PlayerBullet>().dmg);
        }
        else if (other.gameObject.tag == "PlayerBoomerang")
        {
            OnHit(other.gameObject.GetComponent<PlayerBoomerang>().dmg);
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

        GameObject dieEffectObj = Instantiate(effect, transform.position, transform.rotation);
        ParticleSystem dieEffect = dieEffectObj.GetComponent<ParticleSystem>();

        ScoreText = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<Text>();
        ScoreText.text = "Score: " + Score.score;
    }

}
