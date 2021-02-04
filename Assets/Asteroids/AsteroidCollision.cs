using NSubstitute;
using UnityEngine;

internal class AsteroidCollision : MonoBehaviour
{
    private Asteroid asteroid;
    
    public void Initialize(Asteroid asteroid)
    {
        this.asteroid = asteroid;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (!projectile)
        {
            return;
        }
        asteroid.Subdivide();
    }
}