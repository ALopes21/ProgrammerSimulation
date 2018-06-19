using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericClass
{
    static public object condition, thenobj, action;
    static public object basicValue;
    static public int loopCount;

    public void SetConditions(VariableSlot slot, object value)
    {
        switch (slot.thisConditionType)
        {
            case VariableSlot.ConditionType.This:
                condition = value;
                break;
            case VariableSlot.ConditionType.Then:
                thenobj = value;
                break;
            case VariableSlot.ConditionType.That:
                action = value;
                break;
        }
    }
    public void SetValues(VariableSlot slot, object value)
    {
        switch(slot.thisSlotType)
        {
            case VariableSlot.SlotType.Basic:
                basicValue = value;
                Debug.Log(basicValue.GetType().Name);
                break;
            case VariableSlot.SlotType.Conditional:
                SetConditions(slot, value);
                break;
            case VariableSlot.SlotType.Looper:
                if(slot.thisConditionType == VariableSlot.ConditionType.LoopInt)
                {
                    loopCount = (int)value;
                }
                else
                {
                    basicValue = value;
                }
                break;
        }
    }
}

public class CodeHandler : MonoBehaviour {

    public string functionString;
    //References
    public ObjectSelection ObjInt;

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
    }

    public void DoTheMoveThing()
    {
        MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
        data.MoveObject((Vector2)GenericClass.basicValue);
    }

    public void DoTheColliderThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.ToggleCollider((bool)GenericClass.basicValue);
    }

    public void DoTheObjectThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.newTarget = (GameObject)GenericClass.basicValue;
        data.ChangeTarget();
    }

    public void DoTheSpriteThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.newObject = (GameObject)GenericClass.basicValue;
        data.ChangeSprite();
    }

    public void DoTheConditionThing()
    {
        Debug.Log("Run Condition: " + GenericClass.condition + " : " + GenericClass.thenobj + " : " + GenericClass.action);
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.RunCondition(GenericClass.condition, GenericClass.thenobj, GenericClass.action);
    }

    public void DoTheLoopThing()
    {
        Debug.Log("Run Loop: " + GenericClass.loopCount + " : " + GenericClass.basicValue);
        MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
        data.looping = true;
        Invoke(functionString, 1);
    }

    public void BackPeddle(DragAndDropItem item)
    {
        MovableObject moveData = ObjInt.currentObject.GetComponent<MovableObject>();
        ObjectChanger ObjChangData = ObjInt.currentObject.GetComponent<ObjectChanger>();
        switch (item.itemVariableType)
        {
            case VariableType.Type.Vector2:
                moveData.MoveBack();
                break;
            case VariableType.Type.Bool:
                ObjChangData.ToggleCollider(ObjChangData.originalBool);
                break;
            case VariableType.Type.GameObject:
                ObjChangData.newTarget = ObjChangData.originalTarget;
                ObjChangData.ChangeTarget();
                break;
            case VariableType.Type.Sprite:
                ObjChangData.newObject = ObjChangData.originalObject;
                ObjChangData.ChangeSprite();
                break;
            default:
                break;
        } 
    }

    public void ConditionalBackPeddle(DragAndDropItem item)
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        Debug.Log("ConditionBackPeddle: " + GenericClass.condition + " : " + GenericClass.thenobj + " : " + data.OriginalAction);
        switch(data.thisCondition)
        {
            case ObjectChanger.Conditions.BoolToFloat:
                MovableObject moveData = ObjInt.currentObject.GetComponent<MovableObject>();
                moveData.MoveBack();
                break;
            default:
                data.RunCondition(GenericClass.condition, GenericClass.thenobj, data.OriginalAction);
                break;

        }
    }
}
