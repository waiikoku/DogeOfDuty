using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class UIManager : Singleton<UIManager>
    {
        public enum State
        {
            Invalid = -1,
            Initialize = 0,
            Lobby = 1,
            Playmode = 2
        }
        [SerializeField] private State m_state = State.Invalid;

        [Header("Canvas")]
        [SerializeField] private LobbyUI m_lobby; //No implementation
        [SerializeField] private IngameUI m_ingame;

        [Header("Prefabs")]
        [SerializeField] private Transform container;
        private GameObject current;

        //Cache
        private SurvivalManager sm;
        private UpgradeManager um;
        private IngameUI ingame;
        public IngameUI IngameUI { get { return ingame; } }

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Start()
        {
            Setup();
        }

        private void Initialize()
        {

        }

        private void Setup()
        {

        }

        public async void ChangeMode(State state)
        {
            await Despawn();
            switch (state)
            {
                case State.Invalid:
                    break;
                case State.Initialize:
                    break;
                case State.Lobby:
                    Spawn(m_lobby);
                    break;
                case State.Playmode:
                    Setup_Ingame(Spawn(m_ingame));
                    break;
                default:
                    break;
            }
            m_state = state;
        }

        public T Spawn<T>(T uiPrefab) where T : Component
        {
            var obj = Instantiate(uiPrefab, container);
            current = obj.gameObject;
            return obj;
        }

        public async System.Threading.Tasks.Task Despawn()
        {
            if (current != null)
            {
                print("Despawn " + current.name);
                current.SendMessage("SelfDestruct");
                Destroy(current);
                current = null;
            }
            await System.Threading.Tasks.Task.Delay(500);
            await System.Threading.Tasks.Task.CompletedTask;
        }

        private void Setup_Lobby()
        {

        }

        private void Setup_Ingame(IngameUI ui)
        {
            ingame = ui;
            sm = ui.GetComponent<SurvivalManager>();
            sm.OnLevelUp += ui.SetLv;
            sm.OnExp = ui.SetXp;
            sm.OnExpPrg = ui.SetXpProg;
            sm.OnExpCapacityChange = ui.SetXpCap;
            sm.OnPlaytime = ui.SetPlaytime;
            sm.OnSpawn = ui.SetEnemyCount;
            sm.OnKilled = ui.SetEnemyKill;
            sm.OnPlaytime = ui.SetPlaytime;
            sm.Initialize();
            um = ui.GetComponent<UpgradeManager>();
            sm.CountTime(true);
        }

        public void AssignUpgrade(SkillManage skill)
        {
            um.LinkSkill(skill);
        }

        public void ActiveLobby(bool active)
        {

        }

        public void ActiveUpgrade(bool active)
        {
            ingame.ActivateUpgrade(active);
        }

        public UpgradeManager AccessUpgrade()
        {
            return um;
        }
    }
}
