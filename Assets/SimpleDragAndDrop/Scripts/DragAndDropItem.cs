using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Every "drag and drop" item must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public enum ItemVariableType
    {
        Float,
        Bool,
        GameObject,
        Sprite
    }

    public float float_prop;
    public char char_prop;
    public bool bool_prop;
    public GameObject GO_prop;
    public GameObject sprite_prop;

    //public Sprite FloatSprite, BoolSprite, CharSprite, GOSprite, SprSprite;

    public ItemVariableType itemVariableType = ItemVariableType.Float;

    static public DragAndDropItem draggedItem;                                      // Item that is dragged now
    static public GameObject icon;                                                  // Icon of dragged item
    static public VariableSlot sourceCell;

    public delegate void DragEvent(DragAndDropItem item);
    static public event DragEvent OnItemDragStartEvent;                             // Drag start event
    static public event DragEvent OnItemDragEndEvent;                               // Drag end event

    void Start()
    {


        switch (itemVariableType)
        {
            case ItemVariableType.Float:
                //gameObject.GetComponent<Image>().sprite = FloatSprite;
                gameObject.GetComponentInChildren<Text>().text = "F";
                gameObject.GetComponent<Image>().color = Color.blue;
                    break;
            case ItemVariableType.Bool:
                //gameObject.GetComponent<Image>().sprite = BoolSprite;
                gameObject.GetComponentInChildren<Text>().text = "B";
                gameObject.GetComponent<Image>().color = Color.red;
                break;
            case ItemVariableType.GameObject:
                //gameObject.GetComponent<Image>().sprite = GOSprite;
                gameObject.GetComponentInChildren<Text>().text = "O";
                gameObject.GetComponent<Image>().color = Color.green;
                break;
            case ItemVariableType.Sprite:
                //gameObject.GetComponent<Image>().sprite = SprSprite;
                gameObject.GetComponentInChildren<Text>().text = "S";
                gameObject.GetComponent<Image>().color = Color.yellow;
                break;
            default:
                break;

        }
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
}
