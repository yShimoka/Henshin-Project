// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using Henshin.Runtime.Directions.Scene;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Base {

/// <summary>
/// Base class used for all the actions that manipulate an actor.
/// </summary>
public abstract class ActorAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State of the actor action class.
            /// Holds the index of the edited actor.
            /// </summary>
            public class ActorState: ActionState {
                // ---  Attributes ---
                    // -- Serialized Attributes --
                        /// <summary>
                        /// Index of the actor in the owner's list.
                        /// </summary>
                        public int ActorIndex;
                // --- /Attributes ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --    
            /// <summary>
            /// State accessor.
            /// Casts the state object to an <see cref="ActorState"/> reference.
            /// </summary>
            public new ActorState State => (ActorState)base.State;
        
        // -- Protected Attributes --
            /// <summary>
            /// Reference to the actor state in the owner's list.
            /// </summary>
            protected Runtime.Actor.ActorState Actor;
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
            }
            
            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() {
                // Load the actor index.
                this.State.ActorIndex = this.NextSerializedData<int>();
            }
            
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() {
                // Save the actor index.
                this.AddSerializedData(data: this.State.ActorIndex);
            }
    // --- /Methods ---
}
}