using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(VariableSlot))]
[CanEditMultipleObjects]
public class SlotHolder : Editor
{
    SerializedProperty slotVariableType;
    SerializedProperty slotConditionType;
    SerializedProperty slotType;
    SerializedProperty parentItem;
    SerializedProperty isTaken;

    void OnEnable()
    {
        slotVariableType = serializedObject.FindProperty("slotVariableType");
        slotConditionType = serializedObject.FindProperty("thisConditionType");
        slotType = serializedObject.FindProperty("thisSlotType");
        parentItem = serializedObject.FindProperty("parentItem");
        isTaken = serializedObject.FindProperty("isTaken");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(slotType);

        VariableSlot.SlotType type = (VariableSlot.SlotType)slotType.enumValueIndex;
        VariableSlot.ConditionType conditionType = (VariableSlot.ConditionType)slotConditionType.enumValueIndex;

        switch (type)
        {
            case VariableSlot.SlotType.Basic:

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(slotConditionType);
                slotConditionType.intValue = 5;
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.PropertyField(slotVariableType);
                break;
            case VariableSlot.SlotType.Conditional:
                EditorGUILayout.PropertyField(slotConditionType);
                switch(conditionType)
                {
                    case VariableSlot.ConditionType.If:
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.PropertyField(slotVariableType);
                        slotVariableType.intValue = 5;
                        EditorGUI.EndDisabledGroup();
                        break;
                    case VariableSlot.ConditionType.This:
                        EditorGUILayout.PropertyField(slotVariableType);
                        break;
                    case VariableSlot.ConditionType.Then:
                        EditorGUILayout.PropertyField(slotVariableType);
                        break;
                    case VariableSlot.ConditionType.That:
                        EditorGUILayout.PropertyField(slotVariableType);
                        break;
                    case VariableSlot.ConditionType.LoopInt:
                        EditorGUILayout.HelpBox("LoopInt is only used for Slot Type: Looper", MessageType.Warning);
                        break;
                    case VariableSlot.ConditionType.None:
                        EditorGUILayout.HelpBox("You need to select a Condition Type", MessageType.Warning);
                        break;
                }
                break;
            case VariableSlot.SlotType.Looper:
                EditorGUILayout.PropertyField(slotConditionType);
                switch (conditionType)
                {
                    case VariableSlot.ConditionType.If:
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.PropertyField(slotVariableType);
                        slotVariableType.intValue = 5;
                        EditorGUI.EndDisabledGroup();
                        break;
                    case VariableSlot.ConditionType.This:
                        EditorGUILayout.HelpBox("This is only used for Slot Type: Conditional", MessageType.Warning);
                        break;
                    case VariableSlot.ConditionType.Then:
                        EditorGUILayout.HelpBox("Then is only used for Slot Type: Conditional", MessageType.Warning);
                        break;
                    case VariableSlot.ConditionType.That:
                        EditorGUILayout.PropertyField(slotVariableType);
                        break;
                    case VariableSlot.ConditionType.LoopInt:
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.PropertyField(slotVariableType);
                        slotVariableType.intValue = 7;
                        EditorGUI.EndDisabledGroup();
                        break;
                    case VariableSlot.ConditionType.None:
                        EditorGUILayout.HelpBox("You need to select a Condition Type", MessageType.Warning);
                        break;
                }
                break;
            default:
                break;

        }
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(parentItem);
        EditorGUILayout.PropertyField(isTaken);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}