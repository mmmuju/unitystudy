using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class control : MonoBehaviour
{
    Rigidbody2D rigid;
    public float jumpPower;
    bool isJump;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        // check midair
        if(rigid.velocity.y < 0) {
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.3f, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null) {
                if(rayHit.distance < 0.8f)
                    isJump = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // jump
        if(Input.GetButtonDown("Jump") && !isJump) {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJump = true;
            //anim.SetBool("isJump", true);
        }

        /* sprite change
        if(Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // animation
        if(rigid.velocity.x == 0)
            anim.SetBool("isWalk", false);
        else
            anim.SetBool("isWalk", true);*/
    }

}
