using System.Threading.Tasks;
using UnityEngine;

public struct PoolReturner
{
    private readonly Pool<Transform> pool;
    private readonly Transform obj;
    private readonly IPositionSpawner spawner;
    private bool returned;
    
    public PoolReturner(Pool<Transform> pool, Transform obj, IPositionSpawner spawner)
    {
        this.pool = pool;
        this.obj = obj;
        this.spawner = spawner;
        returned = false;
    }

    public async void Begin()
    {
        while (Application.isPlaying && !returned)
        {
            if (!spawner.OutOfRange(obj.position))
            {
                await Task.Delay(500);
                continue;
            }

            Return();
        }
    }

    public void Return()
    {
        obj.gameObject.SetActive(false);
        pool.Return(obj);
        returned = true;
    }
    
}
