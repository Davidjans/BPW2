using System.Collections;
using System.Collections.Generic;
using Helltament.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : Entity
{
	[FoldoutGroup("ExplorationSettings")]
	[SerializeField]
	private List<MonoBehaviour> m_TurnOnOnEnterExploration;

	[FoldoutGroup("ExplorationSettings")]
	[SerializeField]
	private List<MonoBehaviour> m_TurnOffOnEnterExploration;

	[FoldoutGroup("ExplorationSettings")]
	[SerializeField]
	private List<MonoBehaviour> m_TurnOnOnLeaveExploration;

	[FoldoutGroup("ExplorationSettings")]
	[SerializeField]
	private List<MonoBehaviour> m_TurnOffOnLeaveExploration;

	[FoldoutGroup("Additional Settings")] public bool m_IsMainCharacter;
    // Start is called before the first frame update
    void Start()
    {
        OnEnterExploration();
    }

    private void OnExitExploration()
    {
	    foreach (var item in m_TurnOffOnLeaveExploration)
	    {
		    item.enabled = false;
	    }
	    foreach (var item in m_TurnOnOnLeaveExploration)
	    {
		    item.enabled = true;
	    }
	}

    private void OnEnterExploration()
    {
		foreach (var item in m_TurnOffOnEnterExploration)
		{
			item.enabled = false;
		}
		foreach (var item in m_TurnOnOnEnterExploration)
		{
			item.enabled = true;
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
