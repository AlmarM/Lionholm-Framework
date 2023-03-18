using Lionholm.Core.DI;
using UnityEngine;

namespace Lionholm.Unity.DI
{
    public abstract class MonoTypeBinder : MonoBehaviour, ITypeBinder
    {
        public BindInfoCollection Binds { get; set; }

        public abstract void Bind();
    }
}