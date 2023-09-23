using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private float waitTime;
    [SerializeField] private float glideJumpForce;
    [SerializeField] private float glideSpeed;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private UnityEngine.UI.Slider hpSlider;
    [SerializeField] private UnityEngine.UI.Image fillColor;
    [SerializeField] private Color pinkHP, redHP;
    [SerializeField] private TMP_Text cherryText;
    [SerializeField] private AudioClip jumpSound, hurtSound;
    [SerializeField] private AudioClip[] pickupSounds;
    [SerializeField] private GameObject gemParticles, jumpParticles;

    //wallclimb
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    private bool activateTimer = false;
    private bool isGliding;
    private bool isGrounded;
    private bool canMove;
    private float jumpTimer = 0f;
    private float horizontalValue;
    private float rayDistance = 0.25f;
    private Animator anim;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private AudioSource audioSource;
    private int startingHealth = 1;
    private int currentHealth = 0;
    public int cherriesFound = 0;

    // Start is called before the first frame update, here we can connect Variables to Components inside Unity for example
    private void Start()
    {
        jumpTimer = 0f;
        canMove = true;
        currentHealth = startingHealth;
        cherryText.text = "" + cherriesFound;
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame, as fast as possible
    private void Update()
    {
        horizontalValue = Input.GetAxis("Horizontal");

        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }
        else if(horizontalValue > 0)
        {
            FlipSprite(false);
        }


        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true) //hoppa
        {
            Jump();
        }
        else if (Input.GetButtonDown("Jump") && CheckIfGrounded() == false) //glida
        {
            Glide();
        }
        else if(CheckIfGrounded() == true)
        {
            rgbd.drag = 0f;
            isGliding = false;
        }

        if (activateTimer == true) //Cooldown för glidhopp
        {
            jumpTimer -= 1 * Time.deltaTime;
            print(jumpTimer);
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.velocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.velocity.y);
        anim.SetBool("IsGrounded", CheckIfGrounded());
        anim.SetBool("isGliding", isGliding);


        //wallclimb
        vertical = Input.GetAxis("Vertical");

        if (isLadder == true && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }

    }

    //wallclimb

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cherry"))
        {
            Destroy(other.gameObject);
            cherriesFound++;
            cherryText.text = "" + cherriesFound;
            // Using an array[]
            int randomValue = Random.Range(0, pickupSounds.Length);
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(pickupSounds[randomValue], 0.3f);
        }

        //wallclimb
        if (other.CompareTag("Wall"))//för en stege
        {
            isLadder = true;
        }

        if (other.CompareTag("HPGem"))
        {
            RestoreHP(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //för en stege
    {
        if (collision.CompareTag("Wall"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //för klättring
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isLadder = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) //för kättring
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void Glide()//glidning
    {
        rgbd.drag = fallSpeed;

        if (Input.GetButtonDown("Jump") && isGliding == true && jumpTimer <= 0f) //Ett hopp medan man glider
        {
            rgbd.AddForce(new Vector2(0, glideJumpForce));
            jumpTimer = waitTime;
            activateTimer = true; //aktiverar timern i update
        }

        isGliding = true;
    }

    // FixedUpdate is always called at the same speed regardless of fps
    // "!" in if() context means "not" ergo if(!canMove) = if(canMove == false)
    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        if(rgbd.drag == 0)//farten om man är på marken eller hoppar
        {
            rgbd.velocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rgbd.velocity.y);
        }
        else if(rgbd.drag != 0)//farten om man glider
        {
            rgbd.velocity = new Vector2(horizontalValue * glideSpeed * Time.deltaTime, rgbd.velocity.y);
        }

        //wallclimbing
        if (isClimbing == true) //klättra verticalt
        {
            rgbd.gravityScale = 0f;
            rgbd.velocity = new Vector2(rgbd.velocity.x, vertical * speed);
        }
        else if (isClimbing == false)
        {
            rgbd.gravityScale = 2.5f;
        }
    }

    // private = Security level, void = Return type, FlipSprite = Method name, () = Parameter
    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    private void Jump()
    {
        rgbd.AddForce(new Vector2(0, jumpForce));
        // Without the .pitch = 1.0f here the pitch change above will change the sound of jumps and not just pickups
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(jumpSound, 0.3f);
        Instantiate(jumpParticles, transform.position, jumpParticles.transform.localRotation);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHPSlider();
        audioSource.PlayOneShot(hurtSound, 0.5f);

        if (currentHealth <= 0)
        {
            Invoke("Respawn", 0.5f);
        }
    }

    public void TakeKnockback(float knockBackForce, float knockUpForce)
    {
        canMove = false;
        // Using AddForce without resetting velocity will cause weird velocities in the player object
        rgbd.velocity = new Vector2(0, 0);
        rgbd.AddForce(new Vector2(knockBackForce, knockUpForce));
        Invoke("CanMoveAgain", 0.5f);
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }

    private void Respawn()
    {
        currentHealth = startingHealth;
        UpdateHPSlider();
        transform.position = spawnPoint.position;
        rgbd.velocity = Vector2.zero;
    }

    private void UpdateHPSlider()
    {
        hpSlider.value = currentHealth;
        if(currentHealth >= 2)
        {
            fillColor.color = pinkHP;
        }

        else
        {
            fillColor.color = redHP;
        }

    }

    private void RestoreHP(GameObject hpGem)
    {
        if(currentHealth >= startingHealth)
        {
            return;
        }

        else
        {
            int healthToRestore = hpGem.GetComponent<HealthPickup>().healthAmount;
            currentHealth += healthToRestore;
            UpdateHPSlider();
            Instantiate(gemParticles, hpGem.transform.position, Quaternion.identity);
            Destroy(hpGem);


            if (currentHealth >= startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }


    // Since the return type of this method is bool we have to return something
    // or = ||
    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistance, whatIsGround);

        // Debug.DrawRay(leftFoot.position, Vector2.down * rayDistance, Color.blue, 0.25f);
        // Debug.DrawRay(rightFoot.position, Vector2.down * rayDistance, Color.red, 0.25f);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
