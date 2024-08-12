using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TowerAsset;
        [SerializeField] private Text m_Text;
        [SerializeField] private Button m_Button;
        [SerializeField] private Transform m_BuildSite;

        public void SetTowerAsset(TowerAsset towerAsset)
        {
            m_TowerAsset = towerAsset;
        }
        private void Start()
        {
            TDPlayer.Instance.GoldUpdateSubscribe(GoldStatusCheck);

            m_Text.text = m_TowerAsset.m_GoldCost.ToString();
            m_Button.GetComponent<Image>().sprite = m_TowerAsset.m_TowerSpriteGUI;
        }

        private void GoldStatusCheck(int gold)
        {
            if(gold >= m_TowerAsset.m_GoldCost != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_Text.color = m_Button.interactable? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            TDPlayer.Instance.TryBuild(m_TowerAsset, m_BuildSite);
            BuildSite.HideControls();
        }

        public void ChangeBuildSite(Transform buildsite)
        {
            m_BuildSite = buildsite;
        }

        private void OnDestroy()
        {
            TDPlayer.Instance.OnGoldUpdate -= GoldStatusCheck;
        }
    }
}