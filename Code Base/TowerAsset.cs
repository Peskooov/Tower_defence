using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public sealed class TowerAsset : ScriptableObject
    {
        public int m_GoldCost;
        public Sprite m_TowerSpriteGUI;
        public Sprite m_TowerSprite;
        public Sprite m_TowerHeadSprite;
        public TurretProperties m_TurretProperties;
        public TowerAsset[] m_UpgradesTo;
        [SerializeField] private UpgradeAsset m_RequiredUpgrade;
        [SerializeField] private int m_RequiredUpdradeLevel;
        public bool IsAvalable() => !m_RequiredUpgrade ||
                                     m_RequiredUpdradeLevel <= Upgrades.GetUpgradeLevel(m_RequiredUpgrade);   
    }
}