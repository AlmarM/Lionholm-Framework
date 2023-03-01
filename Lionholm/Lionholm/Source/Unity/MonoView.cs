using Lionholm.Core;
using UnityEngine;

namespace Lionholm.Unity
{
    public class MonoView<TModel> : MonoBehaviour, IMonoView
        where TModel : IModel
    {
        protected TModel Model { get; private set; }
    }
}