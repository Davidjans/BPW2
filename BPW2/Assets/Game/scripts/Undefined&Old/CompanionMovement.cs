using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CompanionMovement : MonoBehaviour
{
	[SerializeField]
	private Player m_Player;

    [SerializeField]
    private NavMeshAgent m_NavAgent;

    [SerializeField]
    private Transform m_FollowObject;


    private void Update()
    {
	    if (!CombatManager.Instance.m_InCombat)
	    {
		    m_NavAgent.SetDestination(m_FollowObject.position);
	    }
    }
}
