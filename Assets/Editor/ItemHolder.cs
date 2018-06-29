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
    SerializedProperty Vector2Prop;
    SerializedProperty boolProp;
    SerializedProperty GOProp;
    SerializedProperty SprProp;
    SerializedProperty IntProp;
    SerializedProperty MethodProp;

    SerializedProperty itemTypeProp;

    //SerializedProperty originalColour;
    //SerializedProperty originalString;

    void OnEnable()
    {
        itemTypeProp = serializedObject.FindProperty("itemType");
        
        // Setup the SerializedProperties.
        itemVariableTypeProp = serializedObject.FindProperty("itemVariableType");
        Vector2Prop = serializedObject.FindProperty("vector2_prop");
        boolProp = serializedObject.FindProperty("bool_prop");
        GOProp = serializedObject.FindProperty("GO_prop");
        SprProp = serializedObject.FindProperty("sprite_prop");
        IntProp = serializedObject.FindProperty("int_prop");
        MethodProp = serializedObject.FindProperty("method_prop");

        //originalColour = serializedObject.FindProperty("originalColor");
        //originalString = serializedObject.FindProperty("originalString");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(itemTypeProp);

        EditorGUILayout.PropertyField(itemVariableTypeProp);

        VariableType.Type type = (VariableType.Type)itemVariableTypeProp.enumValueIndex;

        DragAndDropItem.ItemType optionType = (DragAndDropItem.ItemType)itemTypeProp.enumValueIndex;
        switch (optionType)
        {
            case DragAndDropItem.ItemType.Basic:
                switch (type)
                {
                    case VariableType.Type.Vector2:
                        EditorGUILayout.PropertyField(Vector2Prop, new GUIContent("Vector2Prop"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.Bool:
                        EditorGUILayout.PropertyField(boolProp, new GUIContent("BoolProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.GameObject:
                        EditorGUILayout.PropertyField(GOProp, new GUIContent("GameObjectProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.Sprite:
                        EditorGUILayout.PropertyField(SprProp, new GUIContent("SpriteProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.Int:
                        EditorGUILayout.PropertyField(IntProp, new GUIContent("IntProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    default:
                        break;
                }
                break;
            case DragAndDropItem.ItemType.Option:
                switch (type)
                {
                    case VariableType.Type.Vector2:
                        EditorList.Show(serializedObject.FindProperty("vector2List_Prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(Vector2Prop, new GUIContent("Vector2Prop"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.Bool:
                        EditorList.Show(serializedObject.FindProperty("boolList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(boolProp, new GUIContent("BoolProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.GameObject:
                        EditorList.Show(serializedObject.FindProperty("gameObjectList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(GOProp, new GUIContent("GameObjectProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.Sprite:
                        EditorList.Show(serializedObject.FindProperty("spriteList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(SprProp, new GUIContent("SpriteProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    case VariableType.Type.Int:
                        EditorList.Show(serializedObject.FindProperty("intList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(IntProp, new GUIContent("IntProp"));
                        EditorGUILayout.PropertyField(MethodProp);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;

        }
        serializedObject.ApplyModifiedProperties();
    }
}