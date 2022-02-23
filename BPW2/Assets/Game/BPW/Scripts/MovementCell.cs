using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCell : GridCell
{
    public int m_GCost;
    public int m_HCost;
    public int m_FCost;
    public MovementCell m_CameFromCell;
    public WorldObject m_AboveWorldObject;

    public override void CellStart(Grid owner, Vector3 worldPosition, float cellSize,string cellValue,Vector2Int cellPosition)
    {
        base.CellStart(owner,worldPosition,cellSize,cellValue, cellPosition);
        CheckOnWhatWorldObject();
    }

    public void CalculateFCost()
    {
        m_FCost = m_GCost + m_HCost;
    }

    public void CheckOnWhatWorldObject()
    {
        Vector3 worldPosToCheck = m_WorldPosition;
        worldPosToCheck.y += 50;
        worldPosToCheck += (new Vector3(1, 0, 1) * m_CellSize) * 0.5f;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(worldPosToCheck, Vector3.down, out hit, Mathf.Infinity))
        {
           m_AboveWorldObject = hit.transform.gameObject.GetComponent<WorldObject>();
           if (m_AboveWorldObject != null)
           {
               Debug.LogError(m_CellDictionaryPosition.x + " " + m_CellDictionaryPosition.y + "  is on world object" + m_AboveWorldObject.name);
           }
           else
           {
               Debug.LogError(m_CellDictionaryPosition.x + " " + m_CellDictionaryPosition.y + "  is not on any world object");
           }
        }
        else
        {
            Debug.LogError(m_CellDictionaryPosition.x + " " + m_CellDictionaryPosition.y + "  Did not hit anything");
        }
    }
}   
