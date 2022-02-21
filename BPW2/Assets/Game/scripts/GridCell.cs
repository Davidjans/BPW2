using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class GridCell
{
    public Vector3 m_WorldPosition;
    public float m_CellSize;
    public string m_CellValue = "0";
    public Vector2 m_CellDictionaryPosition;
    public TextMeshPro m_CellText;
    public Grid m_Owner;
    public void CellStart(Grid owner, Vector3 worldPosition, float cellSize,string cellValue,Vector2 cellPosition)
    {
        m_WorldPosition = worldPosition;
        m_CellSize = cellSize;
        m_CellValue = cellValue;
        m_CellDictionaryPosition = cellPosition;
        m_Owner = owner;
        CellCreation();
    }

    private async void CellCreation()
    {
        m_CellText = await StackedBeansUtils.CreateWorldTextMeshPro(null, m_CellValue,m_WorldPosition + new Vector3(m_CellSize,0,m_CellSize) * 0.5f,Color.white, 8);
        DrawSquare();
    }
    
    
    
    public void DrawSquare()
    {
        Debug.DrawLine(m_WorldPosition,new Vector3(m_WorldPosition.x,m_WorldPosition.y ,m_WorldPosition.z+m_CellSize)
            ,Color.white, 100f);
        Debug.DrawLine(m_WorldPosition,new Vector3(m_WorldPosition.x+m_CellSize,m_WorldPosition.y,m_WorldPosition.z)
            ,Color.white, 100f);
    }

    public void OnDestroy()
    {
        GameObject.DestroyImmediate(m_CellText.gameObject);
    }
}
