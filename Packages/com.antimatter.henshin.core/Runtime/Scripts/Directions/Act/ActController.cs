// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.



/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Act {

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
            
            // - Play Controller -
            /// <summary>
            /// Plays the specified act.
            /// Plays the first scene of the act.
            /// </summary>
            /// <param name="act">The act to play.</param>
            public static void Play(ActState act) {
                // Check if the act is valid.
                if (act != null) {
                    // Store the reference of the current act.
                    ActState.Current = act;
                    // Reset the scene index counter.
                    act.CurrentSceneIndex = 0;
                    
                    // Check if the act has any scene.
                    if (act.CurrentScene != null) {
                        // Play the first scene of the act.
                        Runtime.Directions.Scene.SceneController.Play(scene: act.CurrentScene);
                    } else {
                        // Log a warning.
                        UnityEngine.Debug.LogWarning(
                            message: $"Skipped over act \"{act.Identifier}\" as it had no scenes to play."
                        );
                        
                        // Play the next act.
                        Runtime.Application.ApplicationController.NextAct();
                    }
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: "Tried to play an act that is null !"
                    );
                }
            }
            
            /// <summary>
            /// Starts the next scene in the current act.
            /// </summary>
            public static void NextScene() {
                // Check if the current act is set.
                if (ActState.Current != null) {
                    // Increment the scene counter.
                    ActState.Current.CurrentSceneIndex++;
                    
                    // Check if there is a next scene.
                    if (ActState.Current.CurrentScene != null) {
                        // Play the next scene.
                        Runtime.Directions.Scene.SceneController.Play(scene: ActState.Current.CurrentScene);
                    } else {
                        // Play the next act.
                        Runtime.Application.ApplicationController.NextAct();
                    }
                } else {
                    // Throw an error.
                    Runtime.Application.ApplicationView.Error(
                        message: "Tried to advance to the next scene when there is no act currently playing."
                    );
                }
            }
    // --- /Methods ---
}
}