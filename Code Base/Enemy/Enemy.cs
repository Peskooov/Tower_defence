using UnityEngine;
using SpaceShooter;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefence
{
    [RequireComponent(typeof(TDPatrolController))]
    [RequireComponent(typeof(Destructible))]
    public class Enemy : MonoBehaviour
    {
        public enum ArmorType
        {
            Base = 0,
            Magic = 1,
            Fire = 2
        }

        private static Func<int, TDProjectile.DamageType, int, int>[] ArmorDamageFunctions =
        {
            (int power, TDProjectile.DamageType type, int armor) =>
            { //ArmorType.Base
                switch (type)
                {
                    case TDProjectile.DamageType.Magic: return power;
                    default: return Mathf.Max(power-armor,1);
                }
            },

            (int power, TDProjectile.DamageType type, int armor) =>
            { //ArmorType.Magic
              if(TDProjectile.DamageType.Base== type)
                {
                    armor = armor/2;
                }
              return Mathf.Max(power-armor,1);
            },

            (int power, TDProjectile.DamageType type, int armor) =>
            { //ArmorType.Magic
              if(TDProjectile.DamageType.Fire== type)
                {
                    armor = (int)power/(int)armor;
                }
              return Mathf.Max(power-armor,1);
            }
        };

        [SerializeField] private ArmorType m_ArmorType;

        private int m_Damage;
        private int m_Gold;
        private int m_Armor;

        private Destructible m_Destructible;

        private const float offset = 0.33f; //измениить в случае изменения размеров спрайта в префабе

        public event Action OnEnd;

        private void Awake()
        {
            m_Destructible = GetComponent<Destructible>();
        }

        private void OnDestroy()
        {
            OnEnd?.Invoke();
        }

        public void Use(EnemyAsset enemyAsset)
        {
            var sr = GetComponentInChildren<SpriteRenderer>();
            GetComponentInChildren<CircleCollider2D>().radius = enemyAsset.radius * enemyAsset.spriteScale.x * offset;

            sr.color = enemyAsset.color;
            sr.transform.localScale = new Vector3(enemyAsset.spriteScale.x, enemyAsset.spriteScale.y, 1f);

            sr.GetComponent<Animator>().runtimeAnimatorController = enemyAsset.animatorController;

            GetComponent<SpaceShip>().Use(enemyAsset);

            m_Damage = enemyAsset.damage;
            m_Armor = enemyAsset.armor;
            m_ArmorType = enemyAsset.armorType; 
            m_Gold = enemyAsset.gold;
        }

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduseLive(m_Damage);
        }

        public void GivePlayerGold()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
        }

        public void TakeDamage(int damage, TDProjectile.DamageType damageType)
        {
            m_Destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage,damageType,m_Armor));
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(Enemy))]
    public class EnemyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAsset a = EditorGUILayout.ObjectField(null,typeof(EnemyAsset),false) as EnemyAsset;
            
            if (a)
            {
                (target as Enemy).Use(a);
            }
        }
    }
#endif
}