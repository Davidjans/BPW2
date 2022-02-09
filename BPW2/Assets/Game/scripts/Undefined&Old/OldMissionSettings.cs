using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionSettings", menuName = "Wolfdog/DavidTools/Missions/MissionSettings", order = 1)]
[ExecuteInEditMode]
public class OldMissionSettings : ScriptableObject
{
	#region Camera Settings
	[TabGroup("Camera Settings")]
	public bool m_SpawnCameras;

	[TabGroup("Camera Settings")]
	[ShowIf("m_SpawnCameras")]
	public float m_CameraSpawnChance;

	//[TabGroup("Camera Settings")]
	//[ShowIf("m_SpawnCameras")]
	//public VariantWrapper m_CameraVariations;

	#endregion Camera Settings

	#region Guard Settings

	[TabGroup("Guard Settings")]
	public bool m_SpawnGuards;
	

	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "MainObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardSpawnChanceMainObjective = 40;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "MainObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public int m_MaxGuardAmmountMainObjective = 900;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "MainObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public int m_MinimalGuardAmmountMainObjective = 5;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "MainObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public VariantWrapper m_GuardVariationsMainObjective;

	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "SideObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardSpawnChanceSideObjective = 40;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "SideObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public int m_MaxGuardAmmountSideObjective = 900;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "SideObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public int m_MinimalGuardAmmountSideObjective = 5;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "SideObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public VariantWrapper m_GuardVariationsSideObjective;

	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "NoObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardSpawnChanceNoObjective = 40;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "NoObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public int m_MaxGuardAmmountNoObjective = 900;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "NoObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public int m_MinimalGuardAmmountNoObjective = 5;
	[TabGroup("_DefaultTabGroup/Guard Settings/GuardSub", "NoObjectiveRoom")]
	[ShowIf("m_SpawnGuards")]
	public VariantWrapper m_GuardVariationsNoObjective;
	[TabGroup("Guard Settings")]
	[ShowIf("m_SpawnGuards")]
	public int m_MinimalGuardAmmountAll = 5;

	[TabGroup("Guard Settings")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardDamageMultiplier = 1;

	[TabGroup("Guard Settings")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardFireRateMultiplier = 1;

	[TabGroup("Guard Settings")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardSpotSpeedMultiplier = 1;

	[TabGroup("Guard Settings")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardViewAngle = 90;

	[TabGroup("Guard Settings")]
	[ShowIf("m_SpawnGuards")]
	public float m_GuardViewDistance = 1000;

	[TabGroup("Guard Settings")]
	[ShowIf("m_SpawnGuards")]
	public VariantWrapper m_GuardVariations;

	#endregion Guard Settings

	private void OnValidate()
	{
		m_GuardVariations.SetEqualSize();
	}
}