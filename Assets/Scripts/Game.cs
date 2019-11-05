using System;
using UnityEngine;

namespace Catlike.TowerDefense
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Vector2Int boardSize = new Vector2Int(11, 11);
        
        [SerializeField] private GameBoard gameBoard = default;
        

        private void Awake()
        {
            gameBoard.Initialize(boardSize);
        }

        private void OnValidate()
        {
            if (boardSize.x < 2) {
                boardSize.x = 2;
            }
            if (boardSize.y < 2) {
                boardSize.y = 2;
            }
        }
    }
}