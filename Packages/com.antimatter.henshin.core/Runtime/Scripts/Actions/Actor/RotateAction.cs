// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;
using Henshin.Runtime.Libraries;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Actor {

/// <summary>
/// Action used to rotate an actor around the z axis.
/// </summary>
[ActionControllerType(stateType: typeof(RotateState)), ActionControllerCategory(category: EActionCategory.Actor)]
public class RotateAction: TimedActorAction {
    // ---  SubObjects ---
        // -- Public Classes --
            public class RotateState: TimedActorState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Target of the rotation.
                        /// </summary>
                        public float Target;
                        
                        /// <summary>
                        /// If the rotation should be relative.
                        /// </summary>
                        public bool Relative;
                        
                        /// <summary>
                        /// If the rotation should be clockwise.
                        /// Used only for absolute rotations.
                        /// </summary>
                        public bool Clockwise;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state to a <see cref="RotateState"/> object.
            /// </summary>
            public new RotateState State => (RotateState)base.State;
            
            /// <summary>
            /// Stores the original rotation of the actor.
            /// </summary>
            public float Original;
            
            /// <summary>
            /// Stores the offset that should be applied to the actor.
            /// </summary>
            public float Offset;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="TimedActorAction.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Get the current rotation of the actor.
                this.Original = ActorView.GetAngle(actor: this.Actor);
                
                // Get the actor offset.
                if (this.State.Relative) {
                    this.Offset = this.State.Target;
                } else {
                    // Get the value of the rotation.
                    this.Offset = this.Original - this.State.Target;
                    
                    // Check the rotation mode.
                    if (this.Offset < 0 && this.State.Clockwise) {
                        this.Offset = 360 + this.Offset;
                    } else if (this.Offset > 0 && !this.State.Clockwise) {
                        this.Offset = 360 - this.Offset;
                    }
                    
                }
            }
            
            /// <inheritdoc cref="TimedActorAction.Update"/>
            protected override void Update(float normalizedTime) {
                // Rotate the actor.
                ActorView.SetAngle(actor: this.Actor, angle: this.Original + this.Offset * normalizedTime);
            }
            
            /// <inheritdoc cref="TimedActorAction.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
                
                // Load the target.
                this.State.Target = this.NextSerializedData<float>();
                this.State.Relative = this.NextSerializedData<bool>();
                this.State.Clockwise = this.NextSerializedData<bool>();
            }
            
            /// <inheritdoc cref="TimedActorAction.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save the target.
                this.AddSerializedData(data: this.State.Target);
                this.AddSerializedData(data: this.State.Relative);
                this.AddSerializedData(data: this.State.Clockwise);
            }
    // --- /Methods ---
}
}