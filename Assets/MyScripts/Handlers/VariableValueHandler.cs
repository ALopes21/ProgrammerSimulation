using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableValueHandler : MonoBehaviour
{
    ObjectSelection ObjInt;
    CodeHandler handler;
    SceneHandler sceneHandler;

    GenericClass Class = new GenericClass();

    private void Start()
    {
        sceneHandler = GameObject.Find("Main Camera").GetComponent<SceneHandler>();
        ObjInt = GameObject.Find("Main Camera").GetComponent<ObjectSelection>();
    }

    public void CheckSlotVariableType(DragAndDropItem item, VariableSlot slot)
    {
        VariableSlot sourceCell = DragAndDropItem.sourceCell;
        //if dropped in the object slot of with the same variableType or Any
        if (slot.slotVariableType == item.itemVariableType || slot.slotVariableType == VariableType.Type.Any)
        {
            if(sourceCell.thisSlotType == VariableSlot.SlotType.Conditional)
            {
                CheckIFConditionalBackPeddle(item, sourceCell);
            }
            slot.SetItem(item, sourceCell);
            CheckItemType(item, slot);
        }
        //if dropped in inventory
        else if(slot.slotVariableType == VariableType.Type.None)
        {
            item.gameObject.GetComponent<Button>().interactable = false;

            switch(sourceCell.slotVariableType)
            {
                case VariableType.Type.None:
                    Debug.Log("Moved within Inventory");
                    slot.SetItem(item, sourceCell);
                    break;
                case VariableType.Type.Layered:
                    Debug.Log("Move Item Back to Inventory From Condition Slot");
                    slot.SetItem(item, sourceCell);
                    GameObject LayeredPanel = item.gameObject.transform.GetChild(1).gameObject;
                    LayeredPanel.SetActive(false);
                    //if the items conditional slots have items in them, put them back in the inventory
                    foreach (GameObject g in item.LayeredSlots)
                    {
                        if (g.transform.childCount > 0)
                        {
                            GameObject thisItem = g.transform.GetChild(0).gameObject;
                            for (int s = 0; s < sceneHandler.InvSlots.Length; s++)
                            {
                                if (sceneHandler.InvSlots[s].GetComponent<VariableSlot>().isTaken == false)
                                {
                                    sceneHandler.InvSlots[s].GetComponent<VariableSlot>().PlaceItem(thisItem);
                                    Destroy(thisItem);
                                    break;

                                }
                            }
                        }
                    }
                    //if all conditional slots are taken then backpeddle
                    if (AllLayeredSlotsTaken(item.LayeredSlots) == true)
                    {
                        Debug.Log("Conditional Backpeddle");
                        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
                        handler.ConditionalBackPeddle(item);
                    }
                    break;
                default:
                    switch(sourceCell.thisSlotType)
                    {
                        case VariableSlot.SlotType.Basic:
                            Debug.Log("Basic Backpeddle");
                            slot.SetItem(item, sourceCell);
                            handler = ObjInt.activePanel.GetComponent<CodeHandler>();
                            handler.BackPeddle(item);
                            StartCoroutine(item.DisableDropdown(0.2f, true));
                            break;
                        case VariableSlot.SlotType.Conditional:
                            slot.SetItem(item, sourceCell);
                            //if item is basic and moving out of a conditional slot...
                            CheckIFConditionalBackPeddle(item, sourceCell);
                            break;
                        case VariableSlot.SlotType.Looper:
                            slot.SetItem(item, sourceCell);
                            //if item is basic and moving out of a conditional slot...
                            CheckIFConditionalBackPeddle(item, sourceCell);
                            break;
                    }
                    break;
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
            default:
                break;
        }
    }

    public void SetItemValues(DragAndDropItem item, VariableSlot slot)
    {
        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
        switch (item.itemVariableType)
        {
            case VariableType.Type.Vector2:
                Vector2 newPosition = GetVector2(item.vector2_prop);
                Class.SetValues(slot, newPosition);
                SetupItemImage(item, Color.blue, item.vector2_prop);
                handler.functionString = "DoTheMoveThing";
                CallCodeHandler(slot);
                break;
            case VariableType.Type.Bool:
                Class.SetValues(slot, item.bool_prop);
                if (item.bool_prop == true)
                {
                    SetupItemImage(item, Color.green, "T");
                }
                else if (item.bool_prop == false)
                {
                    SetupItemImage(item, Color.red, "F");
                }
                handler.functionString = "DoTheColliderThing";
                CallCodeHandler(slot);
                break;
            case VariableType.Type.GameObject:
                Class.SetValues(slot, item.GO_prop);
                SetupItemImage(item, Color.white, item.GO_prop.name);
                handler.functionString = "DoTheObjectThing";
                CallCodeHandler(slot);
                break;
            case VariableType.Type.Sprite:
                Class.SetValues(slot, item.sprite_prop);
                SetupItemImage(item, Color.white, item.sprite_prop.name);
                handler.functionString = "DoTheSpriteThing";
                CallCodeHandler(slot);
                break;
            case VariableType.Type.Layered:
                GameObject LayeredPanel = item.gameObject.transform.GetChild(1).gameObject;
                LayeredPanel.SetActive(true);
                foreach (Transform child in LayeredPanel.transform)
                {
                    if (child.gameObject.tag == "LayeredSlots")
                    {
                        child.gameObject.GetComponent<VariableSlot>().parentItem = item;
                    }
                }
                break;
            case VariableType.Type.Int:
                Class.SetValues(slot, item.int_prop);
                SetupItemImage(item, Color.red, item.int_prop.ToString());
                if(slot.thisSlotType == VariableSlot.SlotType.Looper)
                {
                    CallCodeHandler(slot);
                }
                else
                {
                    handler.functionString = "DoTheIntThing";
                    CallCodeHandler(slot);
                }
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
            case VariableType.Type.Vector2:
                foreach (string s in item.vector2List_Prop)
                {
                    values.Add(s);
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
            case VariableType.Type.Vector2:
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
            case VariableType.Type.Int:
                item.gameObject.GetComponent<Image>().color = color;
                item.gameObject.GetComponentInChildren<Text>().text = display;
                break;
            case VariableType.Type.Any:
                item.gameObject.GetComponent<Image>().color = color; //gameObject.GetComponent<Image>().sprite = AnySprite;
                item.gameObject.GetComponentInChildren<Text>().text = display;
                break;
            default:
                break;
        }
    }



    //Repeated Checks//
    public bool AllLayeredSlotsTaken(List<GameObject> slots)
    {
        foreach(GameObject g in slots)
        {
            if (g.GetComponent<VariableSlot>().isTaken == false)
            {
                return false; }
        }
        return true;
    }

    public void CheckIFConditionalBackPeddle(DragAndDropItem item, VariableSlot source)
    {
        int takenCount = 0;
        foreach (GameObject g in source.parentItem.LayeredSlots)
        {
            if (g.GetComponent<VariableSlot>().isTaken == true)
            {
                takenCount++;
            }
        }
        if (takenCount >= 2)
        {
            Debug.Log("Run Conditional Backpeddle");
            handler = ObjInt.activePanel.GetComponent<CodeHandler>();
            handler.ConditionalBackPeddle(item);
        }
    }

    public void CallCodeHandler(VariableSlot slot)
    {
        switch (slot.thisSlotType)
        {
            case VariableSlot.SlotType.Basic:
                handler.Invoke(handler.functionString, 0);
                break;
            case VariableSlot.SlotType.Conditional:
                if (AllLayeredSlotsTaken(slot.parentItem.LayeredSlots) == true)
                {
                    handler.DoTheConditionThing();
                }
                break;
            case VariableSlot.SlotType.Looper:
                if(AllLayeredSlotsTaken(slot.parentItem.LayeredSlots) == true)
                {
                    handler.DoTheLoopThing();
                }
                break;
        }
    }

    public Vector2 GetVector2(string position)
    {
        string[] temp = position.Split(',');
        float x = System.Convert.ToSingle(temp[0]);
        float y = System.Convert.ToSingle(temp[1]);
        Vector2 rValue = new Vector2(x, y);
        return rValue;
    }
}
