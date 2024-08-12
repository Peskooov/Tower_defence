using SpaceShooter;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class ClickProtection : MonoSingleton<ClickProtection>, IPointerClickHandler
    {
        private Image m_Blocker;
        private Action<Vector2> OnClickAction;

        private void Start()
        {
            m_Blocker = GetComponent<Image>();
            m_Blocker.enabled = false;
        }
        public void Activate(Action<Vector2> mouseAction)
        {
            m_Blocker.enabled = true;
            OnClickAction = mouseAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_Blocker.enabled = false;
            OnClickAction(eventData.pressPosition);
            OnClickAction = null;
        }
    }
}