using UnityEngine;

namespace Catlike.TowerDefense
{
    [System.Serializable]
    public class EnemySpawnSequence
    {
        [SerializeField] private EnemyFactory factory = default;

        [SerializeField] private EnemyType type = EnemyType.Medium;

        [SerializeField, Range(1, 100)] private int amount = 1;

        [SerializeField, Range(0.1f, 10f)] private float cooldown = 1f;
        
        public State Begin () => new State(this);
        
        [System.Serializable]
        public struct State {

            private EnemySpawnSequence sequence;

            private int count;

            private float cooldown;
            
            public State (EnemySpawnSequence sequence) {
                this.sequence = sequence;
                this.count = 0;
                this.cooldown = sequence.cooldown;
            }
            
            public float Progress (float deltaTime) {
                cooldown += deltaTime;
                while (cooldown >= sequence.cooldown) {
                    cooldown -= sequence.cooldown;
                    if (count >= sequence.amount) {
                        return cooldown;
                    }
                    count += 1;
                    Game.SpawnEnemy(sequence.factory, sequence.type);
                }
                return -1f;
            }
        }
    }
}