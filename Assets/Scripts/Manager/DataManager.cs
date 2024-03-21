using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class DataManager : Singleton<DataManager>
    {
        [SerializeField] private ExpDatabase1 dataBase;

        public SkillCard[] GetActiveSkills()
        {
            HashSet<int> idSet = new HashSet<int>(dataBase.availableActive);
            List<SkillCard> activeList = new List<SkillCard>();
            var data = dataBase.GetActives();
            foreach (var skill in data)
            {
                if (idSet.Contains(skill.Identity.ID))
                {
                    activeList.Add(skill);
                }
            }
            return activeList.ToArray();
        }

        public SkillCard[] GetPassiveSkills()
        {
            HashSet<int> idSet = new HashSet<int>(dataBase.availablePassive);
            List<SkillCard> activeList = new List<SkillCard>();
            var data = dataBase.GetPassives();
            foreach (var skill in data)
            {
                if (idSet.Contains(skill.Identity.ID))
                {
                    activeList.Add(skill);
                }
            }
            return activeList.ToArray();
        }

    }
}
