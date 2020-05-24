// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Application;
using Henshin.Runtime.Gameplay.Components.Textbox;
using Henshin.Runtime.Gameplay.Components.Toolbox;

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
            public static void Initialize(GameplayState gameplay) {
                // Create the text box instance.
                TextboxController.Instantiate(parent: ApplicationView.GUI);
                
                // Create the toolbox instance.
                ToolboxController.Instantiate(parent: ApplicationView.GUI);
            }
        
        // -- Private Methods --
    // --- /Methods ---
}
}