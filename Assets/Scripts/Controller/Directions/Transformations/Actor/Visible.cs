// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[TransformationState(stateType: typeof(Henshin.State.Directions.Transformations.Actor.Visible))]
public class Visible: Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new Henshin.State.Directions.Transformations.Actor.Visible State => (Henshin.State.Directions.Transformations.Actor.Visible)base.State;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Visible"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public Visible(State.Directions.Transformation state) : base(state: state) {}
        
        // -- Protected Methods --
            /// <summary>Serialize the timer duration.</summary>
            protected override void _Serialize() {
                // Call the base serialization method.
                base._Serialize();
                
                // Store the flags.
                this._AddSerializedString(serialized: this.State.Activate.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
                this._AddSerializedString(serialized: this.State.AllActors.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
            }
            
            /// <summary>Deserialize the timer duration.</summary>
            protected override void _Deserialize() {
                // Call the base serialization method.
                base._Deserialize();
                
                // Load the active flags.
                this.State.Activate = bool.Parse(value: this._GetNextSerializedString());
                this.State.AllActors = bool.Parse(value: this._GetNextSerializedString());
            }

            protected override void _Apply() {
                // Check the all actors flag.
                if (this.State.AllActors) {
                    // Loop through all the scene's actors.
                    foreach (Henshin.State.Scenery.Actor actor in Henshin.Controller.Directions.Scene.Current.actors) {
                        actor.ActorComponent.SetActive(value: this.State.Activate);
                    }
                } else {
                    this.State.actor.ActorComponent.SetActive(value: this.State.Activate);
                }
                
                // Finish the action.
                this._Finish();
            }
    // --- /Methods ---
}
}