using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Julian.MapSystem
{
    public class Row : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_CostTextGUI;

        [ReadOnly] public int Index;
        [ReadOnly] public int Cost { get; private set; }

        public void SetCost(int cost)
        {
            Cost = cost;
            m_CostTextGUI.text = cost.ToString();
        }

        [ReadOnly] public List<RoomNode> Nodes = new List<RoomNode>();
    }
}
