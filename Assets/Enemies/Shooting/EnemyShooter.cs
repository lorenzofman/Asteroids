using Systems;

namespace Enemies
{
    public readonly struct EnemyShooter : ISystem
    {
        private readonly Shooter shooter;
        private readonly IShootingPredicate predicate;

        public EnemyShooter(Shooter shooter, IShootingPredicate predicate)
        {
            this.shooter = shooter;
            this.predicate = predicate;
        }
        
        public void OnUpdate()
        {
            if (!predicate.ShouldShot())
            {
                return;
            }

            shooter.Shoot();
        }
    }
}