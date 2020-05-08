// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;

namespace Henshin.Runtime.Actions.Actor {

/// <summary>
/// Action that changes an actor's pose.
/// </summary>
[ActionControllerType(stateType: typeof(PoseState)), ActionControllerCategory(category: EActionCategory.Actor)]
public class PoseAction: ActorAction {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State of the actor action class.
            /// Holds the index of the edited actor.
            /// </summary>
            public class PoseState: ActorState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Index of the new actor's pose.
                        /// </summary>
                        public int PoseIndex;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state to a <see cref="PoseState"/> object.
            /// </summary>
            public new PoseState State => (PoseState)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="ActionController.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Update the pose.
                ActorView.SetPose(actor: this.Actor, poseIndex: this.State.PoseIndex);
                
                // Finish the action.
                this.Finish();
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
            
                // Load the index.
                this.State.PoseIndex = this.NextSerializedData<int>();
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save the index.
                this.AddSerializedData(data: this.State.PoseIndex);
            }
    // --- /Methods ---
}
}