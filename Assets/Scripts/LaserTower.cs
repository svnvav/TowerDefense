using UnityEngine;

namespace Catlike.TowerDefense
{
    public class LaserTower : Tower
    {
        public override TowerType TowerType => TowerType.Laser;

        [SerializeField] private Transform turret = default, laserBeam = default;
        
        [SerializeField, Range(1f, 100f)] private float damagePerSecond = 10f;
        
        private TargetPoint target;

        private Vector3 laserBeamScale;

        void Awake () {
            laserBeamScale = laserBeam.localScale;
        }
        
        public override void GameUpdate () {
            if (TrackTarget(ref target) || AcquireTarget(out target)) {
                Shoot();
            }
            else {
                laserBeam.localScale = Vector3.zero;
            }
        }
        
        private void Shoot () {
            Vector3 point = target.Position;
            turret.LookAt(point);
            laserBeam.localRotation = turret.localRotation;
            
            float d = Vector3.Distance(turret.position, point);
            laserBeamScale.z = d;
            laserBeam.localScale = laserBeamScale;
            laserBeam.localPosition =
                turret.localPosition + 0.5f * d * laserBeam.forward;
            
            target.Enemy.ApplyDamage(damagePerSecond * Time.deltaTime);
        }
    }
}