using UnityEngine;

namespace Enemies.SteeringBehaviour
{
    public readonly struct Pursuit : ISteeringBehaviour
    {
        private readonly Transform stalker;
        private readonly Transform evader;
        private readonly float evaderSpeed;

        public Pursuit(Transform stalker, Transform evader, float evaderSpeed)
        {
            this.evader = evader;
            this.evaderSpeed = evaderSpeed;
            this.stalker = stalker;
        }

        public Vector2 Force()
        {
            Vector3 prediction = evader.forward * evaderSpeed * Time.deltaTime;
            return evader.position + prediction - stalker.position;
        }
    }
}