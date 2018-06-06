using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Every "drag and drop" item must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public enum ItemType
    {
        Basic,
        Option,
        Condition,
        Loop
    }

    public List<GameObject> ConditionalSlots = new List<GameObject>();

    //Current value
    public float float_prop;
    public char char_prop;
    public bool bool_prop;
    public GameObject GO_prop;
    public GameObject sprite_prop;

    //List of possible values loaded into the dropdowns
    public float[] floatList_prop;
    public char[] charList_prop;
    public List<bool> boolList_prop = new List<bool>();
    public GameObject[] gameObjectList_prop;
    public GameObject[] spriteList_prop;

    //Original Sprites, Colors and Text of the item
    public Color originalColor; //public Sprite OriginalSprite;
    public string originalString = "";

    public VariableType.Type itemVariableType = VariableType.Type.Float;
    public ItemType itemType = ItemType.Basic;

    static public DragAndDropItem draggedItem;                                      // Item that is dragged now
    static public GameObject icon;                                                  // Icon of dragged item
    static public VariableSlot sourceCell;
    public Dropdown dropdown;
    public VariableValueHandler valueHandler;
    public CodeHandler codeHandler;

    public delegate void DragEvent(DragAndDropItem item);
    static public event DragEvent OnItemDragStartEvent;                             // Drag start event
    static public event DragEvent OnItemDragEndEvent;                               // Drag end event

    void Start()
    {
        gameObject.GetComponent<Button>().interactable = false;
        valueHandler = GameObject.Find("Main Camera").GetComponent<VariableValueHandler>();
        if(itemType != ItemType.Condition)
        {
            dropdown = gameObject.transform.GetChild(1).gameObject.GetComponent<Dropdown>();
            dropdown.gameObject.SetActive(false);
        }
        if(itemType != ItemType.Option)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        if(itemType == ItemType.Condition)
        {
            Transform panel = transform.GetChild(1);
            foreach(Transform child in panel)
            {
                if (child.gameObject.tag == "ConditionalSlot")
                {
                    if(!ConditionalSlots.Contains(child.gameObject))
                    { ConditionalSlots.Add(child.gameObject); }

                }
            }
        }
        SetUpStartProp();
    }

    void SetUpStartProp()
    {
        //Change from Color to Sprite
        switch (itemVariableType)
        {
            case VariableType.Type.Float:
                originalColor = Color.blue;
                originalString = "F";
                if(itemType == ItemType.Basic)
                {
                    originalString = float_prop.ToString();
                }
                break;
            case VariableType.Type.Char:
                originalColor = Color.red;
                originalString = "C";
                if (itemType == ItemType.Basic)
                {
                    originalString = char_prop.ToString();
                }
                break;
            case VariableType.Type.Bool:
                originalColor = Color.cyan;
                originalString = "B";
                if(itemType == ItemType.Basic)
                {
                    if (bool_prop == true)
                    {
                        originalColor = Color.green;
                        originalString = "T";
                    }
                    else if (bool_prop == false)
                    {
                        originalColor = Color.red;
                        originalString = "F";
                    }
                }
                break;
            case VariableType.Type.GameObject:
                originalColor = Color.magenta;
                originalString = "Obj";
                if (itemType == ItemType.Basic)
                {
                    originalColor = Color.white;
                    originalString = GO_prop.name;
                }
                break;
            case VariableType.Type.Sprite:
                originalColor = Color.yellow;
                originalString = "Obj";
                if (itemType == ItemType.Basic)
                {
                    originalColor = Color.white;
                    originalString = sprite_prop.name;
                }
                break;
            case VariableType.Type.Any:
                originalColor = Color.grey;
                originalString = "";
                break;
            default:
                break;
        }
        valueHandler.SetupItemImage(this, originalColor, originalString);
    }

    /// <summary>
    /// This item is dragged
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        sourceCell = GetComponentInParent<VariableSlot>();
        draggedItem = this;                                                         // Set as dragged item
        icon = new GameObject("Icon");                                              // Create object for item's icon
        Image image = icon.AddComponent<Image>();                                   //Add the Image component to the Icon
        image.sprite = GetComponent<Image>().sprite;                                //Give Icon.sprite this.sprite
        image.color = GetComponent<Image>().color;                                  //Give Icon.color this.color
        image.raycastTarget = false;                                                // Disable icon's raycast for correct drop handling
        RectTransform iconRect = icon.GetComponent<RectTransform>();
        // Set icon's dimensions
        iconRect.sizeDelta = new Vector2(   GetComponent<RectTransform>().sizeDelta.x,
                                            GetComponent<RectTransform>().sizeDelta.y);
        Canvas canvas = GetComponentInParent<Canvas>();                             // Get parent canvas
        if (canvas != null)
        {
            // Display on top of all GUI (in parent canvas)
            icon.transform.SetParent(canvas.transform, true);                       // Set canvas as parent
            icon.transform.SetAsLastSibling();                                      // Set as last child in canvas transform
        }
        if (OnItemDragStartEvent != null)
        {
            OnItemDragStartEvent(this);                                             // Notify all about item drag start
        }
    }

    /// <summary>
    /// Every frame on this item drag
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        if (icon != null)
        {
            icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor
        }
    }

    /// <summary>
    /// This item is dropped
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (icon != null)
        {
            Destroy(icon);                                                          // Destroy icon on item drop
        }
        MakeVisible(true);                                                          // Make item visible in cell
        if (OnItemDragEndEvent != null)
        {
            OnItemDragEndEvent(this);                                               // Notify all cells about item drag end
        }
        draggedItem = null;
        icon = null;
        sourceCell = null;
    }

    /// <summary>
    /// Enable item's raycast
    /// </summary>
    /// <param name="condition"> true - enable, false - disable </param>
    public void MakeRaycast(bool condition)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = condition;
        }
    }

    /// <summary>
    /// Enable item's visibility
    /// </summary>
    /// <param name="condition"> true - enable, false - disable </param>
    public void MakeVisible(bool condition)
    {
        GetComponent<Image>().enabled = condition;
        GetComponentInChildren<Text>().enabled = condition;
    }

    public void GetDropdownValue()
    {
        int value = dropdown.GetComponent<Dropdown>().value - 1;
        if(value >= 0)
        {
            switch (itemVariableType)
            {
                case VariableType.Type.Float:
                    float_prop = floatList_prop[value];
                    break;
                case VariableType.Type.Char:
                    char_prop = charList_prop[value];
                    break;
                case VariableType.Type.Bool:
                    bool_prop = boolList_prop[value];
                    break;
                case VariableType.Type.GameObject:
                    GO_prop = gameObjectList_prop[value];
                    break;
                case VariableType.Type.Sprite:
                    sprite_prop = spriteList_prop[value];
                    break;
                case VariableType.Type.Any:
                    //leave this for now > Set type and value
                    break;
                default:
                    Debug.Log("An error occurred");
                    break;
            }
            StartCoroutine(DisableDropdown(0.2f, false));

            valueHandler.SetItemValues(this, transform.parent.GetComponent<VariableSlot>());

            //if (this.GetComponentInParent<VariableSlot>().thisSlotType == VariableSlot.SlotType.Conditional)
            //{
            //    valueHandler.SetConditionValues(this, this.transform.parent.GetComponent<VariableSlot>());
            //}
            //else
            //{
            //    valueHandler.SetPredefinedValues(this, this.transform.parent.GetComponent<VariableSlot>());
            //}

        }
        else
        {
            codeHandler = GameObject.Find("Main Camera").GetComponent<ObjectSelection>().activePanel.GetComponent<CodeHandler>();
            codeHandler.BackPeddle(this);
            valueHandler.SetupItemImage(this, originalColor, originalString);
            StartCoroutine(DisableDropdown(0.2f, true));
        }
       
    }

    public IEnumerator DisableDropdown(float seconds, bool reset)
    {
        yield return new WaitForSeconds(seconds);
        if(reset)
        {
            dropdown.value = 0;
        }
        dropdown.gameObject.SetActive(false);
    }

}
