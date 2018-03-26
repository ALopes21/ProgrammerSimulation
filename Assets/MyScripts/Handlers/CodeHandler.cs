using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeHandler : MonoBehaviour {

    //Variables
    public float droppedFloat;
    public char droppedChar;
    public bool droppedBool;
    public GameObject droppedGO;
    public GameObject droppedSprite;

    //References
    public ObjectSelection ObjInt;

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
    }
	
    public void DoTheMoveThing()
    {
        MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
        if (droppedFloat != 0 && (droppedChar == 'y' || droppedChar == 'x'))
        {
            data.MoveObject(droppedFloat, droppedChar);
        }
    }

    public void DoTheColliderThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        data.ToggleCollider(droppedBool);
    }

    public void DoTheObjectThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        if (droppedGO != null)
        {
            data.newTarget = droppedGO;
            data.ChangeTarget();
        }
        if(droppedSprite != null)
        {
            data.newObject = droppedSprite;
            data.ChangeSprite();
        }
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
                ObjChangData.newTarget = ObjChangData.prevTarget;
                ObjChangData.ChangeTarget();
                break;
            case VariableType.Type.Sprite:
                ObjChangData = ObjInt.currentObject.GetComponent<ObjectChanger>();
                ObjChangData.newObject = ObjChangData.prevObject;
                ObjChangData.ChangeSprite();
                break;
            default:
                break;
        } 
    }
}
