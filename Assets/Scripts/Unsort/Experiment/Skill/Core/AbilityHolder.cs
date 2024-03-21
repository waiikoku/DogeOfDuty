using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class AbilityHolder : MonoBehaviour
    {
        public Ability ability;
        public AbilityTime abilityTime;

        private enum AbilityState
        {
            Invalid = -1,
            Standby = 0,
            Active = 1,
            Cooldown = 2
        }
        [SerializeField] private AbilityState state;
        [SerializeField] private KeyCode key;

        private void Update()
        {
            switch (state)
            {
                case AbilityState.Standby:
                    if (Input.GetKeyDown(key))
                    {
                        ability.Activate();
                        state = AbilityState.Active;
                        abilityTime.ActiveTime = ability.Data.time.ActiveTime;
                    }
                    break;
                case AbilityState.Active:
                    if (abilityTime.ActiveTime > 0)
                    {
                        abilityTime.ActiveTime -= Time.deltaTime;
                    }
                    else
                    {
                        state = AbilityState.Cooldown;
                        abilityTime.ActiveTime = 0; //Pure number
                        abilityTime.Cooldowntime = ability.Data.time.Cooldowntime;
                    }
                    break;
                case AbilityState.Cooldown:
                    if (abilityTime.Cooldowntime > 0)
                    {
                        abilityTime.Cooldowntime -= Time.deltaTime;
                    }
                    else
                    {
                        state = AbilityState.Standby;
                        abilityTime.Cooldowntime = 0; //Pure number
                    }
                    break;
                default:
                    return;
            }

   
        }
    }
}
