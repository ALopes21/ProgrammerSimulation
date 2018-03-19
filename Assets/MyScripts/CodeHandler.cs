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
    public ObjectInteraction ObjInt;
    public bool FloatInCell, BoolInCell, ObjInCell, SprInCell;

    // Use this for initialization
    void Start () {
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectInteraction>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoTheMoveThing()
    {
        MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
        if(!FloatInCell)
        {
            if (droppedFloat != 0 && (droppedChar == 'y' || droppedChar == 'x'))
            {
                data.MoveObject(droppedFloat, droppedChar);
                FloatInCell = true;
            }
        }
    }
    public void DoTheColliderThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        if (!BoolInCell)
        {
            data.ToggleCollider();
            BoolInCell = true;
        }
    }

    public void DoTheObjectThing()
    {
        ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
        if (!ObjInCell)
        {
            if (droppedGO != null)
            {
                data.newTarget = droppedGO;
                data.ChangeTarget();
                ObjInCell = true;
            }
        }
        if(!SprInCell)
        {
            if(droppedSprite != null)
            {
                data.newObject = droppedSprite;
                data.ChangeSprite();
                SprInCell = true;
            }
        }
    }

    public void BackPeddle(DragAndDropItem item)
    {
        switch (item.itemVariableType)
        {
            case DragAndDropItem.ItemVariableType.Float:
                if(FloatInCell)
                {
                    MovableObject data = ObjInt.currentObject.GetComponent<MovableObject>();
                    data.MoveBack();
                    FloatInCell = false;
                }
                break;
            case DragAndDropItem.ItemVariableType.Bool:
                if(BoolInCell)
                {
                    ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
                    data.ToggleCollider();
                    BoolInCell = false;
                }
                break;
            case DragAndDropItem.ItemVariableType.GameObject:
                if (ObjInCell)
                {
                    ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
                    data.newTarget = data.prevTarget;
                    data.ChangeTarget();
                    ObjInCell = false;
                }
                break;
            case DragAndDropItem.ItemVariableType.Sprite:
                if(SprInCell)
                {
                    ObjectChanger data = ObjInt.currentObject.GetComponent<ObjectChanger>();
                    data.newObject = data.prevObject;
                    data.ChangeSprite();
                    SprInCell = false;
                }
                break;
            default:
                break;
        } 
    }
}
