using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class control : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D box;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public float jumpPower;
    public int hp;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    void FixedUpdate() {
        // check midair
        if(rigid.velocity.y < 0) {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.3f, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null) {
                if(rayHit.distance < 0.9f)
                    anim.SetBool("isJump", false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hp);
        // jump
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJump")) {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
        }

        // slide
        if (Input.GetKeyDown("down"))
        {
            anim.SetBool("isSlide", true);
        }

        if (Input.GetKeyUp("down"))
        {
            anim.SetBool("isSlide", false);
        }

    }

    private IEnumerator OnHit(int dmg) {
        hp -= dmg;

        if(hp == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        anim.SetBool("isHit", true);

        yield return new WaitForSeconds(1);

        anim.SetBool("isHit", false);

    }

    void OnCollisionEnter2D(Collision2D other) {    
        // get dmg
        if(anim.GetBool("isHit") == false) {
            if (other.gameObject.tag == "Spike")
            {
                Debug.Log("spike hit");
                StartCoroutine(OnHit(1));
            }
            else if (other.gameObject.tag == "Icicle" && anim.GetBool("isSlide") == false) {
                Debug.Log("icicle hit");
                StartCoroutine(OnHit(1));
            }


        }
        
    }


}
