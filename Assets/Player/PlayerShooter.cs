using Systems;
using UnityEngine;

namespace Projectiles
{
    public struct PlayerShooter : ISystem
    {
        private const KeyCode ShootKey = KeyCode.Space;
        private Shooter shooter;

        public PlayerShooter(Shooter shooter)
        {
            this.shooter = shooter;
        }
        
        public void OnUpdate()
        {
            if (!Input.GetKey(ShootKey))
            {
                return;
            }
            shooter.Shoot();
        }
    }
}