using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Roguelike
{
    public class UpgradePopupUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Image expBar;
        [SerializeField] private TextMeshProUGUI lv;
        [SerializeField] private SlotUpgradeUI[] slots;

        /*
        private void Awake()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].AddListener(() =>
                {
                    SetActive(false);
                });
            }
        }
        */

        private void Start()
        {
            
        }

        public void SetActive(bool active)
        {
            panel.SetActive(active);
        }

        public void Assign(float progress, int lv,Skill[] skills)
        {
            expBar.fillAmount = progress;
            this.lv.text = lv.ToString();
            for (int i = 0; i < slots.Length; i++)
            {
                var sk = skills[i];
                slots[i].Assign(sk.skillName, sk.skillIcon);
            }

        }


    }
}
