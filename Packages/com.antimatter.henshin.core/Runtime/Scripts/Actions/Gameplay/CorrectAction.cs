// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Gameplay;
using Henshin.Runtime.Gameplay.Modes;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used to play a specified Gameplay.
/// </summary>
[ActionControllerType(stateType: typeof(CorrectState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class CorrectAction: TimedAction {
    // ---  SubObjects ---
        // -- Public Classes --
            public class CorrectState: TimedState {}
    // ---  SubObjects ---
    
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>
            /// Flag set if the correction is applied.
            /// </summary>
            private bool _mCorrectionApplied;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Check if the current gameplay is a comparison and is correcting.
                if (GameplayState.Own.CurrentMode == "comparison" && Comparison.IsInCorrection) {
                    // Finish the action immediately.
                    this.Finish();
                } else {
                    // Apply the correction.
                    base.Apply();
                }
            }

            /// <inheritdoc cref="TimedAction.Update"/>
            protected override void _Update(float deltaTime) {
                // Increment the timer value.
                this.Timer += deltaTime;
                
                // Get the normalized time.
                float normalized = Mathf.Clamp(
                    value: this.State.Time == 0 ? 1 : this.Timer / this.State.Time, min: 0, max: 1
                );
                
                // If a quarter of the delay has been passed.
                if (normalized > 0.25 && !this._mCorrectionApplied) {
                    // Correct the gameplay.
                    GameplayController.Correct();
                    
                    // Set the flag.
                    this._mCorrectionApplied = true;
                }
                
                // If the action is over.
                if (normalized >= 1 && GameplayState.Own.CurrentMode != "comparison") {
                    // Finish it.
                    this.Finish();
                }
            }
    // --- /Methods ---
}
}