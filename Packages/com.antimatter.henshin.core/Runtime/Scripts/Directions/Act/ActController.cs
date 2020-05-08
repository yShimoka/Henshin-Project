// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Application;
using Henshin.Runtime.Directions.Scene;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Directions.Act {

/// <summary>
/// Controller class used to manipulate <see cref="ActState"/> objects.
/// </summary>
public static class ActController {
    // ---  Methods ---
        // -- Public Methods --
            // - Serialization -
            /// <summary>
            /// Serializes the specified <see cref="ActState"/> object.
            /// Ensures that the serialized fields are properly set.
            /// </summary>
            /// <param name="owner">The owner of the serialized act.</param>
            /// <param name="act">The raw act object.</param>
            public static void Serialize(ApplicationState owner, ActState act) {
                // If the act's hash is 0.
                if (act.Hash == 0) {
                    // Set the hash of the act.
                    act.Hash = ApplicationController.Random.Next();
                }
                
                // Serialize all the scenes.
                foreach (SceneState sceneState in act.SceneList) {
                    SceneController.Serialize(owner: act, scene: sceneState);
                }
            }
            
            /// <summary>
            /// Deserializes the specified <see cref="ActState"/> object.
            /// Initializes all of its public properties.
            /// </summary>
            /// <param name="owner">The owner of the deserialized act.</param>
            /// <param name="act">The serialized act object.</param>
            public static void Deserialize(ApplicationState owner, ActState act) {
                // Deserialize all the scene objects.
                foreach (SceneState sceneState in act.SceneList) {
                    SceneController.Deserialize(owner: act, scene: sceneState);
                }
                
                // Set the public attributes.
                act.Owner = owner;
                act.Index = owner.ActList.IndexOf(item: act);
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
                        SceneController.Play(scene: act.CurrentScene);
                    } else {
                        // Log a warning.
                        Debug.LogWarning(
                            message: $"Skipped over act \"{act.Identifier}\" as it had no scenes to play."
                        );
                        
                        // Play the next act.
                        ApplicationController.NextAct();
                    }
                } else {
                    // Throw an error.
                    ApplicationView.Error(
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
                        SceneController.Play(scene: ActState.Current.CurrentScene);
                    } else {
                        // Play the next act.
                        ApplicationController.NextAct();
                    }
                } else {
                    // Throw an error.
                    ApplicationView.Error(
                        message: "Tried to advance to the next scene when there is no act currently playing."
                    );
                }
            }
    // --- /Methods ---
}
}