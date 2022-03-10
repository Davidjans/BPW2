using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image m_ItemIcon;
    public BaseGear m_ItemInHere;
    public EquipmentSlot m_SlotType = EquipmentSlot.Inventory;
    
    private bool m_MouseOver = false;
    void Update()
    {
        if (m_MouseOver)
        {
            Debug.LogError("its over");
            if (Input.GetMouseButtonDown(0))
            {
                SelectSlot();
                Debug.LogError("its over2");
            }
        }
    }
 
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_MouseOver = true;
        Debug.Log("Mouse enter");
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        m_MouseOver = false;
        Debug.Log("Mouse exit");
    }
    public void ClearSlot()
    {
        if(m_ItemInHere != null)
            m_ItemInHere.m_InInventorySlot = null;
        m_ItemInHere = null;
        m_ItemIcon.sprite = null;
        if(m_SlotType != EquipmentSlot.Inventory)
            m_ItemIcon.transform.parent.gameObject.SetActive(false);
        m_ItemIcon.gameObject.SetActive(false);
    }
    public void LoadItem(BaseGear gearInHere)
    {
        m_ItemInHere = gearInHere;
        m_ItemIcon.sprite = gearInHere.m_GearSprite;
        gearInHere.m_InInventorySlot = this;
        if(m_SlotType != EquipmentSlot.Inventory)
            m_ItemIcon.transform.parent.gameObject.SetActive(true);
        m_ItemIcon.gameObject.SetActive(true);
    }

    public void LoadItemVisual()
    {
        m_ItemIcon.sprite = m_ItemInHere.m_GearSprite;
    }
    public void UnLoadItem()
    {
        m_ItemInHere = null;
        m_ItemIcon.sprite = null;
        m_ItemIcon.gameObject.SetActive(false);
    }
    [Button]
    public void SelectSlot()
    {
        VisualInventoryManager visualInventoryManager = VisualInventoryManager.Instance;
        if (m_SlotType == EquipmentSlot.Inventory)
        {
            visualInventoryManager.SelectInventorySlot(this);
            InterFaceMouseManager.Instance.MakeNewFollowingItem(this.gameObject);
        }
        else
        {
            visualInventoryManager.SelectLoadoutSlot(this);
        }
    }

    public bool CheckItemAllowed(BaseGear gear)
    {
        foreach (var slotType in gear.m_AllowedEquipmentSlots)
        {
            if (slotType == m_SlotType)
                return true;
        }

        return false;
    }
}
