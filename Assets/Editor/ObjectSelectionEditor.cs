using UnityEngine;
using UnityEditor;
using UnityStandardAssets.Utility;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(ObjectSelection))]
[CanEditMultipleObjects]
public class ObjectSelectionEditor : Editor
{

    SerializedProperty CodePanel;
    SerializedProperty currentObject;
    SerializedProperty activePanel;

    void OnEnable()
    {
        CodePanel = serializedObject.FindProperty("CodePanel");
        currentObject = serializedObject.FindProperty("currentObject");
        activePanel = serializedObject.FindProperty("activePanel");

}

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(CodePanel);
        EditorGUILayout.PropertyField(currentObject);
        EditorGUILayout.PropertyField(activePanel);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(SceneHandler))]
[CanEditMultipleObjects]
public class SceneHandlerEditor : Editor
{
    SerializedProperty SceneNumber;
    SerializedProperty Unclocked;
    SerializedProperty InfoModeActivated;


    SerializedProperty lives;
    SerializedProperty gameOver;
    SerializedProperty EmptyLife, FullLife;
    SerializedProperty ErrorPanel;

    void OnEnable()
    {
        SceneNumber = serializedObject.FindProperty("SceneNumber");
        Unclocked = serializedObject.FindProperty("Unclocked");
        InfoModeActivated = serializedObject.FindProperty("InfoModeActivated");

        lives = serializedObject.FindProperty("lives");
        gameOver = serializedObject.FindProperty("gameOver");
        ErrorPanel = serializedObject.FindProperty("ErrorPanel");

        EmptyLife = serializedObject.FindProperty("EmptyLife");
        FullLife = serializedObject.FindProperty("FullLife");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(SceneNumber);
        EditorGUILayout.PropertyField(Unclocked);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(InfoModeActivated);
        EditorGUILayout.PropertyField(lives);
        EditorGUILayout.PropertyField(gameOver);
        EditorGUILayout.PropertyField(ErrorPanel);
        EditorGUILayout.PropertyField(EmptyLife);
        EditorGUILayout.PropertyField(FullLife);

        EditorList.Show(serializedObject.FindProperty("Lives"), EditorListOption.ListLabel | EditorListOption.Buttons);
        EditorList.Show(serializedObject.FindProperty("InvSlots"), EditorListOption.ListLabel | EditorListOption.Buttons);
        EditorList.Show(serializedObject.FindProperty("Items"), EditorListOption.ListLabel | EditorListOption.Buttons);

        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}


[CustomEditor(typeof(ObjectChanger))]
[CanEditMultipleObjects]
public class ObjectChangerEditor : Editor
{

    SerializedProperty Disabled, Enabled;
    SerializedProperty thisCondition;

    SerializedProperty originalBool;
    SerializedProperty newTarget, originalTarget;
    SerializedProperty newObject, originalObject;
    SerializedProperty Cousin;

    void OnEnable()
    {
        Disabled = serializedObject.FindProperty("Disabled");
        Enabled = serializedObject.FindProperty("Enabled");
        thisCondition = serializedObject.FindProperty("thisCondition");

        originalBool = serializedObject.FindProperty("originalBool");
        newTarget = serializedObject.FindProperty("newTarget");
        originalTarget = serializedObject.FindProperty("originalTarget");
        newObject = serializedObject.FindProperty("newObject");
        originalObject = serializedObject.FindProperty("originalObject");
        Cousin = serializedObject.FindProperty("Cousin");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(Disabled);
        EditorGUILayout.PropertyField(Enabled);
        EditorGUILayout.PropertyField(thisCondition);
        EditorGUILayout.PropertyField(originalBool);
        EditorGUILayout.PropertyField(originalTarget);
        EditorGUILayout.PropertyField(originalObject);

        EditorGUI.BeginDisabledGroup(true);

        EditorGUILayout.PropertyField(newTarget);
        EditorGUILayout.PropertyField(newObject);
        EditorGUILayout.PropertyField(Cousin);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(CodeHandler))]
[CanEditMultipleObjects]
public class CodeHandlerEditor : Editor
{
    SerializedProperty functionString;
    SerializedProperty ObjInt;

    void OnEnable()
    {
        functionString = serializedObject.FindProperty("functionString");
        ObjInt = serializedObject.FindProperty("ObjInt");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(functionString);
        EditorGUILayout.PropertyField(ObjInt);
        EditorGUI.EndDisabledGroup();

        serializedObject.ApplyModifiedProperties();
    }
}

