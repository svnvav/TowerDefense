using UnityEngine;

namespace Catlike.TowerDefense
{
    public class MortarTower : Tower
    {
        public override TowerType TowerType => TowerType.Mortar;
        
        [SerializeField, Range(0.5f, 2f)]
        private float shotsPerSecond = 1f;

        [SerializeField]
        private Transform mortar = default;
    }
}