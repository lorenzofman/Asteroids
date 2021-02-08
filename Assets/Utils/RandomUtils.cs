using Unity.Collections;
using UnityEngine;

public static class RandomUtils
{
    public const float MinVertexDisplacement = 0.4f;
    public const float MaxVertexDisplacement = 1.6f;

    public static Vector3 RandomSpawnPosition(float spawnOffset, float spawnRadius)
    {
        float r = Random.Range(0, Mathf.PI * 2);
        return new Vector3(Mathf.Cos(r), Mathf.Sin(r), 0) * Random.Range(spawnOffset, spawnRadius);
    }
    
    public static NativeArray<Vector2> RandomConvexPolygon(int vertices, float radius)
    {
        NativeArray<Vector2> polygons = new NativeArray<Vector2>(vertices, Allocator.Persistent);

        float dx = Mathf.PI * 2.0f / vertices;
        float initial = Random.Range(0, Mathf.PI);
        for (int i = 0; i < vertices; i++)
        {
            float theta = i * dx + initial;
            polygons[i] = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * Random.Range(MinVertexDisplacement, MaxVertexDisplacement) * radius;
        }
        
        return polygons;
    }
}
