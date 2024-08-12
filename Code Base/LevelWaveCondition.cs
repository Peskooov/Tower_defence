using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class LevelWaveCondition : MonoBehaviour, ILevelCondition
    {
        private bool m_IsComplited;
        public bool IsCompleted => m_IsComplited;

        private void Start()
        {
            FindObjectOfType<EnemyWavesManager>().OnAllWavesDead += () =>
            {
                m_IsComplited = true;
            };
        }
    }
}