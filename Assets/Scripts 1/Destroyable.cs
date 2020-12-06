using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    public class Destroyable : MonoBehaviour
    {
        Mover mover;
        Cell currentCell;
        CombatController combatController;

        private void Start()
        {
            combatController = FindObjectOfType<CombatController>();
            mover = GetComponent<Mover>();
        }

        public void DestroySelf()
        {
            currentCell = combatController.ListOfPlayerCells[mover.GetGridPos()];
            currentCell.isOccupied = false;
            if (currentCell.hasObstacle == true)
            {
                currentCell.hasObstacle = false;
            }
            Destroy(this.gameObject);
        }
    }
}
