using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Vector3Extensions
    {
        public static string GetDirectionName(this Vector3 vector)
        {
            if (vector == Vector3.down)
                return "down";
            if (vector == Vector3.up)
                return "up";
            if (vector == Vector3.right)
                return "right";
            if (vector == Vector3.left)
                return "left";

            return string.Empty;
        }
    }
}
