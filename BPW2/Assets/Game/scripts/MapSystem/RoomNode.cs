using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Julian.MapSystem
{
    public class RoomNode : MonoBehaviour
    {
        [FoldoutGroup("ReadOnly")] [ReadOnly] public int Index;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public bool Revealed;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public bool Availible;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public bool PlayerAtNode;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public Row Row;
        [FoldoutGroup("ReadOnly")] [ReadOnly] public List<RoomNode> Exits = new List<RoomNode>();
        [FoldoutGroup("ReadOnly")] [ReadOnly] public List<RoomNode> Entrances = new List<RoomNode>();

        [FoldoutGroup("ReadOnly")]
        [ReadOnly]
        [SerializeField]
        public RoomSettings RoomSettings { get; private set; }

        [FoldoutGroup("ReadOnly")]
        [ReadOnly] 
        [SerializeField] 
        private GlobalRoomSettings m_GlobalRoomSettings;


        [FoldoutGroup("Node Settings")]
        [SerializeField] private Image nodeIcon;



        public void SetRoomSettings(RoomSettings roomSettings, GlobalRoomSettings globalRoomSettings)
        {
            RoomSettings = roomSettings;
            m_GlobalRoomSettings = globalRoomSettings;

            nodeIcon.sprite = m_GlobalRoomSettings.RoomIcon;
        }
    }
}