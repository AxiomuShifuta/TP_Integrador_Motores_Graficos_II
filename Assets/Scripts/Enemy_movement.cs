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
            // Seguimiento normal mientras est� fuera del rango de embestida
            while (!isCharging && Vector3.Distance(transform.position, playerPosition.position) > minFollowDistance)
            {
                yield return StartCoroutine(FollowPlayer());
            }

            // Si est� dentro del rango de embestida, carga hacia el jugador
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
            /*Apunta hacia la �ltima posici�n del jugador y luego embiste hacia all�.
             Si esa instrucci�n estuviese dentro del while, actualizar�a su direcci�n
            durante la embestida y ser�a demasiado teledirigido.*/

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

        // Reiniciar movimiento y banderas despu�s de la embestida y frenado
        isCharging = false;
        enemyRb.velocity = Vector3.zero; // Detener completamente el movimiento
        yield return new WaitForSeconds(chargeCooldown);


        /*Una vez que se cumple el tiempo de duraci�n programado para la embestida, se eval�a
         la distancia entre enemigo y jugador. Si supera el valor de distancia m�xima, se desactiva
        la bandera isCharging para dar paso a la instrucci�n de frenado con Lerp dentro del m�todo
        FixedUpdate.
        Si no estuviese el cambio de bandera, podr�a ocurrir que el enemigo hiciese embestidas extremadamente
        cortas si se encuentra muy lejos del jugador la pr�xima vez que se ejecute la corrutina.
        Si bien la bandera no evita que el enemigo se quede corto con la embestida en caso de que haya
        mucha distancia de por medio en cuanto reinice el ciclo de la corrutina, la manera en la que est�n 
        organizadas las instrucciones asegura que la acci�n se cumpla durante el tiempo configurado 
        en la variable chargeDuration y reci�n luego eval�e la distancia.*/

    }

}
