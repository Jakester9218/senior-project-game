using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float acceleration;
    public float jumpForce;
    public float horizontalInput;
    public float speedCap;
    private float controlFactor;
    public float airControlFactor;
    public float fallSpeedMultiplier;
    public bool isGrounded;
    public float groundingBoxHeight;
    public LayerMask whatIsGround;
    public float deadzone;
    public int quickTurnBufferSize;
    public List<float> quickTurnBuffer;
    public int remainingJumps;
    public int extraJumps;
    public bool fastFalling;
    public float fastFallForce;
    public int jumpBufferSize;
    public List<bool> jumpBuffer;
    public float boundingBoxConstriction;
    public float playerRepulsion;
    public bool facingRight = true;

    private Vector2 movementInput = Vector2.zero;
    private bool jumpInput = false;

    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        for (int i = 0; i < quickTurnBufferSize; i++)
        {
            quickTurnBuffer.Add(0f);
        }
        for (int i = 0; i < jumpBufferSize; i++)
        {
            jumpBuffer.Add(false);
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpInput = context.action.triggered;
    }
    // Update is called once per frame
    void Update()
    {
        
        horizontalInput = movementInput.x;
        if ((horizontalInput < 0 && facingRight) || (horizontalInput > 0 && !facingRight))
        {
            facingRight = !facingRight;
            gameObject.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        jumpBuffer.RemoveAt(0);
        jumpBuffer.Add(jumpInput);
        quickTurnBuffer.RemoveAt(0);
        quickTurnBuffer.Add(horizontalInput);
        
        GroundCheck();
        if (isGrounded)
        {
            controlFactor = 1;
            remainingJumps = extraJumps;

        }
        else
        {
            controlFactor = airControlFactor;
        }
        

        if (Mathf.Abs(horizontalInput) > deadzone)
        {
            bool quickTurn = false;
            foreach (var item in quickTurnBuffer)
            {
                if ((item < 0 && horizontalInput > 0) || (item > 0 && horizontalInput < 0))
                {
                    quickTurn = true;
                    for (int i = 0; i < quickTurnBuffer.Count; i++)
                    {
                        quickTurnBuffer[i] = 0;
                    }
                    break;
                }
            }
            if (quickTurn)
            {
                rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y, 0);
            }
            rb.AddForce(new Vector3(moveSpeed * horizontalInput * acceleration * Time.deltaTime * controlFactor, 0, 0), ForceMode.Impulse);
        }

        if (Mathf.Abs(rb.velocity.x) > speedCap)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(speedCap * horizontalInput, rb.velocity.y, 0);
            }
            else
            {
                rb.velocity = new Vector3(speedCap * Mathf.RoundToInt(horizontalInput), rb.velocity.y, 0);
            }
        }

        if (isGrounded && Mathf.Abs(horizontalInput) < deadzone)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        if (jumpInput)
        {
            if (remainingJumps >= 1 || isGrounded)
            {
                Jump();
            }
            

        }

        if (!isGrounded && Mathf.Abs(rb.velocity.y) < 0.3 && movementInput.y < -0.5 && !fastFalling)
        {
            rb.AddForce(new Vector3(0, -fastFallForce, 0), ForceMode.Impulse);
            fastFalling = true;
        }

        if (gameObject.transform.position.y < -10)
        {
            Respawn();
        }

       
    }

    void GroundCheck()
    {
        
        Bounds bounds = gameObject.GetComponent<MeshFilter>().mesh.bounds;
        Vector3 loc = gameObject.transform.position;
        Vector3 boxCenter = new Vector3(loc.x, loc.y - bounds.extents.y - groundingBoxHeight, loc.z);
        Vector3 boxSize = new Vector3(bounds.extents.x-boundingBoxConstriction, groundingBoxHeight, bounds.extents.z-boundingBoxConstriction);
        bool boxHit = Physics.CheckBox(boxCenter, boxSize, new Quaternion(0, 0, 0, 0), whatIsGround);
        if (boxHit)
        {
            OnGrounded();
        }
        else
        {
            isGrounded = false;
        }
        Debug.DrawLine(new Vector3(boxCenter.x, boxCenter.y + boxSize.y, boxCenter.z), new Vector3(boxCenter.x, boxCenter.y - boxSize.y, boxCenter.z));
    }

    void OnGrounded()
    {
        if (!isGrounded)
        {
            isGrounded = true;
            rb.velocity = new Vector3(horizontalInput * moveSpeed, 0, 0);
        }
        if (jumpBuffer.Contains(true) || jumpInput)
        {
            Jump();
        }
    }

    void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        fastFalling = false;
        remainingJumps -= 1;
    }

    void Respawn()
    {
        gameObject.transform.position = new Vector3(0, -2, 0);
    }

}
