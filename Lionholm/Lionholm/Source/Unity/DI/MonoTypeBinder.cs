using Lionholm.Core.DI;
using UnityEngine;

namespace Lionholm.Unity.DI
{
    public abstract class MonoTypeBinder : MonoBehaviour, ITypeBinder
    {
        public DependencyContainer Container { get; set; }

        public abstract void Bind();
    }
}