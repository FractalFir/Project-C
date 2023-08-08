using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RoomMeshConstructor{
    MeshBuilder wallEndBuilder;
    MeshBuilder[] wallStyleBuilders;
    public RoomMeshConstructor(int styleCount){
        this.wallEndBuilder = new MeshBuilder();
        this.wallStyleBuilders = new MeshBuilder[styleCount];
        for(int i = 0; i < styleCount; i++)this.wallStyleBuilders[i] = new MeshBuilder();
    }
    public MeshBuilder GetWallEndBuilder()=>wallEndBuilder;
    public MeshBuilder GetBuilderByStyle(int style){
        return this.wallStyleBuilders[style];
    }
    public void CreateVisuals(Material endMaterial,RoomStyle[] styles){
        Mesh[] meshes = wallEndBuilder.IntoMeshes(65536);
        foreach(Mesh mesh in meshes){
            GameObject go = new GameObject();
            //go.transform.position = MeshBuilder.Recenter(mesh);
            go.AddComponent<WorldMeshController>();
            go.AddComponent<MeshFilter>().mesh = mesh;
            go.AddComponent<MeshCollider>().sharedMesh = mesh;
            go.AddComponent<MeshRenderer>().material = endMaterial;
        }
        for(int i = 0; i < styles.Length; i++){
            MeshBuilder wallStyleBuilder = wallStyleBuilders[i];
            meshes = wallStyleBuilder.IntoMeshes(65536);
            foreach(Mesh mesh in meshes){
                GameObject go = new GameObject();
                //go.transform.position = MeshBuilder.Recenter(mesh);
                go.AddComponent<WorldMeshController>();
                go.AddComponent<MeshFilter>().mesh = mesh;
                go.AddComponent<MeshCollider>().sharedMesh = mesh;
                go.AddComponent<MeshRenderer>().material = styles[i].roomMaterial;
            }
        }
    }
}
public class World : MonoBehaviour
{
    WorldData data;
    public int worldSize = 64;
    const float WALL_SIZE = 0.125f;
    public const float WORLD_SCALE = 3.0f;
    public Material wallEndMaterial; 
    public RoomStyle[] roomStyles;
    Color ColorFromDirection(Vector3 dir){
        Vector3 encoded = new Vector3(0.5f,0.5f,0.5f) + (dir/2.0f);
        return new Color(encoded.x,encoded.y,encoded.z,0.5f);
    }
    void CreateWallsAt(RoomMeshConstructor roomBuilder,int ix,int iy,int iz){
        WallState currState = data.GetWallState(ix,iy,iz);
        float x = ix*WORLD_SCALE;
        float y = iy*WORLD_SCALE;
        float z = iz*WORLD_SCALE;
        Color mainDir = ColorFromDirection(data.QueryRoomDirection(ix,iy,iz));
        MeshBuilder mainMeshBuilder = roomBuilder.GetBuilderByStyle(data.QueryRoom(ix,iy,iz).style);
        MeshBuilder meshEndBuilder = roomBuilder.GetWallEndBuilder();
        if(currState.z){
            //Vector3 center = new Vector3(x + 0.5f,y + 0.5f,z + 0.125f);
            //Gizmos.DrawCube(center, new Vector3(1.0f,1.0f,0.25f));    
            mainMeshBuilder.AddQuad((
                (new Vector3(x,y,z + WALL_SIZE),new Vector2(x,y),mainDir),
                (new Vector3(x + WORLD_SCALE,y,z + WALL_SIZE),new Vector2(x + WORLD_SCALE,y),mainDir),
                (new Vector3(x + WORLD_SCALE,y + WORLD_SCALE,z + WALL_SIZE),new Vector2(x + WORLD_SCALE,y + WORLD_SCALE),mainDir),
                (new Vector3(x,y + WORLD_SCALE,z + WALL_SIZE),new Vector2(x,y + WORLD_SCALE),mainDir)
            ));
            if(!data.GetWallState(ix + 1,iy,iz).z){
                meshEndBuilder.AddQuad((                  
                    (new Vector3(x + WORLD_SCALE,   y,                  z - WALL_SIZE),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y,                  z + WALL_SIZE),new Vector2(y + WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy + 1,iz).z){
                meshEndBuilder.AddQuad((                  
                    (new Vector3(x,                 y + WORLD_SCALE,    z + WALL_SIZE),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,                 y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy - 1,iz).z){
                meshEndBuilder.AddQuad((                  
                    (new Vector3(x,                 y,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x + WORLD_SCALE,   y,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,   y,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,                 y,    z + WALL_SIZE),new Vector2(y + WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix - 1,iy,iz).z){
                meshEndBuilder.AddQuad((                    
                    (new Vector3(x,y,                  z + WALL_SIZE),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x,y + WORLD_SCALE,    z + WALL_SIZE ),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,y + WORLD_SCALE,    z - WALL_SIZE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x,y,                  z - WALL_SIZE),new Vector2(y - WALL_SIZE,x))
                ));
            }
            Color mzDir = ColorFromDirection(data.QueryRoomDirection(ix,iy,iz - 1));
            MeshBuilder mzMeshBuilder = roomBuilder.GetBuilderByStyle(data.QueryRoom(ix,iy,iz - 1).style);
            mzMeshBuilder.AddQuad((
                (new Vector3(x,y,z - WALL_SIZE),new Vector2(x,y),mzDir),
                (new Vector3(x,y + WORLD_SCALE,z - WALL_SIZE),new Vector2(x,y + WORLD_SCALE),mzDir),
                (new Vector3(x + WORLD_SCALE,y + WORLD_SCALE,z - WALL_SIZE),new Vector2(x + WORLD_SCALE,y + WORLD_SCALE),mzDir),
                (new Vector3(x + WORLD_SCALE,y,z - WALL_SIZE),new Vector2(x + WORLD_SCALE,y),mzDir)
            ));
        }
        if(currState.y){
            //Vector3 center = new Vector3(x + 0.5f,y + 0.125f,z + 0.5f);
            //Gizmos.DrawCube(center, new Vector3(1.0f,0.25f,1.0f));
            mainMeshBuilder.AddQuad((
                (new Vector3(x,y + WALL_SIZE, z + WORLD_SCALE),new Vector2(x,z + WORLD_SCALE),mainDir),
                (new Vector3(x + WORLD_SCALE,y + WALL_SIZE,z + WORLD_SCALE),new Vector2(x + WORLD_SCALE,z + WORLD_SCALE),mainDir),
                (new Vector3(x + WORLD_SCALE,y + WALL_SIZE,z),new Vector2(x + WORLD_SCALE,z),mainDir),
                (new Vector3(x,y + WALL_SIZE,z),new Vector2(x,z),mainDir)
            ));
            if(!data.GetWallState(ix + 1,iy,iz).y){
                meshEndBuilder.AddQuad((
                    (new Vector3(x + WORLD_SCALE,y + WALL_SIZE, z),new Vector2(x + WALL_SIZE,z)),
                    (new Vector3(x + WORLD_SCALE,y + WALL_SIZE, z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,y - WALL_SIZE, z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,y - WALL_SIZE, z),new Vector2(x - WALL_SIZE,z))
                ));
            }
            if(!data.GetWallState(ix - 1,iy,iz).y){
                meshEndBuilder.AddQuad((
                    (new Vector3(x,y - WALL_SIZE, z),new Vector2(x - WALL_SIZE,z)),
                    (new Vector3(x,y - WALL_SIZE, z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y + WALL_SIZE, z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y + WALL_SIZE, z),new Vector2(x + WALL_SIZE,z))
                ));
            }
            if(!data.GetWallState(ix,iy,iz + 1).y){
                meshEndBuilder.AddQuad((
                    (new Vector3(x,y - WALL_SIZE,                   z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z)),
                    (new Vector3(x + WORLD_SCALE, y - WALL_SIZE,    z + WORLD_SCALE),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE, y + WALL_SIZE,    z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y + WALL_SIZE,                   z + WORLD_SCALE),new Vector2(x - WALL_SIZE,z))
                ));
            }
            if(!data.GetWallState(ix,iy,iz - 1).y){
                meshEndBuilder.AddQuad((
                    (new Vector3(x,y + WALL_SIZE,               z),new Vector2(x - WALL_SIZE,z)),
                    (new Vector3(x + WORLD_SCALE,y + WALL_SIZE, z),new Vector2(x - WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x + WORLD_SCALE,y - WALL_SIZE, z),new Vector2(x + WALL_SIZE,z + WORLD_SCALE)),
                    (new Vector3(x,y - WALL_SIZE,               z),new Vector2(x + WALL_SIZE,z))
                ));
            }
            Color myDir = ColorFromDirection(data.QueryRoomDirection(ix,iy - 1,iz));
            MeshBuilder myMeshBuilder = roomBuilder.GetBuilderByStyle(data.QueryRoom(ix,iy - 1,iz).style);
            myMeshBuilder.AddQuad((
                (new Vector3(x,y - WALL_SIZE,z),new Vector2(x,z),myDir),
                (new Vector3(x + WORLD_SCALE,y - WALL_SIZE,z),new Vector2(x + WORLD_SCALE,z),myDir),
                (new Vector3(x + WORLD_SCALE,y - WALL_SIZE,z + WORLD_SCALE),new Vector2(x + WORLD_SCALE,z + WORLD_SCALE),myDir),
                (new Vector3(x,y - WALL_SIZE, z + WORLD_SCALE),new Vector2(x,z + WORLD_SCALE),myDir)
            ));           
        }
        if(currState.x){
            //Vector3 center = new Vector3(x + 0.125f,y + 0.5f,z + 0.5f);
            //Gizmos.DrawCube(center, new Vector3(0.25f,1.0f,1.0f));
            mainMeshBuilder.AddQuad((
                (new Vector3(x + WALL_SIZE,y,z),new Vector2(y,z),mainDir),
                (new Vector3(x + WALL_SIZE,y + WORLD_SCALE,z),new Vector2(y + WORLD_SCALE,z),mainDir),
                (new Vector3(x + WALL_SIZE,y + WORLD_SCALE,z + WORLD_SCALE),new Vector2(y + WORLD_SCALE,z + WORLD_SCALE),mainDir),
                (new Vector3(x + WALL_SIZE,y, z + WORLD_SCALE),new Vector2(y,z + WORLD_SCALE),mainDir)
            ));
            if(!data.GetWallState(ix,iy + 1,iz).x){
                meshEndBuilder.AddQuad((             
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y + WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy - 1,iz).x){
                meshEndBuilder.AddQuad((             
                    (new Vector3(x + WALL_SIZE,   y,    z),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x + WALL_SIZE,   y,    z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y,    z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y,    z),new Vector2(y - WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy,iz + 1).x){
                meshEndBuilder.AddQuad((             
                    (new Vector3(x + WALL_SIZE,   y,                  z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x - WALL_SIZE,   y,                  z + WORLD_SCALE),new Vector2(y - WALL_SIZE,x))
                ));
            }
            if(!data.GetWallState(ix,iy,iz - 1).x){
                meshEndBuilder.AddQuad((             
                    (new Vector3(x - WALL_SIZE,   y,                  z),new Vector2(y - WALL_SIZE,x)),
                    (new Vector3(x - WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y - WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y + WORLD_SCALE,    z),new Vector2(y + WALL_SIZE,x + WORLD_SCALE)),
                    (new Vector3(x + WALL_SIZE,   y,                  z),new Vector2(y + WALL_SIZE,x))
                ));
            }
            Color mxDir = ColorFromDirection(data.QueryRoomDirection(ix - 1,iy,iz));
            MeshBuilder mxMeshBuilder = roomBuilder.GetBuilderByStyle(data.QueryRoom(ix - 1,iy,iz).style);
            mxMeshBuilder.AddQuad((
                (new Vector3(x - WALL_SIZE,y, z + WORLD_SCALE),new Vector2(y,z + WORLD_SCALE),mxDir),
                (new Vector3(x - WALL_SIZE,y + WORLD_SCALE,z + WORLD_SCALE),new Vector2(y + WORLD_SCALE,z + WORLD_SCALE),mxDir),
                (new Vector3(x - WALL_SIZE,y + WORLD_SCALE,z),new Vector2(y + WORLD_SCALE,z),mxDir),
                (new Vector3(x - WALL_SIZE,y,z),new Vector2(y,z),mxDir)
            ));
        }
    }
    void CreateWalls(){
        RoomMeshConstructor meshBuilder = new RoomMeshConstructor(roomStyles.Length);
        for(int ix = 0; ix <= worldSize; ix++){
            for(int iy = 0; iy <= worldSize; iy++){
                for(int iz = 0; iz <= worldSize; iz++){
                    CreateWallsAt(meshBuilder,ix,iy,iz);
                }
            }
        }
        meshBuilder.CreateVisuals(wallEndMaterial,roomStyles);
        //this.gameObject.GetComponent<MeshFilter>().mesh = meshBuilder.IntoMesh();
    }
    public GameObject testCubePrefab;
    public GameObject GetFloorSpawnablePrefab(){
        return testCubePrefab;
    }
    // Start is called before the first frame update
    void Start()
    {
        data = new WorldData(this);
        data.CarveRandomHoles();
        CreateWalls();
        data.SpawnItems();
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
