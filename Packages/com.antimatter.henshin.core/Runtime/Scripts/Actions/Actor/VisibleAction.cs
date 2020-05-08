// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Directions.Scene;

namespace Henshin.Runtime.Actions.Actor {

/// <summary>
/// Action that toggles the visibility of the specified actor.
/// </summary>
[ActionControllerType(stateType: typeof(VisibleState)), ActionControllerCategory(category: EActionCategory.Actor)]
public class VisibleAction: ActorAction {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State of the actor action class.
            /// Holds the index of the edited actor.
            /// </summary>
            public class VisibleState: ActorState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Flag used to determine if the actor should be rendered or hidden.
                        /// </summary>
                        public bool SetVisible;
                        
                        /// <summary>
                        /// If set, applies the <see cref="SetVisible"/> to all the scene's actors.
                        /// </summary>
                        public bool AllActors;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state to a <see cref="VisibleState"/> object.
            /// </summary>
            public new VisibleState State => (VisibleState)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Check if the AllActors flag is set.
                if (this.State.AllActors) {
                    // Loop through all the scene's actors.
                    foreach (Runtime.Actor.ActorState actorState in SceneState.Current.ActorList) {
                        ActorView.SetVisible(actor: actorState, visible: this.State.SetVisible);
                    }
                } else {
                    // Apply the visible flag to the specified actor.
                    ActorView.SetVisible(actor: this.Actor, visible: this.State.SetVisible);
                }
                
                // Finish the action.
                this.Finish();
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
            
                // Load both flags.
                this.State.SetVisible = this.NextSerializedData<bool>();
                this.State.AllActors = this.NextSerializedData<bool>();
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save both flags.
                this.AddSerializedData(data: this.State.SetVisible);
                this.AddSerializedData(data: this.State.AllActors);
            }
    // --- /Methods ---
}
}