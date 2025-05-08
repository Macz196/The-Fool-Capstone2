using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;            
    public float jumpForce = 10f;       
    public LayerMask groundLayer;       
    public float groundCheckRadius = 0.2f; 
    
    private bool isGrounded;            
    private Rigidbody2D rb;              

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
 
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        transform.Translate(Time.deltaTime * speed * direction);


        isGrounded = CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private bool CheckGround()
    {
        return Physics2D.OverlapCircle(transform.position - new Vector3(0, 0.5f, 0), groundCheckRadius, groundLayer);
    }
}
