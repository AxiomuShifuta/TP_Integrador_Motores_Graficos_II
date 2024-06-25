using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        StartCoroutine(EnemyAI());
    }
    
    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {
      
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
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator ChargeTowardsPlayer()
    {
           
            isCharging = true;

            float chargeDuration = 0.4f; 
            float timer = 0f;
            Vector3 direction = (playerPosition.position - transform.position).normalized;
            direction.y = 0f;
            /*Apunta hacia la �ltima posici�n del jugador y luego embiste hacia all�.
             Si esa instrucci�n estuviese dentro del while, actualizar�a su direcci�n
            durante la embestida y ser�a demasiado teledirigido.*/

            while (timer < chargeDuration)
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

}
