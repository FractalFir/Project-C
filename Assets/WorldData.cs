using UnityEngine;
public struct WallState{
    public bool x;
    public bool y;
    public bool z;
}
class WorldData{
    Vec<Room> rooms;
    int worldSize;
    public int minVolume = 250;
    private WallState[,,] walls;
    public void DebugDisplay(){
        /*
        foreach(Room room in rooms){
            room.DebugDisplay();
        }*/
        Color drawColor = new Color(1.0f,1.0f,1.0f,0.5f);
        Color lastColor = Gizmos.color;
        Gizmos.color = drawColor;
        for(int x = 0; x <= worldSize; x++){
            for(int y = 0; y <= worldSize; y++){
                for(int z = 0; z <= worldSize; z++){
                    WallState currState = this.GetWallState(x,y,z);
                    if(currState.z){
                        Vector3 center = new Vector3(x + 0.5f,y + 0.5f,z + 0.125f);
                        Gizmos.DrawCube(center, new Vector3(1.0f,1.0f,0.25f));
                    }
                    if(currState.y){
                        Vector3 center = new Vector3(x + 0.5f,y + 0.125f,z + 0.5f);
                        Gizmos.DrawCube(center, new Vector3(1.0f,0.25f,1.0f));
                    }
                    if(currState.x){
                        Vector3 center = new Vector3(x + 0.125f,y + 0.5f,z + 0.5f);
                        Gizmos.DrawCube(center, new Vector3(0.25f,1.0f,1.0f));
                    }
                }
            }
        }
        Gizmos.color = lastColor;
    }
    public void CarveRandomHoles(){
         for(int x = 1; x < worldSize; x++){
            for(int y = 1; y < worldSize; y++){
                for(int z = 1; z < worldSize; z++){
                    WallState currState = this.GetWallState(x,y,z);
                    if(Random.Range(0,16) == 0) currState.x = false;
                    if(Random.Range(0,16) == 0) currState.y = false;
                    if(Random.Range(0,16) == 0) currState.z = false;
                    this.SetWallState(x,y,z,currState);
                }
            }
        }
    }
    public void Autosplit(int max){
        for(int i = 0 ; i < max; i++){
            Vec<Room> newRooms =  new Vec<Room>();
            foreach(Room room in rooms){
                if(room.volume*2 < minVolume){
                    newRooms.Push(room);
                }
                else{
                    (Room a, Room b) = room.Split();
                    newRooms.Push(a);
                    newRooms.Push(b);
                }
            }
            this.rooms = newRooms;
        }
        
    }
    public WorldData(int worldSize){
        this.worldSize = worldSize;
        this.rooms = new Vec<Room>();
        this.walls = new WallState[worldSize + 1, worldSize + 1, worldSize + 1];
        rooms.Push(new Room(Vector3Int.zero,new Vector3Int(worldSize,worldSize,worldSize)));
        Autosplit(16);
        foreach(Room room in rooms){
            room.SetupWalls(this);
        }
    }
    public void SetWallState(int x, int y, int z,WallState state){
        if(x < 0 || y < 0 || z < 0)return;
        if(x > worldSize || y > worldSize || y > worldSize)return;
        walls[x,y,z] = state;
    }
    public WallState GetWallState(int x, int y, int z){
        if(x < 0 || y < 0 || z < 0)return new WallState();
        if(x > worldSize || y > worldSize || z > worldSize)return new WallState();
        return walls[x,y,z];
    }
}