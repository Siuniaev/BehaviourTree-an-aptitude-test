using Assets.Scripts.UI;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

namespace Assets.Scripts
{
    /// <summary>
    /// The class of all active game units: character, enemy mob, minion, etc.
    /// </summary>    
    /// <seealso cref="IPoolableObject" />
    /// <seealso cref="IShowableHealth" />
    /// <seealso cref="ITarget" />
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody), typeof(CapsuleCollider))]
    internal class Unit : MonoBehaviour, IPoolableObject, IShowableHealth, ITarget
    {
        public const float DEFAULT_HEALTHBAR_HEIGHT_OFFSET = 50f;
        public const float DEFAULT_HEALTHBAR_WIDTH = 50f;
        public const float ATTACK_RECHARGE_MIN = 0f;
        public const float ATTACK_RECHARGE_FULL = 1f;
        public const int DAMAGE_TAKEN_MIN = 1;
        public const float DEFAULT_FEAR_DISTANCE = 4f;
        public const float DEFAULT_AGENT_ACCELERATION = 100f;
        public const float ROTATION_SPEED_MULTIPLIER = 2f;
        public const float MELEE_ATTACK_RANGE_MAX = 2f;
        public const float ALMOST_DEAD_HEALTHMAX_DEVIDER = 3f;

        private NavMeshAgent _agent;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private CapsuleCollider _collider;
        private Random _randomMissing;
        private Random _randomCritical;
        private ITarget _target;
        private IUnitBehaviour _behaviour;
        private int _usedUnitDataInstanceId;
        private float _attackRecharge = ATTACK_RECHARGE_FULL; // Ready.     
        private Transform _shotPoint;
        private int _healPoints;

        /// <summary>
        /// Occurs when this unit has dead.
        /// </summary>
        public event Action<Unit> OnUnitDie;

        /// <summary>
        /// Occurs when the attached Component is destroying as <see cref="IPoolableObject" />.
        /// </summary>
        public event Action<Component> OnDestroyAsPoolableObject;

        /// <summary>
        /// Occurs when health has changed.
        /// </summary>
        public event Action<ChangedHealthArgs> OnHealthChanges;

        /// <summary>
        /// Occurs when health for this instance does not need to be shown.
        /// </summary>
        public event Action<IShowableHealth> OnHealthStopShowing;

        /// <summary>
        /// Gets the unit position.
        /// </summary>
        /// <value>The position.</value>
        public Vector3 Position => _collider.bounds.center;

        /// <summary>
        /// Gets a value indicating whether this instance is alive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
        /// </value>
        public bool IsAlive => HealPoints > 0;

        /// <summary>
        /// Gets a value indicating whether this instance is ready to attack.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready to attack; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyToAttack => _attackRecharge >= ATTACK_RECHARGE_FULL;

        /// <summary>
        /// Gets a value indicating whether this instance is ready to use power.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready to use power; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyToUsePower => PowerPoints == Parameters.PowerPointsMax;

        /// <summary>
        /// Gets a value indicating whether this instance is almost dead.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is almost dead; otherwise, <c>false</c>.
        /// </value>
        public bool IsAlmostDead => HealPoints < Parameters.HealPointsMax / ALMOST_DEAD_HEALTHMAX_DEVIDER;

        /// <summary>
        /// Gets or sets the prefab instance identifier.
        /// </summary>
        /// <value>The prefab instance identifier.</value>
        public int PrefabInstanceID { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the unit parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public UnitParameters Parameters { get; private set; }

        /// <summary>
        /// Gets the unit team.
        /// </summary>
        /// <value>The team.</value>
        public Team Team { get; private set; }

        /// <summary>
        /// Gets the unit's power points.
        /// </summary>
        /// <value>The power points.</value>
        public int PowerPoints { get; private set; }

        /// <summary>
        /// Gets the unit's skill data.
        /// </summary>
        /// <value>The skill data.</value>
        public SkillData SkillData { get; private set; }

        /// <summary>
        /// Gets the unit's fear distance.
        /// </summary>
        /// <value>The fear distance.</value>
        public float FearDistance { get; private set; } = DEFAULT_FEAR_DISTANCE;

        /// <summary>
        /// Gets or sets the heal points.
        /// </summary>
        /// <value>The heal points.</value>
        public int HealPoints
        {
            get => _healPoints;
            private set
            {
                _healPoints = value;
                _healPoints = Mathf.Min(_healPoints, Parameters.HealPointsMax);
            }
        }

        /// <summary>
        /// Gets the health bar position.
        /// </summary>
        /// <value>The health bar position.</value>
        public Vector3? HealthBarPosition => Position;

        /// <summary>
        /// Gets the health bar height offset.
        /// </summary>
        /// <value>The health bar height offset.</value>
        public float HealthBarHeightOffset => DEFAULT_HEALTHBAR_HEIGHT_OFFSET;

        /// <summary>
        /// Gets the width of the health bar.
        /// </summary>
        /// <value>The width of the health bar.</value>
        public float HealthBarWidth => DEFAULT_HEALTHBAR_WIDTH;

        /// <summary>
        /// Is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
            _animator = GetComponentInChildren<Animator>();
            _shotPoint = GetComponentInChildren<ShootPoint>()?.transform ?? transform;
        }

        /// <summary>
        /// Is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            _agent.acceleration = DEFAULT_AGENT_ACCELERATION;
            _rigidbody.isKinematic = true;
            CreateRandomInstances();
            CheckCriticalReferences();
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            _behaviour.ApplyBehaviour(this);
            RechargeAttack();
        }

        /// <summary>
        /// Sets the behaviour to control this unit.
        /// </summary>
        /// <param name="behaviour">The behaviour.</param>
        /// <exception cref="System.ArgumentNullException">behaviour</exception>
        public void SetBehaviour(IUnitBehaviour behaviour)
        {
            StopAgent();
            _behaviour = behaviour ?? throw new ArgumentNullException(nameof(behaviour));
        }

        /// <summary>
        /// Sets the unit data.
        /// </summary>
        /// <param name="data">The unit data.</param>
        /// <exception cref="System.ArgumentNullException">data</exception>
        /// <exception cref="System.NullReferenceException">UnitParameters</exception>
        public void SetUnitData(UnitData data)
        {
            _usedUnitDataInstanceId = data?.GetInstanceID() ?? throw new ArgumentNullException(nameof(data));
            Parameters = data?.UnitParameters ?? throw new NullReferenceException(nameof(data.UnitParameters));
            Name = data.UnitName;
            SkillData = data.SkillData;
        }

        /// <summary>
        /// Sets the team.
        /// </summary>
        /// <param name="team">The team.</param>
        public void SetTeam(Team team) => Team = team;

        /// <summary>
        /// Returns this object to the object pool.
        /// </summary>
        public void DestroyAsPoolableObject()
        {
            OnHealthStopShowing?.Invoke(this);
            OnDestroyAsPoolableObject?.Invoke(this);
        }

        /// <summary>
        /// Determines whether the same unit data is already set as specified.
        /// </summary>
        /// <param name="data">The unit data.</param>
        /// <returns>
        ///   <c>true</c> if the same unit data is already set as specified; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSameUnitDataAlreadySet(UnitData comparableData)
        {
            if (comparableData == null)
                throw new ArgumentNullException(nameof(comparableData));

            return _usedUnitDataInstanceId == comparableData.GetInstanceID();
        }

        /// <summary>
        /// Determines whether this instance can attack target.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can attack target; otherwise, <c>false</c>.
        /// </returns>
        public bool CanAttackTarget()
        {
            if (_target == null || !_target.IsAlive)
                return false;

            if (Parameters.AttackRange > MELEE_ATTACK_RANGE_MAX)
            {
                var shotDirection = _target.Position - _shotPoint.position;
                if (Physics.Raycast(_shotPoint.position, shotDirection, out var hit, shotDirection.magnitude))
                    return hit.collider.GetComponent<ITarget>() == _target;
            }

            return true;
        }

        /// <summary>
        /// Called when this unit has spawned and have to be updated.
        /// </summary>
        public void OnSpawnedUpdate()
        {
            _agent.enabled = true;
            UpdateParameters();
            _animator?.SetBool("isMoving", false);
            _animator?.SetBool("isDancing", false);
        }

        /// <summary>
        /// Checks the point for reachability.
        /// </summary>
        /// <param name="targetPoint">The target point.</param>
        /// <returns>
        ///   <c>true</c> if the given point is reachability for this unit; otherwise, <c>false</c>.
        /// </returns>
        public bool CheckPointForReachability(Vector3 targetPoint)
        {
            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(targetPoint, path);

            return path.status == NavMeshPathStatus.PathComplete;
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">target</exception>
        public void SetTarget(ITarget target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <summary>
        /// Resets the target.
        /// </summary>
        public void ResetTarget()
        {
            if (_target == null)
                return;

            _target = null;
            _animator?.SetBool("isMoving", false);
            StopAgent();
        }

        /// <summary>
        /// Make the unit dance.
        /// </summary>
        public void Dance()
        {
            _animator?.SetBool("isMoving", false);
            _animator?.SetBool("isDancing", true);
        }

        /// <summary>
        /// Moves unit to the specified target.
        /// </summary>
        public void MoveToTarget()
        {
            if (!IsAlive)
                return;

            if (_target != null)
            {
                _agent.SetDestination(_target.Position);
                _agent.isStopped = false;
            }

            _animator?.SetBool("isMoving", _agent.velocity.magnitude > 0f);
        }

        /// <summary>
        /// Rotates unit to the specified target.
        /// </summary>
        /// <returns></returns>
        public bool LookAtTarget()
        {
            if (_target == null || ReferenceEquals(_target, this))
                return true;

            StopAgent();

            var targetPos = _target.Position;
            var myPos = Position;
            targetPos.y = myPos.y = 0; // Height alignment;

            var targetRot = Quaternion.LookRotation(targetPos - myPos, Vector3.up);
            var isSeeTarget = Quaternion.Angle(transform.rotation, targetRot) <= 10f;

            _animator?.SetBool("isMoving", !isSeeTarget);

            if (!isSeeTarget)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
                    Parameters.Speed * ROTATION_SPEED_MULTIPLIER * Time.deltaTime);

            return isSeeTarget;
        }

        /// <summary>
        /// Unit attacks the specified target.
        /// </summary>
        public void AttackTarget()
        {
            if (_target == null || !IsReadyToAttack)
                return;

            _animator?.SetTrigger("isAttack");

            _attackRecharge = ATTACK_RECHARGE_MIN;
            var damage = Parameters.Attack;

            var missing = _randomMissing.NextDouble();
            if (missing <= Parameters.ChanceToMiss)
            {
                //Debug.Log($"{Name}: Miss!");
                return;
            }

            var critical = _randomCritical.NextDouble();
            if (critical <= Parameters.ChanceToCritical)
            {
                //Debug.Log($"{Name}: Critical damage!");
                damage *= 2;
            }

            _target.ApplyDamage(damage);
            PowerPoints++;
        }

        /// <summary>
        /// Uses the skill on the specified target.
        /// </summary>
        public void UseSkillOnTarget()
        {
            if (_target == null || SkillData == null || !IsReadyToUsePower)
                return;

            SkillData.UseSkillOn(_target);
            PowerPoints = UnitParameters.POWER_MIN;
            _animator?.SetTrigger("isPower");

            Debug.Log($"{Name} used {SkillData.name} skill on {(_target as Unit)?.Name}!");
        }

        /// <summary>
        /// Applies the damage to this unit.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">damage - Must be greater than or equal to 0.</exception>
        public void ApplyDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Must be greater than or equal to 0.");

            if (!IsAlive)
                return;

            var oldHealth = HealPoints;
            HealPoints -= GetDamageReducedByArmor(damage);
            PowerPoints++;

            if (!IsAlive)
                Die();

            UpdateShowableHealth((HealPoints - oldHealth).ToString("+#;-#;0"));
            _animator?.SetTrigger("isDamage");
        }

        /// <summary>
        /// Applies the heal to this unit.
        /// </summary>
        /// <param name="healValue">The heal value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">healValue - Must be greater than or equal to 0.</exception>
        public void ApplyHeal(int healValue)
        {
            if (healValue < 0)
                throw new ArgumentOutOfRangeException(nameof(healValue), "Must be greater than or equal to 0.");

            var oldHealth = HealPoints;
            HealPoints += healValue;
            UpdateShowableHealth((HealPoints - oldHealth).ToString("+#;-#;0"));
            // TODO healing particles.
        }

        /// <summary>
        /// Dies this unit instance.
        /// </summary>
        public void Die()
        {
            _agent.enabled = false;
            OnUnitDie?.Invoke(this);
            DestroyAsPoolableObject();
        }

        /// <summary>
        /// Stops the unit's navmesh agent.
        /// </summary>
        private void StopAgent()
        {
            if (_agent.isActiveAndEnabled && !_agent.isStopped)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
        }

        /// <summary>
        /// Updates the unit parameters.
        /// </summary>
        private void UpdateParameters()
        {
            HealPoints = Parameters.HealPointsMax;
            PowerPoints = UnitParameters.POWER_MIN;
            _agent.speed = Parameters.Speed;
        }

        /// <summary>
        /// Recharges the attack.
        /// </summary>
        private void RechargeAttack()
        {
            if (_attackRecharge < ATTACK_RECHARGE_FULL)
                _attackRecharge += Parameters.AttackSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Updates the showable health in the UI.
        /// </summary>
        private void UpdateShowableHealth(string message = null)
        {
            var changedHealth = new ChangedHealthArgs(
               currentHealth: HealPoints,
               fullness: HealPoints > 0 ? (float)HealPoints / Parameters.HealPointsMax : 0,
               description: message
               );

            OnHealthChanges?.Invoke(changedHealth);
        }

        /// <summary>
        /// Gets the damage value reduced by this unit armor.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <returns>The redused damage value.</returns>
        private int GetDamageReducedByArmor(float damage)
        {
            int damageResult = (int)damage;

            if (Parameters.Defence > 0)
                damageResult = (int)(damage - damage * Parameters.Defence);

            damageResult = Mathf.Max(DAMAGE_TAKEN_MIN, damageResult);

            return damageResult;
        }

        /// <summary>
        /// Checks the critical references in this instance.
        /// </summary>
        /// <exception cref="System.NullReferenceException">
        /// Parameters
        /// or
        /// _randomMissing
        /// or
        /// _randomCritical
        /// or
        /// _behaviour
        /// </exception>
        private void CheckCriticalReferences()
        {
            if (Parameters == null)
                throw new NullReferenceException(nameof(Parameters));

            if (_randomMissing == null)
                throw new NullReferenceException(nameof(_randomMissing));

            if (_randomCritical == null)
                throw new NullReferenceException(nameof(_randomCritical));

            if (_behaviour == null)
                throw new NullReferenceException(nameof(_behaviour));
        }

        /// <summary>
        /// Creates the random instances for missing and critical attacks.
        /// </summary>
        private void CreateRandomInstances()
        {
            unchecked
            {
                var seed = (int)DateTime.Now.Ticks;
                _randomMissing = new Random(seed);
                _randomCritical = new Random(seed * GetHashCode());
            }
        }
    }
}
