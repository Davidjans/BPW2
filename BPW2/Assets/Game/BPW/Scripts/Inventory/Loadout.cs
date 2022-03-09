using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System;

[CreateAssetMenu(fileName = "Loadout", menuName = "Wolfdog/DavidTools/Loadout/PlayerLoadout", order = 1)]
public class Loadout : SerializedScriptableObject
{
	public string m_LoadoutName = "Loadout";
	public Dictionary<LoadoutSlots, float> m_LoadoutSlots = new Dictionary<LoadoutSlots, float>();
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
		foreach (LoadoutSlots item in Enum.GetValues(typeof(LoadoutSlots)))
		{
			if (!m_LoadoutSlots.ContainsKey(item))
			{
				m_LoadoutSlots.Add(item, 0);
			}
		}
		m_FirstCreation = false;
	}
	[Button]
	public void ChangeSlot(LoadoutSlots slotToChange, BaseGear gearInSlot, int backpackSlotReplace = 0)
	{
		if (slotToChange != LoadoutSlots.BackSlot)
		{
			if (!m_LoadoutSlots.ContainsKey(slotToChange))
			{
				m_LoadoutSlots.Add(slotToChange, 0);
			}
			m_LoadoutSlots[slotToChange] = gearInSlot.m_UniqueGearID;
		}
		else
		{
			m_BackpackSlotsUniqueID[backpackSlotReplace] = gearInSlot.m_UniqueGearID;
		}
	}
	
	[Button]
	public void DebugAddToLoadout(LoadoutSlots slotToChange, uint gearID, int backpackSlotReplace = 0)
	{
		if (slotToChange != LoadoutSlots.BackSlot)
		{
			if (!m_LoadoutSlots.ContainsKey(slotToChange))
			{
				m_LoadoutSlots.Add(slotToChange, 0);
			}
			m_LoadoutSlots[slotToChange] = gearID;
		}
		else
		{
			m_BackpackSlotsUniqueID[backpackSlotReplace] = gearID;
		}
	}
}