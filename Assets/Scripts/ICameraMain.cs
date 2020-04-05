using UnityEngine;


namespace Assets.Scripts
{
    /// <summary>
    /// The main camera showing the playing field.
    /// </summary>
    internal interface ICameraMain
    {
        /// <summary>
        /// Gets the Camera component.
        /// </summary>
        /// <value>The Camera component.</value>
        Camera Camera { get; }
    }
}
