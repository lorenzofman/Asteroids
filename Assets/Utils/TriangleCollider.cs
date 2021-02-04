using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class TriangleCollider : MonoBehaviour
{
    /// <remarks>
    /// Triangle was created as an rect triangle but scale 32 pixels up 
    /// </remarks>
    private static readonly Vector2[] Points = 
    {
        new Vector2(+0.42f, -0.5f),
        new Vector2(+0.00f, +0.5f),
        new Vector2(-0.42f, -0.5f),
    };
    
    private void OnValidate()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        if (!IsCorrect(collider))
        {
            SetTriangleVertex(collider);
        }
    }

    private static void SetTriangleVertex(PolygonCollider2D collider)
    {
        collider.pathCount = 1;
        collider.SetPath(0, Points);
    }

    private static bool IsCorrect(PolygonCollider2D collider)
    {
        if (collider.pathCount != 1)
        {
            return false;
        }

        Vector2[] path = collider.GetPath(0);

        return path.SequenceEqual(Points);

    }
}
