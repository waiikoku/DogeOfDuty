using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Dod
{
    public class SkillManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private SkillCard[] activeSkills;
        [SerializeField] private SkillCard[] passiveSkills;
        private const int maxSlot = 6;
        private int indexActive;
        private int indexPassive;


        [Header("UI")]
        public SkillUI[] randomSkills;
        [SerializeField] private Image[] activeIcons;
        [SerializeField] private Image[] passiveIcons;
        private void Awake()
        {
            activeSkills = new SkillCard[maxSlot];
            passiveSkills = new SkillCard[maxSlot];
            indexActive = 0;
            indexPassive = 0;
            foreach (var item in randomSkills)
            {
                item.isButton = true;
            }
        }

        private async void Start()
        {
            await Task.Delay(3000);
            activeSkills = DataManager.Instance.GetActiveSkills();
            passiveSkills = DataManager.Instance.GetPassiveSkills();
        }

        public void AddActive(SkillCard card)
        {
            activeSkills[indexActive] = card;
            indexPassive++;
        }

        public void AddPassive(SkillCard card)
        {
            passiveSkills[indexPassive] = card;
            indexPassive++;
        }
        private const int MAXRANDOM = 3;
        public void RandomSkill(SkillCard[] card, int[] stars)
        {
            for (int i = 0; i < MAXRANDOM; i++)
            {
                randomSkills[i].Assign(card[i].Identity.Name, card[i].SkillIcon, stars[i]);
            }
        }

    }
}
