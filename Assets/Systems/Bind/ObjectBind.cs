using System;
using UnityEngine;

namespace Systems
{
    public readonly struct ObjectBind : IBind
    {
        private readonly GameObject gameObject;

        public ObjectBind(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public void Bind(Action onUnbind)
        {
            gameObject.OnDestroy(onUnbind);
        }
    }
}