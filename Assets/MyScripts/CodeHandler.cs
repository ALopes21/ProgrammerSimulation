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
        switch (item.itemVariableType)
        {
            case DragAndDropItem.ItemVariableType.Float:
                if(data.Move)
                {
                    data = ObjInt.currentObject.GetComponent<ObjectData>();
                    ObjInt.currentObject.gameObject.transform.position = data.previousPos;
                }
                break;
            case DragAndDropItem.ItemVariableType.Bool:
                DoTheBooleanThing();
                break;
            default:
                break;
        }
        
    }
}
