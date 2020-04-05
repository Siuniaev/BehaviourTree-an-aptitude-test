using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class for all unit data, which is set in concrete <see cref="Unit" /> instances.
    /// Provides parameters for the units.
    /// </summary>    
    [CreateAssetMenu(menuName = "Units/Unit", fileName = "New Unit")]
    internal class UnitData : ScriptableObject
    {
        [SerializeField] private string _unitName;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private UnitParameters _unitParameters;
        [SerializeField] private SkillData _skillData;

        /// <summary>
        /// Gets the name of the unit.
        /// </summary>
        /// <value>The name of the unit.</value>
        public string UnitName => _unitName;

        /// <summary>
        /// Gets the unit prefab.
        /// </summary>
        /// <value>The unit prefab.</value>
        public GameObject Prefab => _prefab;

        /// <summary>
        /// Gets the unit parameters.
        /// </summary>
        /// <value>The unit parameters.</value>        
        public UnitParameters UnitParameters => _unitParameters;

        /// <summary>
        /// Gets the skill data.
        /// </summary>
        /// <value>The skill data.</value>
        public SkillData SkillData => _skillData;

        /// <summary>
        /// Checks the correctness of the entered data.
        /// </summary>
        private void OnValidate()
        {
            if (_prefab == null)
                Debug.LogWarning($"There is no {nameof(_prefab)} in {_unitName}");

            if (_unitParameters == null)
                Debug.LogWarning($"There is no {nameof(_unitParameters)} in {_unitName}");

            _unitParameters?.ValidateValues();
        }
    }
}
