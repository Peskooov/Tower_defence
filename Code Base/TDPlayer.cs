using UnityEngine;
using SpaceShooter;
using System;
using UnityEngine.UIElements;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        [SerializeField] private Tower m_TowerPrefab;
        [SerializeField] private int m_Gold;
        [SerializeField] private float m_Mana;
        public static new TDPlayer Instance => Player.Instance as TDPlayer;

        public event Action<int> OnGoldUpdate;
        public event Action<int> OnLifeUpdate;

        private float m_MaxMana;

        public int Gold =>m_Gold;
        public float Mana =>m_Mana;
        public float MaxMana => m_MaxMana;

        public void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            act(Instance.m_Gold);
        }

        public void LiveUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            act(Instance.Numlives);
        }

        public void ChangeGold(int change)
        {
            m_Gold += change;
            OnGoldUpdate?.Invoke(m_Gold);
        }
        public void ReduseLive(int change)
        {
            TakeDamage(change);
            OnLifeUpdate?.Invoke(Numlives);
        }

        public void ChangeMana(int change)
        {
            m_Mana += change;
        }

        private void FillingMana()
        {
            if (m_Mana < m_MaxMana)
                m_Mana += Time.deltaTime;
            else
            {
                m_Mana = m_MaxMana;
            }
        }

        public void TryBuild(TowerAsset m_TowerAsset, Transform m_BuildSite)
        {
            ChangeGold(-m_TowerAsset.m_GoldCost);

            var tower = Instantiate(m_TowerPrefab, m_BuildSite.position, Quaternion.identity);
            tower.Use(m_TowerAsset);

            Destroy(m_BuildSite.gameObject);
        }

        [SerializeField] private UpgradeAsset healthUpgrade;
        
        private void Start()
        {
            m_MaxMana = m_Mana;

            var levelHealth = Upgrades.GetUpgradeLevel(healthUpgrade);
            TakeDamage(-levelHealth);
        }

        private void Update()
        {           
            FillingMana();
        }
    }
}