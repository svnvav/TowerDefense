using UnityEngine;

namespace Catlike.TowerDefense
{
    [CreateAssetMenu]
    public class EnemyAnimationConfig : ScriptableObject
    {
        [SerializeField] private AnimationClip move = default;

        public AnimationClip Move => move;
    }
}