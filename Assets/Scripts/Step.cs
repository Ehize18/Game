using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Step : IOperation
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        private Vector3 moveDirection;

        public OperationType Type => OperationType.Step;
        public Vector3 Direction => moveDirection;

        public Step(Vector3 startPosition, Vector3 direction)
        {
            StartPosition = startPosition;
            moveDirection = direction;
            EndPosition = StartPosition + moveDirection * 1.5f;
        }

        public void Execute(Transform gameObj)
        {
            throw new System.NotImplementedException();
        }
    }
}
