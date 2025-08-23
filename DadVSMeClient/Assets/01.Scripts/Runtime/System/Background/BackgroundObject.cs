using UnityEngine;
using UnityEngine.Animations;

namespace DadVSMe.Background
{
    public class BackgroundObject : MonoBehaviour
    {
        [SerializeField] Transform _pivotTransform;

        [SerializeField] Transform _socketTransform;
        public Vector2 SocketPosition => _socketTransform.position;

        //[SerializeField] PositionConstraint _positionConstraint;

        private int _themeIdx;
        public int ThemeIdx => _themeIdx;

        public void Initialize(Vector2 spawnPosition, Transform followTransform, int themeIdx)
        {
            Vector2 delta = spawnPosition - (Vector2)_pivotTransform.position;
            transform.position = new Vector3
            (
                 transform.position.x + delta.x,
                 transform.position.y + delta.y,
                 transform.position.z
            );

            //Debug.Log($"{gameObject.name} {_pivotTransform.position} {_socketTransform.position}");

            //_positionConstraint.ClearSources();

            //var source = new ConstraintSource { sourceTransform = followTransform, weight = 1 };
            //_positionConstraint.AddSource(source);
            //_positionConstraint.translationOffset = ((Vector2)targetPosition - (Vector2)followTransform.position) * (1 / _positionConstraint.weight);
            //if (gameObject.name.Contains("1"))
            //    Debug.Break();
            //_positionConstraint.constraintActive = true;

            _themeIdx = themeIdx;
        }
    }
}
