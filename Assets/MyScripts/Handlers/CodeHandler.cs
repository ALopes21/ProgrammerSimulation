using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericClass<T>
{
    static public T ifobj, condition, thenobj, action;
    static public T basicValue;

    public void SetConditions(VariableSlot slot, T value)
    {
        switch (slot.thisConditionType)
        {
            case VariableSlot.ConditionType.If:
                ifobj = value;
                break;
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
    public void SetValues(VariableSlot slot, T value)
    {
        switch(slot.thisSlotType)
        {
            case VariableSlot.SlotType.Basic:
                basicValue = value;
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
        Debug.Log(GenericClass<float>.basicValue + " , " +  charValue);
        MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
        if (GenericClass<float>.basicValue != 0 && (charValue == 'y' || charValue == 'x'))
        {
            data.MoveObject(GenericClass<float>.basicValue, charValue);
        }
    }

    public void DoTheColliderThing()
    {
        Debug.Log(GenericClass<bool>.basicValue);
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.ToggleCollider(GenericClass<bool>.basicValue);
    }

    public void DoTheObjectThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.newTarget = GenericClass<GameObject>.basicValue;
        data.ChangeTarget();
    }

    public void DoTheSpriteThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.newObject = GenericClass<GameObject>.basicValue;
        data.ChangeSprite();
    }

    public void DoTheConditionThing()
    {
        //Dosometing
        Debug.Log("Condition: " );
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
