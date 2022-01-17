using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpPower;

    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private Animator anim;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1,1,1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);



        anim.SetBool("walking", horizontalInput != 0);
        anim.SetBool("jumping", !OnGround());
        anim.SetFloat("yVelocity", body.velocity.y);

        if (CombatManager.instance.isAttacking)
        {
            horizontalInput = 0;
            anim.SetBool("walking", false);
        }

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }


    }

    private void Jump()
    {
        if (OnGround())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetBool("jumping", true);
            //anim.SetTrigger("jump");
        }
    }

    public bool OnGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;

    }
}








