using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {

    public float maxSpeed = 4;
    public float jumpForce = 550;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    [HideInInspector]
    public bool lookingRight = true;

    private Rigidbody2D rb2d;
    private Animator anim;
    private bool isGrounded = false;
    private bool jump = false;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump") && isGrounded) {
            jump = true;
        }
	}

    // Fixed Update is for Physics!
    void FixedUpdate() {
        // Left an Right Input
        float hor = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(hor)); // Controls the Animation Parameter Speed. Needs to be positive always
        rb2d.velocity = new Vector2(hor * maxSpeed, rb2d.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, whatIsGround);
        anim.SetBool("Ground", isGrounded);

        // Flip Direction when moving to the left
        if ((hor > 0 && !lookingRight) || hor < 0 && lookingRight) {
            Flip();
        }

        if (jump) {
            rb2d.AddForce(new Vector2(0, jumpForce));
            jump = false;
        }
    }

    public void Flip() {
        lookingRight = !lookingRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }
}
