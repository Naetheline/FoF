using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotContentItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ITooltip
{
    public const int MAX_STACKITEM = 5;

    private Sprite empty;

    private Item content;
    public Item Content { get => content;  }
    public int qty;

    private Image icon;
    private TextMeshProUGUI qtyText;

    public Transform parentToReturnTo;

    private PlayerStats player;

    

    private void Awake()
    {

        player = GameObject.Find("PlayerCharacter").GetComponent<PlayerStats>();

        icon = GetComponentInChildren<Image>();
        if(icon == null)
        {
            Debug.LogError("No icon found for slot in inventory !");
        }
        qtyText = GetComponentInChildren<TextMeshProUGUI>();
        if (qtyText == null)
        {
            Debug.LogError("No qtyText found for slot in inventory !");
        }
        
    }
    private void Start()
    {
        empty = GameManager.itemSpriteDict["TRANSPARENCY"];
    }

    public bool TryAssignItem(Item item)
    {
        if(item == null)
        {
            Debug.LogError("SlotContent : Trying to add a null item !");
        }
        if(Content == null)
        {
            content = item;
            qty = 1;
            if(icon == null)
            {
                Debug.LogError("SlotContent : no icon for the content img...");
            }
            icon.sprite = item.icon;
            if(item.Stackable)
            {
                qtyText.text = qty.ToString();
            }
            else
            {
                qtyText.text = "";
            }
            return true;
        }
        else if (Content.Stackable && Content.Name == item.Name && qty < MAX_STACKITEM)
        {
            qty++;
            qtyText.text = qty.ToString();
            return true;
        }
        return false;
    }

    // Add the quantity toAdd until it reaches the MAX_STACKITEM.
    // Return the number of item that could not be added.
    public int Add(int toAdd)
    {
        int leftOver = 0;
        if (qty + toAdd > MAX_STACKITEM)
        {
            leftOver = toAdd - (MAX_STACKITEM - qty);
            qty = MAX_STACKITEM;
        }
        else
        {
            qty += toAdd;
        }
        qtyText.text = qty.ToString();
        return leftOver;
    }

    public void UseOne(PlayerStats player)
    {
        if (content != null && content.Type == Item.ItemType.WEAPON)
        {
            // Do nothing for now. MAybe try to equip it later
        }
        else
        {
            if (Content != null && Content.TryToUse(player))
            {
                if (qty > 1)
                {
                    qty--;
                    qtyText.text = qty.ToString();
                }
                else
                {
                    EmptyContent();
                }
            }
        }
    }

    public void EmptyContent()
    {
        content = null;
        icon.sprite = empty;
        qtyText.text = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = transform.parent;
        transform.SetParent(transform.root);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        transform.SetParent(parentToReturnTo);
        transform.localPosition = Vector3.zero;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UseOne(player);
        }
    }

    public string GetTooltip()
    {
        if (content != null)
        {
            return content.GetTooltip();
        }
        return null;
    }
}
