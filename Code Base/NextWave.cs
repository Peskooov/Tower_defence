using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class NextWave : MonoBehaviour
    {
        [SerializeField] private Text m_BonusAmount;
        [SerializeField] private Image m_BonusFill;

        private EnemyWavesManager m_EnemyWavesManager;
        private float m_TimeToNextWave;
        private float m_MaxTimeToWave;

        //public int BonusUpgrade;

        private void Start()
        {
            m_EnemyWavesManager = FindObjectOfType<EnemyWavesManager>();

            EnemyWave.OnWavePrepare += (float time) =>
            {
                m_TimeToNextWave = time;
                m_MaxTimeToWave = time;
            };
        }

        private void Update()
        {
            var bonus = (int)m_TimeToNextWave;
            if (bonus < 0)
            {
                bonus = 0;
                m_MaxTimeToWave = m_TimeToNextWave;
            }

            if (m_MaxTimeToWave < 0)
                m_MaxTimeToWave = 0;

            m_BonusFill.fillAmount = m_TimeToNextWave/m_MaxTimeToWave;
            m_BonusAmount.text = bonus.ToString();
            m_TimeToNextWave -= Time.deltaTime;
        }

        public void CallWave()
        {
            m_EnemyWavesManager.ForceNextWave();
        }
    }
}