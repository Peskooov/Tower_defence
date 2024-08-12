using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UIManaUpdate : MonoBehaviour
    {
        [SerializeField] private Image m_ManaFill;

        private void Update()
        {
            m_ManaFill.fillAmount = TDPlayer.Instance.Mana / TDPlayer.Instance.MaxMana;
        }
    }
}