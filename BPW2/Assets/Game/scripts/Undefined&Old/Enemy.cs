using System.Collections;
using System.Collections.Generic;
using Helltament.Entities;
using UnityEngine;
using Sirenix.OdinInspector;
public class Enemy : Entity
{
	[FoldoutGroup("AI")]
	public AIMovement m_Movement;

	[FoldoutGroup("AI")]
	public AIOffense m_Offense;

	public Transform m_AttackTarget;
}
