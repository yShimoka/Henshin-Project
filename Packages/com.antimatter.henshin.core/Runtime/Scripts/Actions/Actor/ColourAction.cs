// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Actor {

/// <summary>
/// Action used to change the actor's colour.
/// </summary>
[ActionControllerType(stateType: typeof(ColourState)), ActionControllerCategory(category: EActionCategory.Actor)]
public class ColourAction: TimedActorAction {
    // ---  SubObjects ---
        // -- Public Classes --
            public class ColourState: TimedActorState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Target color for the actor.
                        /// </summary>
                        public Color Target;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Casts the state to a <see cref="ColourState"/> object.
            /// </summary>
            public new ColourState State => (ColourState)base.State;
            
            /// <summary>
            /// Stores the original colour of the actor.
            /// </summary>
            public Color Original;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="TimedActorAction.Apply"/>
            protected override void Apply() {
                // Call the base method.
                base.Apply();
                
                // Get the current rotation of the actor.
                this.Original = ActorView.GetColour(actor: this.Actor);
            }
            
            /// <inheritdoc cref="TimedActorAction.Update"/>
            protected override void Update(float normalizedTime) {
                // Get the interpolated location.
                ActorView.SetColour(actor: this.Actor, colour: Color.Lerp(
                    a: this.Original,
                    b: this.State.Target,
                    t: normalizedTime
                ));
            }
            
            /// <inheritdoc cref="TimedActorAction.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
                
                // Load the color.
                this.State.Target = new Color(
                    r: this.NextSerializedData<float>(),
                    g: this.NextSerializedData<float>(),
                    b: this.NextSerializedData<float>(),
                    a: this.NextSerializedData<float>()
                );
            }
            
            /// <inheritdoc cref="TimedActorAction.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save the target.
                this.AddSerializedData(data: this.State.Target.r);
                this.AddSerializedData(data: this.State.Target.g);
                this.AddSerializedData(data: this.State.Target.b);
                this.AddSerializedData(data: this.State.Target.a);
            }
    // --- /Methods ---
}
}