using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetParent{
    [SerializeField]
    private GameObject m_parent;
    public GameObject GetMyParent { get { return m_parent; } }
}
