using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Dod
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Fixed")]
        [SerializeField] private ExpPlayer1 playerPrefab;
        [SerializeField] private Mech mechPrefab;

        [Header("Dynamic")]
        [SerializeField] private WaypointData pointCard;
        private Transform spawnPoint;
        private ExpPlayer1 spawnedPlayer;
        private Mech spawnedMech;

        public void StartGame()
        {
            UIManager.Instance.ChangeMode(UIManager.State.Playmode);
            Spawn();
        }

        public void Spawn()
        {
            if (spawnPoint == null)
            {
                SpawnAsync();
                return;
            }
            spawnedPlayer = Instantiate(playerPrefab, GetSpawnpoint(), GetSpawnRotation());
            var skills = DataManager.Instance.GetActiveSkills();
            List<SkillCard> card = new List<SkillCard>
            {
                skills[0]
            };
            spawnedPlayer.Setup(new ExpPlayer1.Param
            {
                Skill2 = PersistentData.Instance.SpawnSkill,
                OwnedSkills = card,
                Input = AddListener
            });
        }

        private void AddListener(UnityAction<Vector2> callback)
        {
            print("GM ALT");
            UIManager.Instance.IngameUI.touch.touchZoneOutputEvent.AddListener(callback);
        }

        public void SpawnMech()
        {
            Vector3 circle = Random.insideUnitSphere * 10;
            circle.y = 0;
            Vector3 aroundPlayer = spawnedPlayer.transform.position + circle;
            spawnedMech = Instantiate(mechPrefab, aroundPlayer, Quaternion.identity);
        }

        private async void SpawnAsync()
        {
            while (spawnPoint == null)
            {
                if (WaypointData.Points.Count == 0)
                {
                    await Task.Delay(100);
                    continue;
                }
                WaypointData.Points.TryGetValue(pointCard.pointID, out spawnPoint);
                await Task.Delay(100);
            }
            Spawn();
        }

        private Vector3 GetSpawnpoint()
        {
            return spawnPoint.position;
        }

        private Quaternion GetSpawnRotation()
        {
            return spawnPoint.rotation;
        }

        public ExpPlayer1 GetPlayer()
        {
            return spawnedPlayer;
        }

        public void Despawn()
        {
            if(spawnedPlayer != null)
            {
                Destroy(spawnedPlayer);
                spawnedPlayer = null;
            }
        }
    }
}
