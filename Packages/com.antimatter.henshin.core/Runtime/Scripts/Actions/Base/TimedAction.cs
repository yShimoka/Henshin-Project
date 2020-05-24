// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Base {

/// <summary>
/// Base class used to represent an action that has a duration.
/// </summary>
public abstract class TimedAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State class used by <see cref="TimedAction"/> objects.
            /// </summary>
            [Serializable]
            public class TimedState: ActionState {
                // ---  Attributes ---
                    // -- Public Attributes --
                        /// <summary>
                        /// Time (in seconds) in which the action should take place.
                        /// </summary>
                        public float Time;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Accessor to the casted base class state.
            /// </summary>
            public new TimedState State => (TimedState)base.State;
             
            /// <summary>
            /// Timer that is incremented on each <see cref="_Update"/> call.
            /// </summary>
            protected float Timer;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            // - Action Methods -
            /// <summary>
            /// Applies the timed action.
            /// Binds to the <see cref="ApplicationController.OnTick"/> for the duration of the action.
            /// </summary>
            protected override void Apply() {
                // Bind to the on tick method.
                ApplicationController.OnTick.AddListener(call: this._Update);
                
                // Reset the tick method.
                this.Timer = 0;
            }
            
            /// <inheritdoc cref="ActionController.Finish"/>
            protected override void Finish() {
                // Remove the tick method.
                ApplicationController.OnTick.RemoveListener(call: this._Update);
                
                // Call the base method.
                base.Finish();
            }

            /// <summary>
            /// Updates the action.
            /// </summary>
            /// <param name="normalizedTime">The current time of the action, normalized in [0;1]</param>
            protected virtual void Update(float normalizedTime) {}
            
            // - Serialization -
            /// <summary>
            /// Saves the current state parameters in the serializable array.
            /// </summary>
            protected override void SaveParameters() {
                // Store the time.
                this.AddSerializedData(data: this.State.Time);
            }
            
            /// <summary>
            /// Loads the state parameters from the serializable array.
            /// </summary>
            protected override void LoadParameters() {
                // Load the time.
                this.State.Time = this.NextSerializedData<float>();
            }
        
            /// <summary>
            /// Updates the <see cref="TimedAction"/> object.
            /// </summary>
            /// <param name="deltaTime">The time since the last call to <see cref="_Update"/></param>
            protected virtual void _Update(float deltaTime) {
                // Increment the timer value.
                this.Timer += deltaTime;
                
                // Get the normalized time.
                float normalized = Mathf.Clamp(
                    value: this.State.Time == 0 ? 1 : this.Timer / this.State.Time, min: 0, max: 1
                );
                
                // Call the update method.
                this.Update(normalizedTime: normalized);
                
                // If the normalized time reached one.
                if (normalized >= 1) {
                    // Finish the action.
                    this.Finish();
                }
            }
    // --- /Methods ---
}
}