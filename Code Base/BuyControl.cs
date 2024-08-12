using UnityEngine;
using System.Collections.Generic;

namespace TowerDefence
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyPrefab;

        private List<TowerBuyControl> m_ActiveControl;
        private RectTransform m_RectTransform;
        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();

            BuildSite.OnClickEvent += MoveToBuildSite;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            BuildSite.OnClickEvent -= MoveToBuildSite;
        }

        private void MoveToBuildSite(BuildSite buildSite)
        {
            if (buildSite != null)
            {
                var position = Camera.main.WorldToScreenPoint(buildSite.transform.root.position);

                m_RectTransform.anchoredPosition = position;


                m_ActiveControl = new List<TowerBuyControl>();
                foreach (var asset in buildSite.buildableTowers)
                {
                    if (asset.IsAvalable())
                    {
                        var newControl = Instantiate(m_TowerBuyPrefab, transform);
                        m_ActiveControl.Add(newControl);
                        newControl.SetTowerAsset(asset);
                    }
                }

                if (m_ActiveControl.Count > 0)
                {
                    gameObject.SetActive(true);

                    var angle = 360 / m_ActiveControl.Count;
                    for (int i = 0; i < m_ActiveControl.Count; i++)
                    {
                        var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.up * 80);
                        m_ActiveControl[i].transform.position += offset;
                    }
                    foreach (var towerByControl in GetComponentsInChildren<TowerBuyControl>())
                    {
                        towerByControl.ChangeBuildSite(buildSite.transform.root);
                    }
                }
            }
            else
            {
                if(m_ActiveControl != null)
                {
                    foreach (var control in m_ActiveControl) Destroy(control.gameObject);
                    m_ActiveControl.Clear();
                }
 
                gameObject.SetActive(false);
            }
        }
    }
}