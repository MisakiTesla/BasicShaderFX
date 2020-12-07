using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("ball");
            other.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
        }
    }
}
