using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceFrog : MonoBehaviour
{
    [SerializeField] private float bounceForce = 200f;
    [SerializeField] private float knockBackForce = 200f;
    [SerializeField] private float knockUpForce = 200f;
    [SerializeField] private int damageGiven = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRigidBody = collision.GetComponent<Rigidbody2D>();
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            playerRigidBody.AddForce(new Vector2 (0, bounceForce));
            GetComponent<Animator>().SetTrigger("FrogBounce");
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage(damageGiven);

            if (other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(knockBackForce, knockUpForce);
            }

            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(-knockBackForce, knockUpForce);
            }
        }
    }
}
