using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class AttackIndicator : MonoBehaviour
    {
        [SerializeField] IndicatorCell volleyCell;
        [SerializeField] IndicatorCell beelineCell;
        CombatController combatController;
        public bool areCellsColored = false;
        List<Cell> saveCellsToClear = new List<Cell>();

        void Start()
        {
            combatController = FindObjectOfType<CombatController>();
        }

        public void HighlightTargettedCells(EnemyAttack attack, EnemyTarget enemy)
        {
            GetTargetCells(attack, enemy);

            if (!areCellsColored)
            {
                saveCellsToClear = GetTargetCells(attack, enemy);
                ColorCells(attack.attackTarget, saveCellsToClear);
                areCellsColored = true;
            }
        }

        private List<Cell> GetTargetCells(EnemyAttack attack, EnemyTarget enemy)
        {
            switch (attack.targettingClass)
            {
                case TargettingClass.relative:
                    return RelativeTargetting(attack, enemy);
                    break;
                case TargettingClass.global:
                    return GlobalTargetting(attack, enemy);
                    break;
                default:
                    Debug.LogError("targetting class not assigned properly");
                    return null;
                    break;
            }
        }

        private void ColorCells(AttackTarget style, List<Cell> cells)
        {
            
            foreach (Cell cell in cells)
            {
                if (cell != null)
                {

                    if (style == AttackTarget.beeline)
                    {
                        cell.DoBeelineColor();
                    }
                    else if (style == AttackTarget.volley)
                    {
                        cell.DoVolleyColor();
                    }
                }
            }
        }

        public void ClearCells()
        {
            foreach (Cell cell in saveCellsToClear)
            {
                if (cell != null)
                {
                    cell.DoDefaultColor();
                }
            }

            saveCellsToClear.Clear();
            areCellsColored = false;
        }

        private List<Cell> RelativeTargetting(EnemyAttack attack, EnemyTarget enemy)
        {
            List<Cell> convertedCells = new List<Cell>();

            for (var i = 0; i < attack.relativeTargetCells.Length; i++)
            {
                var convertedVector = RelativeCoordinateConversion(enemy.wing, enemy.GetComponent<Mover>().GetGridPos(), attack.relativeTargetCells[i]);

                if (combatController.ListOfcells.ContainsKey(convertedVector))
                {
                    convertedCells.Add(combatController.ListOfcells[convertedVector]);
                }
            }

            return convertedCells;
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

        private List<Cell> GlobalTargetting(EnemyAttack attack, EnemyTarget enemy)
        {
            List<Cell> convertedCells = new List<Cell>();

            for (var i = 0; i < attack.globalTargetCells.Length; i++)
            {
                var currentVector = attack.globalTargetCells[i];

                if (combatController.ListOfcells.ContainsKey(currentVector))
                {
                    convertedCells.Add(combatController.ListOfcells[currentVector]);
                }
            }

            return convertedCells;
        }
    }
}
