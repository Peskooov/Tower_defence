using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class StaticDamage : MonoBehaviour
    {
        [SerializeField] private int m_Damage;
        [SerializeField] private float m_Radius;

        void Update()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, m_Radius, transform.position);

            if (hit)
            {
                var destructible = hit.collider.transform.root.GetComponent<Destructible>();

                if (destructible != null)
                {
                    destructible.ApplyDamage(m_Damage);
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }
    }
}