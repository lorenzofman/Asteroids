using System.Threading.Tasks;
using Systems;
using UnityEngine;

public readonly struct PoolReturner : ISystem
{
    private readonly Pool<Transform> pool;
    private readonly Transform obj;
    private readonly IPositionSpawner spawner;
    
    public PoolReturner(Pool<Transform> pool, Transform obj, IPositionSpawner spawner)
    {
        this.pool = pool;
        this.obj = obj;
        this.spawner = spawner;
    }

    public void OnUpdate()
    {
        if (spawner.OutOfRange(obj.position))
        {
            Return();
        }
    }
    
    public void Return()
    {
        obj.gameObject.SetActive(false);
        pool.Return(obj);
        SystemManager.DeregisterSystem(this);
    }
}
