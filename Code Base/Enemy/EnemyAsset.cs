using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public sealed class EnemyAsset : ScriptableObject
    {
        [Header("View")]
        public Color color = Color.white;
        public Vector2 spriteScale = new Vector2(3, 3);
        public RuntimeAnimatorController animatorController;

        [Header("Game Settings")]
        public float moveSpeed;
        public int hitPoints;
        public int armor;
        public Enemy.ArmorType armorType;
        public int score;
        public float radius;
        public int damage;
        public int gold;
    }
}