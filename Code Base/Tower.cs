using SpaceShooter;
using UnityEngine;


namespace TowerDefence
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        [SerializeField] private float m_Lead;

        private Turret[] m_Turrets;
        private Rigidbody2D m_Target = null;

        private void Update()
        {
            if (m_Target)
            {
                if (Vector3.Distance(m_Target.transform.position, transform.position) <= m_Radius)
                {
                    foreach (var turret in m_Turrets)
                    {
                        turret.transform.up = m_Target.transform.position - turret.transform.position + (Vector3)m_Target.velocity *m_Lead;
                        turret.Fire();
                    }
                }
                else
                {
                    m_Target = null;
                }
            }
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);

                if (enter)
                {
                    m_Target = enter.transform.root.GetComponent<Rigidbody2D>();
                }
            }
        }

        public void Use(TowerAsset towerAsset)
        {
            Component[] towerSprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < towerSprites.Length; i++)
            {
                towerSprites[0].GetComponent<SpriteRenderer>().sprite = towerAsset.m_TowerSprite;
                towerSprites[1].GetComponent<SpriteRenderer>().sprite  = towerAsset.m_TowerHeadSprite;
            }

            m_Turrets = GetComponentsInChildren<Turret>();

            foreach (var turret in m_Turrets)
            {
                turret.AssignLoadout(towerAsset.m_TurretProperties);
            }

            GetComponentInChildren<BuildSite>().SetBuildableTowers(towerAsset.m_UpgradesTo);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);

            Gizmos.DrawWireSphere(transform.position, m_Radius);
        } 
#endif
    }
}