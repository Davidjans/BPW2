using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InterFaceMouseManager : MonoBehaviour
{
    #region Instancing
    public static InterFaceMouseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InterFaceMouseManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("InterFaceMouseManager").AddComponent<InterFaceMouseManager>();
                }
            }
            return _instance;
        }
    }

    private static InterFaceMouseManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    #endregion
    public Transform m_ChildFollowing;
    public bool m_ShouldMakeChildFollow;
    public BaseGear m_GearHolding;
    void Update()
    {
        if (m_ShouldMakeChildFollow & m_ChildFollowing != null)
        {
            m_ChildFollowing.position = Input.mousePosition;
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseUpCustom();
            }
        }
    }

    public void OnMouseUpCustom()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    
        if (Physics.Raycast(ray, out hit)) {
            Debug.LogError(hit.transform.gameObject);
            InventorySlot slot = hit.transform.GetComponent<InventorySlot>();
            if(slot != null && slot != m_ChildFollowing.GetComponent<InventorySlot>())
                slot.SelectSlot();
        }
        Destroy(m_ChildFollowing.gameObject);
        m_GearHolding = null;
    }

    public void MakeNewFollowingItem(GameObject objectToChild)
    {
        m_ChildFollowing = Instantiate(objectToChild, objectToChild.transform.position,objectToChild.transform.rotation,transform).transform;
        InventorySlot inventorySlot = GetComponentInChildren<InventorySlot>();
        if (inventorySlot != null)
        {
            m_GearHolding = inventorySlot.m_ItemInHere;
        }
        Image[] allImages = m_ChildFollowing.GetComponentsInChildren<Image>();
        foreach (var image in allImages)
        {
            image.raycastTarget = false;
        }
    }
}
