using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset asset;
        [SerializeField] private Image upgradeIcon;
        [SerializeField] private Text level, cost, max;
        [SerializeField] private Button buyButton;
        public Button BuyBotton => buyButton;
        private int costNumber;

        public void Initialize()
        {
            upgradeIcon.sprite = asset.sprite;

            var savedLevel = Upgrades.GetUpgradeLevel(asset);

            if (savedLevel >= asset.costByLevel.Length)
            {
                buyButton.interactable = false;

                foreach (Transform child in buyButton.transform)
                {
                    child.gameObject.SetActive(false);
                }
                max.gameObject.SetActive(true);
                costNumber = int.MaxValue;
            }
            else
            {
                max.gameObject.SetActive(false);
                buyButton.interactable = true;

                level.text = "Level: " + (savedLevel + 1);
                costNumber = asset.costByLevel[savedLevel];
                cost.text = costNumber.ToString();
            }
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(asset);
            Initialize();
        }

        public void CheckCost(int money)
        {
            buyButton.interactable = money >= costNumber;
        }
    }
}