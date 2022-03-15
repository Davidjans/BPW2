using System;
using System.Collections;
using System.Collections.Generic;
using EVN.MapSystem;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RoomGeneration m_RoomGenerator;
    public RoomSettings m_RoomSettings;
    private void Start()
    {
        m_RoomGenerator = GetComponent<RoomGeneration>();
    }
}
