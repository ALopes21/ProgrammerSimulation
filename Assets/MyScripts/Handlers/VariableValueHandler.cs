using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableValueHandler : MonoBehaviour
{
    ObjectSelection ObjInt;
    CodeHandler handler;
    SceneHandler sceneHandler;

    private void Start()
    {
        sceneHandler = GameObject.Find("Main Camera").GetComponent<SceneHandler>();
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
    }

    public void CheckItemVariableType(DragAndDropItem item, VariableSlot slot)
    {
        VariableSlot sourceCell = DragAndDropItem.sourceCell;
        if (slot.slotVariableType == item.itemVariableType)
        {
            slot.SetItem(item, sourceCell);
            CheckItemType(item);
        }
        else if(slot.slotVariableType == VariableType.Type.Any)
        {
            slot.SetItem(item, sourceCell);
            CheckItemType(item);
        }
        else if(slot.slotVariableType == VariableType.Type.None)
        {
            item.gameObject.GetComponent<Button>().interactable = false;
            if(sourceCell.slotVariableType == VariableType.Type.None)
            {
                Debug.Log("SourceCell: " + sourceCell.gameObject);
                slot.SetItem(item, sourceCell);
            }
            else
            {
                slot.SetItem(item, sourceCell);
                handler = ObjInt.activePanel.GetComponent<CodeHandler>();
                handler.BackPeddle(item);
                StartCoroutine(item.DisableDropdown(0.2f, true));
            }
        }
        else
        {
            Debug.Log("Incorrect Variable Type");
            sceneHandler.lives--;
        }
    }

    public void CheckItemType(DragAndDropItem item)
    {
        switch(item.itemType)
        {
            case DragAndDropItem.ItemType.Basic:
                SetPredefinedValues(item);
                break;
            case DragAndDropItem.ItemType.Option:
                //Set OptionPanels and Get values
                item.gameObject.GetComponent<Button>().interactable = true;
                SetupDropDown(item);
                break;
            default:
                break;
        }
    }

    public void SetPredefinedValues(DragAndDropItem item)
    {
        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
        switch (item.itemVariableType)
        {
            case VariableType.Type.Float:
                handler.droppedFloat = item.float_prop;
                handler.droppedChar = item.char_prop;
                handler.DoTheMoveThing();
                break;
            case VariableType.Type.Bool:
                handler.droppedBool = item.bool_prop;
                handler.DoTheColliderThing();
                break;
            case VariableType.Type.GameObject:
                handler.droppedGO = item.GO_prop;
                handler.DoTheObjectThing();
                break;
            case VariableType.Type.Sprite:
                handler.droppedSprite = item.sprite_prop;
                handler.DoTheObjectThing();
                break;
            default:
                Debug.Log("An error occurred");
                break;
        }
    }

    public void SetupDropDown(DragAndDropItem item)
    {
        List<string> values = new List<string>();
        values.Add("");
        switch (item.itemVariableType)
        {
            case VariableType.Type.Float:
                foreach (float f in item.floatList_prop)
                {
                    values.Add(f.ToString());
                }
                break;
            case VariableType.Type.Bool:
                foreach(bool b in item.boolList_prop)
                {
                    values.Add(b.ToString());
                }
                break;
            case VariableType.Type.GameObject:
                foreach (GameObject g in item.gameObjectList_prop)
                {
                    values.Add(g.ToString());
                }
                break;
            case VariableType.Type.Sprite:
                foreach (GameObject s in item.spriteList_prop)
                {
                    values.Add(s.ToString());
                }
                break;
            case VariableType.Type.Any:
                //leave this for now > Set type and value
                break;
            default:
                Debug.Log("An error occurred");
                break;
        }
        AddDropOptions(item, values);
        values.Clear();

    }

    public void AddDropOptions(DragAndDropItem item, List<string> list)
    {
        item.dropdown.gameObject.SetActive(true);
        item.dropdown.ClearOptions();
        item.dropdown.AddOptions(list);
        item.dropdown.RefreshShownValue();
    }
}
