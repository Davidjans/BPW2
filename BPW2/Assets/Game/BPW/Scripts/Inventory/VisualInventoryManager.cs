using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
public class VisualInventoryManager : MonoBehaviour
{
    public List<InventorySlot> m_InventorySlots = new List<InventorySlot>();
    public InventorySlot m_SelectedSlot;
    public InventorySlot m_PreviouslySelectedSlot;
    public Image m_SelectedItemImage;
    [Button]
    private void Start()
    {
        SetVisualInventory();
    }

    public void SetVisualInventory()
    {
        foreach (var slot in m_InventorySlots)
        {
            slot.ClearSlot();
            if (slot.m_ItemInHere == null)
            {
                break;
            }
        }
        InventoryManager.Instance.LoadInventory();
        for (int i = 0; i < InventoryManager.Instance.m_OwnedGear.Count; i++)
        {
            m_InventorySlots[i].LoadItem(InventoryManager.Instance.m_OwnedGear[i]);
        }
    }
   
    public void SelectSlot(InventorySlot slotSelected)
    {
        if (slotSelected == m_SelectedSlot)
        {
            m_SelectedItemImage.gameObject.SetActive(false);
            m_SelectedSlot = null;
            return;
        }
        else
        {
            m_SelectedItemImage.gameObject.SetActive(true);
        }
        if(m_SelectedSlot != null)
            m_PreviouslySelectedSlot = m_SelectedSlot;
        
        
        m_SelectedSlot = slotSelected;
        //m_SelectedSlot.gameObject.SetActive(true);
        m_SelectedItemImage.transform.parent = m_SelectedSlot.transform;
        m_SelectedItemImage.transform.position = m_SelectedSlot.transform.position;
    }
    public void SelectLoadoutSlot(InventorySlot slotSelected)
    {
        SelectSlot(slotSelected);
        if (m_PreviouslySelectedSlot != null && m_PreviouslySelectedSlot.m_SlotType == LoadoutSlots.Inventory &&
            m_PreviouslySelectedSlot.m_ItemInHere != null && m_SelectedSlot.CheckItemAllowed(m_PreviouslySelectedSlot.m_ItemInHere)
            && m_PreviouslySelectedSlot.m_ItemInHere.m_InLoadout == false)
        {
            m_SelectedSlot.LoadItem(m_PreviouslySelectedSlot.m_ItemInHere);
            m_PreviouslySelectedSlot.m_ItemInHere.m_InLoadout = true;
        }
        //LoadoutManager.Instance.ChangeSelectedLoadoutGear(m_SelectedSlot.m_SlotType,m_SelectedSlot.m_ItemInHere);
    }
    public void SelectInventorySlot(InventorySlot slotSelected)
    {
        SelectSlot(slotSelected);
        if (m_PreviouslySelectedSlot.m_SlotType != LoadoutSlots.Inventory &&
            slotSelected.m_ItemInHere == null && m_PreviouslySelectedSlot.m_ItemInHere != null)
        {
            m_PreviouslySelectedSlot.m_ItemInHere.m_InLoadout = false;
            m_PreviouslySelectedSlot.UnLoadItem();
        }
    }
    
    
    public void UnSelectSlot()
    {
        
    }
}