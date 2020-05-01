// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;

/* Wrap the class within the local namespace. */
namespace Runtime.Application {

/// <summary>
/// Controller class used to alter the state of the application.
/// </summary>
[UnityEngine.AddComponentMenu(menuName: "Henshin/Application Controller"), UnityEngine.DisallowMultipleComponent]
public class ApplicationController: UnityEngine.MonoBehaviour {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Method called upon the start of the runtime application.
            /// Loads the current application state and runs it.
            /// </summary>
            [UnityEngine.RuntimeInitializeOnLoadMethodAttribute]
            private static void _Initialize() {
#if UNITY_EDITOR
                // Seek the debug application state.
                Application.ApplicationState.Own = UnityEngine.Resources
                    .LoadAll<Application.ApplicationState>(path: "")
                    .FirstOrDefault(predicate: state => state.IsDebugState);
#else
                // Seek the non-debug application state.
                Application.State.Own = UnityEngine.Resources
                    .LoadAll<Application.State>(path: "Data/States")
                    .FirstOrDefault(predicate: state => !state.IsDebugState);
#endif
                // Initialize the view.
                Application.ApplicationView.Initialize();
            }
            
        // -- Public Methods --
            // - Serialization Events -
            /// <summary>
            /// Serializes the specified state instance.
            /// Calls the <see cref="Runtime.Directions.Act.ActController.Serialize"/> method on all the acts.
            /// </summary>
            /// <param name="state">The state object to serialize.</param>
            public static void Serialize(ApplicationState state) {
                // Serialize all the acts.
                foreach (Runtime.Directions.Act.ActState actState in state.ActList) {
                    Runtime.Directions.Act.ActController.Serialize(owner: state, act: actState);
                }
            }
            
            /// <summary>
            /// Deserializes the specified state instance.
            /// Calls the <see cref="Runtime.Directions.Act.ActController.Deserialize"/> method on all the acts.
            /// </summary>
            /// <param name="state">The state object to deserialize.</param>
            public static void Deserialize(ApplicationState state) {
                // Deserialize all the acts.
                foreach (Runtime.Directions.Act.ActState actState in state.ActList) {
                    Runtime.Directions.Act.ActController.Deserialize(owner: state, act: actState);
                }
            }
        // -- Protected Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}