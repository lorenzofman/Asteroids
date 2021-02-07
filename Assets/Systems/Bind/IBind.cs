using System;

namespace Systems
{
    public interface IBind
    {
        void Bind(Action onUnbind);
    }
}