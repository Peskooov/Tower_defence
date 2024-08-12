using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int m_Money;
        [SerializeField] private Text m_MoneyText;
        [SerializeField] private BuyUpgrade[] m_Upgrades;
        private void Start()
        {
            foreach (var upgrade in m_Upgrades)
            {
                upgrade.Initialize();
                upgrade.BuyBotton.onClick.AddListener(UpdateMoney);
            }

            UpdateMoney();
        }

        private void UpdateMoney()
        {
            m_Money = MapCompletion.Instance.TotalScore;
            m_Money -= Upgrades.GetTotalCost();

            m_MoneyText.text = m_Money.ToString();

            foreach (var upgrade in m_Upgrades)
            {
                upgrade.CheckCost(m_Money);
            }
        }
        private void OnDestroy()
        {
            foreach (var upgrade in m_Upgrades)
            {
                upgrade.BuyBotton.onClick.RemoveAllListeners();
            }
        }
    }
}