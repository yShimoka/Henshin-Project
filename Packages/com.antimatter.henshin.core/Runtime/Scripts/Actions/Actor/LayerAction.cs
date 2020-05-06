// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using System;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Directions.Scene;

namespace Henshin.Runtime.Actions.Actor {

/// <summary>
/// Action that toggles the visibility of the specified actor.
/// </summary>
[ActionControllerType(stateType: typeof(LayerState)), ActionControllerCategory(category: EActionCategory.Actor)]
public class LayerAction: ActorAction {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State of the actor action class.
            /// </summary>
            public class LayerState: ActorState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Index of the target layer.
                        /// </summary>
                        public int LayerIndex;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state to a <see cref="LayerState"/> object.
            /// </summary>
            public new LayerState State => (LayerState)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Apply the layer to the actor.
                ActorView.SetLayer(actor: this.Actor, layerId: this.State.LayerIndex);
                
                // Finish the action.
                this.Finish();
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
            
                // Load the layer.
                this.State.LayerIndex = this.NextSerializedData<int>();
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save the layer.
                this.AddSerializedData(data: this.State.LayerIndex);
            }
    // --- /Methods ---
}
}