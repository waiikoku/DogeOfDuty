using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleAnimate : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        private Vector2 moveIntend;
        private float moveSpeed;
        private float msc;
        private void LateUpdate()
        {
            moveSpeed = moveIntend.sqrMagnitude;
            msc = Mathf.Clamp(moveSpeed, 0, 1);
            anim.SetBool("Move", msc > 0.01f);
            //anim.SetFloat("Speed", Mathf.Clamp(moveSpeed,0,1));
        }
        
        public void Movement(Vector2 direction)
        {
            moveIntend = direction;
        }

        public void Attack()
        {
            anim.SetTrigger("Shoot");
        }
    }
}
