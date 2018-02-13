using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(DragAndDropItem))]
[CanEditMultipleObjects]
public class ItemHolder : Editor
{
    SerializedProperty itemVariableTypeProp;
    SerializedProperty floatProp;
    SerializedProperty boolProp;
    SerializedProperty charProp;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        itemVariableTypeProp = serializedObject.FindProperty("itemVariableType");
        floatProp = serializedObject.FindProperty("float_prop");
        boolProp = serializedObject.FindProperty("bool_prop");
        charProp = serializedObject.FindProperty("char_prop");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(itemVariableTypeProp);

        DragAndDropItem.ItemVariableType type = (DragAndDropItem.ItemVariableType)itemVariableTypeProp.enumValueIndex;

        switch (type)
        {
            case DragAndDropItem.ItemVariableType.Float:
                floatProp.floatValue = EditorGUILayout.FloatField("FloatProp", floatProp.floatValue, GUILayout.MinWidth(50));
                EditorGUILayout.PropertyField(charProp, new GUIContent("CharProp"));
                break;

            case DragAndDropItem.ItemVariableType.Bool:
                EditorGUILayout.PropertyField(boolProp, new GUIContent("BoolProp"));
                break;

            case DragAndDropItem.ItemVariableType.Char:
                EditorGUILayout.PropertyField(charProp, new GUIContent("CharProp"));
                break;
            default:
                break;

        }


        serializedObject.ApplyModifiedProperties();
    }
}