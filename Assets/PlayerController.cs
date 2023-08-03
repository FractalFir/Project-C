using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CustomGravity customGravity;
    Rigidbody rb;
    public float moveForce = 250.0f;
    public float jumpForce = 250.0f;
    public float rotationSpeed = 45.0f;
    public GameObject camera;
    bool cursorLockState = true;
    // Start is called before the first frame update
    void Start()
    {
        customGravity = gameObject.GetComponent<CustomGravity>();
        rb = gameObject.GetComponent<Rigidbody>();
        UpdateCursor();
    }
    void UpdateCursor(){
        if (cursorLockState)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void CursorControll(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            cursorLockState = !cursorLockState;
            UpdateCursor();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Quaternion.FromToRotation(-transform.up,customGravity.gravity);
        transform.localRotation *= Quaternion.Slerp(Quaternion.identity,rot,Time.deltaTime*8.0f);
        Debug.Log($"rot:{customGravity.gravity}");
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            Vector3 prev = customGravity.gravity;
            customGravity.gravity = -transform.forward;
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow)){
            Vector3 prev = customGravity.gravity;
            customGravity.gravity = transform.forward;
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow)){
            Vector3 prev = customGravity.gravity;
            customGravity.gravity = Vector3.right;
        }
        rb.AddForce(transform.forward*moveForce*Time.deltaTime*Input.GetAxis("Vertical"));
        rb.AddForce(transform.right*moveForce*Time.deltaTime*Input.GetAxis("Horizontal"));
        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(transform.up*jumpForce);
        }
        float rotation = Input.GetAxis("Mouse X") * rotationSpeed;
        //Quaternion cameraHorizontalRotation = Quaternion.FromToRotation(transform.forward,transform.right);
        //transform.localRotation *= Quaternion.Slerp(Quaternion.identity,cameraHorizontalRotation,Time.deltaTime*rotation);
        CursorControll();
    }
}
