using System.Threading.Tasks;
using Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public struct EnemyReallocator : ISystem
    {
        private readonly IPositionSpawner spawner;
        private readonly Transform transform;
        private const int FrameInterval = 30;
        private int turn;
    
        public EnemyReallocator(Transform transform, IPositionSpawner spawner)
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
                DeactivateCollider();
            }
        
            turn = FrameInterval;
        }

        /// <summary>
        /// Gambiarra para os inimigos sobreviverem um pouco mais
        /// </summary>
        private async void DeactivateCollider()
        {
            transform.GetComponentInChildren<Collider2D>().enabled = false;
            await Task.Delay(1000);
            transform.GetComponentInChildren<Collider2D>().enabled = true;
        }
    }
}