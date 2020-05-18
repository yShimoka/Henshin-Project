// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Application;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay {

/// <summary>
/// View class used to render the gameplay elements.
/// Initializes and manipulates the GUI components.
/// </summary>
public static class GameplayView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Initializes the specified <see cref="GameplayState"/>'s view elements.
            /// </summary>
            /// <param name="gameplay">The state instance to initialize.</param>
            /// <exception cref="MissingReferenceException">
            /// There is a prefab reference that is not set in the state object.
            /// </exception>
            /// <exception cref="MissingComponentException">
            /// There is a prefab object that does not have the expected component attached to it.
            /// </exception>
            public static void Initialize(GameplayState gameplay) {
                /*
                // Check if the text box prefab is set.
                if (gameplay.TextboxPrefab == null) {
                    throw new MissingReferenceException(
                        message: "The current gameplay state's textbox prefab is not set."
                    );
                }
                
                // Check if the tool box prefab is set.
                if (gameplay.ToolboxPrefab == null) {
                    throw new MissingReferenceException(
                        message: "The current gameplay state's toolbox instance is not set."
                    );
                }
                
                // Instantiate both objects in the GUI root.
                gameplay.TextboxObject = Object.Instantiate(
                    original: gameplay.TextboxPrefab,
                    parent: ApplicationView.GUI,
                    position: Vector3.zero,
                    rotation: Quaternion.identity
                );
                gameplay.ToolboxObject = Object.Instantiate(
                    original: gameplay.ToolboxPrefab,
                    parent: ApplicationView.GUI,
                    position: Vector3.zero,
                    rotation: Quaternion.identity
                );
                */
            }
        
        // -- Private Methods --
    // --- /Methods ---
}
}