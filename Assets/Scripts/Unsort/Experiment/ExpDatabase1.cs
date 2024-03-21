using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class ExpDatabase1 : MonoBehaviour
    {
        [SerializeField] private SkillCard[] allSkills;
        [SerializeField] private SkillCard[] activeSkills;
        [SerializeField] private SkillCard[] passiveSkills;

        //IDs
        public int[] availableActive;
        public int[] availablePassive;

        public SkillCard[] GetActives() { return activeSkills; }
        public SkillCard[] GetPassives() { return passiveSkills; }

        public void Rearrange()
        {
            List<SkillCard> activeList = new List<SkillCard>();
            List<SkillCard> passiveList = new List<SkillCard>();

            foreach (var skill in allSkills)
            {
                switch (skill.SkillType)
                {
                    case SkillType.Passive:
                        passiveList.Add(skill);
                        break;
                    case SkillType.Active:
                        activeList.Add(skill);
                        break;
                    default:
                        break;
                }
            }

            activeSkills = activeList.ToArray();
            passiveSkills = passiveList.ToArray();
        }

        public void FilterAvailable(int[] ids)
        {
            HashSet<int> idSet = new HashSet<int>(ids);
            List<int> activeList = new List<int>();
            List<int> passiveList = new List<int>();
            int cacheId;
            foreach (var item in activeSkills)
            {
                cacheId = item.Identity.ID;
                if (idSet.Contains(cacheId))
                {
                    activeList.Add(cacheId);
                }
            }
            foreach (var item in passiveSkills)
            {
                cacheId = item.Identity.ID;
                if (idSet.Contains(cacheId))
                {
                    passiveList.Add(cacheId);
                }
            }
            availableActive = activeList.ToArray();
            availablePassive = passiveList.ToArray();
        }

        public void RandomAccessSkill()
        {
            List<int> activeList = new List<int>();
            List<int> passiveList = new List<int>();
            int value;
            foreach (var skill in activeSkills)
            {
                value = Random.Range(0, 100);
                if(value > 50)
                {
                    activeList.Add(skill.Identity.ID);
                }
            }
            foreach (var skill in passiveSkills)
            {
                value = Random.Range(0, 100);
                if (value > 50)
                {
                    passiveList.Add(skill.Identity.ID);
                }
            }
            availableActive = activeList.ToArray();
            availablePassive = passiveList.ToArray();
        }
    }
}
