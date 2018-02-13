using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHandler : MonoBehaviour {

    public float droppedFloat;
    public char droppedChar;
    public bool droppedBool;
    public ObjectInteraction ObjInt;
    public ObjectData data;

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectInteraction>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoTheThing()
    {
        data = ObjInt.currentObject.GetComponent<ObjectData>();
        if (data.Move)
        {
            if (droppedFloat != 0 && (droppedChar == 'y' || droppedChar == 'x'))
            {
                data.MoveObject(droppedFloat, droppedChar);
            }
        }
    }
    public void UndoTheThing()
    {
        data = ObjInt.currentObject.GetComponent<ObjectData>();
        if (data.Move)
        {
            Debug.Log(droppedChar + "," + droppedFloat);
            if (droppedChar == 'y' || droppedChar == 'x')
            {
                if(droppedFloat != 0 )
                {
                    droppedFloat -= droppedFloat * 2;
                    data.MoveObject(droppedFloat, droppedChar);
                }

            }
        }
    }
    public void DoTheBooleanThing()
    {
        data = ObjInt.currentObject.GetComponent<ObjectData>();
        if (data.Collider)
        {
            data.ToggleCollider();
        }
    }

    public void BackPeddle(DragAndDropItem item)
    {
        if (DragAndDropItem.sourceCell.variableType != VariableSlot.VariableType.None)
        {
            switch (item.itemVariableType)
            {
                case DragAndDropItem.ItemVariableType.Float:
                    droppedFloat = item.float_prop;
                    UndoTheThing();
                    //droppedFloat = 0;
                    break;
                case DragAndDropItem.ItemVariableType.Bool:
                    droppedBool = item.bool_prop;
                    DoTheBooleanThing();
                    break;
                default:
                    break;
            }
        }
    }
}
