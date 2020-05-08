// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Actor {

/// <summary>
/// Action used to move an actor to a specified location.
/// </summary>
[ActionControllerType(stateType: typeof(MoveToState)), ActionControllerCategory(category: EActionCategory.Actor)]
public class MoveToAction: TimedActorAction {
    // ---  SubObjects ---
        // -- Public Classes --
            public class MoveToState: TimedActorState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Target of the movement.
                        /// </summary>
                        public Vector2 Target;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state to a <see cref="MoveToState"/> object.
            /// </summary>
            public new MoveToState State => (MoveToState)base.State;
            
            /// <summary>
            /// Stores the original position of the actor.
            /// </summary>
            public Vector2 Original;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="TimedActorAction.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Get the current position of the actor.
                this.Original = ActorView.GetPosition(actor: this.Actor);
            }
            
            /// <inheritdoc cref="TimedActorAction.Update"/>
            protected override void Update(float normalizedTime) {
                // Get the interpolated location.
                ActorView.SetPosition(actor: this.Actor, position: Vector2.Lerp(
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
                this.State.Target = new Vector2(
                    x: this.NextSerializedData<float>(),
                    y: this.NextSerializedData<float>()
                );
            }
            
            /// <inheritdoc cref="TimedActorAction.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save the target.
                this.AddSerializedData(data: this.State.Target.x);
                this.AddSerializedData(data: this.State.Target.y);
            }
    // --- /Methods ---
}
}