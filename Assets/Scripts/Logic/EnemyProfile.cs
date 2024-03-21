using Roguelike;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class EnemyProfile : MonoBehaviour
    {
        [SerializeField] private SimpleAI agent;
        [SerializeField] private LifeStatus lift;

        [Header("Card")]
        [SerializeField] private LifeData lifeCard; //Old
        [SerializeField] private EnemyData card; //New

        private void Awake()
        {
            agent.SetSpeed(card.speed);
            lift.SetHealth(card.health);
        }
    }
}
