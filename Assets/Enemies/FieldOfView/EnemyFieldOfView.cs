using System;
using Systems;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyFieldOfView : ISystem, IDisposableSystem
{
    private const int Quality = 120;
    private readonly Transform parent;
    private readonly float fieldOfView;
    private readonly float length;
    private readonly LayerMask mask;
    private readonly Action onSeePlayer;
    private readonly Mesh mesh;
    private readonly PolygonCollider2D polygonCollider;
    private readonly Vector3[] vertices;
    private readonly Vector2[] collisionPoints;
    private readonly int[] triangles;
    private readonly GameObject gameObject;

    /// <remarks>
    /// Creating a transparent material at runtime is a pain in the ass:
    /// https://answers.unity.com/questions/1608815/change-surface-type-with-lwrp.html
    /// </remarks>
    public EnemyFieldOfView(Transform parent, float fieldOfView, float length, Material mat, LayerMask mask, 
        Action onSeePlayer)
    {
        this.parent = parent;
        this.fieldOfView = fieldOfView;
        this.length = length;
        this.mask = mask;
        this.onSeePlayer = onSeePlayer;
        vertices = new Vector3[Quality + 1];
        collisionPoints = new Vector2[Quality + 2];
        triangles = new int[Quality * 3 - 3];

        mesh = new Mesh();
        gameObject = ObjectUtils.CreateObject("Field of View", Layers.FieldOfView,  mesh);
        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider.isTrigger = true;
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = mat;
        GenerateFieldOfView(parent.rotation.eulerAngles.z + 90.0f, fieldOfView, length);
        gameObject.ListenToCollision(PhysicsEventType.Trigger2D, OnObjectEnterFieldOfView);
    }

    /* Player collision is filtered with physics layers on project settings */
    private void OnObjectEnterFieldOfView(GameObject obj)
    {
        onSeePlayer.Invoke();
        Object.Destroy(gameObject);
        SystemManager.DeregisterSystem(this);
    }

    public void OnUpdate()
    {
        GenerateFieldOfView(parent.rotation.eulerAngles.z + 90.0f, fieldOfView, length);
    }
    
    private void GenerateFieldOfView(float angle, float fieldOfView, float maxDistance)
    {
        Vector2 origin = parent.position + parent.up * 1.0f;

        angle += fieldOfView * 0.5f;
        float angleIncrement = fieldOfView / Quality;
        int verticesIndex = 0;

        vertices[verticesIndex++] = collisionPoints[verticesIndex] = origin;
        for (int i = 0; i < Quality; i++, angle -= angleIncrement)
        {
            Vector2 dir = AngleDirection(angle);
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDistance, mask);
            float distance = hit ? hit.distance : maxDistance;
            Vector2 vertex = origin + dir * distance;
            vertices[verticesIndex++] = collisionPoints[verticesIndex] = vertex;
        }

        collisionPoints[verticesIndex] = origin; // Close the polygon

        int trianglesIndex = 0;
        for (int i = 2; i < mesh.vertices.Length; i++)
        {
            triangles[trianglesIndex++] = 0;
            triangles[trianglesIndex++] = i - 1;
            triangles[trianglesIndex++] = i;
        }
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        
        polygonCollider.SetPath(0, collisionPoints);
    }

    private static Vector2 AngleDirection(float angle)
    {
        float r = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(r), Mathf.Sin(r));
    }

    public void OnStop()
    {
        Object.Destroy(gameObject);
    }
}