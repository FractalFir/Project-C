using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMeshController : MonoBehaviour
{
    bool hasRecentered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasRecentered){
            Mesh mesh = this.gameObject.GetComponent<MeshFilter>().mesh;
            transform.position = MeshBuilder.Recenter(mesh);
            this.gameObject.GetComponent<MeshFilter>().mesh = mesh;
            this.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
            hasRecentered = true;
        }
    }
}
