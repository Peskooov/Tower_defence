using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private CircleArea m_StartArea;
        public CircleArea StartArea => m_StartArea;

        [SerializeField] private AIPointPatrol[] m_PathPoints;

        public int Lenght { get => m_PathPoints.Length; }
        public AIPointPatrol this[int i] { get => m_PathPoints[i]; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);

            foreach (var ponit in m_PathPoints)
            {
                Gizmos.DrawSphere(ponit.transform.position, ponit.Radius);

            }

            for (int i = 0; i < m_PathPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(m_PathPoints[i].transform.position, m_PathPoints[i+1].transform.position);
            }
        }
    }
}