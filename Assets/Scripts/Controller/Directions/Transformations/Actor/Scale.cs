// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[TransformationState(stateType: typeof(Henshin.State.Directions.Transformations.Actor.Scale))]
public class Scale: Henshin.Controller.Directions.Transformations.Scene.Delay {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new Henshin.State.Directions.Transformations.Actor.Scale State => (Henshin.State.Directions.Transformations.Actor.Scale)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Scale"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public Scale(State.Directions.Transformation state) : base(state: state) {}
        
        // -- Protected Methods --
            /// <summary>Serialize the timer duration.</summary>
            protected override void _Serialize() {
                // Call the base serialization method.
                base._Serialize();
                
                // Store the target x y and z positions.
                this._AddSerializedString(serialized: this.State.Target.x.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
                this._AddSerializedString(serialized: this.State.Target.y.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
            }
            
            /// <summary>Deserialize the timer duration.</summary>
            protected override void _Deserialize() {
                // Call the base serialization method.
                base._Deserialize();
                
                // Load the target's x y and z positions.
                this.State.Target = new UnityEngine.Vector2(
                    x: float.Parse(s: this._GetNextSerializedString(), provider: System.Globalization.CultureInfo.InvariantCulture),
                    y: float.Parse(s: this._GetNextSerializedString(), provider: System.Globalization.CultureInfo.InvariantCulture)
                );
            }

            protected override void _Apply() {
                base._Apply();
                
                // Store the current actor's position.
                this.State.Start = this.State.actor.ActorComponent.GetScale();
            }

            /// <summary>Event called on each unity frame.</summary>
            protected override void OnTick(float normalizedTime) {
                // Move along to the target position.
                this.State.actor.ActorComponent.SetScale(scale: UnityEngine.Vector2.Lerp(
                    a: this.State.Start,
                    b: this.State.Target,
                    t: normalizedTime
                ));
            }
    // --- /Methods ---
}
}