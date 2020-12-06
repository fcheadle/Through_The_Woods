using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TTW.Combat
{
    [RequireComponent(typeof(Pushable))]
    public class Obstacle : MonoBehaviour
    {
        Pushable pushable;
        CombatController combatController;
        Cell currentCell;

        // Start is called before the first frame update
        void Start()
        {
            pushable = GetComponent<Pushable>();
            combatController = FindObjectOfType<CombatController>();
            currentCell = CurrentCell();
            currentCell.hasObstacle = true;
        }

        public Cell CurrentCell()
        {
            return combatController.ListOfPlayerCells[pushable.GetGridPos()];
        }
    }
}