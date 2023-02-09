using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
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

    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    void FixedUpdate() {
        // check midair
        if(rigid.velocity.y < 0) {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.3f, LayerMask.GetMask("Floor"));
            if(rayHit.collider != null) {
                if(rayHit.distance < 1.1f) {
                    anim.SetBool("isJump", false);
                    isJump = false;
                }
                    
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hp);
        // jump
        if (Input.GetButtonDown("Jump") && !isJump && !isHit)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            isJump = true;
        }

        // slide
        if (Input.GetKeyDown("down") && !isHit && !isJump)
        {
            anim.SetBool("isSlide", true);
            isSlide = true;
        }

        if (Input.GetKeyUp("down"))
        {
            anim.SetBool("isSlide", false);
            isSlide = false;
        }

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

    private IEnumerator OnHit(int dmg) {
        hp -= dmg;

        if(hp == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);


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
            else if (other.gameObject.tag == "Icicle" && !isSlide) {
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


}
