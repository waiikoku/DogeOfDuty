using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dod
{
    public class SkillUI : MonoBehaviour
    {
        public TextMeshProUGUI label;
        public Image icon;
        public GameObject[] stars;

        public bool isButton = false;
        [SerializeField] private Button btn;

        public void Assign(string name,Sprite sprite,int star)
        {
            label.text = name;
            icon.sprite = sprite;
            UpdateStar(star);
        }

        public void UpdateStar(int star)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                ActivateStar(i, i < star);
            }
        }
        private void ActivateStar(int index, bool active)
        {
            var child = stars[index].transform.GetChild(0).gameObject;
            child.SetActive(active);
        }

        public void AddListener(UnityEngine.Events.UnityAction callback)
        {
            if (isButton == false || callback == null) return;
            btn.onClick.AddListener(callback);
        }
    }
}
