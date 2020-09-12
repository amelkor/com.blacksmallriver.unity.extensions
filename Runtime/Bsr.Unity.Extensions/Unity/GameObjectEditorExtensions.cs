using UnityEditor;
using UnityEngine;

namespace Bsr.Unity.Extensions.Unity
{
#if UNITY_EDITOR
    public static class GameObjectEditorExtensions
    {
        public static bool SelectedInEditor(this GameObject gameObject)
        {
            var selected = Selection.activeGameObject;
            return selected != null && selected.Equals(gameObject);
        }
    }
#endif
}