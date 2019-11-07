using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Catlike.TowerDefense
{
    public class Game : MonoBehaviour
    {
        private static Game instance;
        
        private const float pausedTimeScale = 0f;

        
        [SerializeField] private Vector2Int boardSize = new Vector2Int(11, 11);
        
        [SerializeField] private GameBoard board = default;
        
        [SerializeField] private GameTileContentFactory tileContentFactory = default;
        
        [SerializeField] private WarFactory warFactory = default;
        
        [SerializeField] private GameScenario scenario = default;

        private GameScenario.State activeScenario;
        
        [SerializeField, Range(0, 100)] private int startingPlayerHealth = 10;
        
        private int playerHealth;
        
        [SerializeField, Range(1f, 10f)] private float playSpeed = 1f;

        private GameBehaviorCollection enemies = new GameBehaviorCollection();
        private GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
        
        private TowerType selectedTowerType;
        
        private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

        private void Awake()
        {
            board.Initialize(boardSize, tileContentFactory);
            board.ShowGrid = true;
            activeScenario = scenario.Begin();
            playerHealth = startingPlayerHealth;
        }
        
        private void OnEnable () {
            instance = this;
        }
        
        private void BeginNewGame () {
            playerHealth = startingPlayerHealth;
            enemies.Clear();
            nonEnemies.Clear();
            board.Clear();
            activeScenario = scenario.Begin();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                HandleAlternativeTouch();
            }
            
            if (Input.GetKeyDown(KeyCode.V)) {
                board.ShowPaths = !board.ShowPaths;
            }
            if (Input.GetKeyDown(KeyCode.G)) {
                board.ShowGrid = !board.ShowGrid;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                selectedTowerType = TowerType.Laser;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                selectedTowerType = TowerType.Mortar;
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                Time.timeScale =
                    Time.timeScale > pausedTimeScale ? pausedTimeScale : playSpeed;
            }
            else if (Time.timeScale > pausedTimeScale) {
                Time.timeScale = playSpeed;
            }
            
            if (Input.GetKeyDown(KeyCode.B)) {
                BeginNewGame();
            }

            if (playerHealth <= 0 && startingPlayerHealth > 0) {
                Debug.Log("Defeat!");
                BeginNewGame();
            }
            
            if (!activeScenario.Progress() && enemies.IsEmpty) {
                Debug.Log("Victory!");
                BeginNewGame();
                activeScenario.Progress();
            }

            activeScenario.Progress();
            
            enemies.GameUpdate();
            Physics.SyncTransforms();
            board.GameUpdate();
            nonEnemies.GameUpdate();
        }
        
        public static void EnemyReachedDestination () {
            instance.playerHealth -= 1;
        }
        
        public static Shell SpawnShell () {
            Shell shell = instance.warFactory.Shell;
            instance.nonEnemies.Add(shell);
            return shell;
        }
        
        public static Explosion SpawnExplosion () {
            Explosion explosion = instance.warFactory.Explosion;
            instance.nonEnemies.Add(explosion);
            return explosion;
        }
        
        public static void SpawnEnemy (EnemyFactory factory, EnemyType type) {
            GameTile spawnPoint =
                instance.board.GetSpawnPoint(Random.Range(0, instance.board.SpawnPointCount));
            Enemy enemy = factory.Get(type);
            enemy.SpawnOn(spawnPoint);
            instance.enemies.Add(enemy);
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

        void HandleTouch () {
            GameTile tile = board.GetTile(TouchRay);
            if (tile != null) {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    board.ToggleTower(tile, selectedTowerType);
                }
                else {
                    board.ToggleWall(tile);
                }
            }
        }
        
        void HandleAlternativeTouch () {
            GameTile tile = board.GetTile(TouchRay);
            if (tile != null) {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    board.ToggleDestination(tile);
                }
                else {
                    board.ToggleSpawnPoint(tile);
                }
            }
        }

    }
}