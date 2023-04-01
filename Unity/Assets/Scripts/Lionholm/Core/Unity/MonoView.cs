using UnityEngine;

namespace Lionholm.Core.Unity
{
    public class MonoView<TModel> : MonoBehaviour, IMonoView
        where TModel : IModel
    {
        protected TModel Model { get; private set; }
    }
}