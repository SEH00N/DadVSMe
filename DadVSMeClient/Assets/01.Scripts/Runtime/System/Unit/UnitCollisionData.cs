using UnityEngine;

namespace DadVSMe.Entities
{
    public struct UnitCollisionData
    {
        public Vector2 force;
        public Vector2 normal;
        public Vector2 point;

        public UnitCollisionData(Vector2 force, Vector2 normal, Vector2 point)
        {
            this.force = force;
            this.normal = normal;
            this.point = point;
        }
    }
}