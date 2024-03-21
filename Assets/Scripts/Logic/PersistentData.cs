using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class PersistentData : Singleton<PersistentData>
    {
        public Dictionary<int, GameObject> data;
        private Dictionary<int, SkillCard> cards;

        [System.Serializable]
        public struct CustomSkill
        {
            public SkillCard card;
            public GameObject prefab;
        }

        [SerializeField] private CustomSkill[] skills;

        protected override void Awake()
        {
            base.Awake();
            Method2();
        }

        /*
        [Header("Data 1")]
        [SerializeField] private int[] ids;
        [SerializeField] private GameObject[] prefabs;
        private void Method1()
        {
            data = new Dictionary<int, GameObject>(ids.Length);
            for (int i = 0; i < ids.Length; i++)
            {
                data.Add(ids[i], prefabs[i]);
            }
        }
        */
        private void Method2()
        {
            data = new Dictionary<int, GameObject>(skills.Length);
            cards = new Dictionary<int, SkillCard>(skills.Length);
            for (int i = 0; i < skills.Length; i++)
            {
                int id = skills[i].card.Identity.ID;
                data.Add(id, skills[i].prefab);
                cards.Add(id, skills[i].card);
            }
        }

        public BaseSkill SpawnSkill(int id)
        {
            var go = Instantiate(data[id]);
            BaseSkill skill = go.GetComponent<BaseSkill>();
            skill.SetData(cards[id]);
            return skill;
        }
    }
}
