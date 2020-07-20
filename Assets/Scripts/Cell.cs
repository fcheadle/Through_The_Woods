using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTW.UI;

namespace TTW.Combat
{
    [ExecuteInEditMode]

    public class Cell : MonoBehaviour
    {
        [SerializeField] Color oddColor = Color.grey;
        [SerializeField] Color evenColor = Color.white;
        [SerializeField] Color enemyColor = Color.magenta;
        [SerializeField] Color highlightColor = Color.cyan;
        [SerializeField] Color confirmColor = Color.green;
        [SerializeField] Color errorColor = Color.red;
        [SerializeField] Color volleyColor = Color.magenta;
        [SerializeField] Color beelineColor = Color.yellow;
        [SerializeField] Vector3 position;
        [SerializeField] Icon confirmMove;
        Icon myFlag;
        [SerializeField] Icon invalidMove;
        Icon myX;
        [SerializeField] Icon selectActor;
        Icon myCircle;

        public bool isExplored = false;
        public Cell exploredFrom;
        public bool enemyHighlight = false;

        //does not include traps
        public bool isOccupied = false;
        public bool hasObstacle = false;

        public Vector2Int gridPos;
        const int gridSize = 10;

        public bool isEnemyCell = false;

        //isOccupied for traps
        public bool hasTrap = false;
        public Trap currentTrap = null;
        Trap myTrap;
        Pushable myPushable;

        CombatController combatController;

        private void Awake()
        {
            position = transform.position;

            
            combatController = FindObjectOfType<CombatController>();



            combatController.AddNewPlayerCellKey(GetGridPos(), this);
        }

        private void Start()
        {
            DoDefaultColor();

            if (GetGridPos().x == 0 || GetGridPos().x == 5 || GetGridPos().y == 3)
            {
                isEnemyCell = true;
                return;
            }
        }


        public int GetGridSize()
        {
            return gridSize;
        }

        public Vector2Int GetGridPos()
        {
            gridPos = new Vector2Int
            (
                Mathf.RoundToInt(transform.position.x / gridSize),
                Mathf.RoundToInt(transform.position.z / gridSize)
            );

            return gridPos;
        }

        public void DoDefaultColor()
        {
            //if (enemyHighlight) return;

            //MeshRenderer mesh = GetComponent<MeshRenderer>();

            //if (GetGridPos().y == 0 || Mathf.Abs(GetGridPos().y) % 2 == 0)
            //{
            //    if (GetGridPos().x % 2 != 0)
            //    {
            //        mesh.material.color = oddColor;
            //    }
            //    else
            //    {
            //        mesh.material.color = evenColor;
            //    }
            //}
            //else
            //{
            //    if (GetGridPos().x % 2 != 0)
            //    {
            //        mesh.material.color = evenColor;
            //    }
            //    else
            //    {
            //        mesh.material.color = oddColor;
            //    }
            //}

            //if (GetGridPos().x == 0 || GetGridPos().x == 5 || GetGridPos().y == 3)
            //{
            //    mesh.material.color = enemyColor;
            //}
        }

        public void DoHighlightColor()
        {
            if (enemyHighlight) return;

            //MeshRenderer mesh = GetComponent<MeshRenderer>();
            //mesh.sharedMaterial.color = highlightColor;
        }

        public void DoConfirmationColor()
        {
            if (enemyHighlight) return;

            //MeshRenderer mesh = GetComponent<MeshRenderer>();
            //mesh.sharedMaterial.color = confirmColor;
        }

        public void DoErrorColor()
        {
            //MeshRenderer mesh = GetComponent<MeshRenderer>();
            //mesh.sharedMaterial.color = errorColor;
        }

        public void DoVolleyColor()
        {
            //MeshRenderer mesh = GetComponent<MeshRenderer>();
            //mesh.sharedMaterial.color = volleyColor;
        }

        public void DoBeelineColor()
        {
            //MeshRenderer mesh = GetComponent<MeshRenderer>();
            //mesh.sharedMaterial.color = beelineColor;
        }

        public void SetTrap(Trap setTrap)
        {
            if (!hasTrap)
            {
                hasTrap = true;
                currentTrap = setTrap;
                myTrap = Instantiate(setTrap);
                if (myTrap != null)
                {
                    myTrap.transform.position = transform.position;
                    myTrap.transform.rotation = transform.rotation;
                }
            }
        }

        public void DestroyTrap()
        {
            if (myTrap != null)
            {
                print("destroying trap");
                Destroy(myTrap.gameObject);
            }
        }

        public void SetPushable(Pushable setPushable)
        {
            if (!hasTrap)
            {
                isOccupied = true;
                myPushable = Instantiate(setPushable);
                if (myPushable != null)
                {
                    myPushable.transform.position = transform.position;
                    myPushable.transform.rotation = transform.rotation;
                }
            }
        }

        public void DisplayMoveIcon()
        {
            if (myFlag == null)
            {
                myFlag = Instantiate(confirmMove);
                myFlag.transform.position = transform.position;
            }
        }

        public void DisplayInvalidIcon()
        {
            if (myX == null)
            {
                myX = Instantiate(invalidMove);
                myX.transform.position = transform.position;
            }
        }

        public void DisplayActorIcon()
        {
            if (myCircle == null)
            {
                myCircle = Instantiate(selectActor);
                myCircle.transform.position = transform.position;
            }
        }

        public void DestroyIcons()
        {
            if (myFlag != null)
            {
                Destroy(myFlag.gameObject);
            }

            if (myX != null)
            {
                Destroy(myX.gameObject);
            }

            if (myCircle != null)
            {
                Destroy(myCircle.gameObject);
            }
        }
    }
}
