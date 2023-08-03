using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    Vector3 _gravity = Vector3.up;
    public Vector3 gravity{
        get => _gravity;
        set => _gravity = Vector3.Normalize(value);
    }
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(_gravity*rb.mass*10.0f*Time.deltaTime*30.0f);
    }
}
