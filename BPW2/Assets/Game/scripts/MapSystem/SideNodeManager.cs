using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SideNodeManager : MonoBehaviour
{
    public List<SideNode> m_SideNodesOwned = new List<SideNode>();
    public bool m_Expanded = false;
    
    private void Start()
    {
        m_SideNodesOwned = GetComponentsInChildren<SideNode>().ToList();
    }
}
