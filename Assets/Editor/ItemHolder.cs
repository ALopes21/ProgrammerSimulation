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
    SerializedProperty GOProp;
    SerializedProperty SprProp;

    SerializedProperty itemTypeProp;

    SerializedProperty conditionalslots;

    void OnEnable()
    {
        itemTypeProp = serializedObject.FindProperty("itemType");
        
        // Setup the SerializedProperties.
        itemVariableTypeProp = serializedObject.FindProperty("itemVariableType");
        floatProp = serializedObject.FindProperty("float_prop");
        boolProp = serializedObject.FindProperty("bool_prop");
        charProp = serializedObject.FindProperty("char_prop");
        GOProp = serializedObject.FindProperty("GO_prop");
        SprProp = serializedObject.FindProperty("sprite_prop");

        conditionalslots = serializedObject.FindProperty("ConditionalSlots");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(itemTypeProp);

        EditorGUILayout.PropertyField(itemVariableTypeProp);

        EditorList.Show(serializedObject.FindProperty("ConditionalSlots"), EditorListOption.ListLabel | EditorListOption.Buttons);

        VariableType.Type type = (VariableType.Type)itemVariableTypeProp.enumValueIndex;

        DragAndDropItem.ItemType optionType = (DragAndDropItem.ItemType)itemTypeProp.enumValueIndex;
        switch (optionType)
        {
            case DragAndDropItem.ItemType.Basic:
                switch (type)
                {
                    case VariableType.Type.Float:
                        floatProp.floatValue = EditorGUILayout.FloatField("FloatProp", floatProp.floatValue, GUILayout.MinWidth(50));
                        EditorGUILayout.PropertyField(charProp, new GUIContent("CharProp"));
                        break;
                    case VariableType.Type.Char:
                        EditorGUILayout.PropertyField(charProp, new GUIContent("CharProp"));
                        break;
                    case VariableType.Type.Bool:
                        EditorGUILayout.PropertyField(boolProp, new GUIContent("BoolProp"));
                        break;
                    case VariableType.Type.GameObject:
                        EditorGUILayout.PropertyField(GOProp, new GUIContent("GameObjectProp"));
                        break;
                    case VariableType.Type.Sprite:
                        EditorGUILayout.PropertyField(SprProp, new GUIContent("SpriteProp"));
                        break;
                    default:
                        break;
                }
                break;
            case DragAndDropItem.ItemType.Option:
                switch (type)
                {
                    case VariableType.Type.Float:
                        EditorList.Show(serializedObject.FindProperty("floatList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(charProp, new GUIContent("CharProp"));
                        break;
                    case VariableType.Type.Char:
                        EditorList.Show(serializedObject.FindProperty("charList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(charProp, new GUIContent("CharProp"));
                        break;
                    case VariableType.Type.Bool:
                        EditorList.Show(serializedObject.FindProperty("boolList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(boolProp, new GUIContent("BoolProp"));
                        break;
                    case VariableType.Type.GameObject:
                        EditorList.Show(serializedObject.FindProperty("gameObjectList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(GOProp, new GUIContent("GameObjectProp"));
                        break;
                    case VariableType.Type.Sprite:
                        EditorList.Show(serializedObject.FindProperty("spriteList_prop"), EditorListOption.ListLabel | EditorListOption.Buttons);
                        EditorGUILayout.PropertyField(SprProp, new GUIContent("SpriteProp"));
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