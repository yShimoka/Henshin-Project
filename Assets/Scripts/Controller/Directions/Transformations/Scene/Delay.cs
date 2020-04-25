// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations.Scene {

/// <summary>
/// Transformation controller used to mark the start of a line.
/// Immediately starts its children nodes' application.
/// </summary>
[TransformationState(stateType: typeof(Henshin.State.Directions.Transformations.Scene.Delay))]
public class Delay: Transformation {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new Henshin.State.Directions.Transformations.Scene.Delay State => (Henshin.State.Directions.Transformations.Scene.Delay)base.State;
            
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Delay"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public Delay(State.Directions.Transformation state) : base(state: state) {}
        
        // -- Protected Methods --
            /// <inheritdoc cref="Transformation._Apply"/>
            protected override void _Apply() {
                // Clear the state's timer.
                this.State.Timer = 0;
                
                // Bind to the update call.
                View.Application.AppBehaviour.OnUpdate.AddListener(call: this._Update);
            }
            
            /// <summary>Serialize the timer duration.</summary>
            protected override void _Serialize() {
                // Store the time.
                this._AddSerializedString(serialized: this.State.Time.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
            }
            
            /// <summary>Deserialize the timer duration.</summary>
            protected override void _Deserialize() {
                // Load the time.
                this.State.Time = float.Parse(s: this._GetNextSerializedString(), provider: System.Globalization.CultureInfo.InvariantCulture);
            }
            
            /// <summary>Internal method called on each frame.</summary>
            protected virtual void OnTick(float normalizedTime) {}
            
        // -- Private Methods --
            /// <summary>Event called on each unity frame.</summary>
            private void _Update() {
                // Update the timer.
                this.State.Timer += UnityEngine.Time.deltaTime;
                
                // Call the ontick method.
                this.OnTick(normalizedTime: this.State.Time == 0 ? 1 : this.State.Timer / this.State.Time);
                
                // Check the value of the timer.
                if (this.State.Timer > this.State.Time) {
                    // Stop the update.
                    View.Application.AppBehaviour.OnUpdate.RemoveListener(call: this._Update);
                    
                    // Finish the transformation.
                    this._Finish();
                }
            }
    // --- /Methods ---
}
}