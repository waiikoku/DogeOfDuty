using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Dod
{
    public class SkillManage : MonoBehaviour
    {
        private Dictionary<int, BaseSkill> skills;
        private Dictionary<int, int> skillIndexes;
        private Dictionary<int, List<Coroutine>> skillCoroutines;
        private Dictionary<int, Coroutine> skillCDCoroutines;
        private const int skillCap = 5;
        private const int spamCap = 4;

        private void Awake()
        {
            skills = new Dictionary<int, BaseSkill>();
            skillIndexes = new Dictionary<int, int>();
            skillCoroutines = new Dictionary<int, List<Coroutine>>(skillCap);
            skillCDCoroutines = new Dictionary<int, Coroutine>();
            for (int i = 0; i < skillCap; i++)
            {
                skillCoroutines.Add(i, new List<Coroutine>(spamCap));
                skillCDCoroutines.Add(i, null);
            }
        }

        private void DirectRun(BaseSkill skill,int index)
        {
            print($"Try to directRun {skill.gameObject.name} {index}");
            if (skill.isCooldown)
            {
                print("Rejct run (CD)");
                return;
            }
            if(index > skillCDCoroutines.Count)
            {
                print("Exceed Coroutine");
                return;
            }
            if (skill.IsMultiple)
            {
                if (skillCoroutines[index].Count == spamCap) return;
                skillCoroutines[index].TrimExcess();
                skillCoroutines[index].Add(StartCoroutine(skill.Active()));
            }
            else
            {
                if (skillCoroutines[index].Count > 0)
                {
                    if (skillCoroutines[index][0] != null)
                    {
                        StopCoroutine(skillCoroutines[index][0]);
                    }
                    skillCoroutines[index][0] = StartCoroutine(skill.Active());
                }
                else
                {
                    skillCoroutines[index].Add(StartCoroutine(skill.Active()));
                }

            }
            if (skill.Stat.cooldown == 0) return;
            skillCDCoroutines[index] = StartCoroutine(skill.Cooldown());
        }

        protected BaseSkill.Param passthrough;
        public bool isInitialized = false;
        public void InitializeSkill(BaseSkill.Param param)
        {
            passthrough = param;
            print("Init skill");
            isInitialized = true;
        }

        public void InstallSkill(GameObject prefab)
        {
            var skill = prefab.GetComponent<BaseSkill>();
            skill.Setup(passthrough);
            skill.transform.SetParent(transform, false);
            if(skills == null)
            {
                print("Null " + prefab.name);
            }
            else
            {
                var card = skill.Skill;
                if(card == null)
                {
                    print($"{prefab.name}'s Card is null");
                }
                else
                {
                    AddAndRun(card.Identity.ID, skill);
                }
            }
   
        }

        private async void AddAndRun(int id,BaseSkill skill)
        {
            int count = skills.Count;
            //print($"Try to Add {skill.name} and Starting.. ({count})");
            await AddToList(id, skill);
            print($"Update ui directly {skill.Skill.Identity.Name}");
            PlayerSkillUI.Instance.AddSkill(skill);
            //UIManager.Instance.AccessUpgrade().DirectUpdateSlot(count, skill.Skill.SkillIcon, skill.Level);
            //print($"{skill.gameObject.name}'s add to list");
            print($"ADDRUN ID({id}) I:{count}");
            DirectRun(skill, count);
        }

        private async Task AddToList(int id, BaseSkill skill)
        {
            skills.Add(id, skill);
            skillIndexes.Add(id, skillIndexes.Count);
            await Task.CompletedTask;
        }

        public void DirectAdd(BaseSkill skill)
        {
            int id = skill.Skill.Identity.ID;
            skill.Setup(passthrough);
            skills.Add(id, skill);
            PlayerSkillUI.Instance.AddSkill(skill);
            //skillIndexes.Add(id, skillIndexes.Count);
        }

        public BaseSkill AccessSkill(int id)
        {
            skills.TryGetValue(id, out BaseSkill tempo);
            return tempo;
        }

        public bool HasSkill(int id)
        {
            return skills.ContainsKey(id);
        }

        public int GetSkillIndex(int id)
        {
            return skillIndexes[id];
        }

        public int GetSlotCount()
        {
            return skills.Count;
        }
    }
}
