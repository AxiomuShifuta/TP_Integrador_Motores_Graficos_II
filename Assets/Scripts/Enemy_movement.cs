using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    private Rigidbody enemyRb;
    [SerializeField] Transform playerPosition;
    private float targetX;
    private float targetZ;
    private Vector3 direction;
    [SerializeField] float enemySpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        targetX = playerPosition.position.x;
        targetZ = playerPosition.position.z;
        direction = new Vector3 (targetX, 0f, targetZ);
        Debug.Log(playerPosition.position - this.transform.position);
    }
    
    // Update is called once per frame
    void Update()
    {
        targetX = playerPosition.position.x;
        targetZ = playerPosition.position.z;
        direction = new Vector3(targetX, 0f, targetZ) - transform.position.normalized;

        if (Vector3.Distance(transform.position, playerPosition.position) > 2f)
        enemyRb.velocity = direction * enemySpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        
    }
}
