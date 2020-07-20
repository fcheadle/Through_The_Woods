using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AttackIndicator : MonoBehaviour
    {

        public List<Cell> convertedCells = new List<Cell>();
        CombatController combatController;
        public bool areCellsColored = false;

        void Start()
        {
            combatController = FindObjectOfType<CombatController>();
        }

        public void HighlightTargettedCells(EnemyAttack attack, EnemyTarget enemy)
        {

            switch (attack.targettingClass)
            {
                case TargettingClass.relative:
                    RelativeTargetting(attack, enemy);
                    break;
                case TargettingClass.global:
                    GlobalTargetting(attack, enemy);
                    break;
                default:
                    Debug.LogError("targetting class not assigned properly");
                    break;
            }

            if (!areCellsColored)
            {
                ColorCells(attack);
                areCellsColored = true;
            }
        }

        private void ColorCells(EnemyAttack attack)
        {
            foreach (Cell cell in convertedCells)
            {
                cell.enemyHighlight = true;

                if (cell != null)
                {
                    if (attack.attackTarget == AttackTarget.beeline)
                    {
                        cell.DoBeelineColor();
                    }
                    else if (attack.attackTarget == AttackTarget.volley)
                    {
                        cell.DoVolleyColor();
                    }
                }
            }
        }

        private void RelativeTargetting(EnemyAttack attack, EnemyTarget enemy)
        {
            for (var i = 0; i < attack.relativeTargetCells.Length; i++)
            {
                var convertedVector = RelativeCoordinateConversion(enemy.wing, enemy.GetComponent<Mover>().GetGridPos(), attack.relativeTargetCells[i]);

                if (combatController.ListOfcells.ContainsKey(convertedVector))
                {
                    convertedCells.Add(combatController.ListOfcells[convertedVector]);
                }
            }
        }

        private Vector2Int RelativeCoordinateConversion(Wing wing, Vector2Int enemyCoordinates, Vector2Int attackCoordinates)
        {
            Vector2Int convertedVector;

            switch (wing)
            {
                case Wing.port:
                    convertedVector = new Vector2Int(enemyCoordinates.x + attackCoordinates.x, enemyCoordinates.y + attackCoordinates.y);
                    break;
                case Wing.starboard:
                    convertedVector = new Vector2Int(enemyCoordinates.x - attackCoordinates.x, enemyCoordinates.y + attackCoordinates.y);
                    break;
                case Wing.bow:
                    convertedVector = new Vector2Int(enemyCoordinates.x - attackCoordinates.y, enemyCoordinates.y - attackCoordinates.x);
                    break;
                default:
                    convertedVector = new Vector2Int(0, 0);
                    Debug.LogError("enemy wing is not defined.");
                    break;
            }

            return convertedVector;
        }

        private void GlobalTargetting(EnemyAttack attack, EnemyTarget enemy)
        {
            for (var i = 0; i < attack.globalTargetCells.Length; i++)
            {
                var currentVector = attack.globalTargetCells[i];

                if (combatController.ListOfcells.ContainsKey(currentVector))
                {
                    convertedCells.Add(combatController.ListOfcells[currentVector]);
                }
            }
        }
    }
}
