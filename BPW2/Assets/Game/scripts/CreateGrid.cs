using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class CreateGrid : MonoBehaviour
{
    public float m_CellSize = 1;
    public Vector2 m_GridSize = new Vector2(5, 5);
    public Dictionary<(int x, int y), GridCell> m_Grid = new Dictionary<(int x, int y), GridCell>();
    
    [Button]
    private void CreateGridInDictionary()
    {
        m_Grid.Clear();
        for (int x = 0; x < m_GridSize.x; x++)
        {
            for (int y = 0; y < m_GridSize.y; y++)
            {
                m_Grid[(x, y)] = new GridCell();
            }
        }
    }
    [Button]
    private void ClearGrid()
    {
        m_Grid.Clear();
    }
}
