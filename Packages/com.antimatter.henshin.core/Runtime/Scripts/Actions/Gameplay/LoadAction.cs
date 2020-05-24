// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Runtime.Gameplay;

namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used to load a specified Gameplay.
/// </summary>
[ActionControllerType(stateType: typeof(LoadState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class LoadAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            public class LoadState: ActionState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// The identifier of the gameplay node to load.
                        /// </summary>
                        public string Identifier;
                // --- /Attributes ---
            }
    // ---  SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the <see cref="ActionController.State"/> to a <see cref="LoadState"/>.  
            /// </summary>
            public new LoadState State => (LoadState)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Load the gameplay.
                GameplayController.Load(identifier: this.State.Identifier);
                
                // Finish the action.
                this.Finish();
            }

            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Save the identifier.
                this.AddSerializedData(data: this.State.Identifier);
            }

            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Load the identifier.
                this.State.Identifier = this.NextSerializedData<string>();
        }
    // --- /Methods ---
}
}