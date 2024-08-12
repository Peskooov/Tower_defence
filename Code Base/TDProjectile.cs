using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class TDProjectile : Projectile
    {
        public enum DamageType
        {
            Base,
            Magic,
            Fire
        }

        [SerializeField] private DamageType m_DamageType;
        [SerializeField] private Sound m_ShotSound = Sound.Arrow;
        [SerializeField] private Sound m_HitSound = Sound.ArrowHit;

        private void Start()
        {
            m_ShotSound.Play();
        }

        protected override void OnHit(RaycastHit2D hit)
        {
            var enemy = hit.collider.transform.root.GetComponent<Enemy>();

            if (enemy != null)
            {
                m_HitSound.Play();
                enemy.TakeDamage(m_Damage, m_DamageType);
            }
        }
    }
}