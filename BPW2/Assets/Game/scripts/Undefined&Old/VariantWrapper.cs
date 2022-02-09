using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class VariantWrapper
{
	[HorizontalGroup("Group 1")]
	public List<GameObject> m_VariantObjects;
	[HorizontalGroup("Group 1")]
	public List<Vector2> m_SpawnBetweenPercentages;
	public void SetEqualSize()
	{
		if(m_VariantObjects.Count > m_SpawnBetweenPercentages.Count)
		{
			m_SpawnBetweenPercentages.Add(Vector2.zero);
		}
		if (m_VariantObjects.Count < m_SpawnBetweenPercentages.Count)
		{
			m_SpawnBetweenPercentages.RemoveAt(m_SpawnBetweenPercentages.Count -1);
		}
	}

	public GameObject GetObject()
	{
		float variationRand = UnityEngine.Random.Range(0, 100);
		GameObject objectToSpawn = null;
		for (int i = 0; i < m_VariantObjects.Count; i++)
		{
			if (variationRand >= m_SpawnBetweenPercentages[i].x && variationRand <= m_SpawnBetweenPercentages[i].y)
			{
				objectToSpawn = m_VariantObjects[i];
				return objectToSpawn;
			}
		}
		return objectToSpawn;
	}
}
