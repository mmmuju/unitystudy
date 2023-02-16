using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Text ScoreText;

    float time = 0f;

    Rigidbody2D rigid;
    BoxCollider2D box;
    SpriteRenderer spriteRenderer;
    Animator anim;

    [Header("Physics")]
    public float jumpPower;

    [Header("Attack")]
    public GameObject bulletTypeA;
    public float attackDelay;
    float curAttackDelay = 0;

    [Header("Status")]
    public int hp;
    public float hitDelay;

    bool isJump;
    bool isSlide;
    bool isHit;

    float timer = 0.0f;

    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }
    
    void FixedUpdate() {
        // check midair
        if (rigid.velocity.y < 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Floor"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    Jump(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        // survive score
        timer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0;
            Score.score += 1;

            ScoreText = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<Text>();
            ScoreText.text = "Score: " + Score.score;
        }

        // jump
        if (Input.GetButton("Jump") && !isJump && !isHit)
        {
            Jump(true);
            Slide(false);
        }

        // slide
        if (!isHit && !isJump) {
            if(Input.GetKey("down"))
                Slide(true);
            else
                Slide(false);
        }   

        // attack
        Attack();
        
        curAttackDelay += Time.deltaTime;
    }

    void Attack() {
        if (curAttackDelay < attackDelay)
            return;
        GameObject bullet = Instantiate(bulletTypeA, transform.position, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.right * 20, ForceMode2D.Impulse);
        curAttackDelay = 0;
    }

    void Jump(bool b)
    {
        if(b)
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            
        anim.SetBool("isJump", b);
        isJump = b;
    }

    void Slide(bool b) {
        anim.SetBool("isSlide", b);
        isSlide = b;
    }


    private IEnumerator OnHit(int dmg) {
        hp -= dmg;

        if(hp == 0)
            gameOver();


        anim.SetBool("isHit", true); // 맞는 애니메이션 전환
        isHit = true;
        this.gameObject.layer = 3; // 레이어를 잠깐 변경 (3: 무적)해서 장애물간 충돌 무시

        yield return new WaitForSeconds(hitDelay); // 피격 지속시간 (다른 동작 불가능)

        anim.SetBool("isHit", false); // 원상복구
        isHit = false;
        this.gameObject.layer = 0; // 원상복구2

    }

    void OnTriggerStay2D(Collider2D other) {   
        // get dmg
        if(!isHit) {
            if (other.gameObject.tag == "Spike")
            {
                StartCoroutine(OnHit(1));
            }
            else if (other.gameObject.tag == "Icicle") {
                if(!isSlide)
                    StartCoroutine(OnHit(1));
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!isHit)
        {
            if (other.gameObject.tag == "EnemyBullet")
            {
                StartCoroutine(OnHit(1));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy")
        {
            StartCoroutine(OnHit(1));
        }
    }

    void gameOver() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Score.score = 0;
    }


}
