using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMSlowOverTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity *= 2.5f;
    }

    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity /= 1.03f;
    }
}
