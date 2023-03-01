using Lionholm.Core.DI;
using UnityEngine;

namespace Lionholm.Unity.DI
{
    public abstract class MonoBinder : MonoBehaviour, IBinder
    {
        public DependencyContainer Container { get; set; }

        public abstract void Bind();
    }
}