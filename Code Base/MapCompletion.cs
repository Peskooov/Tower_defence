using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefence
{
    public class MapCompletion : MonoSingleton<MapCompletion>
    {
        public const string filename = "completion.dat";

        [Serializable]
        private class EpisodeScore
        {
            public Episode m_Episode;
            public int m_Score;
        }

        [SerializeField] private EpisodeScore[] completionData;

        private int m_totalScore;
        public int TotalScore => m_totalScore;

        private new void Awake()
        {
            base.Awake();

            SaveScore();
        }

        public static void SaveEpisodeResult(int levelScore)
        {
            if (Instance)
            {
                foreach (var item in Instance.completionData)
                { // Save new episode score
                    if (item.m_Episode == LevelSequenceController.Instance.CurrentEpisode)
                    {
                        if (levelScore > item.m_Score)
                        {
                            Instance.m_totalScore += levelScore - item.m_Score;
                            item.m_Score = levelScore;

                            Saver<EpisodeScore[]>.Save(filename, Instance.completionData);

                        }
                    }
                }

                //Instance.SaveScore();
            }
        }

        public int GetEpisodeScore(Episode m_episode)
        {
            foreach (var data in completionData)
            {
                if (data.m_Episode == m_episode)
                    return data.m_Score;
            }
            return 0;
        }

        private void SaveScore()
        {
            Saver<EpisodeScore[]>.TryLoad(filename, ref completionData);

            foreach (var episodeScore in completionData)
            {
                m_totalScore += episodeScore.m_Score;
            }
        }
    }
}