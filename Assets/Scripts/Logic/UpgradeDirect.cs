using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dod
{
    public class UpgradeDirect : MonoBehaviour
    {
        [Header("Database")]
        private SkillCard[] cards;
        private SkillCard[] availableCards;

        [Header("Player")]
        private Dictionary<int,BaseSkill> data;

        [Header("Agent")]
        private Action OnLevelUp;
        private int upgradeStack;
        private List<SkillCard> currentAvailable;
        private SkillCard[] currentRandom;

        [Header("UI")]
        private Image[] activeSkill_UI;
        private Image[] randomSkill_UI;
        private Button[] upgradeBtn;

        private void Awake()
        {
            OnLevelUp += DoUpgrade;

            for (int i = 0; i < 3; i++)
            {
                int index = i;
                upgradeBtn[i].onClick.AddListener(() =>
                {
                    UpgradeAction(index);
                });
            }

            currentAvailable = new List<SkillCard>(availableCards);
            currentRandom = new SkillCard[3];
        }

        #region Player
        private void InitSkill()
        {
            data.Add(0,null);
            SetActiveSkill(data.Count - 1, data[0].Skill.SkillIcon);
        }

        private void InstallSkill(BaseSkill skill)
        {
            data.Add(skill.Skill.Identity.ID, skill);
        }
        #endregion

        #region Agent
        private void DoUpgrade()
        {
            upgradeStack++;
            RandomUpgrade();
        }

        //Remove Max Level from Skills
        private void CleanList(ref List<SkillCard> list)
        {
            foreach (var item in list)
            {
                int id = item.Identity.ID;
                data.TryGetValue(id,out var dat);
                if(dat != null)
                {
                    if(dat.Level >= 5)
                    {
                        list.Remove(item);
                    }
                }
            }
        }

        private void RandomUpgrade()
        {
            //Access Datas & Random Implement
            CleanList(ref currentAvailable);
            List<SkillCard> randomList = new List<SkillCard>(currentAvailable);

            for (int i = 0; i < 3;)
            {
                int number = UnityEngine.Random.Range(0, randomList.Count);
                SetRandom(i, randomList[number].SkillIcon);
                currentRandom[i] = randomList[number];
                randomList.Remove(randomList[number]);
                i++;
            }
        }
        #endregion

        #region UI
        private void SetRandom(int index,Sprite sprite)
        {
            randomSkill_UI[index].sprite = sprite;
        }

        private void SetActiveSkill(int index,Sprite sprite)
        {
            activeSkill_UI[index].sprite = sprite;
        }

        private void SetPassiveSkill(int index,Sprite sprite)
        {

        }

        private void UpgradeAction(int index)
        {
            int id = currentRandom[index].Identity.ID;
            data.TryGetValue(id, out var skill);
            if(skill == null)
            {
                var newSkill = PersistentData.Instance.SpawnSkill(id);
                newSkill.Upgrade();
                InstallSkill(newSkill);
                SetActiveSkill(data.Count - 1, newSkill.Skill.SkillIcon);
            }
            else
            {
                skill.Upgrade();
            }
        }
        #endregion
    }
}
