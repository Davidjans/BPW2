using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region Instance

	public static UIManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<UIManager>();
				if (_instance == null)
				{
					_instance = new GameObject("UIManager").AddComponent<UIManager>();
				}
			}
			return _instance;
		}
	}

	private static UIManager _instance;

	#endregion

	[FoldoutGroup("CombatUI")] public TextMeshProUGUI m_CurrentAP;

	
	[FoldoutGroup("CombatUI/TurnOrder")] public GameObject m_TurnOrderParent;

	[FoldoutGroup("CombatUI/TurnOrder")] public List<Image> m_PortraitImages;

	[FoldoutGroup("CombatUI/TurnOrder")] public Transform m_PortraitSelected;


	[FoldoutGroup("CombatUI/Abilities")] public GameObject m_AbilitiesParent;

	[FoldoutGroup("CombatUI/Abilities")] public List<Image> m_AbilityImages;
	
	[FoldoutGroup("CombatUI/Abilities")] public Transform m_AbilitiesSelected;

	// Start is called before the first frame update
	void Awake()
    {
		// Destroy any duplicate instances that may have been created
		if (_instance != null && _instance != this)
		{
			Destroy(this);
			return;
		}

		_instance = this;
	}

	public void ChangeCurrentAPUI()
	{
		m_CurrentAP.text = "CurrentAP: " + TurnManager.Instance.CurrentTurn.m_CurrentActionPoints;
	}
	
	public void ReArangePortraits()
	{
		m_PortraitImages[0].transform.root.gameObject.SetActive(true);
		for (int i = 0; i < TurnManager.Instance.m_EntitiesByInitiative.Count; i++)
		{
			m_PortraitImages[i].gameObject.SetActive(true);
			m_PortraitImages[i].sprite = TurnManager.Instance.m_EntitiesByInitiative[i].m_ImageRepresentation;
		}

		m_PortraitSelected.position = m_PortraitImages[0].transform.position;

		if (m_PortraitImages.Count > TurnManager.Instance.m_EntitiesByInitiative.Count)
		{
			for (int i = TurnManager.Instance.m_EntitiesByInitiative.Count; i < m_PortraitImages.Count; i++)
			{
				m_PortraitImages[i].transform.parent.gameObject.SetActive(false);
			}
		}
	}

	public void ReArangeAbilities()
	{
		m_AbilityImages[0].transform.root.gameObject.SetActive(true);
		for (int i = 0; i < TurnManager.Instance.CurrentTurn.m_Abilities.Count; i++)
		{
			m_AbilityImages[i].gameObject.SetActive(true);
			m_AbilityImages[i].sprite = TurnManager.Instance.CurrentTurn.m_Abilities[i].m_AbilityImage;
		}

		if (m_AbilityImages.Count > TurnManager.Instance.CurrentTurn.m_Abilities.Count)
		{
			for (int i = TurnManager.Instance.CurrentTurn.m_Abilities.Count; i < m_AbilityImages.Count; i++)
			{
				m_AbilityImages[i].transform.parent.gameObject.SetActive(false);
			}
		}
	}

	public void SelectAbilityUI(int SelectedAbility)
	{
		if (SelectedAbility == TurnManager.Instance.CurrentTurn.m_SelectedAbility)
		{
			SelectedAbility = -1;
		}
		if (SelectedAbility != -1)
		{
			m_AbilitiesSelected.gameObject.SetActive(true);
			m_AbilitiesSelected.position = m_AbilityImages[SelectedAbility].transform.position;
			if (TurnManager.Instance.CurrentTurn.m_SelectedAbility != -1)
				TurnManager.Instance.CurrentTurn.SetAbilityValuesDeselect();
			TurnManager.Instance.CurrentTurn.m_SelectedAbility = SelectedAbility;
			TurnManager.Instance.CurrentTurn.SetAbilityValuesSelect();
		}
		else
		{
			m_AbilitiesSelected.gameObject.SetActive(false);
			if(TurnManager.Instance.CurrentTurn.m_SelectedAbility != -1)
				TurnManager.Instance.CurrentTurn.SetAbilityValuesDeselect();
			TurnManager.Instance.CurrentTurn.m_SelectedAbility = SelectedAbility;
		}
		
	}
}
