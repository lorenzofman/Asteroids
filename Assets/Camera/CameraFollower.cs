using Systems;
using UnityEngine;

public class CameraFollower : ISystem
{
    private readonly Transform camera;
    private readonly Transform target;

    private readonly Vector3 dif;
    public CameraFollower(Transform camera, Transform target)
    {
        this.target = target;
        this.camera = camera;
        dif = camera.position - target.position;
    }

    public void OnUpdate()
    {
        camera.position = target.position + dif;
    }
}
