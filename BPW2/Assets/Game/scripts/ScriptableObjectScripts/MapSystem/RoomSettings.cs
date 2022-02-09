using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Julian.MapSystem
{
    [CreateAssetMenu(fileName = "New Room Settings", menuName = "Room Settings", order = 1)]
    public class RoomSettings : ScriptableObjectFileEdit
    {
        [Required]
        [FoldoutGroup("General")]
        public RoomType RoomType;

        [Required]
        [FoldoutGroup("General")]
        public string BaseSceneName;

        [FoldoutGroup("Room")]
        [LabelText("")]
        public EnumToggles SideInfoToggles;


        private void OnValidate()
        {
            SideInfoToggles.ValidateEnumToggle(typeof(SideInfoType), "Room Side Info");

        }
    }
}