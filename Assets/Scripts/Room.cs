using UnityEngine;
class Room{
    Vector3Int _start;
    Vector3Int _end;
    Vector3 _gravDir;
    int _styleCount;
    int _style;
    public int style{
        get => _style;
    }
    public Vector3 gravity{
        get=>_gravDir;
    }
    Vector3Int size{
        get=>_end - _start;
    }
    public Vector3Int start{
        get=>_start;
    }
    public Vector3Int end{
        get=>_end;
    }
    public int volume{
        get => size.x * size.y * size.z;
    }
    public void DebugDisplay(){
        Vector3 center = ((Vector3)(_start + _end))/2.0f;
        Vector3 size = (Vector3)this.size;
        //size.x -= 0.125f;
        //size.y -= 0.125f;
        //size.z -= 0.125f;
        Gizmos.DrawWireCube(center,size);
    }
    Vector3 RandomPositionOnFloor(){
        int x = Random.Range(_start.x + 1,_end.x - 1);
        int y = Random.Range(_start.y + 1,_end.y - 1);
        int z = Random.Range(_start.z + 1,_end.z - 1);
        return new Vector3(x*World.WORLD_SCALE,y*World.WORLD_SCALE,z*World.WORLD_SCALE);
    }
    public void SpawnItems(World world){
        SpawnFloorItems(world);
    }
    public void SpawnFloorItems(World world){
        for(int i = 0; i < 4; i++){
            GameObject pref = world.GetFloorSpawnablePrefab();
            GameObject child = GameObject.Instantiate(pref,RandomPositionOnFloor(),Quaternion.identity);
            child.GetComponent<CustomGravity>().gravity = _gravDir;
        }
       
    }
    public Room(Vector3Int _start,Vector3Int _end,int _styleCount){
        this._start = _start;
        this._end = _end;
        Vector3 gravDir = ((Vector3)size).normalized;
        float max = Mathf.Max(gravDir.x,Mathf.Max(gravDir.y,gravDir.z));
        gravDir -= Vector3.one*max;
        if(Random.Range(0,1) == 1)gravDir *= -1;
        this._gravDir = CustomGravity.AxisSnap(gravDir);
        this._styleCount = _styleCount;
        this._style = Random.Range(0,_styleCount);
    }
    (Room,Room) SplitX(){
        int split = (size.x*2 + Random.Range(0,size.x))/5;
        int splitPosX = _start.x + split;
        Vector3Int splitStart = _start;
        splitStart.x = splitPosX;
        Vector3Int splitEnd = _end;
        splitEnd.x = splitPosX;
        return (new Room(_start,splitEnd,_styleCount),new Room(splitStart,_end,_styleCount));
    }
    (Room,Room) SplitY(){
        int split = (size.y*2 + Random.Range(0,size.y))/5;
        int splitPosY = _start.y + split;
        Vector3Int splitStart = _start;
        splitStart.y = splitPosY;
        Vector3Int splitEnd = _end;
        splitEnd.y = splitPosY;
        return (new Room(_start,splitEnd,_styleCount),new Room(splitStart,_end,_styleCount));
    }
    (Room,Room) SplitZ(){
        int split = (size.z*2 + Random.Range(0,size.z))/5;
        int splitPosZ = _start.z + split;
        Vector3Int splitStart = _start;
        splitStart.z = splitPosZ;
        Vector3Int splitEnd = _end;
        splitEnd.z = splitPosZ;
        return (new Room(_start,splitEnd,_styleCount),new Room(splitStart,_end,_styleCount));
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
        for(int x = _start.x; x < _end.x; x++){
            for(int y = _start.y; y < _end.y; y++){
                // StartZ face
                WallState currState = data.GetWallState(x,y,_start.z);
                currState.z = true;
                data.SetWallState(x,y,_start.z,currState);
                //EndZ face
                currState = data.GetWallState(x,y,_end.z);
                currState.z = true;
                data.SetWallState(x,y,_end.z,currState);
            }
        }
        for(int x = _start.x; x < _end.x; x++){
            for(int z = _start.z; z < _end.z; z++){
                // StartY face
                WallState currState = data.GetWallState(x,_start.y,z);
                currState.y = true;
                data.SetWallState(x,_start.y,z,currState);
                //EndY face
                currState = data.GetWallState(x,_end.y,z);
                currState.y = true;
                data.SetWallState(x,_end.y,z,currState);
            }
        }
        for(int y = _start.y; y < _end.y; y++){
            for(int z = _start.z; z < _end.z; z++){
                // StartX face
                WallState currState = data.GetWallState(_start.x,y,z);
                currState.x = true;
                data.SetWallState(_start.x,y,z,currState);
                // EndX face
                currState = data.GetWallState(_end.x,y,z);
                currState.x = true;
                data.SetWallState(_end.x,y,z,currState);
            }
        }
    }
}