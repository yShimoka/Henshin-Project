// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[TransformationState(stateType: typeof(Henshin.State.Directions.Transformations.Actor.Colour))]
public class Colour: Henshin.Controller.Directions.Transformations.Scene.Delay {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new Henshin.State.Directions.Transformations.Actor.Colour State => (Henshin.State.Directions.Transformations.Actor.Colour)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Colour"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public Colour(State.Directions.Transformation state) : base(state: state) {}
        
        // -- Protected Methods --
            /// <summary>Serialize the timer duration.</summary>
            protected override void _Serialize() {
                // Call the base serialization method.
                base._Serialize();
                
                // Store the target color's values.
                this._AddSerializedString(serialized: this.State.Target.r.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
                this._AddSerializedString(serialized: this.State.Target.g.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
                this._AddSerializedString(serialized: this.State.Target.b.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
                this._AddSerializedString(serialized: this.State.Target.a.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
            }
            
            /// <summary>Deserialize the timer duration.</summary>
            protected override void _Deserialize() {
                // Call the base serialization method.
                base._Deserialize();
                
                // Load the target's x y and z positions.
                this.State.Target = new UnityEngine.Color(
                    r: float.Parse(s: this._GetNextSerializedString(), provider: System.Globalization.CultureInfo.InvariantCulture),
                    g: float.Parse(s: this._GetNextSerializedString(), provider: System.Globalization.CultureInfo.InvariantCulture),
                    a: float.Parse(s: this._GetNextSerializedString(), provider: System.Globalization.CultureInfo.InvariantCulture),
                    b: float.Parse(s: this._GetNextSerializedString(), provider: System.Globalization.CultureInfo.InvariantCulture)
                );
            }

            protected override void _Apply() {
                base._Apply();
                
                // Store the current actor's colour.
                this.State.Start = this.State.actor.ActorComponent.GetColour();
            }

            /// <summary>Event called on each unity frame.</summary>
            protected override void OnTick(float normalizedTime) {
                // Update the actor's colour
                this.State.actor.ActorComponent.SetColour(colour: UnityEngine.Color.Lerp(
                    a: this.State.Start,
                    b: this.State.Target,
                    t: normalizedTime
                ));
            }
    // --- /Methods ---
}
}