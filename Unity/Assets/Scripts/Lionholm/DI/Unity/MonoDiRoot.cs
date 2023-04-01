using System;
using System.Linq;
using UnityEngine;

namespace Lionholm.DI.Unity
{
    public class MonoDiRoot : MonoBehaviour
    {
        [SerializeField] private MonoTypeBinder[] _rootBinders = Array.Empty<MonoTypeBinder>();

        private DiRoot _diRoot;

        private void Awake()
        {
            _diRoot = new DiRoot();

            foreach (MonoTypeBinder typeBinder in _rootBinders)
            {
                typeBinder.Binds = _diRoot.InfoCollection;
            }

            var rootBinders = _rootBinders.ToArray<ITypeBinder>();
            _diRoot.PerformDependencyInjection(rootBinders);
        }
    }
}