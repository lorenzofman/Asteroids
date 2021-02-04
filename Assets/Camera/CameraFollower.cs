using Assets.AllyaExtension;
using UnityEngine;

public class CameraFollower
{
    private readonly Transform camera;
    private readonly Transform target;

    private Vector3 dif;
    public CameraFollower(Transform camera, Transform target)
    {
        this.target = target;
        this.camera = camera;
    }

    public void Begin()
    {
        dif = camera.position - target.position;
        Scheduler.OnUpdate.Subscribe(OnUpdate);
    }

    private void OnUpdate()
    {
        camera.position = target.position + dif;
    }
}
