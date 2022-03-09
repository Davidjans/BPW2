using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class LoadoutManager : MonoBehaviour
{
	#region Instancing
	public static LoadoutManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<LoadoutManager>();
				if (_instance == null)
				{
					_instance = new GameObject("LoadoutManager").AddComponent<LoadoutManager>();
				}
			}
			return _instance;
		}
	}

	private static LoadoutManager _instance;

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
	public int m_CurrentLoadout;
	public List<Loadout> m_PlayerLoadouts;

	public void SetLoadout(int id)
	{
		m_CurrentLoadout = id;
	}

	private void Start()
	{
		LoadLoadouts();
	}

	[Button]
	private void LoadLoadouts()
	{
		List<Loadout> tempList = new List<Loadout>();
		List<string> tempStringList = new List<string>();
		string fileName = "PlayerLoadouts";
		ReplaceInstancesWithUniqueInstance();
		if (SaveManager.CheckJsonExistence(fileName))
		{
			tempStringList = JsonConvert.DeserializeObject<List<string>>(SaveManager.LoadTheJson(fileName));
		}
		else
		{
			SaveLoadouts();
			tempStringList = JsonConvert.DeserializeObject<List<string>>(SaveManager.LoadTheJson(fileName));
		}
		foreach (string loadout in tempStringList)
		{
			Loadout newLoadout = Loadout.CreateInstance<Loadout>();
			newLoadout = JsonConvert.DeserializeObject<Loadout>(loadout);
			tempList.Add(newLoadout);
		}
		m_PlayerLoadouts = tempList;
	}
	[Button]
	private void SaveLoadouts()
	{
		string fileName = "PlayerLoadouts";
		List<string> tempList = new List<string>();
		ReplaceInstancesWithUniqueInstance();
		foreach (var loadout in m_PlayerLoadouts)
		{
			tempList.Add(JsonUtility.ToJson(loadout));
		}
		SaveManager.SaveTheJson(fileName, JsonConvert.SerializeObject(tempList));
	}

	public void ReplaceInstancesWithUniqueInstance()
	{
		List<Loadout> tempList = new List<Loadout>();
		string gearValuesJson;
		foreach (var gear in m_PlayerLoadouts)
		{
			Loadout newLoadout = Loadout.CreateInstance<Loadout>();
			gearValuesJson = JsonUtility.ToJson(newLoadout);
			JsonUtility.FromJsonOverwrite(gearValuesJson, newLoadout);
			tempList.Add(newLoadout);
		}
		m_PlayerLoadouts = tempList;
	}

	[Button]
	public void ChangeSelectedLoadoutGear(LoadoutSlots slotToChange, BaseGear gearInSlot, int backpackSlotReplace = 0)
	{
		foreach (var item in gearInSlot.m_AllowedLoadoutSlots)
		{
			if(item == slotToChange)
			{
				m_PlayerLoadouts[m_CurrentLoadout].ChangeSlot(slotToChange, gearInSlot, backpackSlotReplace);
				break;
			}
		}
		SaveLoadouts();
	}

	public BaseGear GetGearInLoadoutSlot(LoadoutSlots slotType)
	{
		foreach (var ownedGear in InventoryManager.Instance.m_OwnedGear)
		{
			if (m_PlayerLoadouts[m_CurrentLoadout].m_LoadoutSlots.ContainsKey(slotType))
			{
				if (ownedGear.m_UniqueGearID == m_PlayerLoadouts[m_CurrentLoadout].m_LoadoutSlots[slotType])
				{
				
					Debug.LogError("Found correct gear");
					return ownedGear;
				}
			}
		}
		return null;
	}
}