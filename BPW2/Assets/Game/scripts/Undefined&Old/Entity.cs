using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Examples;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Helltament.Entities
{
	public class Entity : MonoBehaviour
	{
		[FoldoutGroup("Assign Things")] 
		public EntityHealth m_EntityHealth;

		[FoldoutGroup("Assign Things/Ranged Attacks")]
		public Transform m_RangedAbilityOrigin;

		[FoldoutGroup("Assign Things/Ranged Attacks")]
		public LineRenderer m_RangedAbilityLineRenderer;

		[FoldoutGroup("Stats")]
		public float m_Initiative;

		[FoldoutGroup("Stats")]
		public float m_Health;

		[FoldoutGroup("Stats")]
		[HideInEditorMode]
		public float m_CurrentHealth;


		[FoldoutGroup("Stats")]
		public float m_Damage;

		[FoldoutGroup("Stats")]
		public float m_ActionPoints = 3;

		[FoldoutGroup("Stats")]
		public float m_MovementPerActionPoint = 2;

		[FoldoutGroup("Visual")]
		public Sprite m_ImageRepresentation;

		[FoldoutGroup("Debug Values")]
		[HideInEditorMode]
		public bool m_MyTurn = false;

		[FoldoutGroup("Debug Values")]
		[HideInEditorMode]
		public float m_CurrentActionPoints = 0;

		[FoldoutGroup("Debug Values")]
		[HideInEditorMode]
		public bool m_UsedFreeMovement = false;

		[FoldoutGroup("CombatSettings")]
		[SerializeField]
		private List<MonoBehaviour> m_TurnOnOnEnterTurn;

		[FoldoutGroup("CombatSettings")]
		[SerializeField]
		private List<MonoBehaviour> m_TurnOffOnEnterTurn;

		[FoldoutGroup("CombatSettings")]
		[SerializeField]
		private List<MonoBehaviour> m_TurnOffOnEndTurn;

		[FoldoutGroup("CombatSettings")]
		[SerializeField]
		private List<MonoBehaviour> m_TurnOnOnEndTurn;

		[FoldoutGroup("CombatSettings")]
		public List<Ability> m_Abilities = new List<Ability>();

		[FoldoutGroup("CombatSettings")] [HideInEditorMode]
		public int m_SelectedAbility = -1;

		[FoldoutGroup("CombatSettings")]
		public bool m_Friendly = false;

		#region PathfindingStuff

		[HideInInspector]
		public BlockManager.TraversalProvider traversalProvider;
		[HideInInspector]
		public BlockManager blockManager;
		[HideInInspector]
		public SingleNodeBlocker blocker;

		#endregion


		public void ChangeAPByValue(float value,bool changeUI = true)
		{
			m_CurrentActionPoints += value;
			if (changeUI)
			{
				UIManager.Instance.ChangeCurrentAPUI();
			}
			if(m_CurrentActionPoints <= 0)
			{
				TurnManager.Instance.NextTurn();
			}
		}


		

		public virtual void OnNoLongerMyTurn()
		{
			m_UsedFreeMovement = false;
			m_MyTurn = false;
			foreach (var item in m_TurnOffOnEndTurn)
			{
				item.enabled = false;
			}
			foreach (var item in m_TurnOnOnEndTurn)
			{
				item.enabled = true;
			}

			m_SelectedAbility = -1;
			UIManager.Instance.SelectAbilityUI(-1);
		}

		public virtual void OnBecomeMyTurn()
		{
			//TurnBasedManager.Instance.SetUnitTurn(this);
			m_MyTurn = true;
			m_UsedFreeMovement = false;
			m_CurrentActionPoints = m_ActionPoints;
			VisualizeManager.Instance.m_CurrentStart = transform;
			UIManager.Instance.ChangeCurrentAPUI();
			foreach (var item in m_TurnOnOnEnterTurn)
			{
				item.enabled = false;
			}
			foreach (var item in m_TurnOffOnEnterTurn)
			{
				item.enabled = true;
			}
			m_SelectedAbility = -1;
			UIManager.Instance.SelectAbilityUI(-1);
			GetComponent<AIDestinationSetter>().target.position = transform.position;
		}

		public void SetAbilityValuesDeselect()
		{
			Ability ability = m_Abilities[m_SelectedAbility];
			if (ability.m_AbilityType == AbilityType.Ranged)
			{
				SetRangedAbilityValuesEnd(ability);
			}
		}


		public void SetAbilityValuesSelect()
		{
			Ability ability = m_Abilities[m_SelectedAbility];
			if (ability.m_AbilityType == AbilityType.Ranged)
			{
				SetRangedAbilityValuesStart(ability);
			}
		}

		private void SetRangedAbilityValuesEnd(Ability ability)
		{
			RangedAttack rangedAbility = (RangedAttack)ability;
			rangedAbility.ManualSetValuesOnEnd();
		}
		 
		private void SetRangedAbilityValuesStart(Ability ability)
		{
			RangedAttack rangedAbility = (RangedAttack)ability;
			if (m_RangedAbilityOrigin != null)
			{
				rangedAbility.ManualSetValuesOnStart(m_RangedAbilityOrigin,m_RangedAbilityLineRenderer);
			}
		}

		public void OnCombatStart()
		{

		}

		private void Awake()
		{
			blockManager = FindObjectOfType<BlockManager>();
			if (blockManager != null)
			{
				blocker = GetComponentInChildren<SingleNodeBlocker>();
				if (blocker != null)
				{
					// Set the traversal provider to block all nodes that are blocked by a SingleNodeBlocker
					// except the SingleNodeBlocker owned by this AI (we don't want to be blocked by ourself)
					traversalProvider = new BlockManager.TraversalProvider(blockManager, BlockManager.BlockMode.AllExceptSelector, new List<SingleNodeBlocker>() { blocker });
				}
			}

			for (int i = 0; i < m_Abilities.Count; i++)
			{
				m_Abilities[i].m_BelongTo = this;
				m_Abilities[i].m_AbilityNumberOnEntity = i;
			}

			if (m_EntityHealth == null)
			{
				m_EntityHealth = GetComponent<EntityHealth>();
			}

			if (m_EntityHealth != null)
			{
				m_EntityHealth.m_MaxHealth = m_Health;
				m_EntityHealth.HealToFull();
				m_EntityHealth.m_EntityBelongingTo = this;
			}
		}
	}

}
