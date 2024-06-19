using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    private Rigidbody enemyRb;
    [SerializeField] Transform playerPosition;
    private Vector3 direction;
    
    [SerializeField] float enemySpeed;
    [SerializeField] float chargeCooldown = 3f;
    private float maxChargeDistance = 2f;
    

    private bool isCharging = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        StartCoroutine(ChargeTowardsPlayer());
    }
    
    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {
        if (!isCharging && enemyRb.velocity.magnitude > 0)
        {
            enemyRb.velocity = Vector3.Lerp(enemyRb.velocity, Vector3.zero, 0.1f);
        }
    }

    IEnumerator ChargeTowardsPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(chargeCooldown);


            isCharging = true;

            // Charge towards player
            float chargeDuration = 0.2f; // Adjust as needed
            float timer = 0f;
            Vector3 direction = (playerPosition.position - transform.position).normalized;
            direction.y = 0f;
            while (timer < chargeDuration)
            {
                // Update direction towards player continuously

                enemyRb.velocity = direction * enemySpeed;
                timer += Time.deltaTime;
                yield return null;
            }

            if (Vector3.Distance(transform.position, playerPosition.position) >= maxChargeDistance)
            {
                isCharging = false;
            }

                

        }
    }

}
