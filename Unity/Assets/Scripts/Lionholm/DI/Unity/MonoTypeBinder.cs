using UnityEngine;

namespace Lionholm.DI.Unity
{
    public abstract class MonoTypeBinder : MonoBehaviour, ITypeBinder
    {
        public BindInfoCollection Binds { get; set; }

        public abstract void Bind();
    }
}