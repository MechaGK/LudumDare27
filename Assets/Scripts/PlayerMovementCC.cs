using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementCC : MonoBehaviour
{
    Transform myTransform;
    CharacterController controller;

    public float speed = 8.0f;
    public float jumpSpeed= 10.0f;
    float modifiedJumSpeed = 10.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;
    bool controlable = true;
    float pushTimer = 0;
    bool jumpButtonHeld = false;
    float jumpModifier = 0;

    public Texture playerRight;
    public Texture playerLeft;
    public Texture armsRight;
    public Texture armsLeft;
    public Texture armsStraightRight;
    public Texture armsStraightLeft;

    PlayerAttack attackScript;
    public Transform attackTrigger;
    public Renderer arms;

    int face = 1;

    void Start()
    {
        myTransform = transform;
        controller = GetComponent<CharacterController>();

        attackScript = attackTrigger.GetComponent<PlayerAttack>();
    }

    void Update()
    {
        if (controlable)
        {
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
                moveDirection *= speed;

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed / 2;
                    jumpButtonHeld = true;
                }
            }
            else
            {
                if (jumpButtonHeld)
                {
                    if (Input.GetButton("Jump") && jumpModifier < jumpSpeed * 0.6f )
                    {
                        jumpModifier += 0.5f;
                        moveDirection.y += 0.5f;
                    }
                    else
                    {
                        jumpModifier = 0;
                        jumpButtonHeld = false;
                    }
                }
                moveDirection.x = Input.GetAxis("Horizontal") * speed;

                if ((controller.collisionFlags & CollisionFlags.Above) != 0)
                {
                    moveDirection.y = 0;
                    moveDirection.y -= gravity * Time.deltaTime * 2;
                }
            }
        }
        else
        {
            pushTimer += Time.deltaTime;
            moveDirection.x += 4.0f * Time.deltaTime;

            if (pushTimer >= 0.5f)
            {
                pushTimer = 0;
                controlable = true;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);


        if (Input.GetAxis("Horizontal") < 0)
        {
            face = -1;
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            face = 1;
        }

        if (face == 1)
        {
            renderer.material.mainTexture = playerRight;
            arms.material.mainTexture = attackScript.armsReset ? armsRight : armsStraightRight;
            attackTrigger.localPosition = new Vector3(Mathf.Abs(attackTrigger.localPosition.x), 0, 0);
        }
        else
        {
            renderer.material.mainTexture = playerLeft;
            arms.material.mainTexture = attackScript.armsReset ? armsLeft : armsStraightLeft;
            attackTrigger.localPosition = new Vector3(-Mathf.Abs(attackTrigger.localPosition.x), 0, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            moveDirection.x = collision.transform.position.x > myTransform.position.x ? -10.0f : 10.0f;
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
