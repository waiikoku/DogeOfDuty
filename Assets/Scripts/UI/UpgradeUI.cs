using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private UpgradeManager upgrade;
        [SerializeField] private Transform container;
        [SerializeField] private SlotUpgradeUI upgradePrefab;
        private SlotUpgradeUI[] upgradeSlots;

        private void Awake()
        {
            SpawnRandom();
        }

        private void SpawnRandom()
        {
            upgradeSlots = new SlotUpgradeUI[upgrade.MAXRANDOM];
            for (int i = 0; i < upgrade.MAXRANDOM; i++)
            {
                upgradeSlots[i] = Instantiate(upgradePrefab, container);
                int index = i;
                upgradeSlots[i].AddListener(delegate { upgrade.SelectUpgrade(index); });
            }

            upgrade.OnActiveSlot += (index, active) =>
            {
                upgradeSlots[index].Activate(active);
            };
            upgrade.OnLevelSlot += (index, level) =>
            {
                upgradeSlots[index].ActiveStar(level);
            };
            upgrade.OnUpdateSlot += (int index, SkillCard card) =>
            {
                upgradeSlots[index].Assign(card.Identity.Name, card.SkillIcon);
            };
        }
    }
}
