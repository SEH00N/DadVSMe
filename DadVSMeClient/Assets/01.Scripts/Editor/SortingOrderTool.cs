using UnityEngine;
using UnityEditor;

public class SortingOrderTool
{
    [MenuItem("Tools/Set Sorting Order to 0 (Selection) %g")]
    private static void SetSortingOrderZero()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("오브젝트를 선택해주세요.");
            return;
        }

        int count = 0;
        foreach (GameObject obj in selectedObjects)
        {
            SpriteRenderer[] renderers = obj.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var renderer in renderers)
            {
                Undo.RecordObject(renderer, "Set Sorting Order 0"); // Undo 지원
                renderer.sortingOrder = 0;
                count++;
            }
        }
    }
}