using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image m_ItemIcon;
    public BaseGear m_ItemInHere;
    public LoadoutSlots m_SlotType = LoadoutSlots.Inventory;
    public void ClearSlot()
    {
        if(m_ItemInHere != null)
            m_ItemInHere.m_InInventorySlot = null;
        m_ItemInHere = null;
        m_ItemIcon.sprite = null;
        m_ItemIcon.gameObject.SetActive(false);
    }
    public void LoadItem(BaseGear gearInHere)
    {
        m_ItemInHere = gearInHere;
        m_ItemIcon.sprite = gearInHere.m_GearSprite;
        gearInHere.m_InInventorySlot = this;
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
        VisualInventoryManager visualInventoryManager = GetComponentInParent<VisualInventoryManager>();
        if (m_SlotType == LoadoutSlots.Inventory)
        {
            visualInventoryManager.SelectInventorySlot(this);
        }
        else
        {
            visualInventoryManager.SelectLoadoutSlot(this);
        }
    }

    public bool CheckItemAllowed(BaseGear gear)
    {
        foreach (var slotType in gear.m_AllowedLoadoutSlots)
        {
            if (slotType == m_SlotType)
                return true;
        }

        return false;
    }
}
