using UnityEngine;

namespace Enemies
{
    public readonly struct LineOfSightPredicate : IShootingPredicate
    {
        private readonly Transform agent;

        public LineOfSightPredicate(Transform agent)
        {
            this.agent = agent;
        }
        public bool ShouldShot()
        {
            int mask = 1 << Layers.Player | 1 << Layers.Asteroid;
            RaycastHit2D hit = Physics2D.Raycast(agent.position, agent.up, Mathf.Infinity, mask);
            return hit && hit.collider.gameObject.layer == Layers.Player;
        }
    }
}