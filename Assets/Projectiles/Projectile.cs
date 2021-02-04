using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PoolReturner returner;

    public void Initialize(PoolReturner returner)
    {
        this.returner = returner;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<AsteroidCollision>())
        {
            returner.Return();
        }
    }
}