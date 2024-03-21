using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dod
{
    public class SkillSlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject star;
        [SerializeField] private TextMeshProUGUI lvText;

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void SetLevel(int level)
        {
            lvText.SetText(level.ToString());
        }

        public void Activate(bool active)
        {
            icon.enabled = active;
            star.SetActive(active);
            lvText.enabled = active;
        }
    }
}
