using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOposumMovement : MonoBehaviour
{
    // SerializeField makes a private variable available through the editor (Unity)
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float bounceForce = 100f;
    [SerializeField] private float knockBackForce = 200f;
    [SerializeField] private float knockUpForce = 200f;
    [SerializeField] private int damageGiven = 1;
    private SpriteRenderer rend;
    private bool canMove = true;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // transform.Translate() is another way to move something without the use of a RigidBody
    void FixedUpdate()
    {

        if (!canMove)
            return;

        transform.Translate(new Vector2 (moveSpeed, 0) * Time.deltaTime);

        if(moveSpeed > 0 )
        {
            rend.flipX = true;
        }

        if(moveSpeed < 0 )
        {
            rend.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyPathBlocker"))
        {
            moveSpeed = -moveSpeed;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = -moveSpeed;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage(damageGiven);

            if(other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(knockBackForce, knockUpForce);
            }

            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(-knockBackForce, knockUpForce);
            }
        }
    }

    // gameObject refers to the larger entity which the script is attached to in game
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounceForce));
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            canMove = false;
            Destroy(gameObject, 0.5f);

        }
    }
}
