using System.Collections.Generic;
using System.Linq;
using StackedBeans.Utils;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    private RoomManager m_RoomManager;
    public List<SpawnNode> m_SpawnNodesInRoom = new List<SpawnNode>();
    public List<Collider> m_AllFloors;
    public Vector3 m_LeftBottomRoomPoint;
    public Vector3 m_RightTopROomPoint;
    private void Start()
    {
        m_RoomManager = GetComponent<RoomManager>();
        m_AllFloors = GetComponentsInChildren<Collider>().ToList();
        m_LeftBottomRoomPoint = m_AllFloors[0].GetBoundsWorldPosition(DirectionLeftToRight.Left,DirectionBackToFront.Front);
        m_RightTopROomPoint = m_AllFloors[m_AllFloors.Count - 1].GetBoundsWorldPosition(DirectionLeftToRight.Right, DirectionBackToFront.Back);

        Vector3 playerSpawnPoint = Vector3.zero;
        playerSpawnPoint.z = m_LeftBottomRoomPoint.z + 0.5f;
        float xDiff = m_RightTopROomPoint.x - m_LeftBottomRoomPoint.x;
        playerSpawnPoint.x = Random.Range(xDiff * 0.3f, xDiff * 0.7f) + 0.5f;
        playerSpawnPoint.y = m_LeftBottomRoomPoint.y;

        GameObject testPrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        playerSpawnPoint.y += testPrimitive.GetComponent<Collider>().bounds.extents.y;
        testPrimitive.transform.position = playerSpawnPoint;
    }
}
