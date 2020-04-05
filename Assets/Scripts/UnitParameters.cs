using System;
using UnityEngine;


namespace Assets.Scripts
{
    /// <summary>
    /// The base structure of parameters for all <see cref="Unit" /> classes in the game.
    /// </summary>
    [Serializable]
    internal class UnitParameters
    {
        public const float ATTACK_MIN = 0f;
        public const float DEFENCE_MIN = 0f;
        public const float DEFENCE_MAX = 1f;
        public const int HEALTH_MIN = 1;
        public const float SPEED_MIN = 0f;
        public const float ATTACK_SPEED_MIN = 0f;
        public const float ATTACK_RANGE_MIN = 0f;
        public const int POWER_MIN = 0;
        public const float CHANCE_MIN = 0f;
        public const float CHANCE_MAX = 1f;

        [SerializeField] private float _attack;
        [SerializeField] private float _defence;
        [SerializeField] private int _healPointsMax;
        [SerializeField] private float _speed;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _attackRange;
        [SerializeField] private int _powerPointsMax;
        [SerializeField] private float _chanceToMiss;
        [SerializeField] private float _chanceToCritical;

        /// <summary>
        /// Gets the attack.
        /// </summary>
        /// <value>The attack.</value>
        public float Attack => _attack;

        /// <summary>
        /// Gets the armor.
        /// </summary>
        /// <value>The armor.</value>
        public float Defence => _defence;

        /// <summary>
        /// Gets the heal points maximum limit.
        /// </summary>
        /// <value>The heal points maximum.</value>
        public int HealPointsMax => _healPointsMax;

        /// <summary>
        /// Gets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public float Speed => _speed;

        /// <summary>
        /// Gets the attack speed.
        /// </summary>
        /// <value>The attack speed.</value>
        public float AttackSpeed => _attackSpeed;

        /// <summary>
        /// Gets the attack range.
        /// </summary>
        /// <value>The attack range.</value>
        public float AttackRange => _attackRange;

        /// <summary>
        /// Gets the power points maximum limit.
        /// </summary>
        /// <value>The power points maximum.</value>
        public int PowerPointsMax => _powerPointsMax;

        /// <summary>
        /// Gets the chance to miss.
        /// </summary>
        /// <value>The chance to miss.</value>
        public float ChanceToMiss => _chanceToMiss;

        /// <summary>
        /// Gets the chance to critical.
        /// </summary>
        /// <value>The chance to critical.</value>
        public float ChanceToCritical => _chanceToCritical;

        /// <summary>
        /// Checks the correctness of the entered data.        
        /// </summary>
        public void ValidateValues()
        {
            _attack = Mathf.Max(ATTACK_MIN, _attack);
            _defence = Mathf.Clamp(_defence, DEFENCE_MIN, DEFENCE_MAX);
            _healPointsMax = Mathf.Max(HEALTH_MIN, _healPointsMax);
            _speed = Mathf.Max(SPEED_MIN, _speed);
            _attackSpeed = Mathf.Max(ATTACK_SPEED_MIN, _attackSpeed);
            _attackRange = Mathf.Max(ATTACK_RANGE_MIN, _attackRange);
            _powerPointsMax = Math.Max(POWER_MIN, _powerPointsMax);
            _chanceToMiss = Mathf.Clamp(_chanceToMiss, CHANCE_MIN, CHANCE_MAX);
            _chanceToCritical = Mathf.Clamp(_chanceToCritical, CHANCE_MIN, CHANCE_MAX);
        }
    }
}
