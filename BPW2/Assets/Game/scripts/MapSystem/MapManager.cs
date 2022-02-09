using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Julian.MapSystem
{
    public class MapManager : MonoBehaviour
    {

        [SerializeField] private RingMapSettings m_mapSettings;

        private int m_PlayerPosition;


        [FoldoutGroup("Static Settings")]
        [SerializeField]
        private RingMap m_RingMapPrefab;
        [FoldoutGroup("Static Settings")]
        [SerializeField]
        private ScrollRect m_ScrollRect;
        [FoldoutGroup("Static Settings")]
        [SerializeField]
        private Transform m_ScrollRectViewport;

        private RoomAssets roomAssets;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            roomAssets = new RoomAssets();
            roomAssets.LoadAssets();
        }



        private void Start()
        {
            // testing
            RingMap newMap = Instantiate(m_RingMapPrefab, m_ScrollRectViewport);
            newMap.GenerateMap(m_mapSettings, roomAssets);
            m_ScrollRect.content = (RectTransform)newMap.transform;
        }

    }


    public class RoomAssets
    {
        private Dictionary<RoomType, GlobalRoomSettings> m_GlobalRoomSettings;
        private Dictionary<RoomType, List<RoomSettings>> m_Rooms;

        public void LoadAssets()
        {
            // Crate new Dictionarys
            m_GlobalRoomSettings = new Dictionary<RoomType, GlobalRoomSettings>();
            m_Rooms = new Dictionary<RoomType, List<RoomSettings>>();

            // Load Global Rooms
            Object[] globalRooms = Resources.LoadAll("GlobalRooms", typeof(GlobalRoomSettings));

            for (int i = 0; i < globalRooms.Length; i++)
            {
                var settings = (GlobalRoomSettings)globalRooms[i];
                m_GlobalRoomSettings.Add(settings.RoomType, settings);
            }

            // Load all rooms
            Object[] rooms = Resources.LoadAll("Rooms", typeof(RoomSettings));

            for (int i = 0; i < rooms.Length; i++)
            {
                var room = (RoomSettings)rooms[i];

                // If the key does not exist add a new list
                if (!m_Rooms.ContainsKey(room.RoomType))
                {
                    m_Rooms.Add(room.RoomType, new List<RoomSettings>());
                }

                m_Rooms[room.RoomType].Add(room);
            }

        }

        public RoomSettings GetRandomRoom(RoomType roomType)
        {
            if(!m_Rooms.ContainsKey(roomType))
            {
                Debug.LogError("There is no room to pick from, create a new room!");
            }

            List<RoomSettings> roomSettings = m_Rooms[roomType];
            return roomSettings[Random.Range(0, roomSettings.Count)];
        }

        public List<RoomSettings> GetRoomsFromType(RoomType roomType)
        {
            if (!m_Rooms.ContainsKey(roomType))
            {
                Debug.LogError("There is no room to pick from, create a new room!");
            }

            return m_Rooms[roomType];
        }

        public GlobalRoomSettings GetGlobalRoom(RoomType roomType)
        {
            if (!m_GlobalRoomSettings.ContainsKey(roomType))
            {
                Debug.LogError("There is no global room to pick from, create a new room!");
            }

            return m_GlobalRoomSettings[roomType];
        }

    }


    // !!!Important!!!  NEVER REMOVE ITEM FROM THE LIST, ONLY ADD AT THE BOTTOM OR REPLACE
    public enum RoomType
    {
        FirstRoom,
        CombatEncounter,
        Shop,
        Boss,
        Fountain,
        Event,
    }

    // !!!Important!!!  NEVER REMOVE ITEM FROM THE LIST, ONLY ADD AT THE BOTTOM OR REPLACE
    public enum SideInfoType
    {
        Chest,
        HostileType,
        RoomHazards,
        RoomModifyers,
        Secret
    }

    public enum RingType
    {
        Ring1,
        Ring2,
        Ring3
    }
}