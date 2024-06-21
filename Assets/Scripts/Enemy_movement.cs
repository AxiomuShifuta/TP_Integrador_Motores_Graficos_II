using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    private Rigidbody enemyRb;
    [SerializeField] Transform playerPosition;
    private Vector3 direction;
    
    [SerializeField] float enemyNormalSpeed;
    [SerializeField] float enemyChargeSpeed;
    [SerializeField] float chargeCooldown = 3f;
    private float maxChargeDistance = 3f;
    private float minFollowDistance = 15f;

    

    [SerializeField] private bool isCharging = false;

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
         //if (!isCharging && enemyRb.velocity.magnitude > 0)
         //   {
         //       enemyRb.velocity = Vector3.Lerp(enemyRb.velocity, Vector3.zero, 0.1f);
         //   }
    }

    IEnumerator EnemyAI()
    {
        while (true)
        {
            // Seguimiento normal mientras esté fuera del rango de embestida
            while (!isCharging && Vector3.Distance(transform.position, playerPosition.position) > minFollowDistance)
            {
                yield return StartCoroutine(FollowPlayer());
            }

            // Si está dentro del rango de embestida, carga hacia el jugador
            if (!isCharging && Vector3.Distance(transform.position, playerPosition.position) <= minFollowDistance)
            {
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

            float chargeDuration = 0.2f; 
            float timer = 0f;
            Vector3 direction = (playerPosition.position - transform.position).normalized;
            direction.y = 0f;
            /*Apunta hacia la última posición del jugador y luego embiste hacia allí.
             Si esa instrucción estuviese dentro del while, actualizaría su dirección
            durante la embestida y sería demasiado teledirigido.*/

            while (timer < chargeDuration)
            {
                enemyRb.velocity = direction * enemyChargeSpeed;
                timer += Time.deltaTime;
                yield return null;
            }

        while (enemyRb.velocity.magnitude != 0 && Vector3.Distance(transform.position, playerPosition.position) > maxChargeDistance)
        {
            // Frena gradualmente el movimiento del enemigo
            enemyRb.velocity = Vector3.Lerp(enemyRb.velocity, Vector3.zero, 0.1f);
            yield return null;
        }

        // Reiniciar movimiento y banderas después de la embestida y frenado
        isCharging = false;
        enemyRb.velocity = Vector3.zero; // Detener completamente el movimiento
        yield return new WaitForSeconds(chargeCooldown);


        /*Una vez que se cumple el tiempo de duración programado para la embestida, se evalúa
         la distancia entre enemigo y jugador. Si supera el valor de distancia máxima, se desactiva
        la bandera isCharging para dar paso a la instrucción de frenado con Lerp dentro del método
        FixedUpdate.
        Si no estuviese el cambio de bandera, podría ocurrir que el enemigo hiciese embestidas extremadamente
        cortas si se encuentra muy lejos del jugador la próxima vez que se ejecute la corrutina.
        Si bien la bandera no evita que el enemigo se quede corto con la embestida en caso de que haya
        mucha distancia de por medio en cuanto reinice el ciclo de la corrutina, la manera en la que están 
        organizadas las instrucciones asegura que la acción se cumpla durante el tiempo configurado 
        en la variable chargeDuration y recién luego evalúe la distancia.*/

    }

}
