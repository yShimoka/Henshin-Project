/*
 * Copyright © 2020 - Zimproov.
 */

using UnityEditor;
using UnityEngine;


/* Wrap the class within the local namespace. */
namespace Henshin.Core.App {

/// <summary>
/// Main manager class used across the entire game.
/// Handles the application through <see cref="State"/> components.
/// </summary>
public static class Manager {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            /// <summary>
            /// Method called after initialization of the <see cref="State"/> resources.
            /// Sets up the game following its current state and specified parameters.
            /// </summary>
            public static void Initialize() {
                Debug.Log(message: "Initializing Manager class.");
                
                // TODO: Load the correct assets.
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}