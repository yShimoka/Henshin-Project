// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using System;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Directions.Scene;

namespace Henshin.Runtime.Actions.Actor {

/// <summary>
/// Action that toggles the actors horizontal and vertical flipping.
/// </summary>
[ActionControllerType(stateType: typeof(FlipState)), ActionControllerCategory(category: EActionCategory.Actor)]
public class FlipAction: ActorAction {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State of the actor action class.
            /// Holds the index of the edited actor.
            /// </summary>
            public class FlipState: ActorState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Flag set if the actor should be flipped vertically.
                        /// </summary>
                        public bool Vertical;
                        
                        /// <summary>
                        /// Flag set if the actor should be flipped horizontally.
                        /// </summary>
                        public bool Horizontal;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state to a <see cref="FlipState"/> object.
            /// </summary>
            public new FlipState State => (FlipState)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Apply the flip flags.
                ActorView.SetVerticalFlip(actor: this.Actor, flipped: this.State.Vertical);
                ActorView.SetHorizontalFlip(actor: this.Actor, flipped: this.State.Horizontal);
                
                // Finish the action.
                this.Finish();
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
            
                // Load both flags.
                this.State.Vertical = this.NextSerializedData<bool>();
                this.State.Horizontal = this.NextSerializedData<bool>();
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save both flags.
                this.AddSerializedData(data: this.State.Vertical);
                this.AddSerializedData(data: this.State.Horizontal);
            }
    // --- /Methods ---
}
}