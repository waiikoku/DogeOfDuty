using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Dod
{
    public class IngameUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform container;
        [SerializeField] private TextMeshProUGUI playTime;
        [Header("Panel")]
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject upgradePanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject defeatPanel;
        private bool isPause;
        [Header("Level")]
        [SerializeField] private TextMeshProUGUI lv;
        [SerializeField] private TextMeshProUGUI xp;
        [SerializeField] private TextMeshProUGUI xpCap;
        [SerializeField] private Image xpProg;
        [Header("Enemy")]
        [SerializeField] private TextMeshProUGUI emyCount;
        [SerializeField] private TextMeshProUGUI emyKill;
        [Header("Player")]
        [SerializeField] private Image hpProg;
        [SerializeField] private Image guage;
        public UIVirtualTouchJoy touch;
        #region Updater
        public void SetLv(uint level)
        {
            SlickUI.SetText(lv, level.ToString());
        }
        public void SetXp(int exp)
        {
            SlickUI.SetText(xp, exp.ToString());
        }
        private const string prefixXPC = "/";
        public void SetXpCap(uint cap)
        {
            SlickUI.SetText(xpCap, prefixXPC + cap);
        }
        public void SetXpProg(float value)
        {
            SlickUI.SetFillAmount(xpProg, value);
        }
        public void SetEnemyCount(uint count)
        {
            SlickUI.SetText(emyCount, count.ToString());
        }
        public void SetEnemyKill(uint kill)
        {
            SlickUI.SetText(emyKill, kill.ToString());
        }
        private const string format = "hh':'mm':'ss";
        public void SetPlaytime(float time)
        {
            string timeText = System.TimeSpan.FromSeconds(time).ToString(format);
            SlickUI.SetText(playTime, timeText);
        }
        #endregion

        public void SetHp(float hp)
        {
            SlickUI.SetFillAmount(hpProg, hp);
        }

        public void ActivateUpgrade(bool active)
        {
            upgradePanel.SetActive(active);
        }

        public void UpdateGuage(float value)
        {
            guage.fillAmount = value;
        }
        public void ActivatePause(bool active)
        {
            isPause = active;
            pausePanel.SetActive(active);
            Time.timeScale = active ? 0 : 1;
        }

        public void ActivateDefeat(bool active)
        {
            isPause = active;
            defeatPanel.SetActive(active);
            Time.timeScale = active ? 0 : 1;
        }
    }
}
