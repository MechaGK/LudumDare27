using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    Transform myTransform;
    public Transform attackTrigger;
    public Renderer arms;

    public Texture playerRight;
    public Texture playerLeft;
    public Texture armsRight;
    public Texture armsLeft;
    public Texture armsStraightRight;
    public Texture armsStraightLeft;

    PlayerAttack attackScript;

    public float speed = 2;
    public float jumpSpeed = 3;

    float jumpTimer = 10.000f;
    bool hasJumped = false;
    Vector3 posBeforeJump;

    float characterHeight;

    bool controlable = false;
    float pushTimer = 0;

    void Start()
    {
        myTransform = transform;

        characterHeight = renderer.bounds.size.y;
        attackScript = attackTrigger.GetComponent<PlayerAttack>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            renderer.material.mainTexture = playerLeft;
            arms.material.mainTexture = attackScript.armsReset ? armsLeft : armsStraightLeft;
            attackTrigger.localPosition = new Vector3(-Mathf.Abs(attackTrigger.localPosition.x), 0, 0);
            
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            renderer.material.mainTexture = playerRight;
            arms.material.mainTexture = attackScript.armsReset ? armsRight : armsStraightRight;
            attackTrigger.localPosition = new Vector3(Mathf.Abs(attackTrigger.localPosition.x), 0, 0);
        }
    }

    void FixedUpdate()
    {
        if (controlable)
        {


            if (isGrounded())
            {
                rigidbody.velocity = rigidbody.velocity + new Vector3(1, 0, 1);

                if (Input.GetButton("Jump"))
                {
                    rigidbody.velocity += Vector3.up * jumpSpeed;
                }
            }

            //rigidbody.velocity = new Vector3(speed * Input.GetAxis("Horizontal"), rigidbody.velocity.y, 0);
            rigidbody.AddForce(Vector3.right * speed * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        }
        else
        {
            pushTimer += Time.deltaTime;

            if (pushTimer >= 0.5f)
            {
                pushTimer = 0;
                controlable = true;
            }
        }

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rigidbody.AddForce(Vector3.left * 10, ForceMode.Impulse);
            controlable = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gold"))
        {
            GameController.gold++;
            Destroy(other.gameObject);
        }
    }
}