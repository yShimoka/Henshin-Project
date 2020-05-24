// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using Henshin.Runtime.Gameplay;
using Henshin.Runtime.Gameplay.Modes;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used to play a specified Gameplay.
/// </summary>
[ActionControllerType(stateType: typeof(PlayState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class PlayAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            public class PlayState: ActionState {
                public PlayState() { this.WaitForAllParents = false; }
            }
    // ---  SubObjects ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Check if the current gameplay is a comparison and is correcting.
                if (GameplayState.Own.CurrentMode == "comparison" && Comparison.IsInCorrection) {
                    // Finish the action immediately.
                    this.Finish();
                } else {
                    // Play the gameplay.
                    GameplayController.Play(callback: this.Finish);
                }
            }

            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {}

            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {}
    // --- /Methods ---
}
}