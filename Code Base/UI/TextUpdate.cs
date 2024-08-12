using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource
        {
            Gold,
            Life
        }
        public UpdateSource source = UpdateSource.Gold;

        private Text m_Text;

        private void Start()
        {
            m_Text = GetComponentInChildren<Text>();

            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.Instance.GoldUpdateSubscribe(UpdateText);
                    break;
                case UpdateSource.Life:
                    TDPlayer.Instance.LiveUpdateSubscribe(UpdateText);
                    break;
            }
        }
        private void UpdateText(int count)
        {
            m_Text.text = count.ToString();
        }
        private void OnDestroy()
        {
            switch (source)
            {
                case UpdateSource.Gold:
                    TDPlayer.Instance.OnGoldUpdate -= UpdateText;
                    break;
                case UpdateSource.Life:
                    TDPlayer.Instance.OnLifeUpdate -= UpdateText;
                    break;
            }
        }
    }
}