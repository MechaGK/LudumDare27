using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    Transform myTransform;

    public float speed = 2;
    public float jumpSpeed = 3;

    float jumpTimer = 10.000f;
    bool hasJumped = false;
    public TextMesh timer;
    Vector3 posBeforeJump;

    float characterHeight;

    void Start()
    {
        myTransform = transform;

        characterHeight = renderer.bounds.size.y;
    }

    void FixedUpdate()
    {
        if (jumpTimer <= 0)
        {
            if (jumpTimer < 0)
            {
                jumpTimer = 0;
            }
            
            if (isGrounded() && hasJumped)
            {
                jumpTimer = 10.0f;
                hasJumped = false;
            }
            else if (!hasJumped)
            {
                rigidbody.velocity += Vector3.up * jumpSpeed;
                hasJumped = true;
            }
            
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }

        timer.text = jumpTimer.ToString("F3");

        rigidbody.velocity = new Vector3(speed * Input.GetAxis("Horizontal"), rigidbody.velocity.y, 0);

        rigidbody.useGravity = isGrounded() ? false : true;
    }

    bool isGrounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterHeight / 2))
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }
}
