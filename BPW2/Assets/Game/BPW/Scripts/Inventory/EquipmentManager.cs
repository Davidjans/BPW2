using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class EquipmentManager : MonoBehaviour
{
	#region Instancing
	public static EquipmentManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<EquipmentManager>();
				if (_instance == null)
				{
					_instance = new GameObject("EquipmentManager").AddComponent<EquipmentManager>();
				}
			}
			return _instance;
		}
	}

	private static EquipmentManager _instance;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		if (_instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
	}
	#endregion
	public EquipmentSet m_PlayerEquipment;

	private void Start()
	{
		LoadSet();
	}

	[Button]
	private void LoadSet()
	{
		string fileName = "PlayerLoadouts";
		string tempEquipmentSetJson;
		ReplaceInstancesWithUniqueInstance();
		if (SaveManager.CheckJsonExistence(fileName))
		{
			m_PlayerEquipment = JsonConvert.DeserializeObject<EquipmentSet>(SaveManager.LoadTheJson(fileName));
		}
		else
		{
			SaveLoadouts();
			m_PlayerEquipment = JsonConvert.DeserializeObject<EquipmentSet>(SaveManager.LoadTheJson(fileName));
		}
	}
	[Button]
	private void SaveLoadouts()
	{
		string fileName = "PlayerLoadouts";
		ReplaceInstancesWithUniqueInstance();
		SaveManager.SaveTheJson(fileName, JsonConvert.SerializeObject(m_PlayerEquipment));
	}

	public void ReplaceInstancesWithUniqueInstance()
	{
		EquipmentSet tempSet;
		string gearValuesJson;
		EquipmentSet newEquipmentSet = EquipmentSet.CreateInstance<EquipmentSet>();
		gearValuesJson = JsonConvert.SerializeObject(m_PlayerEquipment);
		m_PlayerEquipment = JsonConvert.DeserializeObject<EquipmentSet>(gearValuesJson);
	}

	public BaseGear GetGearInLoadoutSlot(EquipmentSlot slotType)
	{
		foreach (var ownedGear in InventoryManager.Instance.m_OwnedGear)
		{
			if (ownedGear.m_UniqueGearID == m_PlayerEquipment.m_LoadoutSlots[slotType])
			{
				Debug.LogError("Found correct gear");
				return ownedGear;
			}
		}
		return null;
	}
}