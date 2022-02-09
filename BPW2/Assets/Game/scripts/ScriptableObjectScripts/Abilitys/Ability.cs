using System;
using System.Collections;
using System.Collections.Generic;
using Helltament.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{
	Melee,
	Ranged,
	AOE,
	ETC
}
[CreateAssetMenu(fileName = " Base", menuName = "Abilities/Base", order = -99)]
public class Ability : UnityEngine.ScriptableObject
{
	[HideInEditorMode] public Entity m_BelongTo;

	public float APCost;
	public Sprite m_AbilityImage;
	public int m_AbilityNumberOnEntity = 0;
	public bool m_IsAbilityUnlocked;
	public bool m_IsAbilityBought;
	public AbilityType m_AbilityType = AbilityType.Ranged;


	public virtual void ShowAbilityVisual()
	{

	}

	public virtual void ManualSetValuesOnStart()
	{

	}

	public virtual void ManualSetTarget()
	{
	}

	public virtual void ManualSetValuesOnEnd()
	{

	}


	public virtual void AbilityAction()
	{
		Debug.Log("Ability action performed!");
	}
}