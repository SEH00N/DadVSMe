using DadVSMe.Background;
using DadVSMe.GameCycles;
using UnityEditor;
using UnityEngine;

namespace DadVSMe.Editors
{
    [CustomEditor(typeof(BackgroundObject))]
    public class BackgroundObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Add GroundBehaviour"))
            {
                AddGroundBehaviour();
            }
        }

        private void AddGroundBehaviour()
        {
            BackgroundObject backgroundObject = (BackgroundObject)target; 
            Undo.RegisterFullObjectHierarchyUndo(backgroundObject, "BackgroundObject Add GroundBehaviour"); 

            Transform groundBehaviourTransform = backgroundObject.transform.Find("GroundBehaviour");
            if(groundBehaviourTransform == null)
            {
                groundBehaviourTransform = new GameObject("GroundBehaviour", typeof(GroundBehaviour)).transform;
                groundBehaviourTransform.SetParent(backgroundObject.transform);
            }

            groundBehaviourTransform.SetAsFirstSibling();
            groundBehaviourTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            groundBehaviourTransform.localScale = Vector3.one;

            MigrateGround(groundBehaviourTransform);

            if(groundBehaviourTransform.TryGetComponent<GroundBehaviour>(out GroundBehaviour _))
                return;

            groundBehaviourTransform.gameObject.AddComponent<GroundBehaviour>();
            EditorUtility.SetDirty(backgroundObject);
        }

        private void MigrateGround(Transform groundBehaviourTransform)
        {
            BackgroundObject backgroundObject = (BackgroundObject)target; 
            Transform groundObjectTransform = backgroundObject.transform.Find("Ground");
            if(groundObjectTransform == null)
                return;

            // if ground object exist and used as container
            if(groundObjectTransform.TryGetComponent<SpriteRenderer>(out SpriteRenderer _))
                return;

            for(int i = groundObjectTransform.childCount - 1; i >= 0; i--)
                groundObjectTransform.GetChild(i).SetParent(groundBehaviourTransform);

            DestroyImmediate(groundObjectTransform.gameObject);
            if(groundBehaviourTransform.transform.childCount > 0)
                Selection.activeGameObject = groundBehaviourTransform.transform.GetChild(0).gameObject;
        }
    }
}