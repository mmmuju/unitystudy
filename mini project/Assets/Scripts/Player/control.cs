using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public float jumpPower;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
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

}
