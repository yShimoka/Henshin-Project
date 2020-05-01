// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[TransformationState(stateType: typeof(Henshin.State.Directions.Transformations.Actor.Flip))]
public class Flip: Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new Henshin.State.Directions.Transformations.Actor.Flip State => (Henshin.State.Directions.Transformations.Actor.Flip)base.State;
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Flip"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public Flip(State.Directions.Transformation state) : base(state: state) {}
        
        // -- Protected Methods --
            /// <summary>Serialize the timer duration.</summary>
            protected override void _Serialize() {
                // Call the base serialization method.
                base._Serialize();
                
                // Store the flags.
                this._AddSerializedString(serialized: this.State.Vertical.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
                this._AddSerializedString(serialized: this.State.Horizontal.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
            }
            
            /// <summary>Deserialize the timer duration.</summary>
            protected override void _Deserialize() {
                // Call the base serialization method.
                base._Deserialize();
                
                // Load the flags.
                this.State.Vertical = bool.Parse(value: this._GetNextSerializedString());
                this.State.Horizontal = bool.Parse(value: this._GetNextSerializedString());
            }

            protected override void _Apply() {
                // Update the flipping.
                this.State.actor.ActorComponent.SetHorizontalFlip(value: this.State.Horizontal);
                this.State.actor.ActorComponent.SetVerticalFlip  (value: this.State.Vertical);
                
                // Finish the action.
                this._Finish();
            }
    // --- /Methods ---
}
}