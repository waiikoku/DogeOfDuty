using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class PlayerSkillUI : Singleton<PlayerSkillUI>
    {
        [SerializeField] private SkillSlotUI prefab;
        [SerializeField] private Transform containerActive;
        [SerializeField] private Transform containerPassive;
        private const int maxActive = 5;
        private const int maxPassive = 5;
        [SerializeField] private bool preSlot = false;
        [SerializeField] private SkillSlotUI[] activeSkills;
        [SerializeField] private SkillSlotUI[] passiveSkills;
        private Dictionary<int, SkillSlotUI> skills;
        private int currentActiveCount;
        private int currentPassiveCount;
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void OnDestroy()
        {
            SelfDestruct();
        }

        private void Start()
        {
            foreach (var skill in activeSkills)
            {
                skill.Activate(false);
            }
            foreach (var skill in passiveSkills)
            {
                skill.Activate(false);
            }
        }

        private void Init()
        {
            skills = new Dictionary<int, SkillSlotUI>();
            if (preSlot == false)
            {
                SpawnActive();
                SpawnPassive();
            }
        }

        private void SpawnActive()
        {
            activeSkills = new SkillSlotUI[maxActive];
            for (int i = 0; i < maxActive; i++)
            {
                activeSkills[i] = Instantiate(prefab, containerActive);
            }
        }

        private void SpawnPassive()
        {
            passiveSkills = new SkillSlotUI[maxPassive];
            for (int i = 0; i < maxPassive; i++)
            {
                passiveSkills[i] = Instantiate(prefab, containerPassive);
            }
        }

        public void AddSkill(BaseSkill skill)
        {
            skill.OnUpgrade += UpdateLevelSkill;
            switch (skill.Skill.SkillType)
            {
                case SkillType.Passive:
                    AddPassive(skill);
                    break;
                case SkillType.Active:
                    AddActive(skill);
                    break;
                default:
                    break;
            }
        }

        private void AddActive(BaseSkill skill)
        {
            activeSkills[currentActiveCount].SetIcon(skill.Skill.SkillIcon);
            activeSkills[currentActiveCount].SetLevel(skill.Level);
            activeSkills[currentActiveCount].Activate(true);
            skills.Add(skill.Skill.Identity.ID, activeSkills[currentActiveCount]);
            currentActiveCount++;
        }

        private void AddPassive(BaseSkill skill)
        {
            passiveSkills[currentPassiveCount].SetIcon(skill.Skill.SkillIcon);
            passiveSkills[currentPassiveCount].SetLevel(skill.Level);
            passiveSkills[currentPassiveCount].Activate(true);
            skills.Add(skill.Skill.Identity.ID, passiveSkills[currentPassiveCount]);
            currentPassiveCount++;
        }

        private void UpdateLevelSkill(int id,int level)
        {
            skills[id].SetLevel(level);
        }
    }
}
