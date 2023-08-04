using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    public Vector3 _gravity = Vector3.up;
    public static Vector3 AxisSnap(Vector3 input){
        float ax = Mathf.Abs(input.x);
        float ay = Mathf.Abs(input.y);
        float az = Mathf.Abs(input.z);
        if(ax > ay){
            if(ax > az) return new Vector3(Mathf.Sign(input.x),0.0f,0.0f);
            else return new Vector3(0.0f,0.0f,Mathf.Sign(input.z));
        }
        else{
            if(ay > az) return new Vector3(0.0f,Mathf.Sign(input.y),0.0f);
            else return new Vector3(0.0f,0.0f,Mathf.Sign(input.z));
        }   
    }
    public Vector3 gravity{
        get => _gravity;
        set => _gravity = AxisSnap(value);
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
