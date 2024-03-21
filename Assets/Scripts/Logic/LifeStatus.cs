using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike
{
    public class LifeStatus : MonoBehaviour
    {
        [SerializeField] private LifeData card;

        [Header("Base")]
        [SerializeField] private Life lp;

        [Header("Stats")]
        public float currentHealth;
        public float currentStamina;

        [Header("Events")]
        public Action<float> percentageHp;
        public Action<float, float> updateHp;
        public Action OnHealthFull;
        public Action OnHealthDepleted;

        private bool isAlive = false;

        private enum InitializeMode
        {
            Origin,
            Gen2
        }
        [SerializeField] private InitializeMode mode = InitializeMode.Origin;

        private void Start()
        {
            switch (mode)
            {
                case InitializeMode.Origin:
                    lp = new Life(card.Stat);
                    break;
                case InitializeMode.Gen2:
                    break;
                default:
                    break;
            }
            ResetHealth();
        }

        public void SetData(LifeData card)
        {
            this.card = card;
        }

        public void SetHealth(int health)
        {
            lp = new Life();
            lp.Health = (uint)health;
        }

        public void ResetHealth()
        {
            currentHealth = lp.Health;
            percentageHp?.Invoke(1);
            updateHp?.Invoke(lp.Health, lp.Health);
            isAlive = true;
        }

        public void AddGuageListener(Action<float> callback)
        {
            percentageHp += callback;
            percentageHp?.Invoke(currentHealth / lp.Health);
        }

        public void AddMinMaxListener(Action<float, float> callback)
        {
            updateHp += callback;
            updateHp?.Invoke(currentHealth, lp.Health);
        }

        public void AddHealth(float hp)
        {
            if (currentHealth == lp.Health) return;
            currentHealth += hp;
            if (currentHealth > lp.Health)
            {
                currentHealth = lp.Health;
                OnHealthFull?.Invoke();
            }
            Recall();
        }

        public void RemoveHealth(float hp)
        {
            if (isAlive == false) return;
            currentHealth -= hp;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnHealthDepleted?.Invoke();
                isAlive = false;
            }
            Recall();
        }

        public void Recall()
        {
            float max = (float)lp.Health;
            percentageHp?.Invoke(currentHealth / max);
            updateHp?.Invoke((uint)currentHealth, lp.Health);
        }
    }
}