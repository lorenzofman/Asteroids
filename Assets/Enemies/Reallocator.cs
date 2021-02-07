using Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public struct Reallocator : ISystem
    {
        private readonly IPositionSpawner spawner;
        private readonly Transform transform;
        private const int FrameInterval = 30;
        private int turn;
    
        public Reallocator(Transform transform, IPositionSpawner spawner)
        {
            this.transform = transform;
            this.spawner = spawner;
            turn = Random.Range(0, FrameInterval);
        }

        public void OnUpdate()
        {
            if (--turn > 0)
            {
                return;
            }

            if (spawner.OutOfRange(transform.position))
            {
                transform.position = spawner.Position();
            }
        
            turn = FrameInterval;
        }
    }
}