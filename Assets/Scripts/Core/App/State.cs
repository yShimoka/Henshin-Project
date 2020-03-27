/*
 * Copyright © 2020 - Zimproov.
 */

using System;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.App {

/// <summary>
/// Exception thrown if no <see cref="State"/> object is found within its resource folder.
/// </summary>
internal class MissingStateException: Exception {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Creates a new <see cref="MissingStateException"/> instance.
            /// Calls the base constructor.
            /// <seealso cref="Exception"/>
            /// </summary>
            /// <param name="message">The message that will be rendered to the user.</param>
            public MissingStateException(string message): base(message: message) {}
    // --- /Methods ---
}

/// <summary>
/// Application state used by the <see cref="Manager"/> class.
/// Stores the state of the application and its parameters.
/// </summary>
[CreateAssetMenu(menuName = "Henshin/App/State", fileName = "APP_STATE_State", order = 1)]
public class State: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
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
            [SerializeField, Tooltip(tooltip: "TEMPORARY, DO NOT MODIFY HERE !!!")]
            public bool isAppState;
        
        // -- Public Attributes --
            /// <summary>
            /// Reference to the <see cref="State"/> with its <see cref="isAppState"/> flag set.
            /// Initialized in the <see cref="_Initialize"/> method.
            /// </summary>
            public static State Current { get; private set; }
            
            /// <summary>
            /// Default location of the <see cref="State"/> instances.
            /// </summary>
            public const string DEFAULT_PATH = "Serialized/Application";
        
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
        // -- Protected Methods --
        // -- Private Methods --
            /// <summary>
            /// Method called to initialize the application's <see cref="State"/> resources.
            /// Checks if a <see cref="State"/> is defined as the application's state.
            /// Calls <see cref="Manager.Initialize"/>.
            /// </summary>
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