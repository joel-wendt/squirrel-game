using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float knockBackForce = 200f;
    [SerializeField] private float knockUpForce = 200f;
    [SerializeField] private int damageGiven = 1;

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
