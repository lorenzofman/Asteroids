using Unity.Collections;
using UnityEngine;

public static class ObjectUtils
{
    private static readonly Material Unlit = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

    public static GameObject CreatePolygonalObject(string name, NativeSlice<Vector2> polygons, float width, Vector3 position = default)
    {
        Mesh mesh = PolygonUtils.CreateMesh(polygons, width);
        GameObject go = new GameObject(name);
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = Unlit;
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
        go.AddComponent<PolygonCollider2D>().SetPath(0, polygons.ToArray());
        go.transform.position = position;
        
        return go;
    }
}
