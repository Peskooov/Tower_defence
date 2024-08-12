using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;
using System;

namespace TowerDefence
{
    public class Abilities : MonoSingleton<Abilities>
    {
        [Serializable]
        public class FireAbility
        {
            [SerializeField] private int m_Cost;
            [SerializeField] private int m_Mana;
            [SerializeField] private int m_Damage;

            public void Use()
            {
                TDPlayer.Instance.ChangeGold(-m_Cost);
                TDPlayer.Instance.ChangeMana(-m_Mana);
                ClickProtection.Instance.Activate((Vector2 v) =>
                {
                    Vector3 position = v;
                    position.z = -Camera.main.transform.position.z;
                    position = Camera.main.ScreenToWorldPoint(position);

                    foreach (var collider in Physics2D.OverlapCircleAll(position, 5))
                    {
                        if (collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                        {
                            enemy.TakeDamage(m_Damage, TDProjectile.DamageType.Magic);
                        }
                    }
                });
            }

            public void AddUpgrade(int bonus)
            {
                m_Cost *= bonus;
                m_Mana += bonus;
                m_Damage *= bonus;
            }

            public void UpdateText()
            {
                Instance.m_FireText.text = m_Cost.ToString();
                Instance.m_FireManaText.text = m_Mana.ToString();
            }

            public void CheckResources()
            {
                if (TDPlayer.Instance.Gold < m_Cost || TDPlayer.Instance.Mana < m_Mana)
                    Instance.FireButtonOff();
                else
                    Instance.FireButtonOn();
            }
        }

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private int m_Cost;
            [SerializeField] private int m_Mana;
            [SerializeField] private float m_Cooldown;
            [SerializeField] private float m_Duration;

            private bool IsCoroutine = false;

            public void Use()
            {
                TDPlayer.Instance.ChangeGold(-m_Cost);
                TDPlayer.Instance.ChangeMana(-m_Mana);
                foreach (var ship in FindObjectsOfType<SpaceShip>())
                    ship.HalfMaxLeniarVelocity();

                EnemyWavesManager.OnEnemySpawn += Slow;
                Instance.StartCoroutine(Restore());
                Instance.StartCoroutine(TimeAbilityButton());
            }

            private void Slow(Enemy ship)
            {
                ship.GetComponent<SpaceShip>().HalfMaxLeniarVelocity();
            }

            IEnumerator Restore()
            {
                yield return new WaitForSeconds(m_Duration);
                foreach (var ship in FindObjectsOfType<SpaceShip>())
                    ship.RestoreMaxLeniarVelocity();

                EnemyWavesManager.OnEnemySpawn -= Slow;
            }

            IEnumerator TimeAbilityButton()
            {
                Instance.TimeButtonOff();
                IsCoroutine = true;
                yield return new WaitForSeconds(m_Cooldown);
                Instance.TimeButtonOn();
                IsCoroutine = false;
            }

            public void AddUpgrade(int bonus)
            {
                m_Cost *= bonus;
                m_Mana += bonus;
                m_Duration *= bonus;
                m_Cooldown *= bonus;
            }

            public void UpdateText()
            {
                Instance.m_SlowText.text = m_Cost.ToString();
                Instance.m_SlowManaText.text = m_Mana.ToString();
            }

            public void CheckResources()
            {
                if (TDPlayer.Instance.Gold < m_Cost || TDPlayer.Instance.Mana < m_Mana || IsCoroutine)
                    Instance.TimeButtonOff();
                else
                    Instance.TimeButtonOn();
            }
        }

        [SerializeField] private FireAbility m_FireAbility;
        [SerializeField] private TimeAbility m_TimeAbility;

        [SerializeField] private Button m_FireButton;
        [SerializeField] private Button m_TimeButton;

        [SerializeField] private Image m_TargetCircle;

        [SerializeField] private UpgradeAsset m_FireUpgrade;
        [SerializeField] private UpgradeAsset m_SlowUpgrade;

        [SerializeField] private GameObject m_FireGameObject;
        [SerializeField] private GameObject m_SlowGameObject;

        [SerializeField] private Text m_FireText;
        [SerializeField] private Text m_SlowText;
        [SerializeField] private Text m_FireManaText;
        [SerializeField] private Text m_SlowManaText;

        [SerializeField] private GameObject m_ManaBar;

        public void UseFireAbility() => m_FireAbility.Use();
        public void UseTimeAbility() => m_TimeAbility.Use();

        private void Start()
        {
            var levelFireUpgrade = Upgrades.GetUpgradeLevel(m_FireUpgrade);
            var levelSlowUpgrade = Upgrades.GetUpgradeLevel(m_SlowUpgrade);

            if (levelFireUpgrade < 1)
                m_FireGameObject.SetActive(false);
            if (levelSlowUpgrade < 1)
                m_SlowGameObject.SetActive(false);

            if (levelFireUpgrade < 1 || levelSlowUpgrade < 1)
                m_ManaBar.SetActive(false);

            m_FireAbility.AddUpgrade(levelFireUpgrade);
            m_TimeAbility.AddUpgrade(levelSlowUpgrade);

            m_FireAbility.UpdateText();
            m_TimeAbility.UpdateText();
        }

        private void Update()
        {
            m_FireAbility.CheckResources();
            m_TimeAbility.CheckResources();
        }

        public void ChangeText(Text text)
        {
            m_FireText.text = text.ToString();
        }

        public void FireButtonOn() { m_FireButton.interactable = true; }
        public void FireButtonOff() { m_FireButton.interactable = false; }

        public void TimeButtonOn() { m_TimeButton.interactable = true; }
        public void TimeButtonOff() { m_TimeButton.interactable = false; }

    }
}