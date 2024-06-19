using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_behaviour : MonoBehaviour
{
    public GameObject playerPosition;
    private Vector3 offsetPosition;
    // Start is called before the first frame update
    void Start()
    {
        offsetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offsetPosition + playerPosition.transform.position;
    }
}
