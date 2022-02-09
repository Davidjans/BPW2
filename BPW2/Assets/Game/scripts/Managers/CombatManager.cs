using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Helltament.Entities;
public class CombatManager : MonoBehaviour
{
	/// <summary>
	/// Instance of our Singleton
	/// </summary>
	public static CombatManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<CombatManager>();
				if (_instance == null)
				{
					_instance = new GameObject("CombatManager").AddComponent<CombatManager>();
				}
			}
			return _instance;
		}
	}

	private static CombatManager _instance;

	[FoldoutGroup("Assign Things")]
	public TurnManager m_TurnManager;

	[HideInEditorMode]
	[FoldoutGroup("Debug Values")] public List<Entity> m_EntitiesInCombat;

	

	[FoldoutGroup("Debug Values")] public bool m_InCombat;



	[HideInEditorMode]
	[Button("Start Combat")]
	public void StartCombat()
	{
		m_InCombat = true;
		AddEntitiesToCombat();
		foreach (var entity in m_EntitiesInCombat)
		{
			entity.OnCombatStart();
		}
		
		m_TurnManager.OnCombatStart();
	}

	private void Awake()
	{
		// Destroy any duplicate instances that may have been created
		if (_instance != null && _instance != this)
		{
			Destroy(this);
			return;
		}

		_instance = this;
		if(m_TurnManager == null)
		{
			m_TurnManager = TurnManager.Instance;
		}
	}

	private void AddEntitiesToCombat()
	{
		Entity[] entityArray = FindObjectsOfType<Entity>();
		for (int i = 0; i < entityArray.Length; i++)
		{
			m_EntitiesInCombat.Add(entityArray[i]);
		}
	}
}
