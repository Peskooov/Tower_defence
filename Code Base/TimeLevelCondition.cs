using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class TimeLevelCondition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private float m_TimeLimit;
        public bool IsCompleted => Time.time > m_TimeLimit;

        private void Start()
        {
            m_TimeLimit += Time.time;
        }
    }
}