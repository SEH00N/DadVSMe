using DadVSMe.GameCycles;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using UnityEditor;
using UnityEngine;

namespace DadVSMe.Editors
{
    [CustomEditor(typeof(GroundBehaviour))]
    public class GroundBehaviourEditor : Editor
    {
        private const float NAV_MESH_SURFACE_VOLUME_OFFSET = 5f;
        private const float COLLIDER_WIDTH_OFFSET = 5f;
        private const float COLLIDER_HEIGHT = 5f;
        private const float COLLIDER_TOP_POSITION = 6f;
        private const float COLLIDER_BOTTOM_POSITION = -2.4f;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Set Up"))
            {
                SetUp();
            }
        }

        private void SetUp()
        {
            GroundBehaviour groundBehaviour = (GroundBehaviour)target;
            Undo.RegisterFullObjectHierarchyUndo(groundBehaviour, "GroundBehaviour Set Up");

            Bounds bounds = SetUpSpriteRenderersAndGetBounds(groundBehaviour);
            SetUpCollider(groundBehaviour, "TopCollider", bounds.size.x, new Vector2(bounds.center.x, 0), Vector2.up * COLLIDER_HEIGHT / 2 + Vector2.up * COLLIDER_TOP_POSITION);
            SetUpCollider(groundBehaviour, "BottomCollider", bounds.size.x, new Vector2(bounds.center.x, 0), Vector2.down * COLLIDER_HEIGHT / 2 + Vector2.up * COLLIDER_BOTTOM_POSITION);
            SetUpNavMeshSurface(groundBehaviour, bounds);

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(groundBehaviour);
        }

        private Bounds SetUpSpriteRenderersAndGetBounds(GroundBehaviour groundBehaviour)
        {
            Bounds bounds = new Bounds();
            bool first = true;
            SpriteRenderer[] spriteRenderers = groundBehaviour.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                if(spriteRenderer.TryGetComponent<NavMeshModifier>(out NavMeshModifier navMeshModifier) == false)
                    navMeshModifier = spriteRenderer.gameObject.AddComponent<NavMeshModifier>();

                if (first)
                {
                    Bounds spriteBounds = spriteRenderer.bounds;
                    spriteBounds.center -= spriteRenderer.transform.parent.position;
                    bounds = spriteBounds;
                    first = false;
                }
                else
                {
                    Bounds spriteBounds = spriteRenderer.bounds;
                    spriteBounds.center -= spriteRenderer.transform.parent.position;
                    bounds.Encapsulate(spriteBounds);
                }
            }

            return bounds;
        }

        private void SetUpCollider(GroundBehaviour groundBehaviour, string name, float width, Vector2 center, Vector2 offset)
        {
            Transform colliderTransform = groundBehaviour.transform.Find(name);
            if(colliderTransform == null)
            {
                colliderTransform = new GameObject(name, typeof(BoxCollider2D)).transform;
                colliderTransform.SetParent(groundBehaviour.transform);
            }

            colliderTransform.SetAsFirstSibling();
            colliderTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            colliderTransform.localScale = Vector3.one;
            
            if(colliderTransform.TryGetComponent<BoxCollider2D>(out BoxCollider2D collider) == false)
                collider = colliderTransform.gameObject.AddComponent<BoxCollider2D>();

            collider.size = Vector2.one;
            collider.offset = Vector2.zero;

            float scaleDelta = 1f / (colliderTransform.lossyScale.x / colliderTransform.localScale.x);

            Vector2 size = new Vector2(width * scaleDelta + COLLIDER_WIDTH_OFFSET, COLLIDER_HEIGHT);
            colliderTransform.localScale = new Vector3(size.x, size.y, 1);

            center *= scaleDelta;
            center += offset;
            colliderTransform.localPosition = new Vector3(center.x, center.y, 0);
        }

        private void SetUpNavMeshSurface(GroundBehaviour groundBehaviour, Bounds bounds)
        {
            Transform navMeshTransform = groundBehaviour.transform.Find("NavMesh");
            if(navMeshTransform != null)
                DestroyImmediate(navMeshTransform.gameObject);
            
            return;
            SerializedProperty navMeshSurfaceProperty = serializedObject.FindProperty("navMeshSurface");

            NavMeshSurface navMeshSurface = navMeshSurfaceProperty.objectReferenceValue as NavMeshSurface;
            if (navMeshSurface == null)
            {
                navMeshSurface = groundBehaviour.GetComponentInChildren<NavMeshSurface>();
                if (navMeshSurface == null)
                {
                    GameObject navMeshObject = new GameObject("NavMesh", typeof(NavMeshSurface), typeof(CollectSources2d));
                    navMeshSurface = navMeshObject.GetComponent<NavMeshSurface>();
                    navMeshSurface.transform.SetParent(groundBehaviour.transform);
                    navMeshSurfaceProperty.objectReferenceValue = navMeshSurface;
                }
            }

            navMeshSurface.transform.SetAsFirstSibling();
            navMeshSurface.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(-90f, 0, 0));
            navMeshSurface.transform.localScale = Vector3.one;
            navMeshSurface.collectObjects = CollectObjects.Volume;

            bounds.size += Vector3.one * NAV_MESH_SURFACE_VOLUME_OFFSET;
            bounds = RotateBounds(bounds, new Vector3(90f, 0f, 0f));
            navMeshSurface.size = bounds.size;
            navMeshSurface.center = bounds.center;
        }

        private static Bounds RotateBounds(Bounds bounds, Vector3 eulerAngles)
        {
            Vector3[] corners = new Vector3[8];
            corners[0] = bounds.min;
            corners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            corners[3] = new Vector3(bounds.min.x, bounds.min.z, bounds.min.z);
            corners[4] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            corners[5] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            corners[6] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            corners[7] = bounds.max;

            Quaternion rotation = Quaternion.Euler(eulerAngles);
            Bounds rotatedBounds = new Bounds(rotation * corners[0], Vector3.zero);

            // 나머지 꼭짓점들을 회전시키고 새로운 Bounds에 포함
            for (int i = 1; i < 8; i++)
            {
                Vector3 rotatedCorner = rotation * corners[i];
                rotatedBounds.Encapsulate(rotatedCorner);
            }

            return rotatedBounds;
        }
    }
}