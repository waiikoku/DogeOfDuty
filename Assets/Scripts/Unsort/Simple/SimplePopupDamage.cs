using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimplePopupDamage : MonoBehaviour
    {
        [SerializeField] private TextMesh popUp;
        [SerializeField] private Transform point;

        [SerializeField] private SimpleAI agent;
        private Camera cam;

        private void Awake()
        {
            agent.OnDamage += Popup;
            cam = Camera.main;
        }

        private void Popup(float dmg)
        {
            Vector3 dir = point.position - cam.transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
            TextMesh textMesh = Instantiate(popUp, point.position, rot);
            textMesh.text = dmg.ToString();
            Destroy(textMesh.gameObject, 1.5f);
        }
    }
}
