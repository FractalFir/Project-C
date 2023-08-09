using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CustomGravity customGravity;
    Rigidbody rb;
    // Movoment
    public float moveForce = 250.0f;
    public float jumpForce = 250.0f;
    
    public float gravityChangeSpeed = 4.0f;
    // Camera controlss
    float cameraAngle = 0.0f;
    public float rotationSpeed = 45.0f;
    public GameObject camera;
    // Used to lock the crusor
    bool cursorLockState = true;
    // Used to slow down time
    private float fixedDeltaTime;
    // Gravity change UI
    public GameObject gravityUI;
    bool isInGravityChangeUI;
    Vector2 uiPos;
    public GameObject gravUICursor;
    public GameObject[] gravChangeUIArrows;
    // Start is called before the first frame update
    void Start()
    {
        customGravity = gameObject.GetComponent<CustomGravity>();
        rb = gameObject.GetComponent<Rigidbody>();
        UpdateCursor();
        fixedDeltaTime = Time.fixedDeltaTime;
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
    void ChangeGravityUI(){
        if (Input.GetButtonDown("Fire1"))
        {
            Time.timeScale = 0.25f;   
            gravityUI.SetActive(true); 
            isInGravityChangeUI = true;
            uiPos = Vector2.zero;   
        }
        else if (Input.GetButtonUp("Fire1")){
            Time.timeScale = 1.0f;
            gravityUI.SetActive(false);
            isInGravityChangeUI = false;
            Vector2 changeDir = uiPos.normalized;
            if(uiPos.magnitude < 48.0f){
                changeDir = Vector2.zero;
            }
            RotateGravity(changeDir);
        }
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
    void CursorControll(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            cursorLockState = !cursorLockState;
            UpdateCursor();
        }
    }

    Quaternion ToGravityRotation() => Quaternion.Slerp(Quaternion.identity,Quaternion.FromToRotation(transform.up, -customGravity.gravity),Time.deltaTime*gravityChangeSpeed) * transform.rotation;
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
    void RotateGravity(Vector2 dir){
        if(dir == Vector2.zero)return;
        dir = CustomGravity.AxisSnap(dir);
        Debug.Log($"changeDir:{dir}");
        if(dir == Vector2.up){
            customGravity.gravity = transform.forward;
        }
        else if(dir == Vector2.down){
            customGravity.gravity = -transform.forward;
        }
        else if(dir == Vector2.left){
            customGravity.gravity = -transform.right;
        }
        else if(dir == Vector2.right){
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
    void CameraRotation(){
        float mainAngle = Input.GetAxis("Mouse X") * rotationSpeed;
        cameraAngle -=  Input.GetAxis("Mouse Y") * rotationSpeed;
        //Quaternion cameraHorizontalRotation = Quaternion.FromToRotation(transform.forward,transform.right);
        //transform.localRotation *= Quaternion.Slerp(Quaternion.identity,cameraHorizontalRotation,Time.deltaTime*rotation);
        cameraAngle = Mathf.Clamp(cameraAngle,-60.0f,60.0f);
        camera.transform.localRotation = Quaternion.Euler(cameraAngle,0.0f,0.0f);
        transform.localRotation *= Quaternion.AngleAxis(mainAngle, Vector3.up);
    }
    void GravityChangeUI(){
        uiPos += new Vector2(Input.GetAxis("Mouse X")*32.0f,Input.GetAxis("Mouse Y")*32.0f);
        if(uiPos.magnitude > 100.0f)uiPos = uiPos.normalized * 100.0f;
        gravUICursor.transform.localPosition = uiPos;
        GameObject closestArrow = null;
        float minDistance = 1000000.0f;
        foreach(GameObject gravArrow in gravChangeUIArrows){
            float currDistance = Vector2.Distance(uiPos,gravArrow.transform.localPosition);
            if(currDistance < minDistance){
                minDistance = currDistance;
                closestArrow = gravArrow;
                gravArrow.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
            }
        }
        closestArrow.transform.localScale = new Vector3(1.25f,1.25f,1.0f);
    }
    // Update is called once per frame
    void Update()
    {
        AlignToGravity();
        MovePlayer();
        //RotateGravity();
        ChangeGravityUI();
        if(!isInGravityChangeUI)CameraRotation();
        else GravityChangeUI();   
        CursorControll();
    }
}
