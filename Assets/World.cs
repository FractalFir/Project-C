using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    WorldData data;
    public int worldSize = 64;
    const float WALL_SIZE = 0.125f;
    const float WORLD_SCALE = 3.0f;
    void CreateWallsAt(MeshBuilder meshBuilder,int ix,int iy,int iz){
        WallState currState = data.GetWallState(ix,iy,iz);
        float x = ix*WORLD_SCALE;
        float y = iy*WORLD_SCALE;
        float z = iz*WORLD_SCALE;
        if(currState.z){
            //Vector3 center = new Vector3(x + 0.5f,y + 0.5f,z + 0.125f);
            //Gizmos.DrawCube(center, new Vector3(1.0f,1.0f,0.25f));    
            meshBuilder.AddQuad((
                (new Vector3(x,y,z + WALL_SIZE),new Vector2(x,y)),
                (new Vector3(x + WORLD_SCALE,y,z + WALL_SIZE),new Vector2(x + WORLD_SCALE,y)),
                (new Vector3(x + WORLD_SCALE,y + WORLD_SCALE,z + WALL_SIZE),new Vector2(x + WORLD_SCALE,y + WORLD_SCALE)),
                (new Vector3(x,y + WORLD_SCALE,z + WALL_SIZE),new Vector2(x,y + WORLD_SCALE))
            ));
            if(!data.GetWallState(ix + 1,iy,iz).z){
                meshBuilder.AddQuad((                  
                    (new Vector3(x + WORLD_SCALE,   y,                  z - WALL_SIZE),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y,                  z + WALL_SIZE),new Vector2(y + WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy + 1,iz).z){
                meshBuilder.AddQuad((                  
                    (new Vector3(x,                 y + WORLD_SCALE,    z + WALL_SIZE),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,                 y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy - 1,iz).z){
                meshBuilder.AddQuad((                  
                    (new Vector3(x,                 y,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x + WORLD_SCALE,   y,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,                 y,    z + WALL_SIZE),new Vector2(y + WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix - 1,iy,iz).z){
                meshBuilder.AddQuad((                    
                    (new Vector3(x,y,                  z + WALL_SIZE),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x,y + WORLD_SCALE,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,y,                  z - WALL_SIZE),new Vector2(y - WALL_SIZE,x))
                ));
            }
            meshBuilder.AddQuad((
                (new Vector3(x,y,z - WALL_SIZE),new Vector2(x,y)),
                (new Vector3(x,y + WORLD_SCALE,z - WALL_SIZE),new Vector2(x,y + WORLD_SCALE)),
                (new Vector3(x + WORLD_SCALE,y + WORLD_SCALE,z - WALL_SIZE),new Vector2(x + WORLD_SCALE,y + WORLD_SCALE)),
                (new Vector3(x + WORLD_SCALE,y,z - WALL_SIZE),new Vector2(x + WORLD_SCALE,y))
            ));
        }
        if(currState.y){
            //Vector3 center = new Vector3(x + 0.5f,y + 0.125f,z + 0.5f);
            //Gizmos.DrawCube(center, new Vector3(1.0f,0.25f,1.0f));
            meshBuilder.AddQuad((
                (new Vector3(x,y + WALL_SIZE, z + WORLD_SCALE),new Vector2(x,z + WORLD_SCALE)),
                (new Vector3(x + WORLD_SCALE,y + WALL_SIZE,z + WORLD_SCALE),new Vector2(x + WORLD_SCALE,z + WORLD_SCALE)),
                (new Vector3(x + WORLD_SCALE,y + WALL_SIZE,z),new Vector2(x + WORLD_SCALE,z)),
                (new Vector3(x,y + WALL_SIZE,z),new Vector2(x,z))
            ));
            if(!data.GetWallState(ix + 1,iy,iz).y){
                meshBuilder.AddQuad((
                    (new Vector3(x + WORLD_SCALE,y + WALL_SIZE, z),new Vector2(x + WALL_SIZE,z)),
                    (new Vector3(x + WORLD_SCALE,y + WALL_SIZE, z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,y - WALL_SIZE, z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,y - WALL_SIZE, z),new Vector2(x - WALL_SIZE,z))
                ));
            }
            if(!data.GetWallState(ix - 1,iy,iz).y){
                meshBuilder.AddQuad((
                    (new Vector3(x,y - WALL_SIZE, z),new Vector2(x - WALL_SIZE,z)),
                    (new Vector3(x,y - WALL_SIZE, z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y + WALL_SIZE, z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y + WALL_SIZE, z),new Vector2(x + WALL_SIZE,z))
                ));
            }
            if(!data.GetWallState(ix,iy,iz + 1).y){
                meshBuilder.AddQuad((
                    (new Vector3(x,y - WALL_SIZE,                   z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z)),
                    (new Vector3(x + WORLD_SCALE, y - WALL_SIZE,    z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE, y + WALL_SIZE,    z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y + WALL_SIZE,                   z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z))
                ));
            }
            if(!data.GetWallState(ix,iy,iz - 1).y){
                meshBuilder.AddQuad((
                    (new Vector3(x,y + WALL_SIZE,               z),new Vector2(x - WALL_SIZE,z)),
                    (new Vector3(x + WORLD_SCALE,y + WALL_SIZE, z),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,y - WALL_SIZE, z),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y - WALL_SIZE,               z),new Vector2(x + WALL_SIZE,z))
                ));
            }
            meshBuilder.AddQuad((
                (new Vector3(x,y - WALL_SIZE,z),new Vector2(x,z)),
                (new Vector3(x + WORLD_SCALE,y - WALL_SIZE,z),new Vector2(x + WORLD_SCALE,z)),
                (new Vector3(x + WORLD_SCALE,y - WALL_SIZE,z + WORLD_SCALE),new Vector2(x + WORLD_SCALE,z + WORLD_SCALE)),
                (new Vector3(x,y - WALL_SIZE, z + WORLD_SCALE),new Vector2(x,z + WORLD_SCALE))
            ));           
        }
        if(currState.x){
            //Vector3 center = new Vector3(x + 0.125f,y + 0.5f,z + 0.5f);
            //Gizmos.DrawCube(center, new Vector3(0.25f,1.0f,1.0f));
            meshBuilder.AddQuad((
                (new Vector3(x + WALL_SIZE,y,z),new Vector2(y,z)),
                (new Vector3(x + WALL_SIZE,y + WORLD_SCALE,z),new Vector2(y + WORLD_SCALE,z)),
                (new Vector3(x + WALL_SIZE,y + WORLD_SCALE,z + WORLD_SCALE),new Vector2(y + WORLD_SCALE,z + WORLD_SCALE)),
                (new Vector3(x + WALL_SIZE,y, z + WORLD_SCALE),new Vector2(y,z + WORLD_SCALE))
            ));
            if(!data.GetWallState(ix,iy + 1,iz).x){
                meshBuilder.AddQuad((             
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y + WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy - 1,iz).x){
                meshBuilder.AddQuad((             
                    (new Vector3(x + WALL_SIZE,   y,    z),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x + WALL_SIZE,   y,    z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y,    z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y,    z),new Vector2(y - WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy,iz + 1).x){
                meshBuilder.AddQuad((             
                    (new Vector3(x + WALL_SIZE,   y,                  z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y,                  z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy,iz - 1).x){
                meshBuilder.AddQuad((             
                    (new Vector3(x - WALL_SIZE,   y,                  z),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y,                  z),new Vector2(y + WALL_SIZE,x))
                ));
            }
            meshBuilder.AddQuad((
                (new Vector3(x - WALL_SIZE,y, z + WORLD_SCALE),new Vector2(y,z + WORLD_SCALE)),
                (new Vector3(x - WALL_SIZE,y + WORLD_SCALE,z + WORLD_SCALE),new Vector2(y + WORLD_SCALE,z + WORLD_SCALE)),
                (new Vector3(x - WALL_SIZE,y + WORLD_SCALE,z),new Vector2(y + WORLD_SCALE,z)),
                (new Vector3(x - WALL_SIZE,y,z),new Vector2(y,z))
            ));
        }
    }
    void CreateWalls(){
        MeshBuilder meshBuilder = new MeshBuilder();
        for(int ix = 0; ix <= worldSize; ix++){
            for(int iy = 0; iy <= worldSize; iy++){
                for(int iz = 0; iz <= worldSize; iz++){
                    CreateWallsAt(meshBuilder,ix,iy,iz);
                }
            }
        }
        Mesh[] meshes = meshBuilder.IntoMeshes(65536);
        Material material = this.gameObject.GetComponent<MeshRenderer>().material;
        foreach(Mesh mesh in meshes){
            GameObject go = new GameObject();
            //go.transform.position = MeshBuilder.Recenter(mesh);
            go.AddComponent<WorldMeshController>();
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshCollider>().sharedMesh = mesh;
            go.AddComponent<MeshRenderer>().material = material;
        }
        //this.gameObject.GetComponent<MeshFilter>().mesh = meshBuilder.IntoMesh();
    }
    // Start is called before the first frame update
    void Start()
    {
        data = new WorldData(worldSize);
        data.CarveRandomHoles();
        CreateWalls();
    }
    void OnDrawGizmos(){
        if(data != null){
            //data.DebugDisplay();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
