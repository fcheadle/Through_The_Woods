using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTW.Combat
{
    [ExecuteInEditMode]
    [SelectionBase]
    [RequireComponent(typeof(Cell))]


    public class EditorSnap : MonoBehaviour
    {
        Cell cell;
        CombatController combatController;

        private void Awake()
        {
            cell = GetComponent<Cell>();
            combatController = FindObjectOfType<CombatController>();
        }

        void Update()
        {
            SnapToGrid();
            UpdateLabel();
        }

        private void SnapToGrid()
        {
            int gridSize = cell.GetGridSize();
            transform.position = new Vector3(cell.GetGridPos().x * gridSize, 0f, cell.GetGridPos().y * gridSize);
            combatController.AddNewKey(cell.gridPos, cell);
        }

        private void UpdateLabel()
        {
            Vector2Int snapPos = cell.GetGridPos();

            string labelText = snapPos.x + "," + snapPos.y;
            gameObject.name = labelText;
        }


    }
}