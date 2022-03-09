using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System;

[CreateAssetMenu(fileName = "EquipmentSet", menuName = "Wolfdog/DavidTools/Loadout/PlayerSet", order = 1)]
public class EquipmentSet : SerializedScriptableObject
{
	public string m_LoadoutName = "Set";
	public Dictionary<EquipmentSlot, float> m_LoadoutSlots = new Dictionary<EquipmentSlot, float>();
	public List<float> m_BackpackSlotsUniqueID;

	[HideInEditorMode]
	public bool m_FirstCreation = true;
	private void Awake()
	{
		if (m_FirstCreation)
		{
			OnCreate();
		}
	}

	[Button]
	private void OnCreate()
	{
		foreach (EquipmentSlot item in Enum.GetValues(typeof(EquipmentSlot)))
		{
			if (!m_LoadoutSlots.ContainsKey(item))
			{
				m_LoadoutSlots.Add(item, 0);
			}
		}
		m_FirstCreation = false;
	}
	[Button]
	public void ChangeSlot(EquipmentSlot slotToChange, BaseGear gearInSlot, int backpackSlotReplace = 0)
	{
		if (slotToChange != EquipmentSlot.Inventory)
		{
			if (!m_LoadoutSlots.ContainsKey(slotToChange))
			{
				m_LoadoutSlots.Add(slotToChange, 0);
			}
			m_LoadoutSlots[slotToChange] = gearInSlot.m_UniqueGearID;
		}
	}
	
	[Button]
	public void DebugAddToLoadout(EquipmentSlot slotToChange, uint gearID, int backpackSlotReplace = 0)
	{
		if (slotToChange != EquipmentSlot.Inventory)
		{
			if (!m_LoadoutSlots.ContainsKey(slotToChange))
			{
				m_LoadoutSlots.Add(slotToChange, 0);
			}
			m_LoadoutSlots[slotToChange] = gearID;
		}
	}
}