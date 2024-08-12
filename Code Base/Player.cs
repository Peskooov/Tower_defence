using UnityEngine;
using System;

namespace SpaceShooter
{
    /// <summary>
    /// Класс сущности игрока. Содержит в себе все что связано с игроком.
    /// </summary>
    public class Player : MonoSingleton<Player>
    {
        public event Action OnPlayerDeath;

        [SerializeField] private int m_NumLives;
        public int Numlives => m_NumLives;

        [SerializeField] private SpaceShip m_Ship;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private SpaceShip m_PlayerShipPrefab;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;

        private void Start()
        {
            if (m_Ship)
                m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

        protected void TakeDamage(int m_Damage)
        {
            m_NumLives -= m_Damage;
            
            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDeath?.Invoke();
            }                                
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
                Respawn();
            else
                LevelSequenceController.Instance.FinishCurrentLevel(false);
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(m_PlayerShipPrefab.gameObject);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

           // m_CameraController.SetTarget(m_Ship.transform);
           // m_MovementController.SetTargetShip(m_Ship);

            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }
            

        #region Score (current level only)

        public int Score { get; private set; }

        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }  

        #endregion
    }
}