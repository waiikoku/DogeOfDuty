using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Dod
{
    public class PreskillManage : MonoBehaviour
    {
        [SerializeField] private BaseSkill[] preSkills;
        [SerializeField] private SkillManage manage;

        private async void Awake()
        {
            while (manage.isInitialized == false)
            {
                await Task.Yield();
            }
            foreach (var skill in preSkills)
            {
                manage.DirectAdd(skill);
            }
            print("PreAdd");
        }
    }
}
