using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_Wall : MonoBehaviour
{
    public Enemy_movement enemy;
    private float bounceForce = 100f; // Fuerza del rebote.
    private int breakCount = 2;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == enemy.gameObject)
        {
            if (enemy.isCharging)
            {
                
                Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
                if (enemyRigidbody != null)
                {
                    enemyRigidbody.velocity = Vector3.zero;
                    // Calcula dirección del rebote
                    Vector3 collisionPoint = collision.contacts[0].point;
                    Debug.Log("Collision point: " + collisionPoint);
                    Vector3 enemyPosition = enemy.transform.position;
                    Debug.Log("Enemy position: " + enemyPosition);
                    Vector3 bounceDirection = (enemyPosition - collisionPoint).normalized;
                    bounceDirection.y = 0f;
                    Debug.Log("Bounce direction: " + bounceDirection);

                    // Aplica la fuerza de rebote
                    enemyRigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
                    breakCount--;
                }

                if(breakCount <= 0)
                Destroy(this.gameObject);
            }
        }
    }
}
