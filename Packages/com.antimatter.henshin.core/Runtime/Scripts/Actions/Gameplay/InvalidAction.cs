// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Runtime.Gameplay.Components.Textbox;
using Henshin.Runtime.Gameplay.Modes;
using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used in the <see cref="Comparison"/> gameplay.
/// Defines the root of the invalid transformation.
/// </summary>
[ActionControllerType(stateType: typeof(InvalidState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class InvalidAction: ActionController {
    // ---  SubObjects --
        // -- Public Classes --
            public class InvalidState: ActionState {}
    // --- /SubObjects --
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Store the callback.
                Comparison.InvalidCallback = this._Apply;
                
                // Try to finish the action.
                if (!Comparison.ValidInitialState) {
                    // Apply the transformation.
                    this._Apply();
                }
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {}
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {}
            
        // -- Private Methods --
            private void _Apply() {
                // Find the texts below the invalid selectable.
                foreach (Text text in TextboxState.Instance.InvalidTexts) {
                    text.fontStyle = FontStyle.Bold;
                }
                
                // Show the arrow.
                TextboxState.Instance.ValidArrow.enabled = false;
                TextboxState.Instance.InvalidArrow.enabled = true;
                
                // Finish the action.
                this.Finish();
            }
    // --- /Methods ---
}
}