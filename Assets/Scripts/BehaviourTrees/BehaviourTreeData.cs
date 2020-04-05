using UnityEngine;

namespace Assets.Scripts.BehaviourTrees
{
    /// <summary>
    /// The base class for BehaviourTrees' datas.
    /// </summary>    
    internal abstract class BehaviourTreeData : ScriptableObject
    {
        /// <summary>
        /// Creates the behaviour tree.
        /// </summary>
        /// <param name="context">The environment context.</param>
        /// <returns></returns>
        public abstract BehaviourTree CreateBehaviourTree(IBehaviourContext context);
    }
}
