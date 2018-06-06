using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericClass
{
    static public object condition, thenobj, action;
    static public object basicValue;

    public void SetConditions(VariableSlot slot, object value)
    {
        switch (slot.thisConditionType)
        {
            case VariableSlot.ConditionType.That:
                condition = value;
                break;
            case VariableSlot.ConditionType.Then:
                thenobj = value;
                break;
            case VariableSlot.ConditionType.This:
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
        }
    }
}

public class CodeHandler : MonoBehaviour {

    public string functionString;
    public char charValue;

    //References
    public ObjectSelection ObjInt;

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
    }
	
    public void DoTheMoveThing()
    {
        MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
        if ((float)GenericClass.basicValue != 0 && (charValue == 'y' || charValue == 'x'))
        {
            data.MoveObject((float)GenericClass.basicValue, charValue);
        }
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

    public void BackPeddle(DragAndDropItem item)
    {
        MovableObject moveData = ObjInt.currentObject.GetComponent<MovableObject>();
        ObjectChanger ObjChangData = ObjInt.currentObject.GetComponent<ObjectChanger>();
        switch (item.itemVariableType)
        {
            case VariableType.Type.Float:
                moveData = ObjInt.currentObject.GetComponent<MovableObject>();
                moveData.MoveBack();
                break;
            case VariableType.Type.Bool:
                ObjChangData = ObjInt.currentObject.GetComponent<ObjectChanger>();
                ObjChangData.ToggleCollider(ObjChangData.originalBool);
                break;
            case VariableType.Type.GameObject:
                ObjChangData = ObjInt.currentObject.GetComponent<ObjectChanger>();
                ObjChangData.newTarget = ObjChangData.originalTarget;
                ObjChangData.ChangeTarget();
                break;
            case VariableType.Type.Sprite:
                ObjChangData = ObjInt.currentObject.GetComponent<ObjectChanger>();
                ObjChangData.newObject = ObjChangData.originalObject;
                ObjChangData.ChangeSprite();
                break;
            default:
                break;
        } 
    }

    public void ConditionalBackPeddle(DragAndDropItem item)
    {
        //Do Something
        Debug.Log("ConditionBackPeddle: ");
    }
}
