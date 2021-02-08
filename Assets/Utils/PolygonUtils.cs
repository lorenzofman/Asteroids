using System.Collections.Generic;
using GizmosHelper;
using Unity.Collections;
using UnityEngine;

public static class PolygonUtils
{
    private static readonly List<Vector3> Vertices = new List<Vector3>(); 
    private static readonly List<int> Indices = new List<int>(); 

    public static Mesh CreateMesh(NativeSlice<Vector2> points, float width)
    {
        Vertices.Clear();
        Indices.Clear();
        Vertices.Capacity = points.Length * 2;
        Indices.Capacity = (points.Length - 1) * 6;
        
        CalculateVertices(points, width);

        CalculateIndices(points);
        
        Mesh mesh = new Mesh
        {
            vertices = Vertices.ToArray(),
            triangles = Indices.ToArray()
        };
        return mesh;
    }
    
    private static void CalculateVertices(NativeSlice<Vector2> points, float width)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 previous = points[CircularIndex(i - 1, points.Length)];
            Vector2 current = points[i];
            Vector2 next = points[CircularIndex(i + 1, points.Length)];

            Vector2 previousToCurrent = current - previous;
            Vector2 nextToCurrent = next - current;

            Vector2 dir = nextToCurrent + previousToCurrent;
            dir = PerpendicularClockwise(dir.normalized);
            Vertices.Add(current - dir * width * 0.5f);
            Vertices.Add(current + dir * width * 0.5f);
        }
    }

    private static void CalculateIndices(NativeSlice<Vector2> points)
    {
        for (int i = 0; i < points.Length - 1; i++)
        {
            int j = i * 2;
            {
                Indices.Add(j + 1);
                Indices.Add(j);

                Indices.Add(j + 3);
            }
            {
                Indices.Add(j);
                Indices.Add(j + 2);

                Indices.Add(j + 3);
            }
        }

        int l = (points.Length - 1) * 2;
        {
            Indices.Add(l + 1);
            Indices.Add(l);
            Indices.Add(1);
        }
        {
            Indices.Add(0);
            Indices.Add(1);
            Indices.Add(l);
        }
    }

    private static Vector2 PerpendicularClockwise(Vector2 vector2)
    {
        return new Vector2(vector2.y, -vector2.x);
    }

    private static int CircularIndex(int index, int length)
    {
        if (index >= length)
        {
            index -= length;
        }
        else if (index < 0)
        {
            index += length;
        }

        return index;
    }
}
