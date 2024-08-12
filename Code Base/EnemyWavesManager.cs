using System;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path[] m_Paths;
        [SerializeField] private EnemyWave m_CurrentWave;

        [Header("Upgrade")]
        [SerializeField] private UpgradeAsset goldPerWaveUpgrade;
        [SerializeField] private int goldBonusPerWave;

        public event Action OnAllWavesDead;
        public static event Action<Enemy> OnEnemySpawn;

        private int activeEnemyCount;
        private int levelUpgradeGold;

        private void Awake()
        {
            if (goldPerWaveUpgrade)
            {
                var goldBonus = Upgrades.GetUpgradeLevel(goldPerWaveUpgrade);
                levelUpgradeGold = goldBonus;
            }
        }

        private void Start()
        {
            m_CurrentWave.Prepare(SpawnEnemies);
        }

        private void RecordEnemyCount()
        {
            if (--activeEnemyCount == 0)
            {
                if (m_CurrentWave)
                    ForceNextWave();
                else
                {
                    if (activeEnemyCount == 0)
                        OnAllWavesDead?.Invoke();
                }
            }
        }

        private void SpawnEnemies()
        {
            foreach ((EnemyAsset asset, int count, int pathIndex) in m_CurrentWave.EnumerateSquads())
            {
                if (pathIndex < m_Paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var e = Instantiate(m_EnemyPrefab, m_Paths[pathIndex].StartArea.RandomInsideZone, Quaternion.identity);

                        e.OnEnd += RecordEnemyCount;
                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(m_Paths[pathIndex]);

                        activeEnemyCount += 1;

                        OnEnemySpawn?.Invoke(e);
                    }
                }
            }

            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
        }

        public void ForceNextWave()
        {          
            if (m_CurrentWave)
            {
                TDPlayer.Instance.ChangeGold((int)m_CurrentWave.WavePrepareTime() + levelUpgradeGold * goldBonusPerWave);

                SpawnEnemies();
            }
        }
    }
}