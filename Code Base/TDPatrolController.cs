using UnityEngine;
using SpaceShooter;
using UnityEngine.Events;

namespace TowerDefence
{
    public class TDPatrolController : AIController
    {
        [SerializeField] private UnityEvent OnEndPath;

        private Path m_Path;
        private int m_Index;
        public void SetPath(Path newPath)
        {
            m_Path = newPath;
            m_Index = 0;

            SetPatrolBehaviour(m_Path[m_Index]);
        }

        protected override void GetNewPoint()
        {
            // ++m_Index => m_Index +=1;
            m_Index+=1;

            if (m_Path.Lenght > m_Index)
            {
                SetPatrolBehaviour(m_Path[m_Index]);
            }
            else
            {             
                OnEndPath.Invoke();
                Destroy(gameObject);
            }
        }
    }
}