using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Triangle{
    public (Vector3,Vector3,Vector3) points;
    public (Vector2,Vector2,Vector2) uv;
    public (Color,Color,Color) colors;
    public static Triangle FromPoints((Vector3,Vector2) p1, (Vector3,Vector2) p2, (Vector3,Vector2) p3){
        Triangle triangle = new Triangle();
        triangle.points = (p1.Item1,p2.Item1,p3.Item1);
        triangle.uv = (p1.Item2,p2.Item2,p3.Item2);
        return triangle;
    }
    public static Triangle FromPoints((Vector3,Vector2,Color) p1, (Vector3,Vector2,Color) p2, (Vector3,Vector2,Color) p3){
        Triangle triangle = new Triangle();
        triangle.points = (p1.Item1,p2.Item1,p3.Item1);
        triangle.uv = (p1.Item2,p2.Item2,p3.Item2);
        triangle.colors = (p1.Item3,p2.Item3,p3.Item3);
        return triangle;
    }
}
public class MeshBuilder 
{
    Vec<Triangle> triangles;
    /*
    public void ApplyVertexFilter<F>(F map) where F: Func<Vector3, Vector3>{
        for
    }*/
    public static Vector3 Recenter(Mesh mesh){
        Vector3 sum = new Vector3();
        foreach(Vector3 point in mesh.vertices){
            sum += point;
        }
        Vector3 center = sum/((float)mesh.vertices.Length);
        Vector3[] vertices = mesh.vertices;
        for(int i = 0; i < vertices.Length; i++){
            vertices[i] -= center;
        }
        mesh.vertices = vertices;
        Bounds bounds = mesh.bounds;
        bounds.center -= center;
        mesh.bounds = bounds;
        return center;
    }
    public MeshBuilder(){
        triangles = new Vec<Triangle>(0xFFFF);
    }
    public void AddTriangle(Triangle triangle){
        triangles.Push(triangle);
    }
    public void AddQuad(((Vector3,Vector2),(Vector3,Vector2),(Vector3,Vector2),(Vector3,Vector2)) quad){
        (var p1,var p2,var p3,var p4) = quad;
        Triangle t1 = Triangle.FromPoints(p1,p2,p4);
        Triangle t2 = Triangle.FromPoints(p2,p3,p4);
        this.AddTriangle(t1);
        this.AddTriangle(t2);
    }
     public void AddQuad(((Vector3,Vector2,Color),(Vector3,Vector2,Color),(Vector3,Vector2,Color),(Vector3,Vector2,Color)) quad){
        (var p1,var p2,var p3,var p4) = quad;
        Triangle t1 = Triangle.FromPoints(p1,p2,p4);
        Triangle t2 = Triangle.FromPoints(p2,p3,p4);
        this.AddTriangle(t1);
        this.AddTriangle(t2);
    }
    public Mesh IntoMesh(){
        Mesh mesh = new Mesh();
        Vec<Vector3> vertices = new Vec<Vector3>(triangles.Length*3);
        Vec<Vector2> uv = new Vec<Vector2>(triangles.Length*3);
        foreach(Triangle tri in triangles){
            vertices.Push(tri.points.Item1);
            vertices.Push(tri.points.Item2);
            vertices.Push(tri.points.Item3);
            uv.Push(tri.uv.Item1);
            uv.Push(tri.uv.Item2);
            uv.Push(tri.uv.Item3);
        }
        mesh.vertices = vertices.GetInnerArray();
        mesh.uv = uv.GetInnerArray();
        int[] indices = new int[vertices.Length];
        for(int i = 0; i < vertices.Length; i++){
            indices[i] = i;
        }
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        return mesh;
    }
    public Mesh[] IntoMeshes(int maxMeshSize){
        maxMeshSize /= 3;
        int trisLeft = triangles.Length;
        int currTri = 0;
        Vec<Mesh> meshes = new Vec<Mesh>((triangles.Length/maxMeshSize) + 1);
        while(trisLeft > 0){
            Mesh mesh = new Mesh();
            Vec<Vector3> vertices = new Vec<Vector3>(triangles.Length*3);
            Vec<Vector2> uv = new Vec<Vector2>(triangles.Length*3);
            Vec<Color> colors = new Vec<Color>(triangles.Length*3);
            //
            {
                Triangle tri = triangles[currTri];
                vertices.Push(tri.points.Item1);
                vertices.Push(tri.points.Item2);
                vertices.Push(tri.points.Item3);
                uv.Push(tri.uv.Item1);
                uv.Push(tri.uv.Item2);
                uv.Push(tri.uv.Item3);
                colors.Push(tri.colors.Item1);
                colors.Push(tri.colors.Item2);
                colors.Push(tri.colors.Item3);
                currTri += 1;
                trisLeft -= 1;
            }
            //
            while(currTri % maxMeshSize != 0 && trisLeft > 0){
                Triangle tri = triangles[currTri];
                vertices.Push(tri.points.Item1);
                vertices.Push(tri.points.Item2);
                vertices.Push(tri.points.Item3);
                uv.Push(tri.uv.Item1);
                uv.Push(tri.uv.Item2);
                uv.Push(tri.uv.Item3);
                colors.Push(tri.colors.Item1);
                colors.Push(tri.colors.Item2);
                colors.Push(tri.colors.Item3);
                currTri += 1;
                trisLeft -= 1;
            }
            mesh.SetVertices(vertices.GetInnerArray());
            mesh.colors = colors.GetInnerArray();
            mesh.uv = uv.GetInnerArray();
            int[] indices = new int[vertices.Length];
            for(int i = 0; i < vertices.Length; i++){
                indices[i] = i;
            }
            mesh.triangles = indices;
            //Debug.Log($"Bounds:{mesh.bounds}");
            mesh.RecalculateNormals();
            meshes.Push(mesh);
        }
        return meshes.GetInnerArray();
    }
}
