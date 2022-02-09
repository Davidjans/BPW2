using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Helltament.Entities;
using Pathfinding.Examples;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
	/// <summary>
	/// Instance of our Singleton
	/// </summary>
	public static TurnManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<TurnManager>();
				if (_instance == null)
				{
					_instance = new GameObject("TurnManager").AddComponent<TurnManager>();
				}
			}
			return _instance;
		}
	}

	private static TurnManager _instance;

	[FoldoutGroup("Assign Things")]
	[SerializeField]
	private CombatManager m_CombatManager;

	

	[FoldoutGroup("Debug Values")]
	[HideInEditorMode]
	public Entity CurrentTurn;

	[HideInEditorMode]
	[FoldoutGroup("Debug Values")] public List<Entity> m_EntitiesByInitiative;

	
	public void OnCombatStart()
	{
		UIManager.Instance.m_TurnOrderParent.SetActive(true);
		UIManager.Instance.m_AbilitiesParent.SetActive(true);
		OrderByInitiative();
		
	}

	public void OnCombatStop()
	{
		UIManager.Instance.m_PortraitImages[0].transform.root.gameObject.SetActive(false);
		UIManager.Instance.m_TurnOrderParent.SetActive(false);
		UIManager.Instance.m_AbilitiesParent.SetActive(false);
	}



	private void OrderByInitiative()
	{ 
		m_EntitiesByInitiative = new List<Entity>();
		m_EntitiesByInitiative.Add(m_CombatManager.m_EntitiesInCombat[0]);
		bool gotAdded = false;
		for (int i = 1; i < m_CombatManager.m_EntitiesInCombat.Count; i++)
		{
			m_CombatManager.m_EntitiesInCombat[i].OnCombatStart();
			gotAdded = false;
			for (int j = 0; j < m_EntitiesByInitiative.Count; j++)
			{
				if (m_CombatManager.m_EntitiesInCombat[i].m_Initiative > m_EntitiesByInitiative[j].m_Initiative)
				{
					m_EntitiesByInitiative.Insert(j, m_CombatManager.m_EntitiesInCombat[i]);
					gotAdded = true;
					break;
				}
			}

			if (!gotAdded)
			{
				m_EntitiesByInitiative.Add(m_CombatManager.m_EntitiesInCombat[i]);
			}
		}

		CurrentTurn = m_EntitiesByInitiative[0];
		m_EntitiesByInitiative[0].OnBecomeMyTurn();
		UIManager.Instance.ReArangePortraits();
		UIManager.Instance.ReArangeAbilities();
	}

	


	[HideInEditorMode]
	[Button("Next Turn")]
	public void NextTurn()
	{
		Entity entity = m_EntitiesByInitiative[0];
		entity.OnNoLongerMyTurn();
		m_EntitiesByInitiative.RemoveAt(0);
		m_EntitiesByInitiative.Add(entity);
		CurrentTurn = m_EntitiesByInitiative[0];
		m_EntitiesByInitiative[0].OnBecomeMyTurn();
		UIManager.Instance.ReArangeAbilities();
		UIManager.Instance.ReArangePortraits();
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
		if (m_CombatManager == null)
		{
			m_CombatManager = CombatManager.Instance;
		}
	}

}
