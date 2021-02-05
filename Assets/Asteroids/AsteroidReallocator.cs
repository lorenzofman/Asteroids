using System.Threading.Tasks;
using Assets.AllyaExtension;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public struct AsteroidReallocator
    {
        private readonly IPositionSpawner spawner;
        private readonly Transform asteroid;
        private const int FrameInterval = 30;
        private int turn;
        
        public AsteroidReallocator(Transform asteroid, IPositionSpawner spawner)
        {
            this.asteroid = asteroid;
            this.spawner = spawner;
            turn = 0;
        }

        public async void Begin()
        {
            int waitSpin = Random.Range(0, FrameInterval);
            while (waitSpin-- > 0)
            {
                await Task.Yield();
            }
            Scheduler.OnUpdate.Subscribe(OnUpdate);
        }

        private void OnUpdate()
        {
            if (--turn > 0)
            {
                 return;
            }

            if (spawner.OutOfRange(asteroid.position))
            {
                asteroid.position = spawner.Position();
                AsteroidUtils.ApplyImpulse(asteroid.gameObject.GetComponent<Rigidbody2D>());
            }
            
            turn = FrameInterval;
        }
    }
}