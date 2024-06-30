using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_Wall : MonoBehaviour
{
    public Enemy_movement enemy;
    private int breakCount = 2;

    private void Start()
    {
        enemy.onDelImpact += ShouldBreak;
    }

    private void Update()
    {
            ChangeColor();
    }
    private void ShouldBreak()
    {
        breakCount--;
        if (breakCount <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void ChangeColor()
    {
        if (breakCount == 1)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
