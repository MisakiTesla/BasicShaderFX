using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform)), CanEditMultipleObjects]
public class TransformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Reset LocalPosition", EditorStyles.miniButton))
        {
            //获取所有Property
            var prop = serializedObject.GetIterator();
            while (prop.Next(true))
            {
                Debug.LogError(prop.name);
            }
            
            // ((Transform)serializedObject.targetObject).position = Vector3.zero;
            
            //Reset LocalPosition
            serializedObject.FindProperty("m_LocalPosition").vector3Value = Vector3.zero;
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}