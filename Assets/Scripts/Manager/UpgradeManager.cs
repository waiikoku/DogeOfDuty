using Roguelike;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private SkillManage skillManage;

        //Static
        public readonly int MAXRANDOM = 3;
        public readonly int MAXLEVEL = 5;

        //Dynamic
        /*
        [SerializeField] private SkillSlotUI[] activeSkillSlots;
        [SerializeField] private SkillSlotUI[] passiveSkillSlots;
        */

        //Datas
        private Dictionary<int, SkillCard> cardDB;
        private List<int> availableIDs;
        private int[] skillIDs_RandomSlot;
        private BaseSkill[] randomSkills;

        private const int MAXINSTALL = 5;
        private int installCount;
        private int maxCount;

        private void Awake()
        {
            ImportID();
            randomSkills = new BaseSkill[MAXRANDOM];
            skillIDs_RandomSlot = new int[MAXRANDOM];
            /*
            for (int i = 0; i < activeSkillSlots.Length; i++)
            {
                activeSkillSlots[i].Activate(false);
            }
            */
            SurvivalManager.Instance.OnLevelUp += ProceedUpgrade;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UIManager.Instance.ActiveUpgrade(true);
                ProceedUpgrade(0);
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                ProceedUpgrade(0);
            }

        }

        #region External
        public void LinkSkill(SkillManage manager)
        {
            skillManage = manager;
        }

        /*
        public void DirectUpdateSlot(int index, Sprite sprite, int lv)
        {
            if(index > activeSkillSlots.Length - 1)
            {
                Debug.LogWarning("Index Exceed ActiveSlot");
                return;
            }
            activeSkillSlots[index].SetIcon(sprite);
            activeSkillSlots[index].SetLevel(lv);
            activeSkillSlots[index].Activate(true);
        }
        */
        #endregion
        #region Initialize
        private void ImportID()
        {
            var cards = DataManager.Instance.GetActiveSkills();
            int length = cards.Length;
            availableIDs = new List<int>(length);
            cardDB = new Dictionary<int, SkillCard>(length);
            foreach (var item in cards)
            {
                int id = item.Identity.ID;
                cardDB.Add(id, item);
                availableIDs.Add(id);
            }
            var passiveCards = DataManager.Instance.GetPassiveSkills();
            foreach (var item in passiveCards)
            {
                int id = item.Identity.ID;
                cardDB.Add(id, item);
                availableIDs.Add(id);
            }
        }
        #endregion

        #region Data
        private void RemoveMaximumLevel()
        {
            if (skillManage == null) return;
            List<int> removeId = new List<int>();
            foreach (var id in availableIDs)
            {
                var skill = skillManage.AccessSkill(id);
                if (skill == null) continue;
                if (skill.Level < MAXLEVEL) continue;
                //Exceed Level So Remove From List
                maxCount++;
                Debug.LogWarning($"Skill Maxed ({maxCount})");
                removeId.Add(id);
            }
            for (int i = 0; i < removeId.Count; i++)
            {
                availableIDs.Remove(removeId[i]);
            }
        }
        #endregion

        #region Upgrade
        private int upgradePoint;
        private bool isUpgrading = false;
        private bool firstTimeRject = false;
        private void ProceedUpgrade(uint value)
        {
            if(firstTimeRject == false)
            {
                firstTimeRject = true;
                installCount++;
                return;
            }
            upgradePoint++; //Stack Points
            if (isUpgrading) return; //Reject While still choosing
            if (installCount >= MAXINSTALL && maxCount >= MAXLEVEL - 1)
            {
                print("Rejct Install&Max");
                return;
            }
            RandomUpgrades();
            UIManager.Instance.ActiveUpgrade(true);
        }

        public Action<int, bool> OnActiveSlot;
        public Action<int, int> OnLevelSlot;
        public Action<int, SkillCard> OnUpdateSlot; 
        private void RandomUpgrades()
        {
            isUpgrading = true;
            RemoveMaximumLevel();
            List<int> temporalID = new List<int>(availableIDs);
            for (int i = 0; i < MAXRANDOM; i++)
            {
                if (temporalID.Count == 0)
                {
                    //upgradeSlots[i].Activate(false);
                    OnActiveSlot(i, false);
                }
                else
                {
                    int index = UnityEngine.Random.Range(0, temporalID.Count);
                    int id = temporalID[index];
                    print($"I:{index} ID:{id}");
                    skillIDs_RandomSlot[i] = id;
                    temporalID.Remove(id);
                    if (skillManage == null)
                    {
                        continue;
                    }
                    var skill = skillManage.AccessSkill(id);
                    randomSkills[i] = skill;
                    if (skill != null)
                    {
                        //upgradeSlots[i].Assign(skill.Skill.Identity.Name, skill.Skill.SkillIcon);
                        OnUpdateSlot(i, skill.Skill);
                    }
                    else
                    {
                        var dat = cardDB[id];
                        //upgradeSlots[i].Assign(dat.Identity.Name,dat.SkillIcon);
                        OnUpdateSlot(i, dat);
                    }
                    //upgradeSlots[i].ActiveStar(skill == null ? 0 : skill.Level);
                    int level = skill == null ? 0 : skill.Level;
                    OnLevelSlot(i, level);
                }
            }
            print("Current Count:" + temporalID.Count);
            print("==========================================================================================");
        }

        public void SelectUpgrade(int index)
        {
            if (randomSkills[index] != null)
            {
                randomSkills[index].Upgrade();
                //int id = randomSkills[index].Skill.Identity.ID;
                //int slotIndex = skillManage.GetSkillIndex(id);
                //DirectUpdateSlot(slotIndex, randomSkills[index].Skill.SkillIcon, randomSkills[index].Level);
            }
            else
            {
                int id = skillIDs_RandomSlot[index];
                if (skillManage.HasSkill(id))
                {
                    var skill = skillManage.AccessSkill(id);
                    skill.Upgrade();
                }
                else
                {
                    var skill = PersistentData.Instance.SpawnSkill(id);
                    skill.Upgrade();
                    skillManage.InstallSkill(skill.gameObject);
                    installCount++;
                }
            }
            isUpgrading = false;
            upgradePoint--;
            if(upgradePoint <= 0)
            {
                UIManager.Instance.ActiveUpgrade(false);
            }
            else
            {
                print($"IC:{installCount} MC:{maxCount}");
                if (installCount >= MAXINSTALL && maxCount >= MAXLEVEL - 1)
                {
                    print("Rejct Install&Max");
                    UIManager.Instance.ActiveUpgrade(false);
                    return;
                }
                RandomUpgrades();
            }
        }
        #endregion
    }
}
