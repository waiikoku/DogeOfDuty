using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Roguelike
{
    public class SlotUpgradeUI : MonoBehaviour
    {
        public TextMeshProUGUI skillName;
        public Image skillIcon;
        public TextMeshProUGUI skillDesc;
        public GameObject[] stars;
        public Button btn;

        private void OnDestroy()
        {
            btn.onClick.RemoveAllListeners();
        }
        public void Assign(string name, Sprite icon, string desc = "")
        {
            skillName.text = name;
            skillIcon.sprite = icon;
            skillDesc.text = desc;
        }

        public void Activate(bool active)
        {
            btn.interactable = active;
            skillIcon.enabled = active;
            ActiveStar(active == false ? 0 : lastStar);
        }
        private int lastStar;
        public void ActiveStar(int amount)
        {
            int count = 0;
            foreach (var item in stars)
            {
                var go = item.transform.GetChild(0);
                go.gameObject.SetActive(count < amount);
                count++;
            }
            //print("Active Star:" + count);
            lastStar = amount;
        }

        public void AddListener(UnityAction callback)
        {
            btn.onClick.AddListener(callback);
        }
    }
}
