using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCell : GridCell
{
    public int m_GCost;
    public int m_HCost;
    public int m_FCost;

    public MovementCell m_CameFromCell;
    
    
    public void MovementCellStart()
    {
        
    }

    public void CalculateFCost()
    {
        m_FCost = m_GCost + m_HCost;
    }
}
