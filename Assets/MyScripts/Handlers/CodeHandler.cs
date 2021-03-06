﻿using System.Collections;
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
        switch (slot.thisSlotType)
        {
            case VariableSlot.SlotType.Basic:
                basicValue = value;
                Debug.Log(basicValue.GetType().Name);
                break;
            case VariableSlot.SlotType.Conditional:
                SetConditions(slot, value);
                break;
            case VariableSlot.SlotType.Looper:
                if (slot.thisConditionType == VariableSlot.ConditionType.LoopInt)
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
    public bool backpeddle;
    //References
    public ObjectSelection ObjInt;

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
        backpeddle = false;
    }

    public void DoTheMoveThing()
    {
        MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
        if (backpeddle){data.MoveBack();}
        else{data.MoveObject((Vector2)GenericClass.basicValue);}
        backpeddle = false;
    }

    public void DoTheColliderThing()
    {
        Debug.Log("Callin DoTheCOlliderThing");
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        if (backpeddle == true) { data.ToggleCollider(data.originalBool); backpeddle = false; return; }
        else { data.ToggleCollider((bool)GenericClass.basicValue); }
    }

    public void DoTheObjectThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        if (backpeddle){ data.newTarget = data.originalTarget; data.ChangeTarget();}
        else { data.newTarget = (GameObject)GenericClass.basicValue; data.ChangeTarget(); }
        backpeddle = false;
    }

    public void DoTheSpriteThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        if (backpeddle) { data.newObject = data.originalObject; data.ChangeSprite(); }
        else { data.newObject = (GameObject)GenericClass.basicValue; data.ChangeSprite(); }
        backpeddle = false;
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

    public void BackPeddle(DragAndDropItem item){
        backpeddle = true;
        Invoke(item.method_prop, 0);
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
