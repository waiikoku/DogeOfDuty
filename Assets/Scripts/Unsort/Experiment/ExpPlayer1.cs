using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Dod
{
    public class ExpPlayer1 : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform head;
        [SerializeField] private Transform character;
        [SerializeField] private SkillManage skillManage;

        [SerializeField] private Roguelike.LifeStatus life;
        [SerializeField] private SimpleRigidPlayer mover;
        [SerializeField] private SimpleAnimate animate;
        [SerializeField] private SimpleGun weapon;

        [Header("Animations")]
        [SerializeField] private Animator anim;
        [SerializeField] private GameObject explodeFx;
        public struct Param
        {
            public Func<int, BaseSkill> Skill2;
            public List<SkillCard> OwnedSkills;
            public Action<UnityAction<Vector2>> Input;
        }
        private Param m_param;
        public void Setup(Param param)
        {
            m_param = param;
            skillManage.InitializeSkill(new BaseSkill.Param
            {
                center = transform,
                characterAnim = anim,
            });
            foreach (var skill in m_param.OwnedSkills)
            {
                int id = skill.Identity.ID;
                skillManage.InstallSkill(m_param.Skill2(id).gameObject);
            }
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }
            GameController gc = FindFirstObjectByType<GameController>();
            if (gc != null)
            {
                gc.Setup(new GameController.Param
                {
                    target = player,
                    head = head
                });
            }
            life.AddGuageListener(UIManager.Instance.IngameUI.SetHp);
            UIManager.Instance.AssignUpgrade(skillManage);
            param.Input?.Invoke(mover.SetInput);
        }

        /*
        private const string targetTag = "Enemy";
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag) == false) return;
            print("Player has hit by enemy");
            life.RemoveHealth(1);
        }
        */

        private void Awake()
        {
            weapon.OnFire += animate.Attack;
            life.OnHealthDepleted += Died;
        }

        private void LateUpdate()
        {
            animate.Movement(mover.GetInput());
        }

        private async void Died()
        {
            Instantiate(explodeFx,transform.position,Quaternion.identity);
            await Task.Delay(3000);
            gameObject.SetActive(false);
            UIManager.Instance.IngameUI.ActivateDefeat(true);
        }
    }
}
