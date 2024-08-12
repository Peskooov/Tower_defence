using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWave : MonoBehaviour
    {
        public static Action<float> OnWavePrepare;

        [Serializable]
        private class Squad
        {
            public EnemyAsset m_Asset;
            public int m_Count;
        }
        [Serializable]
        private class PathGroup
        {
            public Squad[] m_Squads;
        }

        [SerializeField] private PathGroup[] m_PathGroups;

        [SerializeField] private float m_PrepareTime;
        public float WavePrepareTime() { return m_PrepareTime - Time.time; }

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (Time.time >= m_PrepareTime)
            {
                enabled = false;

                //if OnWaveReady != null
                OnWaveReady?.Invoke(); 
            }
        }

        public IEnumerable<(EnemyAsset asset, int count, int pathIndex)> EnumerateSquads()
        {
            for (int i = 0; i < m_PathGroups.Length; i++)
            {
                foreach (var squad in m_PathGroups[i].m_Squads)
                {
                    yield return (squad.m_Asset, squad.m_Count, i);
                }
            }
        }

        private event Action OnWaveReady;

        public void Prepare(Action spawnEnemies)
        {
            OnWavePrepare?.Invoke(m_PrepareTime);

            m_PrepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }

        [SerializeField] private EnemyWave nextWave;
        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;
            
            if(nextWave)
            nextWave.Prepare(spawnEnemies);

            return nextWave;
        }       
    }
}