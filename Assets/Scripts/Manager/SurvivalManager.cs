using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dod
{
    public class SurvivalManager : Singleton<SurvivalManager>
    {
        [System.Serializable]
        public struct Survival
        {
            [Header("Player")]
            public uint currentLevel;
            public int currentExp;
            public uint expCap;
            public float expProgress;

            [Header("Playtime")]
            public uint enemyCount;
            public uint killCount;
            public float playTime;
        }

        [SerializeField] private Survival data;

        public Action<uint> OnLevelUp;
        public Action<int> OnExp;
        public Action<uint> OnExpCapacityChange;
        public Action<float> OnExpPrg;
        public Action<float> OnPlaytime;
        public Action<uint> OnSpawn;
        public Action<uint> OnKilled;

        public void Initialize()
        {
            SetLv(1);
            SetXp(0);
            SetXpProg(0);
            SetXpCap(XPCapOffset);
            SetEnemyKill(0);
            SetEnemyCount(0);
        }

        private bool isCounting = false;
        private Coroutine timerCoroutine;
        private Coroutine invCoroutine;

        public void CountTime(bool count)
        {
            isCounting = count;
            if (count)
            {
                IEnumerator Invoker()
                {
                    while (true)
                    {
                        Call(ActionType.Playtime, Mathf.Round(data.playTime));
                        yield return new WaitForSeconds(1f);
                    }
                }

                timerCoroutine = StartCoroutine(nameof(Timer));
                invCoroutine = StartCoroutine(Invoker());

            }
            else
            {
                StopCoroutine(invCoroutine);
                StopCoroutine(timerCoroutine);
            }
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                data.playTime += Time.deltaTime;
                yield return null;
            }
        }

        public enum ActionType
        {
            Exp,
            ExpProgress,
            ExpCap,
            Level,
            Playtime,
            Kill,
            Spawn
        }
        public void AddListener<T>(ActionType action, Action<T> callback)
        {
            switch (action)
            {
                case ActionType.Exp:
                    OnExp += callback as Action<int>;
                    break;
                case ActionType.ExpProgress:
                    OnExpPrg += callback as Action<float>;
                    break;
                case ActionType.ExpCap:
                    OnExpCapacityChange += callback as Action<uint>;
                    break;
                case ActionType.Level:
                    OnLevelUp += callback as Action<uint>;
                    print($"Add {callback.Method.Name} Event When LvUp");
                    break;
                case ActionType.Playtime:
                    OnPlaytime += callback as Action<float>;
                    break;
                case ActionType.Kill:
                    OnKilled += callback as Action<uint>;
                    break;
                case ActionType.Spawn:
                    OnSpawn += callback as Action<uint>;
                    break;
                default:
                    break;
            }
        }

        private void Call(ActionType act,object value)
        {
            switch (act)
            {
                case ActionType.Exp:
                    OnExp?.Invoke((int)value);
                    break;
                case ActionType.ExpProgress:
                    OnExpPrg?.Invoke((float)value);
                    break;
                case ActionType.ExpCap:
                    OnExpCapacityChange?.Invoke((uint)value);
                    break;
                case ActionType.Level:
                    OnLevelUp?.Invoke((uint)value);
                    break;
                case ActionType.Playtime:
                    OnPlaytime?.Invoke((float)value);
                    break;
                case ActionType.Kill:
                    OnKilled?.Invoke((uint)value);
                    break;
                case ActionType.Spawn:
                    OnSpawn?.Invoke((uint)value);
                    break;
                default:
                    break;
            }
        }
        private float expProgress;
        public void AddXp(int xp)
        {
            data.currentExp += xp;
            if(data.currentExp >= data.expCap)
            {
                int exceed = (int)data.expCap - data.currentExp;
                LevelUp();
                SetXp(0);
                if(exceed> 0) AddXp(exceed);
            }
            else
            {
                Call(ActionType.Exp, data.currentExp);
            }
            expProgress = (float)data.currentExp / (float)data.expCap;
            Call(ActionType.ExpProgress, expProgress);

        }
        public void AddKill()
        {
            data.killCount++;
            Call(ActionType.Kill, data.killCount);

            data.enemyCount--;
            Call(ActionType.Spawn, data.enemyCount);
        }

        public void AddEnemy()
        {
            data.enemyCount++;
            Call(ActionType.Spawn, data.enemyCount);
        }
        private enum GraphType
        {
            Undefined,
            Linear,
            Exponetial,
            Percentage
        }

        private const uint XPCapOffset = 10;
        private const uint XPMultipier = 1;
        private void LevelUp()
        {
            Call(ActionType.Level, ++data.currentLevel);
            var level = data.currentLevel;
            SetXpCap(level * level / XPMultipier + XPCapOffset);
            print("Level up!");
        }

        #region Modifier
        private void SetLv(uint level)
        {
            data.currentLevel = level;
            Call(ActionType.Level, level);
        }
        private void SetXp(int exp)
        {
            data.currentExp = exp;
            Call(ActionType.Exp, exp);
        }
        private void SetXpCap(uint cap)
        {
            data.expCap = cap;
            Call(ActionType.ExpCap, cap);
        }
        private void SetXpProg(float value)
        {
            data.expProgress = value;
            Call(ActionType.ExpProgress, value);
        }
        private void SetEnemyCount(uint count)
        {
            data.enemyCount = count;
            Call(ActionType.Spawn, count);
        }
        private void SetEnemyKill(uint kill)
        {
            data.killCount = kill;
            Call(ActionType.Kill, kill);
        }
        private void SetPlaytime(float time)
        {
            data.playTime = time;
            Call(ActionType.Playtime, time);
        }
        #endregion
    }
}
