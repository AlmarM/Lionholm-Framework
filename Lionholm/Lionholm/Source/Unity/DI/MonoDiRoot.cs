using System;
using System.Linq;
using Lionholm.Core.DI;
using UnityEngine;

namespace Lionholm.Unity.DI
{
    public class MonoDiRoot : MonoBehaviour
    {
        [SerializeField] private MonoBinder[] _rootBinders = Array.Empty<MonoBinder>();

        private DiRoot _diRoot;

        private void Awake()
        {
            _diRoot = new DiRoot(_rootBinders.ToArray<IBinder>());
        }
    }
}