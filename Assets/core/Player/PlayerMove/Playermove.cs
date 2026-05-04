using TMPro;
using UnityEngine;

public class LegMove : MonoBehaviour
{
    public float speed = 1f;

    public Animator animator;
    public TextMeshProUGUI directionText;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (animator != null)
        {
            if (moveInput.magnitude > 0)
            {
                animator.SetFloat("moveX", moveInput.x);
                animator.SetFloat("moveY", moveInput.y);
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        UpdateDirectionText();
    }

    void UpdateDirectionText()
    {
        directionText.text = (moveInput.x, moveInput.y) switch
        {
            ( > 0, 0) => ">",  
            ( < 0, 0) => "<",  
            (0, > 0) => "^",  
            (0, < 0) => "v",  
            _ => directionText.text
        };
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * speed;
    }
}
