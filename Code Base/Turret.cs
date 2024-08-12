using System.Collections;
using System.Collections.Generic;
using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Турелька корабля. Требует аудио источник для выдачи спецэффекта при стрельбе.
    /// Требует на верхенм уровне скрипт SpaceShip для вычитания патронов и энергии.
    /// </summary>
    public class Turret : MonoBehaviour
    {
        /// <summary>
        /// Тип турели, первичный или вторичный.
        /// </summary>
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        /// <summary>
        /// Текущие патроны в турели.
        /// </summary>
        [SerializeField] private TurretProperties m_TurretProperties;

        /// <summary>
        /// Таймер повторного выстрела.
        /// </summary>
        private float m_RefireTimer;

        /// <summary>
        /// Стрелять можем? 
        /// </summary>
        public bool CanFire => m_RefireTimer <= 0;

        /// <summary>
        /// Кешированная ссылка на родительский шип.
        /// </summary>
        private SpaceShip m_Ship;

        [SerializeField] private UpgradeAsset countTurretUpgrade;
        private int levelOfTurret;

        #region Unity events

        private void Awake()
        {
            if(countTurretUpgrade)
            levelOfTurret = Upgrades.GetUpgradeLevel(countTurretUpgrade);
        }

        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;

            else if (Mode == TurretMode.Auto)
                Fire();
        }

        #endregion

        #region Public API

        /// <summary>
        /// Метод стрельбы турелью. 
        /// </summary>
        public void Fire()
        {
            if (m_RefireTimer > 0)
                return;

            if (m_TurretProperties == null)
                return;

            if (m_Ship)
            {
                // кушаем энергию
                if (!m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage))
                    return;

                // кушаем патроны
                if (!m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage))
                    return;
            }

            CreateProjectille(levelOfTurret);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                // SFX на домашку
            }
        }

        /// <summary>
        /// Установка свойств турели. Будет использовано в дальнейшем для паверапки.
        /// </summary>
        /// <param name="props"></param>
        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode)
                return;

            m_TurretProperties = props;
            m_RefireTimer = 0;
        }

        private void CreateProjectille(int levelBonus)
        {

            // инстанцируем прожектайл который уже сам полетит.
            var projectile = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
            
            if((levelBonus+1)%2 == 1)
            {
                projectile.transform.position = transform.position;

                if(levelBonus == 2)
                {
                    var projectile0 = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
                    var projectile1 = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
                    projectile0.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
                    projectile1.transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);

                    projectile0.transform.up = transform.up;
                    projectile1.transform.up = transform.up;
                }
            }
            else
            {
                var projectile0 = Instantiate(m_TurretProperties.ProjectilePrefab.gameObject).GetComponent<Projectile>();
                projectile0.transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
                projectile.transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);

                projectile0.transform.up = transform.up;
            }
            
            projectile.transform.up = transform.up;

            // метод выставления данных прожектайлу о том кто стрелял для избавления от попаданий в самого себя
            projectile.SetParentShooter(m_Ship);
        }

        #endregion
    }
}