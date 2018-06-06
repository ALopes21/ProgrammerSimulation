using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableValueHandler : MonoBehaviour
{
    ObjectSelection ObjInt;
    CodeHandler handler;
    SceneHandler sceneHandler;

    GenericClass<float> floatClass = new GenericClass<float>();
    GenericClass<bool> boolClass = new GenericClass<bool>();
    GenericClass<GameObject> GOClass = new GenericClass<GameObject>();

    private void Start()
    {
        sceneHandler = GameObject.Find("Main Camera").GetComponent<SceneHandler>();
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
    }

    public void CheckItemVariableType(DragAndDropItem item, VariableSlot slot)
    {
        VariableSlot sourceCell = DragAndDropItem.sourceCell;
        //only for basic items
        if (slot.slotVariableType == item.itemVariableType && item.itemType != DragAndDropItem.ItemType.Condition)
        {
                slot.SetItem(item, sourceCell);
                CheckItemType(item, slot);
        }
        //if dropped in slot that takes any item type && used for condition items
        else if(slot.slotVariableType == VariableType.Type.Any)
        {
            slot.SetItem(item, sourceCell);
            CheckItemType(item, slot);
        }
        //if dropped in inventory
        else if(slot.slotVariableType == VariableType.Type.None)
        {
            item.gameObject.GetComponent<Button>().interactable = false;
            //if moving withing inventory
            if(sourceCell.slotVariableType == VariableType.Type.None)
            {
                Debug.Log("Moved within Inventory");
                slot.SetItem(item, sourceCell);
            }
            //if moving back to inventory from a conditional or condition slot
            else if (sourceCell.thisSlotType == VariableSlot.SlotType.Conditional)
            {
                Debug.Log("Move Item Back to Inventory");
                slot.SetItem(item, sourceCell);

                //If item is a Conditional Item and has all its Conditional slots Taken then BackPeddle
                if (item.itemType == DragAndDropItem.ItemType.Condition)
                {
                    GameObject conditionPanel = item.gameObject.transform.GetChild(1).gameObject;
                    conditionPanel.SetActive(false);
                    foreach (GameObject g in item.ConditionalSlots)
                    {
                        //if the conditional slots have items in them...
                        if (g.transform.childCount > 0)
                        {
                            GameObject thisItem = g.transform.GetChild(0).gameObject;
                            GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot");
                            for (int s = 0; s < slots.Length; s++)
                            {
                                if (slots[s].GetComponent<VariableSlot>().isTaken == false)
                                {
                                    //put the items back in the inventory panel.
                                    slots[s].GetComponent<VariableSlot>().PlaceItem(thisItem);
                                    Destroy(thisItem);
                                    break;

                                }
                            }
                        }
                    }
                    if (AllConditionalSlotsTaken(item.ConditionalSlots) == true)
                    {
                        Debug.Log("Conditional Backpeddle");
                        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
                        handler.ConditionalBackPeddle(item);
                    }
                }
                //if item is basic and moving out of a conditional slot...
                else if(item.itemType != DragAndDropItem.ItemType.Condition)
                {
                    int takenCount = 0;
                    foreach (GameObject g in sourceCell.parentItem.ConditionalSlots)
                    {
                        if (g.GetComponent<VariableSlot>().isTaken == true)
                        {
                            takenCount++;
                        }
                    }
                    if(takenCount >= 3)
                    {
                        Debug.Log("Conditional Backpeddle");
                        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
                        handler.ConditionalBackPeddle(item);
                    }
                }
            }
            // if no conditions involved - do basic backpeddle
            else if (sourceCell.thisSlotType != VariableSlot.SlotType.Conditional)
            {
                Debug.Log("Basic Backpeddle");
                slot.SetItem(item, sourceCell);
                handler = ObjInt.activePanel.GetComponent<CodeHandler>();
                handler.BackPeddle(item);
                StartCoroutine(item.DisableDropdown(0.2f, true));
            }
        }
        //if dropped in wrong slot
        else
        {
            Debug.Log("Incorrect Variable Type");
            sceneHandler.lives--;
        }
    }

    public void CheckItemType(DragAndDropItem item, VariableSlot slot)
    {
        switch(item.itemType)
        {
            case DragAndDropItem.ItemType.Basic:
                SetItemValues(item, slot);
                break;
            case DragAndDropItem.ItemType.Option:
                item.gameObject.GetComponent<Button>().interactable = true;
                SetupDropDown(item);
                break;
            case DragAndDropItem.ItemType.Condition:
                GameObject conditionPanel = item.gameObject.transform.GetChild(1).gameObject;
                conditionPanel.SetActive(true);
                foreach(Transform child in conditionPanel.transform)
                {
                    if(child.gameObject.tag == "ConditionalSlot")
                    {
                        child.gameObject.GetComponent<VariableSlot>().parentItem = item;
                    }
                }
                break;
            case DragAndDropItem.ItemType.Loop:
                item.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                //wait for children items to be filled
                //set Loop values
                break;
            default:
                break;
        }
    }

    public void SetItemValues(DragAndDropItem item, VariableSlot slot)
    {
        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
        switch (item.itemVariableType)
        {
            case VariableType.Type.Float:
                floatClass.SetValues(slot, item.float_prop);
                handler.charValue = item.char_prop;
                SetupItemImage(item, Color.blue, item.float_prop.ToString());
                handler.functionString = "DoTheMoveThing";
                break;
            case VariableType.Type.Char:
                handler.charValue = item.char_prop;
                SetupItemImage(item, Color.red, item.char_prop.ToString());
                //handler.functionString = "DoTheMoveThing";
                //handler.DoTheMoveThing();
                break;
            case VariableType.Type.Bool:
                boolClass.SetValues(slot, item.bool_prop);
                if (item.bool_prop == true)
                {
                    SetupItemImage(item, Color.green, "T");
                }
                else if (item.bool_prop == false)
                {
                    SetupItemImage(item, Color.red, "F");
                }
                handler.functionString = "DoTheColliderThing";
                break;
            case VariableType.Type.GameObject:
                GOClass.SetValues(slot, item.GO_prop);
                SetupItemImage(item, Color.white, item.GO_prop.name);
                handler.functionString = "DoTheObjectThing";
                break;
            case VariableType.Type.Sprite:
                GOClass.SetValues(slot, item.sprite_prop);
                SetupItemImage(item, Color.white, item.sprite_prop.name);
                handler.functionString = "DoTheObjectThing";
                break;
            default:
                Debug.Log("An error occurred");
                break;
        }
        switch (slot.thisSlotType)
        {
            case VariableSlot.SlotType.Basic:
                handler.Invoke(handler.functionString, 0);
                break;
            case VariableSlot.SlotType.Conditional:
                if (AllConditionalSlotsTaken(slot.parentItem.ConditionalSlots) == true)
                {
                    handler.DoTheConditionThing();
                }
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
            case VariableType.Type.Char:
                foreach (char c in item.charList_prop)
                {
                    values.Add(c.ToString());
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

    public void SetupItemImage(DragAndDropItem item, Color color, string display)
    {
        switch (item.itemVariableType)
        {
            case VariableType.Type.Float:
                item.gameObject.GetComponent<Image>().color = color; //gameObject.GetComponent<Image>().sprite = FloatSprite;
                item.gameObject.GetComponentInChildren<Text>().text = display;
                break;
            case VariableType.Type.Char:
                item.gameObject.GetComponent<Image>().color = color; //gameObject.GetComponent<Image>().sprite = FloatSprite;
                item.gameObject.GetComponentInChildren<Text>().text = display;
                break;
            case VariableType.Type.Bool:
                item.gameObject.GetComponent<Image>().color = color; //gameObject.GetComponent<Image>().sprite = BoolSprite;
                item.gameObject.GetComponentInChildren<Text>().text = display;
                break;
            case VariableType.Type.GameObject:
                item.gameObject.GetComponent<Image>().color = color;
                item.gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("ItemSprites/" + display, typeof(Sprite));
                item.gameObject.GetComponentInChildren<Text>().text = "";
                break;
            case VariableType.Type.Sprite:
                item.gameObject.GetComponent<Image>().color = color;
                item.gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("ItemSprites/" + display, typeof(Sprite));
                item.gameObject.GetComponentInChildren<Text>().text = "";
                break;
            case VariableType.Type.Any:
                item.gameObject.GetComponent<Image>().color = color; //gameObject.GetComponent<Image>().sprite = AnySprite;
                item.gameObject.GetComponentInChildren<Text>().text = display;
                break;
            default:
                break;
        }
    }

    public bool AllConditionalSlotsTaken(List<GameObject> slots)
    {
        foreach(GameObject g in slots)
        {
            if (g.GetComponent<VariableSlot>().isTaken == false)
            { return false; }
        }
        return true;
    }
}
