// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Gameplay;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Gameplay {

/// <summary>
/// Action used to prepare the specified gameplay.
/// </summary>
[ActionControllerType(stateType: typeof(PrepareState)), ActionControllerCategory(category: EActionCategory.Gameplay)]
public class PrepareAction: ActionController {
    // ---  SubObjects ---
         // -- Public Classes --
            /// <summary>
            /// State class used for the prepare actions.
            /// </summary>
            public class PrepareState: ActionState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Stores the gameplay mode to use.
                        /// </summary>
                        public GameplayState.EGameplayMode GameplayMode;
                        
                        /// <summary>
                        /// Index of the gameplay in the xml file.
                        /// </summary>
                        public int GameplayIndex;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state variable to a <see cref="PrepareState"/> object.
            /// </summary>
            public new PrepareState State => (PrepareState)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Load the specified gameplay mode.
                GameplayController.PrepareMode(mode: this.State.GameplayMode, gameplayIndex: this.State.GameplayIndex);
                
                // Finish the action.
                this.Finish();
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Save the gameplay mode and index.
                this.AddSerializedData(data: this.State.GameplayMode);
                this.AddSerializedData(data: this.State.GameplayIndex);
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Load the gameplay mode and index.
                this.State.GameplayMode  = this.NextSerializedData<GameplayState.EGameplayMode>();
                this.State.GameplayIndex = this.NextSerializedData<int>();
            }
        // -- Private Methods --
    // --- /Methods ---
}
}