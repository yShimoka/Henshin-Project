// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;

/* Wrap the class within the local namespace. */
namespace Runtime.Directions.Act {

/// <summary>
/// Controller class used to manipulate <see cref="ActState"/> objects.
/// </summary>
public static class ActController {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - Serialization -
            /// <summary>
            /// Serializes the specified <see cref="ActState"/> object.
            /// Ensures that the serialized fields are properly set.
            /// </summary>
            /// <param name="owner">The owner of the serialized act.</param>
            /// <param name="act">The raw act object.</param>
            public static void Serialize(Runtime.Application.ApplicationState owner, ActState act) {
                // Serialize all the scenes.
                foreach (Runtime.Directions.Scene.SceneState sceneState in act.SceneList) {
                    Runtime.Directions.Scene.SceneController.Serialize(owner: act, scene: sceneState);
                }
            }
            
            /// <summary>
            /// Deserializes the specified <see cref="ActState"/> object.
            /// Initializes all of its public properties.
            /// </summary>
            /// <param name="owner">The owner of the deserialized act.</param>
            /// <param name="act">The serialized act object.</param>
            public static void Deserialize(Runtime.Application.ApplicationState owner, ActState act) {
                // Deserialize all the scene objects.
                foreach (Runtime.Directions.Scene.SceneState sceneState in act.SceneList) {
                    Runtime.Directions.Scene.SceneController.Deserialize(owner: act, scene: sceneState);
                }
                
                // Set the public attributes.
                act.Owner = owner;
                act.Index = System.Array.IndexOf(array: owner.ActList, value: act);
            }
            
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}