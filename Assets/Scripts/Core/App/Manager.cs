/*
 * Copyright © 2020 - Zimproov.
 */

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


/* Wrap the class within the local namespace. */
namespace Henshin.Core.App {

/// <summary>
/// Main manager class used across the entire game.
/// Handles the application through <see cref="State"/> components.
/// </summary>
public static class Manager {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            /// <summary>
            /// Method called after initialization of the <see cref="State"/> resources.
            /// Sets up the game following its current state and specified parameters.
            /// </summary>
            public static void Initialize() {
                Debug.Log(message: "Initializing Manager class.");
                
                // Load the player scene.
                AsyncOperation load = SceneManager.LoadSceneAsync(sceneName: State.Current.theatreScene, mode: LoadSceneMode.Single);
                
                // Check if the loading has started.
                if (load != null) {
                    // Register the loading method.
                    load.completed += Manager._OnTheatreLoaded;
                    load.allowSceneActivation = true;
                } else {
                    // Throw an error.
                    throw new InvalidTheatreException(message: $"There is no scene named \"{State.Current.theatreScene}\" for the theatre.");
                }
            }
        
        // -- Protected Methods --
        // -- Private Methods --
            /// <summary>
            /// Method called once the theatre scene has finished loading.
            /// </summary>
            /// <exception cref="InvalidTheatreException">If the scene does not contain exactly one root object.</exception>
            private static void _OnTheatreLoaded(AsyncOperation finishedOperation) {
                Debug.Log(message: "Loaded theatre scene.");
                
                // Get the root of the scene.
                GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
                
                // If there is no root in the scene.
                if (roots.Length == 0) {
                    throw new InvalidTheatreException(message: $"There MUST be exactly at least one root object in the theatre. (Found {roots.Length})");
                } else if (roots.Length > 1) {
                    Debug.LogWarning(message: $"There SHOULD NOT more than one root object in the theatre. (Found {roots.Length})");
                }
                
                // Get the overall root.
                State.Root = roots[0].transform;
                State.Root.name = "Root";
                
                // Create the Audience game object.
                GameObject audience = new GameObject { name = "Audience" };
                audience.transform.SetParent(p: State.Root);
                
                // Initialize the spectator.
                State.InitializeSpectator(parent: audience.transform);
            }
    // --- /Methods ---
}
}