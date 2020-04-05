using System.Collections.Generic;

namespace Assets.Scripts
{
    /// <summary>
    /// Collects and gives access to all units in the scene.
    /// </summary>
    internal interface IUnitsCollector
    {
        /// <summary>
        /// Gets the units by given team.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>The units.</returns>
        IReadOnlyCollection<Unit> GetUnitsByTeam(Team team);
    }
}
