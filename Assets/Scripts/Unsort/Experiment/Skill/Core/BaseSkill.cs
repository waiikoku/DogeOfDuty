using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    [System.Serializable]
    public class BaseSkillStat
    {
        /// <summary>
        /// Dealt Damage
        /// </summary>
        public float strength;
        /// <summary>
        /// Energy Consume
        /// </summary>
        public float efficiency;
        /// <summary>
        /// How long it last
        /// </summary>
        public float duration;
        /// <summary>
        /// How far
        /// </summary>
        public float range;
        /// <summary>
        /// How long til it can use again
        /// </summary>
        public float cooldown;
    }

    [System.Serializable]
    public class SkillStatTier
    {
        public float[] strength = new float[4];
        public float[] efficiency = new float[4];
        public float[] duration = new float[4];
        public float[] range = new float[4];

        public float[] GetTier(int tier)
        {
            return new float[]
            {
                strength[tier],
                efficiency[tier],
                duration[tier],
                range[tier]
            };
        }
    }
    public class BaseSkillCallback
    {
        public float percentage;
        public float time;
        public Coroutine coroutine;
        public Action<bool> Active;
        private event Action<float> OnPercentage;
        private event Action<float> OnDuration;

        public BaseSkillCallback()
        {

        }

        public BaseSkillCallback(Action<float> onPercentage, Action<float> onDuration)
        {
            OnPercentage = onPercentage;
            OnDuration = onDuration;
        }
        #region Base
        public void AddListener(ref Action<float> callback, Action<float> action)
        {
            if (action == null) return;
            callback += action;
        }

        public void RemoveListener(ref Action<float> callback, Action<float> action)
        {
            if (action == null) return;
            callback -= action;
        }
        #endregion
        public void AddPListener(Action<float> action)
        {
            AddListener(ref OnPercentage, action);
        }

        public void RemovePListener(Action<float> action)
        {
            RemoveListener(ref OnPercentage, action);
        }

        public void AddTListener(Action<float> action)
        {
            AddListener(ref OnDuration, action);
        }

        public void RemoveTListener(Action<float> action)
        {
            RemoveListener(ref OnDuration, action);
        }

        public void InvokeP()
        {
            OnPercentage?.Invoke(percentage);
        }

        public void InvokeT()
        {
            OnDuration?.Invoke(time);
        }
    }

    public abstract class BaseSkill : MonoBehaviour
    {
        [SerializeField] protected SkillCard m_skill;
        public SkillCard Skill => m_skill;
        private const int MAXLEVEL = 5;
        public int m_level;
        public int Level => m_level;
        [SerializeField] protected BaseSkillStat m_stat;
        public BaseSkillStat Stat => m_stat;
        public bool isActive;
        public bool isCooldown;
        public BaseSkillCallback OnActive;
        public BaseSkillCallback OnCooldown;
        [SerializeField] protected bool isMultiple = false;
        public bool IsMultiple => isMultiple;
        [SerializeField] protected bool activeLoop = false;

        public Action<int, int> OnUpgrade;
        protected virtual void Awake()
        {
            OnActive = new BaseSkillCallback();
            OnCooldown = new BaseSkillCallback();
            if (activeLoop)
            {
                m_stat.duration = Mathf.Infinity;
            }
        }
        [System.Serializable]
        public struct Param
        {
            public Transform center;
            public Animator characterAnim;
        }
        protected Param m_param;
        public void Setup(Param param)
        {
            m_param = param;
            print("Set " + gameObject.name);
        }
        public void SetData(SkillCard card)
        {
            m_skill = card;
        }

        public abstract void Activation(bool active);

        public IEnumerator Active()
        {
            isActive = true;
            ActiveStart();
            OnActive.Active?.Invoke(isActive);
            while (m_stat.duration > OnActive.time)
            {
                OnActive.time += Time.deltaTime;
                OnActive.percentage = OnActive.time / m_stat.duration;
                OnActive.InvokeP();
                OnActive.InvokeT();
                ActiveUpdate();
                yield return null;
            }
            isActive = false;
            OnActive.Active?.Invoke(isActive);
            ActiveEnd();
        }

        protected virtual void ActiveStart()
        {

        }

        protected virtual void ActiveUpdate()
        {

        }

        protected virtual void ActiveEnd()
        {

        }

        protected virtual void CooldownStart()
        {

        }

        protected virtual void CooldownUpdate()
        {

        }

        protected virtual void CooldownEnd()
        {
            OnCooldown.time = 0;
            OnCooldown.percentage = 0;
        }

        public IEnumerator Cooldown()
        {
            isCooldown = true;
            CooldownStart();
            OnCooldown.Active?.Invoke(isCooldown);
            while (m_stat.cooldown > OnCooldown.time)
            {
                OnCooldown.time += Time.deltaTime;
                OnCooldown.percentage = OnCooldown.time / m_stat.cooldown;
                OnCooldown.InvokeP();
                OnCooldown.InvokeT();
                CooldownUpdate();
                yield return null;
            }
            isCooldown = false;
            OnCooldown.Active?.Invoke(isCooldown);
            CooldownEnd();
        }

        public virtual void Upgrade()
        {
            if (m_level == MAXLEVEL) return;
            m_level++;
            /*
            var stat = m_skill.SkillStats.GetTier(m_level);
            m_stat.strength = stat[0];
            m_stat.efficiency = stat[1];
            m_stat.duration = stat[2];
            m_stat.range = stat[3];
            */
            OnUpgrade?.Invoke(m_skill.Identity.ID, m_level);
        }

        public virtual void Degrade()
        {
            if (m_level == 0) return;
            m_level--;
            /*
            var stat = m_skill.SkillStats.GetTier(m_level);
            m_stat.strength = stat[0];
            m_stat.efficiency = stat[1];
            m_stat.duration = stat[2];
            m_stat.range = stat[3];
            */
        }
    }
}
