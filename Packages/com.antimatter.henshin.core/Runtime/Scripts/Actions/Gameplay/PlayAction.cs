// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Runtime.Gameplay;

namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Starts playing the prepared gameplay.
/// </summary>
[ActionControllerType(stateType: typeof(PlayState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class PlayAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// Stores the state of the play action.
            /// </summary>
            public class PlayState: ActionState {}
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Call the gameplay controller's play method.
                GameplayController.Play(callback: this.Finish);
            }

            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {}

            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {}
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}