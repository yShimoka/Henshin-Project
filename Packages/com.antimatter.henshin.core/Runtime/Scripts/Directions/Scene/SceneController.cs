// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Runtime.Directions.Scene {

/// <summary>
/// Controller class used to manipulate <see cref="SceneState"/> objects.
/// </summary>
public static class SceneController {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            // - Serialization Events -
            /// <summary>
            /// Serializes the specified scene object.
            /// </summary>
            /// <param name="owner">The owner of this scene.</param>
            /// <param name="scene">The raw scene object.</param>
            public static void Serialize(Runtime.Directions.Act.ActState owner, SceneState scene) {
                // Serialize all the transformations.
                foreach (Runtime.Actions.ActionState actionState in scene.ActionList) {
                    Runtime.Actions.ActionController.Serialize(owner: scene, action: actionState);
                }
            }
            
            /// <summary>
            /// Deserializes the specified scene object.
            /// Sets all the public attributes to their correct values.
            /// </summary>
            /// <param name="owner">The owner of this scene.</param>
            /// <param name="scene">The serializes scene object.</param>
            public static void Deserialize(Runtime.Directions.Act.ActState owner, SceneState scene) {
                // Set the owner of the scene.
                scene.Owner = owner;
                scene.Index = System.Array.IndexOf(array: owner.SceneList, value: scene);
                
                // Deserialize all the transformations.
                foreach (Runtime.Actions.ActionState actionState in scene.ActionList) {
                    Runtime.Actions.ActionController.Deserialize(owner: scene, action: actionState);
                }
                
                // Check if the action list is not empty.
                if (scene.ActionList.Length > 0) {
                    // Search for the Start action.
                    scene.RootAction = scene.ActionList
                        .FirstOrDefault(predicate: action => 
                            action.GetType() == typeof(Runtime.Actions.Scene.StartAction)
                    );
                }
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}