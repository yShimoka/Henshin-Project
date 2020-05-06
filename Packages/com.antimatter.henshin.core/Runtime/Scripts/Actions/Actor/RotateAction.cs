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
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="TimedActorAction.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Get the current rotation of the actor.
                this.Original = ActorView.GetAngle(actor: this.Actor);
            }
            
            /// <inheritdoc cref="TimedActorAction.Update"/>
            protected override void Update(float normalizedTime) {
                // Get the interpolated location.
                ActorView.SetAngle(actor: this.Actor, angle: Mathf.Lerp(
                    a: this.Original,
                    b: this.State.Target,
                    t: normalizedTime
                ));
            }
            
            /// <inheritdoc cref="TimedActorAction.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
                
                // Load the target.
                this.State.Target = this.NextSerializedData<float>();
            }
            
            /// <inheritdoc cref="TimedActorAction.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save the target.
                this.AddSerializedData(data: this.State.Target);
            }
    // --- /Methods ---
}
}