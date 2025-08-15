using H00N.Resources.Pools;
using UnityEngine.Animations;
using UnityEngine;
using UEPositionConstraint = UnityEngine.Animations.PositionConstraint;

namespace DadVSMe.Background
{
    [RequireComponent(typeof(UEPositionConstraint))]
    public class BackgroundObject : MonoBehaviour
    {
        private UEPositionConstraint _constraint;

        private SpriteRenderer[] _visualArr;

        public float Weight => _constraint.weight;

        public Vector2 PivotPosition => GetPivotPosition();
        public Vector2 SocketPosition => GetSocketPosition();

        private void Awake()
        {
            _visualArr = transform.GetComponentsInChildren<SpriteRenderer>(false);
            _constraint ??= GetComponent<UEPositionConstraint>();
        }

        public void Initialize(Transform followTrm, float weight, Vector2 spawnPos)
        {
            _constraint.ClearSources();

            var source = new ConstraintSource { sourceTransform = followTrm };

            _constraint.AddSource(source);
            _constraint.weight = weight;

            AttatchPivotTrmToSpawnTrm(spawnPos);
        }

        private void AttatchPivotTrmToSpawnTrm(Vector2 spawnPos)
        {
            Vector2 pivotPos = PivotPosition;

            var desiredPivotWorld = new Vector3(spawnPos.x, spawnPos.y);
            var currentPivotWorld = new Vector3(pivotPos.x, pivotPos.y);

            transform.position += (desiredPivotWorld - currentPivotWorld);
        }

        private Vector2 GetPivotPosition()
        {
            var bounds = GetBoundsFromRenderers();
            return new Vector2(bounds.min.x, bounds.min.y);
        }

        private Vector2 GetSocketPosition()
        {
            var bounds = GetBoundsFromRenderers();
            return new Vector2(bounds.max.x, bounds.min.y);
        }

        private Bounds GetBoundsFromRenderers()
        {
            Bounds bounds = default;

            if (_visualArr.Length == 0)
            {
                Debug.LogError("Can not found Bounds info");
                return bounds;
            }

            bounds = _visualArr[0].bounds;
            for (int i = 1; i < _visualArr.Length; i++)
                bounds.Encapsulate(_visualArr[i].bounds);

            return bounds;
        }
    }
}
