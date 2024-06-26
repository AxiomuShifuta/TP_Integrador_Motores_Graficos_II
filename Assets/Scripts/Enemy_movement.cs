using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    private Rigidbody enemyRb;
    [SerializeField] Transform playerPosition;
    public Vector3 direction;
    
    [SerializeField] float enemyNormalSpeed;
    [SerializeField] float enemyChargeSpeed;
    [SerializeField] float chargeCooldown = 3f;
    private float maxChargeDistance = 3f;
    private float minFollowDistance = 9f;

    
    public bool isCharging = false;
    public bool hitBWall = false;

    public delegate void DelImpact();
    public event DelImpact onDelImpact;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        StartCoroutine(EnemyAI());
    }
    
    IEnumerator EnemyAI()
    {
        while (true)
        {
            // Seguimiento normal mientras est� fuera del rango de embestida
            while (!isCharging && Vector3.Distance(transform.position, playerPosition.position) > minFollowDistance)
            {
                yield return StartCoroutine(FollowPlayer());
            }

            // Si est� dentro del rango de embestida, frena a cero, espera y  carga hacia el jugador
            if (!isCharging && Vector3.Distance(transform.position, playerPosition.position) <= minFollowDistance)
            {
                enemyRb.velocity = Vector3.zero;
                yield return StartCoroutine(WaitBeforeCharge());
                yield return StartCoroutine(ChargeTowardsPlayer());
            }

            yield return null;
        }
    }

    IEnumerator FollowPlayer()
    {
        Vector3 direction = (playerPosition.position - transform.position).normalized;
        direction.y = 0f;

        enemyRb.velocity = direction * enemyNormalSpeed;

        yield return null;
    }


    IEnumerator WaitBeforeCharge()
    {
        // Espera un breve momento antes de iniciar la embestida
        yield return new WaitForSeconds(1f);
    }

    IEnumerator ChargeTowardsPlayer()
    {
           
            isCharging = true;

            Vector3 direction = (playerPosition.position - transform.position).normalized;
            direction.y = 0f;
            float chargeDuration = 0.3f; 
            float timer = 0f;
            
        yield return new WaitForSeconds(1f);
            /*Apunta hacia la �ltima posici�n del jugador y luego embiste hacia all�.
             Si esa instrucci�n estuviese dentro del while, actualizar�a su direcci�n
            durante la embestida y ser�a demasiado teledirigido.*/

            while (timer < chargeDuration && hitBWall == false)
            {
                enemyRb.velocity = direction * enemyChargeSpeed;
                timer += Time.deltaTime;
                yield return null;
            }

        while (enemyRb.velocity.magnitude >= 1 && Vector3.Distance(transform.position, playerPosition.position) > maxChargeDistance)
        {
            /* Frena gradualmente el movimiento del enemigo. En un principio hab�a establecido como primer
             condici�n del while que la magnitud de la velocidad sea mayor a cero, pero parece que por
            cuestiones del motor de f�sicas, a pesar de que el objeto parec�a completamente detenido, 
            en el inspector se pod�an ver variaciones muy peque�as en la posici�n, por lo que la condici�n
            segu�a cumpliendose y nunca sal�a del while.
            Al configurar que el frenado act�e hasta que sea menor que 1 en magnitud, el movimiento residual es 
            imperceptible y logra seguir ejecutando el c�digo posterior.*/

            enemyRb.velocity = Vector3.Lerp(enemyRb.velocity, Vector3.zero, 0.1f);
            yield return null;
        }

        // Reiniciar movimiento y banderas despu�s de la embestida y frenado
        isCharging = false;
        enemyRb.velocity = Vector3.zero; // Detener completamente el movimiento
        yield return new WaitForSeconds(chargeCooldown);


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("UnbreakableWall"))
        {
            StartCoroutine(Ricochet(enemyRb.velocity));
        }


        if (collision.gameObject.CompareTag("BreakableWall"))
        {
            hitBWall = true;
            Vector3 impactVelocity = enemyRb.velocity;
            enemyRb.velocity = Vector3.zero;
            StartCoroutine(Ricochet(impactVelocity));
            onDelImpact();
            hitBWall= false;
        }
    }

    private IEnumerator Ricochet(Vector3 impactVelocity)
    {
        
        float timer = 0f;
        impactVelocity.y = 0f;

        while (timer < 0.3f)
        {
            enemyRb.velocity = ((-impactVelocity.normalized) * 50);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
