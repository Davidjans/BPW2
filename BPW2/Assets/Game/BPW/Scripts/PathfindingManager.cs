using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public int m_MoveStraightCost = 10;
    public int m_MoveDiagonalCost = 14;
    
    public float m_CellSize = 1;
    public Vector2 m_GridSize = new Vector2(5, 5);
    private List<MovementCell> m_OpenList;
    private List<MovementCell> m_ClosedList;
    private Grid m_Grid;
    void Start()
    {
        m_Grid = new Grid(this,m_CellSize,m_GridSize);
    }

    private List<MovementCell> FindPathWorldPos(Vector3 startPosition, Vector3 endPosition)
    {
        Vector2 startXY = m_Grid.GetXYBasedOnWorldPos(startPosition);
        Vector2 endXY = m_Grid.GetXYBasedOnWorldPos(endPosition);
        return FindPath((int)startXY.x,(int)startXY.y,(int)endXY.x,(int)endXY.y);
    }
    
    private List<MovementCell> FindPath(int startX, int startY, int endX, int endY)
    {
        MovementCell startCell = (MovementCell)m_Grid.m_Grid[(startX, startY)];
        MovementCell endCell = (MovementCell)m_Grid.m_Grid[(endX, endY)];
        m_OpenList = new List<MovementCell>{startCell};
        m_ClosedList = new List<MovementCell>();
        for (int x = 0; x < m_GridSize.x; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                MovementCell movementCell = (MovementCell)m_Grid.m_Grid[(x, y)];
                movementCell.m_GCost = int.MaxValue;
                movementCell.CalculateFCost();
                movementCell.m_CameFromCell = null;
            } 
        }

        startCell.m_GCost = 0;
        startCell.m_HCost = CalculateDistanceCost(startCell,endCell);
        startCell.CalculateFCost();
        while (m_OpenList.Count > 0)
        {
            MovementCell currentCell = GetLowestFCostCell(m_OpenList);
            if (currentCell == endCell)
            {
                return CalculatePath(endCell);
            }

            m_OpenList.Remove(currentCell);
            m_ClosedList.Add(currentCell);
        }
        return null;
    }

    private List<MovementCell> CalculatePath(MovementCell endCell)
    {
        return null;
    }
    
    private int CalculateDistanceCost(MovementCell start, MovementCell end)
    {
        int xDistance = Mathf.Abs((int)start.m_CellDictionaryPosition.x - (int)end.m_CellDictionaryPosition.x);
        int yDistance = Mathf.Abs((int)start.m_CellDictionaryPosition.y - (int)end.m_CellDictionaryPosition.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return m_MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + m_MoveStraightCost * remaining;
    }

    private MovementCell GetLowestFCostCell(List<MovementCell> cellList)
    {
        MovementCell lowestFCostCell = cellList[0];
        for (int i = 0; i < cellList.Count; i++)
        {
            if (cellList[i].m_FCost < lowestFCostCell.m_FCost)
            {
                lowestFCostCell = cellList[i];
            }
        }

        return lowestFCostCell;
    }
}
