using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHandler : MonoBehaviour {

    //Variables
    public float droppedFloat;
    public char droppedChar;
    public bool droppedBool;
    public GameObject droppedGO;

    //References
    public ObjectInteraction ObjInt;
    public ObjectData data;

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectInteraction>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoTheMoveThing()
    {
        data = ObjInt.currentObject.GetComponent<ObjectData>();
        if(!data.FloatinCell)
        {
            if (droppedFloat != 0 && (droppedChar == 'y' || droppedChar == 'x'))
            {
                data.MoveObject(droppedFloat, droppedChar);
                data.FloatinCell = true;
            }
        }
    }
    public void DoTheColliderThing()
    {
        data = ObjInt.currentObject.GetComponent<ObjectData>();
        if (!data.BoolinCell)
        {
            data.ToggleCollider();
            data.BoolinCell = true;
        }
    }

    public void DoTheObjectThing()
    {
        data = ObjInt.currentObject.GetComponent<ObjectData>();
        if (!data.ObjinCell)
        {
            if (droppedGO != null)
            {
                data.ChangeTarget(droppedGO);
                data.ObjinCell = true;
            }
        }
    }

    public void BackPeddle(DragAndDropItem item)
    {
        switch (item.itemVariableType)
        {
            case DragAndDropItem.ItemVariableType.Float:
                if(data.FloatinCell)
                {
                    data = ObjInt.currentObject.GetComponent<ObjectData>();
                    ObjInt.currentObject.gameObject.transform.position = data.previousPos;
                    data.FloatinCell = false;
                }
                break;
            case DragAndDropItem.ItemVariableType.Bool:
                if(data.BoolinCell)
                {
                    data.ToggleCollider();
                    data.BoolinCell = false;
                }
                break;
            default:
                break;
        }
        
    }
}
