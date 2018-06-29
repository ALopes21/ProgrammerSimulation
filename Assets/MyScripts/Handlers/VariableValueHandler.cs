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
        Debug.Log("Setting Item: " + item.name + " in " + slot.name);

        switch (sourceCell.slotVariableType)
        {
            case VariableType.Type.None: //if sourcecell is an Inventory Slot
                if (slot.slotVariableType == VariableType.Type.None)
                {
                    Debug.Log("Moved within Inventory");
                    slot.SetItem(item, sourceCell);
                }
                else if(slot.slotVariableType == item.itemVariableType || slot.slotVariableType == VariableType.Type.Any)
                {
                    Debug.Log("Dropped into the CodePanel");
                    slot.SetItem(item, sourceCell);
                    CheckItemType(item, slot);
                }
                else{ Debug.Log("Incorrect Variable Type"); sceneHandler.lives--;}
                break;

            case VariableType.Type.Layered: //if source cell is a layered slot
                if(slot.slotVariableType == VariableType.Type.None)
                {
                    Debug.Log("Move layered to inv & all slots inside");
                    slot.isTaken = true;
                    ResetLayeredSlots(sourceCell, item);
                    slot.SetItem(item, sourceCell);
                    SetupItemImage(item, item.originalColor, item.originalString, item.originalRect);
                }
                else if(slot.slotVariableType == item.itemVariableType || slot.slotVariableType == VariableType.Type.Any)
                {
                    Debug.Log("Move layered to layered");
                    slot.SetItem(item, sourceCell);
                    ResetLayeredSlots(sourceCell, item);
                    CheckItemType(item, slot);
                }
                else { Debug.Log("Incorrect Variable Type"); sceneHandler.lives--; }
                break;

            default: //if source cell has a variable type
                if(slot.slotVariableType == VariableType.Type.None)
                {
                    Debug.Log("Put back into Inventory -> Do backpeddle");
                    slot.SetItem(item, sourceCell);
                    SetupItemImage(item, item.originalColor, item.originalString, item.originalRect);
                    CheckBackPeddleType(sourceCell, item, slot);
                    StartCoroutine(item.DisableDropdown(0.2f, true));
                    sourceCell.ResetSlot();
                }
                else if(slot.slotVariableType == item.itemVariableType || slot.slotVariableType == VariableType.Type.Any)
                {
                    Debug.Log("Moved within code panel");
                    slot.SetItem(item, sourceCell);
                    CheckBackPeddleType(sourceCell, item, slot);
                    CheckItemType(item, slot);
                    sourceCell.ResetSlot();

                }
                else { Debug.Log("Incorrect Variable Type"); sceneHandler.lives--; }
                break;
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
                //item.gameObject.GetComponent<Button>().interactable = true;
                SetupItemImage(item, Color.clear, "", new Vector2(50, 10));
                item.GetComponent<RectTransform>().sizeDelta = slot.GetComponent<RectTransform>().sizeDelta;
                SetupDropDown(item);
                break;
            default:
                break;
        }
    }

    public void SetItemValues(DragAndDropItem item, VariableSlot slot)
    {
        handler = ObjInt.activePanel.GetComponent<CodeHandler>();
        SetupItemImage(item, Color.clear, "", new Vector2(50,10));
        item.GetComponent<RectTransform>().sizeDelta = slot.GetComponent<RectTransform>().sizeDelta;

        switch (item.itemVariableType)
        {
            case VariableType.Type.Vector2:
                Vector2 newPosition = GetVector2(item.vector2_prop);
                Class.SetValues(slot, newPosition);
                SetSlotImage(slot, "(" + item.vector2_prop + ");");
                handler.functionString = item.method_prop;
                CallCodeHandler(slot);
                break;
            case VariableType.Type.Bool:
                Class.SetValues(slot, item.bool_prop);
                SetSlotImage(slot, item.bool_prop.ToString() + ";");
                handler.functionString = item.method_prop;
                CallCodeHandler(slot);
                break;
            case VariableType.Type.GameObject:
                Class.SetValues(slot, item.GO_prop);
                SetSlotImage(slot, item.GO_prop.name + ";");
                handler.functionString = item.method_prop;
                CallCodeHandler(slot);
                break;
            case VariableType.Type.Sprite:
                Class.SetValues(slot, item.sprite_prop);
                SetSlotImage(slot, item.sprite_prop.name + ";");
                handler.functionString = item.method_prop;
                CallCodeHandler(slot);
                break;
            case VariableType.Type.Layered:
                GameObject LayeredPanel = slot.gameObject.transform.GetChild(1).gameObject;
                LayeredPanel.SetActive(true);
                SetSlotImage(slot, "");
                break;
            case VariableType.Type.Int:
                Class.SetValues(slot, item.int_prop);
                SetSlotImage(slot, item.int_prop.ToString() + ";");
                if (slot.thisSlotType == VariableSlot.SlotType.Looper)
                {
                    CallCodeHandler(slot);
                }
                else
                {
                    handler.functionString = item.method_prop;
                    CallCodeHandler(slot);
                }
                break;
            default:
                Debug.Log("An error occurred");
                break;
        }
    }

    public void CallCodeHandler(VariableSlot slot)
    {
        Debug.Log("Calling Code Handler");
        switch (slot.thisSlotType)
        {
            case VariableSlot.SlotType.Basic:
                handler.Invoke(handler.functionString, 0);
                break;
            case VariableSlot.SlotType.Conditional:
                if (AllLayeredSlotsTaken(slot.gameObject.transform.parent.GetComponentInParent<VariableSlot>().LayeredSlots) == true)
                {
                    handler.DoTheConditionThing();
                }
                break;
            case VariableSlot.SlotType.Looper:
                if (AllLayeredSlotsTaken(slot.gameObject.transform.parent.GetComponentInParent<VariableSlot>().LayeredSlots) == true)
                {
                    handler.DoTheLoopThing();
                }
                break;
        }
    }

    public void SetupDropDown(DragAndDropItem item)
    {
        Debug.Log("SetupDropdownvalues");
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

    public void SetupItemImage(DragAndDropItem item, Color color, string display, Vector2 rect)
    {
        item.gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta =  rect;
        switch (item.itemVariableType)
        {
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
                item.gameObject.GetComponent<Image>().color = color;
                item.gameObject.GetComponentInChildren<Text>().text = display;
                break;
        }
    }

    public void SetSlotImage(VariableSlot slot, string display)
    {
        slot.GetComponentInChildren<Text>().text = display;
        slot.GetComponentInChildren<Text>().color = Color.white;
    }



    //Repeated Checks//
    public void CheckBackPeddleType(VariableSlot sourceCell, DragAndDropItem item, VariableSlot slot)
    {
        switch (sourceCell.thisSlotType)
        {
            case VariableSlot.SlotType.Basic:
                handler = ObjInt.activePanel.GetComponent<CodeHandler>();
                handler.BackPeddle(item);
                break;
            case VariableSlot.SlotType.Conditional:
                //if item is basic and moving out of a conditional slot...
                CheckIFConditionalBackPeddle(item, sourceCell);
                break;
            case VariableSlot.SlotType.Looper:
                //if item is basic and moving out of a conditional slot...
                CheckIFConditionalBackPeddle(item, sourceCell);
                break;
        }
    }
    
    public void ResetLayeredSlots(VariableSlot sourceCell, DragAndDropItem item)
    {
        GameObject LayeredPanel = sourceCell.gameObject.transform.GetChild(1).gameObject;
        LayeredPanel.SetActive(false);

        //if all layered slots are taken then backpeddle
        if (AllLayeredSlotsTaken(sourceCell.LayeredSlots) == true)
        {
            Debug.Log("Conditional Backpeddle");
            handler = ObjInt.activePanel.GetComponent<CodeHandler>();
            handler.ConditionalBackPeddle(item);
        }

        //if the cells layered slots have items in them, put them back in the inventory
        foreach (GameObject g in sourceCell.LayeredSlots)
        {
            if (g.transform.childCount > 1)
            {
                GameObject thisItem = g.transform.GetChild(1).gameObject;

                for (int s = 0; s < sceneHandler.InvSlots.Length; s++)
                {
                    if (sceneHandler.InvSlots[s].GetComponent<VariableSlot>().isTaken == false)
                    {
                        Debug.Log(thisItem.name);
                        sceneHandler.InvSlots[s].GetComponent<VariableSlot>().PlaceItem(thisItem);
                        Destroy(thisItem);
                        g.GetComponent<VariableSlot>().ResetSlot();
                        break;
                    }
                }
            }
        }
        sourceCell.ResetSlot();
    }

    public bool AllLayeredSlotsTaken(List<GameObject> slots)
    {
        foreach (GameObject g in slots)
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
        foreach (GameObject g in source.transform.parent.parent.GetComponent<VariableSlot>().LayeredSlots)
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

    public Vector2 GetVector2(string position)
    {
        string[] temp = position.Split(',');
        float x = System.Convert.ToSingle(temp[0]);
        float y = System.Convert.ToSingle(temp[1]);
        Vector2 rValue = new Vector2(x, y);
        return rValue;
    }
}
