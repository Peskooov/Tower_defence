using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_MainButtons;
        [SerializeField] private GameObject m_ApprovedButtons;

        [SerializeField] private Button m_ContinueButton;

        private void Start()
        {
            MainMenuButtonIsEnable();

            m_ContinueButton.interactable = FileHandler.HasFile(MapCompletion.filename);
        }

        public void NewGame()
        {
            if(FileHandler.HasFile(MapCompletion.filename))
            {
                MainMenuButtonIsDisable();
            }
            else
            {
                ApproveAnswer();
            }  
        }
        public void Continue()
        {
            SceneManager.LoadScene(1);
        }
        public void Quit()
        {
            Application.Quit();
        }

        public void ApproveAnswer()
        {
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);
            SceneManager.LoadScene(1);
        }

        public void Back()
        {
            MainMenuButtonIsEnable();
        }

        private void MainMenuButtonIsEnable()
        {        
            m_MainButtons.SetActive(true);
            m_ApprovedButtons.SetActive(false);
        }
        private void MainMenuButtonIsDisable()
        {
            m_MainButtons.SetActive(false);
            m_ApprovedButtons.SetActive(true);
        }
    }
}