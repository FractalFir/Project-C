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
    public float horizontalRotationSpeed = 180.0f;
    float cameraAngle = 0.0f;
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

    Quaternion ToGravityRotation() => Quaternion.Slerp(Quaternion.identity,Quaternion.FromToRotation(transform.up, -customGravity.gravity),Time.deltaTime*16.0f) * transform.rotation;
    void AlignToGravity(){
        transform.rotation = ToGravityRotation();
    }
    void MovePlayer(){
        rb.AddForce(transform.forward*moveForce*Time.deltaTime*Input.GetAxis("Vertical"));
        rb.AddForce(transform.right*moveForce*Time.deltaTime*Input.GetAxis("Horizontal"));
        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(transform.up*jumpForce);
        }
    }
    void RotateGravity(){
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            customGravity.gravity = transform.forward;
        }
        else if(Input.GetKeyUp(KeyCode.DownArrow)){
            customGravity.gravity = -transform.forward;
        }
        else if(Input.GetKeyUp(KeyCode.LeftArrow)){
            customGravity.gravity = -transform.right;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow)){
            customGravity.gravity = transform.right;
        }
    }
    void OnDrawGizmos(){
        if(customGravity != null){
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position,transform.forward);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position,customGravity.gravity);
            Gizmos.color = Color.red;
            Quaternion rot = ToGravityRotation();
            (Vector3 rotAxis,float ignored) = (new Vector3(),0.0f);
            rot.ToAxisAngle(out rotAxis,out ignored);
            Gizmos.DrawRay(transform.position,rotAxis);
        }
    }
    // Update is called once per frame
    void Update()
    {
        AlignToGravity();
        MovePlayer();
        RotateGravity();
        float mainAngle = Input.GetAxis("Mouse X") * rotationSpeed;
        cameraAngle +=  Input.GetAxis("Mouse Y") * horizontalRotationSpeed;
        //Quaternion cameraHorizontalRotation = Quaternion.FromToRotation(transform.forward,transform.right);
        //transform.localRotation *= Quaternion.Slerp(Quaternion.identity,cameraHorizontalRotation,Time.deltaTime*rotation);
        cameraAngle = Mathf.Clamp(cameraAngle,-60.0f,60.0f);
        camera.transform.localRotation = Quaternion.Euler(cameraAngle,0.0f,0.0f);
        transform.localRotation *= Quaternion.AngleAxis(mainAngle,CustomGravity.AxisSnap(transform.up));
        CursorControll();
    }
}
