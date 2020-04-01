/*
 * Copyright © 2020 - Zimproov.
 */

using System;
using Henshin.Core.Scene.Directions;
using UnityEngine;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.App {

/// <summary>
/// Application state used by the <see cref="Manager"/> class.
/// Stores the state of the application and its parameters.
/// </summary>
[CreateAssetMenu(menuName = "Henshin/App/State", fileName = "APP_STATE_State", order = 1)]
public class State: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Utility Attributes -
            /// <summary>
            /// Unique identifier for this state.
            /// Used in debug and editor to ensure which instance is being modified.
            /// </summary>
            [SerializeField]
            public string identifier;
            
            /// <summary>
            /// Flag set if this is the state used for the application.
            /// There should only be one <see cref="State"/> object with this flag set.
            /// </summary>
            [SerializeField, HideInInspector]
            public bool isAppState;
            
            // - Theatre Info -
            /// <summary>
            /// Identifier of the scene that will hold the entire application.
            /// Loaded by the <see cref="Manager"/> at startup.
            /// </summary>
            //[HideInInspector]
            public string theatreScene;
            
            /// <summary>
            /// Prefab instance used for the spectator element.
            /// This MUST be a prefab with a <see cref="Camera"/> object that will render the whole application.
            /// </summary>
            public GameObject spectator;
            
            // - Play Info -
        
        // -- Public Attributes --
            // - Utility Attributes -
            /// <summary>
            /// Reference to the <see cref="State"/> with its <see cref="isAppState"/> flag set.
            /// Initialized in the <see cref="_Initialize"/> method.
            /// </summary>
            public static State Current { get; private set; }
            
            /// <summary>
            /// Default location of the <see cref="State"/> instances.
            /// </summary>
            public const string DEFAULT_PATH = "Serialized/Application";
            
            // - Theatre Info -
            /// <summary>
            /// Reference to the main camera of the scene.
            /// This is the instance found within the instanced <see cref="spectator"/> prefab.
            /// </summary>
            public static Camera MainCamera { get; private set; }
            
            /// <summary>
            /// Reference to the root transform of the theatre. 
            /// </summary>
            public static Transform Root;
            
            // - Play Info -
            // DEBUG
            public Act debugAct;
        
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            /// <summary>
            /// Initializes the spectator prefab.
            /// Attaches it to the specified <see cref="parent"/>.
            /// </summary>
            /// <param name="parent">The transform to attach the <see cref="spectator"/> to.</param>
            /// <exception cref="MissingPrefabException{State}">If the <see cref="spectator"/> prefab is null.</exception>
            /// <exception cref="MissingComponentException">If the <see cref="spectator"/> prefab has no <see cref="Camera"/> child component.</exception>
            public static void InitializeSpectator(Transform parent) {
                // Check if the spectator prefab was specified.
                if (State.Current.spectator != null) {
                    // Instantiate the spectator prefab, ensuring a null position and rotation.
                    GameObject spectatorInstance = Object.Instantiate(
                        original: State.Current.spectator, 
                        parent: parent, 
                        position: Vector3.zero, 
                        rotation: Quaternion.identity
                    );
                    spectatorInstance.name = "Spectator";
                    
                    // Try to load the camera from the instance.
                    State.MainCamera = spectatorInstance.GetComponentInChildren<Camera>(includeInactive: true);
                    
                    // Check if the camera object was found.
                    if (State.MainCamera == null) {
                        throw new MissingComponentException(message: "There is no Camera component in the specified spectator prefab.");
                    }
                } else {
                    // Throw an exception.
                    throw new MissingPrefabException<State>(attributeName: nameof(State.spectator), containerIdentifier: State.Current.identifier);
                }
            }
            
        // -- Protected Methods --
        // -- Private Methods --
            /// <summary>
            /// Method called to initialize the application's <see cref="State"/> resources.
            /// Checks if a <see cref="State"/> is defined as the application's state.
            /// Calls <see cref="Manager.Initialize"/>.
            /// </summary>
            /// <exception cref="MissingStateException">If there is no valid <see cref="State"/> in a Resources folder.</exception>
            [RuntimeInitializeOnLoadMethod]
            private static void _Initialize() {
                // Load all the application state objects.
                State[] appStates = Resources.LoadAll<State>(path: State.DEFAULT_PATH);
                Debug.Log(message: $"Found {appStates.Length} State instance(s).");
                
                // Loop through all the states.
                foreach (State appState in appStates) {
                    // Check if the app state flag is set.
                    if (appState.isAppState) {
                        Debug.Log(message: $"Loading state \"{appState.identifier}\"");
                        
                        // Store the reference in the helper property.
                        State.Current = appState;
                        
                        // Call the manager's initialization method.
                        Manager.Initialize();
                        
                        // Stop the method.
                        return;
                    }
                }
                
                // If no state was found, throw an error.
                throw new MissingStateException(message: $"There are no valid State instances within the Resources/{State.DEFAULT_PATH} folder.");
            }
    // --- /Methods ---
}
}