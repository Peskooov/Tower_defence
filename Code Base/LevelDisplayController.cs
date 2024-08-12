using UnityEngine;

namespace TowerDefence
{
    public class LevelDisplayController : MonoBehaviour
    {
        [SerializeField] private MapLevel[] m_Levels;
        [SerializeField] private BonusLevel[] m_BonusLevels;

        private void Start()
        {
            var drawLevel = 0;
            var score = 1;

            while (score != 0 && drawLevel < m_Levels.Length)
            {
                score = m_Levels[drawLevel].Initialise();
                drawLevel += 1;
            }

            for (int i = drawLevel; i < m_Levels.Length; i++)
            {
                m_Levels[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < m_BonusLevels.Length; i++)
            {
                m_BonusLevels[i].TryActivate();
            }
        }
    }
}