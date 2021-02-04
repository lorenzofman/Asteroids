using UnityEngine;

public readonly struct AsteroidsReturn : IPoolReturner
{
    private readonly Transform transform;

    public AsteroidsReturn(Transform transform)
    {
        this.transform = transform;
    }
    
    public void Return()
    {
        
    }
}
