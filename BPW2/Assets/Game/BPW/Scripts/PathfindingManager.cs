using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

public class PathfindingManager : SerializedMonoBehaviour
{
    public int m_MoveStraightCost = 10;
    public int m_MoveDiagonalCost = 14;
    
    public float m_CellSize = 1;
    public Vector2 m_GridSize = new Vector2(5, 5);
    private List<MovementCell> m_OpenList;
    private List<MovementCell> m_ClosedList;
    [SerializeField]
    private Grid m_Grid;

    public Dictionary<(int, int), GridCell> m_DebugDictionary;
    void Start()
    {
        m_Grid = new Grid(this,m_CellSize,m_GridSize,nameof(MovementCell));
        m_DebugDictionary = m_Grid.m_Grid;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = StackedBeansUtils.GetMouseWorldPositionWithRay();
            Vector2Int xYPos =  m_Grid.GetXYBasedOnWorldPos(mouseWorldPosition);
            //Debug.LogError(mouseWorldPosition);
            List<MovementCell> path = FindPath(0, 0, xYPos.x, xYPos.y);
            if (path != null)
            {
                for (int i = 0; i < path.Count -1; i++)
                {
                    Debug.LogError("its doing this");
                    Debug.DrawLine(new Vector3(path[i].m_WorldPosition.x,path[i].m_WorldPosition.y,path[i].m_WorldPosition.z) + (new Vector3(1,0,1) * m_Grid.m_CellSize) * 0.5f ,
                        new Vector3(path[i +1].m_WorldPosition.x,path[i +1].m_WorldPosition.y,path[i +1].m_WorldPosition.z)+ (new Vector3(1,0,1) * m_Grid.m_CellSize) * 0.5f, 
                        Color.green,1000f);
                }
            } 
        }
    }

    private List<MovementCell> FindPathWorldPos(Vector3 startPosition, Vector3 endPosition)
    {
        Vector2 startXY = m_Grid.GetXYBasedOnWorldPos(startPosition);
        Vector2 endXY = m_Grid.GetXYBasedOnWorldPos(endPosition);
        return FindPath((int)startXY.x,(int)startXY.y,(int)endXY.x,(int)endXY.y);
    }
    
    private List<MovementCell> FindPath(int startX, int startY, int endX, int endY)
    {
        MovementCell startCell = m_Grid.m_Grid[(startX, startY)] as MovementCell;
        MovementCell endCell = m_Grid.m_Grid[(endX, endY)] as MovementCell;
        
        if (startCell == null || endCell == null) {
            // Invalid Path
            return null;
        }
        
        m_OpenList = new List<MovementCell>{startCell};
        m_ClosedList = new List<MovementCell>();
        for (int x = 0; x < m_GridSize.x; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                MovementCell movementCell = m_Grid.m_Grid[(x, y)] as MovementCell;
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

            foreach (MovementCell neighbourCell in GetNeighbourList(currentCell))
            {
                if (m_ClosedList.Contains(neighbourCell)) continue;
                int tentativeGCost = currentCell.m_GCost + CalculateDistanceCost(currentCell, neighbourCell);
                if (tentativeGCost < neighbourCell.m_GCost)
                {
                    neighbourCell.m_CameFromCell = currentCell;
                    neighbourCell.m_GCost = tentativeGCost;
                    neighbourCell.m_HCost = CalculateDistanceCost(neighbourCell, endCell);
                    neighbourCell.CalculateFCost();
                    if (!m_OpenList.Contains(neighbourCell))
                    {
                        m_OpenList.Add(neighbourCell);
                    }
                }
            }
        }
        Debug.LogError("invalid path");
        return null;
    }

    private List<MovementCell> CalculatePath(MovementCell endCell)
    {
        List<MovementCell> path = new List<MovementCell>();
        path.Add(endCell);
        MovementCell currentCell = endCell;
        while (currentCell.m_CameFromCell != null)
        {
            path.Add(currentCell.m_CameFromCell);
            currentCell = currentCell.m_CameFromCell;
        }
        path.Reverse();
        return path;
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

    private List<MovementCell> GetNeighbourList(MovementCell currentCell)
    {
        List<MovementCell> neighbourList = new List<MovementCell>();

        if (currentCell.m_CellDictionaryPosition.x - 1 >= 0)
        {
            // left
            neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x -1,currentCell.m_CellDictionaryPosition.y)]);
            
            // left down
            if(currentCell.m_CellDictionaryPosition.y -1 >= 0)
                neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x -1,currentCell.m_CellDictionaryPosition.y -1)]);
            // left up
            if(currentCell.m_CellDictionaryPosition.y + 1 < m_Grid.m_GridSize.y)
                neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x -1,currentCell.m_CellDictionaryPosition.y +1)]);
        }
        if (currentCell.m_CellDictionaryPosition.x + 1 < m_Grid.m_GridSize.x)
        {
            // right
            neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x +1,currentCell.m_CellDictionaryPosition.y)]);
            
            // right down
            if(currentCell.m_CellDictionaryPosition.y -1 >= 0)
                neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x +1,currentCell.m_CellDictionaryPosition.y -1)]);
            // right up
            if(currentCell.m_CellDictionaryPosition.y + 1 < m_Grid.m_GridSize.y)
                neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x +1,currentCell.m_CellDictionaryPosition.y +1)]);
        }
        
        // Down
        if(currentCell.m_CellDictionaryPosition.y -1 >= 0)
            neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x,currentCell.m_CellDictionaryPosition.y -1)]);
        // Up
        if(currentCell.m_CellDictionaryPosition.y + 1 < m_Grid.m_GridSize.y)
            neighbourList.Add((MovementCell)m_Grid.m_Grid[(currentCell.m_CellDictionaryPosition.x,currentCell.m_CellDictionaryPosition.y +1)]);

        return neighbourList;
    }
}
