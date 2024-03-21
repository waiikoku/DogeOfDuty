using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private WaypointData card;
        [SerializeField] private Transform m_transform;
        private void Awake()
        {
            WaypointData.Points.Add(card.pointID, m_transform);
        }

        private void OnDestroy()
        {
            WaypointData.Points.Remove(card.pointID);
        }
    }
}
