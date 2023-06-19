using UnityEngine;

namespace Assets.Scripts
{
    public class Interaction : IOperation
    {
        private Vector3 direction;

        public OperationType Type { get; private set; }
        public Vector3 Direction => direction;

        public Interaction(Vector3 interactDirection, OperationType type)
        {
            direction = interactDirection;
            Type = type;
        }

        public void Execute(Transform gameObj)
        {
            throw new System.NotImplementedException();
        }
    }
}
