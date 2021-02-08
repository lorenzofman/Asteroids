using UnityEngine;

namespace Enemies.SteeringBehaviour
{
    public readonly struct Seek : ISteeringBehaviour
    {
        private readonly Transform seeker;
        private readonly Transform target;

        public Seek(Transform seeker, Transform target)
        {
            this.seeker = seeker;
            this.target = target;
        }

        public Vector2 Force()
        {
            return (target.position - seeker.position).normalized;
        }
    }
}