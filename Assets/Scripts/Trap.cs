using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Trap : MonoBehaviour
    {
        enum TrapType
        {
            none,
            spikes,
            tar,
            ice,
            poppy,
            pit,
            caltrop,
            shadow,
            meat
        }

        [SerializeField] float damage = 5f;
        [SerializeField] float poppyTimer = 20f;

        [SerializeField] TrapType trapType = TrapType.none;


        public void TrapTrigger(AttackReceiver attackReceiver)
        {
            switch (trapType)
            {
                case TrapType.spikes:
                    attackReceiver.TakeDamage(damage);
                    print("trap dealt " + damage + " to " + attackReceiver.name);
                    break;

                case TrapType.poppy:
                    attackReceiver.ApplyBadStatusEffect(StatusEffect.madness, poppyTimer);
                    print("trap caused " + attackReceiver.name + " to go MAD!" );
                    break;

                case TrapType.caltrop:
                    attackReceiver.GetComponent<StatHandler>().ApplyNerf(StatHandler.StatChanges.StrengthNerf, 1);
                    print("trap caused " + attackReceiver.name + "to lose strength!");
                    break;

                case TrapType.tar:
                    attackReceiver.GetComponent<StatHandler>().ApplyNerf(StatHandler.StatChanges.SpeedNerf, 1);
                    print("trap caused " + attackReceiver.name + "to lose speed!");
                    break;

                case TrapType.ice:
                    attackReceiver.GetComponent<StatHandler>().ApplyNerf(StatHandler.StatChanges.ArmorNerf, 1);
                    print("trap caused " + attackReceiver.name + "to lose armor!");
                    break;

                case TrapType.pit:
                    print(attackReceiver.name + "has disappeared!");
                    attackReceiver.GetComponent<Fighter>().Remove();
                    break;

                case TrapType.shadow:
                    attackReceiver.ApplyBadStatusEffect(StatusEffect.burn, poppyTimer);
                    print("trap burned " + attackReceiver.name);
                    break;

                case TrapType.meat:
                    attackReceiver.Heal(damage, false);
                    print(attackReceiver.name + " ate the raw meat and gained " + damage + " health!");
                    break;

                default:
                    print("trap did nothing");
                    break;
            }
        }
    }
}