using UnityEngine;

namespace Assets.Scripts
{
    public interface IOperation
    {
        OperationType Type { get; }
        Vector3 Direction { get; }
        void Execute(Transform gameObj);
    }
}
