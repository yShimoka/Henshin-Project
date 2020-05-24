// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Application;
using Henshin.Runtime.Directions.Scene;
using Henshin.Runtime.Libraries;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Base {

/// <summary>
/// Base class used for all the actions that manipulate an actor over a span of time.
/// </summary>
public abstract class TimedActorAction: TimedAction {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State of the actor action class.
            /// Holds the index of the edited actor.
            /// </summary>
            public class TimedActorState: TimedState {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        /// <summary>
                        /// Index of the actor in the owner's list.
                        /// </summary>
                        public int ActorIndex;
                        
                        /// <summary>
                        /// Stores the easing mode of the movement.
                        /// </summary>
                        public EasingFunction.Ease EasingMode;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --    
            /// <summary>
            /// State accessor.
            /// Casts the state object to an <see cref="TimedActorState"/> reference.
            /// </summary>
            public new TimedActorState State => (TimedActorState)base.State;
        
        // -- Protected Attributes --
            /// <summary>
            /// Reference to the actor state in the owner's list.
            /// </summary>
            protected Runtime.Actor.ActorState Actor;
            
        // -- Private Attributes --
            /// <summary>
            /// Timer that is incremented on each <see cref="_Update"/> call.
            /// </summary>
            private float _mTimer;
            
            /// <summary>
            /// Stores the reference to the used easing function.
            /// </summary>
            private EasingFunction.Function _mEasingFunc;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <summary>
            /// Applies the transformation.
            /// Loads the actor in the current scene's list.
            /// </summary>
            protected override void Apply() {
                // Check if the scene is set.
                if (SceneState.Current != null) {
                    // Load the actor from the current scene.
                    this.Actor = SceneState.Current.ActorList[index: this.State.ActorIndex];
                } else {
                    // Log an error.
                    ApplicationView.Error(message: "Tried to apply an actor action when there is no scene playing !");
                }
                
                // Clear the timer method.
                this._mTimer = 0;
                
                // Load the easing function.
                this._mEasingFunc = EasingFunction.GetEasingFunction(easingFunction: this.State.EasingMode);
                
                // Bind to the update.
                ApplicationController.OnTick.AddListener(call: this._Update);
            }
            
            /// <inheritdoc cref="ActionController.Finish"/>
            protected override void Finish() {
                // Remove the tick method.
                ApplicationController.OnTick.RemoveListener(call: this._Update);
                
                // Call the base method.
                base.Finish();
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Call the base method.
                base.LoadParameters();
                
                // Load the actor index.
                this.State.ActorIndex = this.NextSerializedData<int>();
                
                // Load the easing function.
                this.State.EasingMode = this.NextSerializedData<EasingFunction.Ease>();
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Call the base method.
                base.SaveParameters();
                
                // Save the actor index.
                this.AddSerializedData(data: this.State.ActorIndex);
                
                // Save the easing function.
                this.AddSerializedData(data: this.State.EasingMode);
            }
            
        // -- Private Methods --
            private new void _Update(float deltaTime) {
                // Increment the timer value.
                this._mTimer += deltaTime;
                
                // Get the normalized time.
                float normalized = this._mEasingFunc(s: 0, e: 1, v: Mathf.Clamp(
                    value: this.State.Time == 0 ? 1 : this._mTimer / this.State.Time, min: 0, max: 1
                ));
                
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