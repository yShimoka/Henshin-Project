// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using Henshin.State;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Wrap the class within the local namespace. */
namespace Henshin.Controller {

/// <summary>
/// 
/// </summary>
public static class Application {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Loads all the <see cref="State.Application"/> assets from file.
            /// Uses the <see cref="State.Application.RESOURCE_PATH"/> to know where to look.
            /// </summary>
            public static void LoadAssets() {
                // Get all the asset resources in the specified folder.
                State.Application.Assets = Resources.LoadAll<State.Application>(path: State.Application.RESOURCE_PATH);
                
                // Find the current state in the list.
                State.Application.Current = Application.GetCurrent();
            }
            
            /// <summary>
            /// Finds the first <see cref="State.Application"/> object with the <see cref="State.Application.isAppState"/> flag set.
            /// </summary>
            /// <returns>The instance of the </returns>
            public static State.Application GetCurrent() {
                try {
                    // Seek the first state with the isAppState flag set.  
                    return State.Application.Assets.First(predicate: state => state.isAppState);
                } catch (InvalidOperationException) {
                    // Route to the error scene.
                    throw Application.Error(message: "Could not find a valid Application State object !");
                }
            }
            
            /// <summary>Advances to the next act in the list.</summary>
            public static void NextAct() {
                // Increment the act counter.
                if (State.Application.Current.IncrementActIndex()) {
                    // DEBUG: Throw an error message.
                    throw Application.Error(message: "Vous êtes arrivé à la fin du jeu dans son état actuel."); 
                } else {
                    // Play the next act.
                    Directions.Act.Play(act: State.Application.Current.CurrentAct);
                }
            }
            
            // - Error Management -
            /// <summary>
            /// Handles application errors.
            /// Loads the error scene and renders the error message to the user.
            /// </summary>
            /// <param name="message">The error message to pass to the user.</param>
            public static InvalidOperationException Error(string message) {
                // Try to load the error scene object.
//#if !UNITY_EDITOR
                SceneManager.LoadSceneAsync(sceneName: "Error", mode: LoadSceneMode.Single).completed += operation => Application._OnErrorSceneLoaded(errorMessage: message);
//#endif
                
                // Return the error to thrown.
                return new InvalidOperationException(message: message);
            }
            
        // -- Private Methods --
            // - Initialization -
            /// <summary>
            /// Initializes the application on startup.
            /// Loads the theatre scene and sets up all of its required components.
            /// </summary>
            [RuntimeInitializeOnLoadMethod]
            private static void _Initialize() {
                // Load all the state assets.
                Application.LoadAssets();
                
                // Reset the current act index.
                State.Application.Current.ClearActIndex();
                
                // Load the theatre.
                View.Application.CreateTheatreScene();
                
#if UNITY_EDITOR
                    // Check if there is a scene that is marked as testing.
                    Henshin.State.Directions.Scene debugged = State.Application.Current.acts
                        ?.SelectMany(selector: act => act.scenes)
                        .Where(predicate: scene => scene != null)
                        .FirstOrDefault(predicate: scene => scene.testScene);
                    if (debugged) {
                        // Play the debugged scene.
                        Directions.Scene.Play(scene: debugged);
                        
                        // Stop the method.
                        return;
                    }
#endif                
                
                    // Play the current act.
                    Directions.Act.Play(act: State.Application.Current.CurrentAct);
            }
            
            // - Asynchronous Calls -
            /// <summary>
            /// Asynchronous method called when the "Error" scene is loaded.
            /// Updates the error message of the scene.
            /// </summary>
            /// <param name="errorMessage">The message to draw to the user.</param>
            /// <exception cref="InvalidOperationException">If the "Error" scene is in an invalid state.</exception>
            private static void _OnErrorSceneLoaded(string errorMessage) {
                // Get the scene's root objects.
                GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
                
                // Throw an exception if there are no root objects in the scene.
                if (roots.Length == 0) {
                    throw new InvalidOperationException(message: "The Error scene has no game objects !");
                }
                
                // Seek for the error component in the scene.
                View.Misc.Error error = roots
                    .Select(selector: gameObject => gameObject.GetComponentInChildren<View.Misc.Error>())
                    .FirstOrDefault(predicate: component => component != null);
                    
                // Check if there is an error component.
                if (error == null) {
                    throw new InvalidOperationException(message: "The Error scene has no Error component anywhere !");
                }
                
                // Draw the error message.
                error.SetMessage(message: errorMessage);
            }
    // --- /Methods ---
}
}