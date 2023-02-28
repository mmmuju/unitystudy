using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Text ScoreText;
    Text GameOverText;

    Rigidbody2D rigid;
    BoxCollider2D box;
    SpriteRenderer spriteRenderer;
    Animator anim;

    [Header("Physics")]
    public float jumpPower;

    [Header("Attack")]
    public GameObject bulletTypeA;
    public float bulletSpeed = 20;
    public float bulletOffsetX = 0;
    public float bulletOffsetY = -0.25f;
    public float attackDelay;
    float curAttackDelay = 0;

    [Header("Status")]
    public int hp;
    public float hitDelay;
    public float timer = 0.0f;

    [Header("Skills")]
    [Header("Roll")]
    public float rollDelay;
    public float rollCool;

    [Header("Boomerang")]
    public GameObject bulletTypeB;
    GameObject boomerang;
    public float boomerangCool;
    bool boomerangType; // 스킬 단축키에 따른 부메랑 타입을 지정하여 되돌아오는 부메랑 획득 시 해당 단축키 스킬의 쿨타임이 초기화됨
                        // true = A false = B

    [Header("Smite")]
    public GameObject bulletTypeC;
    public float smiteCool;

    [Header("Accel")]
    public float accelDuration;
    public float accelDelay;
    public float accelCool;

    [Header("Multiple")]
    public float multipleDuration;
    public float multipleCool;
    bool isMultiple = false;

    bool isJump;
    bool isSlide;
    bool isHit;
    

    

    float CoolSkillA = 0; // Q스킬 쿨타임 
    float CoolSkillB = 0; // W스킬 쿨타임 


    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        Score.maxScore = PlayerPrefs.GetInt("maxScore");

        ScoreText = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<Text>();
        ScoreText.text = "Max Score: " + Score.maxScore + "\n" + "Score: " + Score.score;

        PlayerSkill.skillOrder[0] = 1; // Q스킬에 할당된 스킬 타입 (임시)
        PlayerSkill.skillOrder[1] = 2; // W스킬에 할당된 스킬 타입 (임시)
    }
    
    void FixedUpdate() {
        if (!Score.isRunning) return;
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
        Debug.Log(hp);
        if (!Score.isRunning)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(Score.score > Score.maxScore)
                {
                    PlayerPrefs.SetInt("maxScore", Score.score);
                }

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Score.isRunning = true;
                Score.score = 0;
            }

            return;
        }

        // survive score
        timer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0;
            Score.score += 1;

            ScoreText.text = "Max Score: " + Score.maxScore + "\n" + "Score: " + Score.score;
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

        // skill
        if (Input.GetButtonDown("Skill1")) {
            if(CoolSkillA < 1.0f)
                SelectSkill(PlayerSkill.skillOrder[0], true);
        }

        if (Input.GetButtonDown("Skill2"))
        {
            if(CoolSkillB < 1.0f)
                SelectSkill(PlayerSkill.skillOrder[1], false);
        }

        if (Input.GetKey("a")) {
            PlayerSkill.skillOrder[0] = 1; // Q스킬에 할당된 스킬 타입 (임시)
            PlayerSkill.skillOrder[1] = 2; // W스킬에 할당된 스킬 타입 (임시)
        }

        if (Input.GetKey("s"))
        {
            PlayerSkill.skillOrder[0] = 3; // Q스킬에 할당된 스킬 타입 (임시)
            PlayerSkill.skillOrder[1] = 4; // W스킬에 할당된 스킬 타입 (임시)
        }

        if (Input.GetKey("d"))
        {
            PlayerSkill.skillOrder[0] = 5; // Q스킬에 할당된 스킬 타입 (임시)
            PlayerSkill.skillOrder[1] = 1; // W스킬에 할당된 스킬 타입 (임시)
        }
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

    void Attack()
    {
        if (curAttackDelay < attackDelay)
            return;
        Vector3 bulletLoc = transform.position + new Vector3(bulletOffsetX, bulletOffsetY, 0);
        GameObject bullet = Instantiate(bulletTypeA, bulletLoc, transform.rotation);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.right * bulletSpeed, ForceMode2D.Impulse);
        if(isMultiple) {
            GameObject bullet2 = Instantiate(bulletTypeA, bulletLoc, transform.rotation);
            Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
            rigid2.AddForce(new Vector2(bulletSpeed, bulletSpeed/3), ForceMode2D.Impulse);

            GameObject bullet3 = Instantiate(bulletTypeA, bulletLoc, transform.rotation);
            Rigidbody2D rigid3 = bullet3.GetComponent<Rigidbody2D>();
            rigid3.AddForce(new Vector2(bulletSpeed, -(bulletSpeed/3)), ForceMode2D.Impulse);
        }

        curAttackDelay = 0;
    }

    void SelectSkill(int id, bool isSkillA) {
        switch (id)
        {
            case 1: // type 1: roll
                if (!isJump) {
                    StartCoroutine(Roll());
                    if(isSkillA)
                        StartCoroutine(CooldownSkillA(rollCool));
                    else
                        StartCoroutine(CooldownSkillB(rollCool));
                }
                break;
            case 2: // type 2: boomerang
                Boomerang(isSkillA);
                if(isSkillA)
                    StartCoroutine(CooldownSkillA(boomerangCool));
                else
                    StartCoroutine(CooldownSkillB(boomerangCool));
                break;
            case 3: // type 3: smite
                Smite();
                if(isSkillA)
                    StartCoroutine(CooldownSkillA(smiteCool));
                else
                    StartCoroutine(CooldownSkillB(smiteCool));
                break;
            case 4: // type 4: accel
                StartCoroutine(Accel(accelDuration));
                if(isSkillA)
                    StartCoroutine(CooldownSkillA(accelCool));
                else
                    StartCoroutine(CooldownSkillB(accelCool));
                break;
            case 5: // type 5: multiple
                StartCoroutine(Multiple(multipleDuration));
                if (isSkillA)
                    StartCoroutine(CooldownSkillA(multipleCool));
                else
                    StartCoroutine(CooldownSkillB(multipleCool));
                break;
        }
    }

    private IEnumerator CooldownSkillA(float cool)
    {
        CoolSkillA = cool;
    
        while(CoolSkillA > 1.0f) {
            CoolSkillA -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

    }

    private IEnumerator CooldownSkillB(float cool)
    {
        CoolSkillB = cool;

        while (CoolSkillB> 1.0f) {
            CoolSkillB -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

    }
        
    private IEnumerator Roll() {
        anim.SetBool("isRoll", true);
        isHit = true;
        this.gameObject.layer = 3; // 레이어를 잠깐 변경(3: 무적)해서 장애물간 충돌 무시

        yield return new WaitForSeconds(rollDelay); // 피격 지속시간 (다른 동작 불가능)

        anim.SetBool("isRoll", false); // 원상복구
        isHit = false;
        this.gameObject.layer = 0; // 원상복구2
    }

    void Boomerang(bool b) {
        Instantiate<GameObject>(bulletTypeB, transform.position + new Vector3(0, 0.3f, 0), transform.rotation);
        boomerangType = b;
    }

    void Smite() {
        Instantiate<GameObject>(bulletTypeC, transform.position + new Vector3(1.5f, 0, 0), transform.rotation);
    }
    
    private IEnumerator Accel(float duration) {
        float tempAttackDelay = attackDelay;
        attackDelay = accelDelay;

        yield return new WaitForSeconds(duration);
        
        attackDelay = tempAttackDelay;
    }

    private IEnumerator Multiple(float duration)
    {
        isMultiple = true;
        yield return new WaitForSeconds(duration);
        isMultiple = false;
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
        anim.SetBool("isJump", false);
        isHit = false;
        isJump = false;
        this.gameObject.layer = 0; // 원상복구2

    }

    void OnTriggerStay2D(Collider2D other) {
        if (!Score.isRunning) return;
        // get dmg
        if (!isHit) {
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
        if (!Score.isRunning) return;
        if (!isHit)
        {
            if (other.gameObject.tag == "EnemyBullet")
            {
                StartCoroutine(OnHit(other.gameObject.GetComponent<EnemyBullet>().dmg));
            }
        }
        if(other.gameObject.tag == "PlayerBoomerang") { // 부메랑 획득 시
            if(other.gameObject.GetComponent<PlayerBoomerang>().status == 2) {
                if(boomerangType) {
                    CoolSkillA = 0;
                }
                else {
                    CoolSkillB = 0;
                }
                Destroy(other.gameObject); 
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
        Score.isRunning = false;
        GameOverText = GameObject.Find("Canvas").transform.Find("GameOverText").GetComponent<Text>();
        GameOverText.text = "  Game Over\nRestart: Space";
    }
}
