using UnityEngine;
class Room{
    Vector3Int start;
    Vector3Int end;
    Vector3Int size{
        get=>end - start;
    }
    public int volume{
        get => size.x * size.y * size.z;
    }
    public void DebugDisplay(){
        Vector3 center = ((Vector3)(start + end))/2.0f;
        Vector3 size = (Vector3)this.size;
        //size.x -= 0.125f;
        //size.y -= 0.125f;
        //size.z -= 0.125f;
        Gizmos.DrawWireCube(center,size);
    }
    public Room(Vector3Int start,Vector3Int end){
        this.start = start;
        this.end = end;
    }
    (Room,Room) SplitX(){
        int split = (size.x/2 + Random.Range(0,size.x))/2;
        int splitPosX = start.x + split;
        Vector3Int splitStart = start;
        splitStart.x = splitPosX;
        Vector3Int splitEnd = end;
        splitEnd.x = splitPosX;
        return (new Room(start,splitEnd),new Room(splitStart,end));
    }
    (Room,Room) SplitY(){
        int split = (size.y/2 + Random.Range(0,size.y))/2;
        int splitPosY = start.y + split;
        Vector3Int splitStart = start;
        splitStart.y = splitPosY;
        Vector3Int splitEnd = end;
        splitEnd.y = splitPosY;
        return (new Room(start,splitEnd),new Room(splitStart,end));
    }
    (Room,Room) SplitZ(){
        int split = (size.z/2 + Random.Range(0,size.z))/2;
        int splitPosZ = start.z + split;
        Vector3Int splitStart = start;
        splitStart.z = splitPosZ;
        Vector3Int splitEnd = end;
        splitEnd.z = splitPosZ;
        return (new Room(start,splitEnd),new Room(splitStart,end));
    }
    public (Room,Room) Split(){
        if(size.x > size.y){
            if(size.x > size.z)return SplitX();
            else return SplitZ();
        }
        else{
            if(size.y > size.z)return SplitY();
            else return SplitZ();
        }
    }
    public void SetupWalls(WorldData data){
        for(int x = start.x; x < end.x; x++){
            for(int y = start.y; y < end.y; y++){
                // StartZ face
                WallState currState = data.GetWallState(x,y,start.z);
                currState.z = true;
                data.SetWallState(x,y,start.z,currState);
                //EndZ face
                currState = data.GetWallState(x,y,end.z);
                currState.z = true;
                data.SetWallState(x,y,end.z,currState);
            }
        }
        for(int x = start.x; x < end.x; x++){
            for(int z = start.z; z < end.z; z++){
                // StartY face
                WallState currState = data.GetWallState(x,start.y,z);
                currState.y = true;
                data.SetWallState(x,start.y,z,currState);
                //EndY face
                currState = data.GetWallState(x,end.y,z);
                currState.y = true;
                data.SetWallState(x,end.y,z,currState);
            }
        }
        for(int y = start.y; y < end.y; y++){
            for(int z = start.z; z < end.z; z++){
                // StartY face
                WallState currState = data.GetWallState(start.x,y,z);
                currState.x = true;
                data.SetWallState(start.x,y,z,currState);
            }
        }
    }
}