// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Linq;
using Henshin.Core.App;
using Henshin.Core.Scene.Directions;
using Henshin.Editor.Misc;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.App {

/// <summary>
/// State class used by the <see cref="ManagerEditorWindow"/> class.
/// Loads all the required data for that window.
/// </summary>
public static class ManagerEditorState {

    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>List of all the states found in the <see cref="AssetDatabase"/></summary>
            public static State[] States { get; private set; } = null;
            
            /// <summary>Helper accessor to get the currently selected state.</summary>
            /// <seealso cref="CurrentStateIndex"/>.
            public static State CurrentState {
                get {
                    // If the state array is null.
                    if (ManagerEditorState.States == null) {
                        // Load all the states.
                        ManagerEditorState._LoadProjectStates();
                    }
                    
                    // Check if the array has a length.
                    return ManagerEditorState.States.Length > 0 ? ManagerEditorState.States[ManagerEditorState.CurrentStateIndex] : null;
                }
            }

    /// <summary>Index of the current state selected.</summary>
            public static int CurrentStateIndex { get => ManagerEditorState._msCurrentStateIndex; set => ManagerEditorState.SetCurrentStateIndex(index: value); }
        // -- Protected Attributes --
        // -- Private Attributes --
            /// <summary>Stores the index of the <see cref="CurrentState"/> object.</summary>
            private static int _msCurrentStateIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - State Alteration -
            /// <summary>
            /// Sets the value of the <see cref="CurrentStateIndex"/>.
            /// Loads all <see cref="State"/> objects if they were not already loaded.
            /// </summary>
            /// <param name="index">The new value of the <see cref="CurrentStateIndex"/>.</param>
            public static void SetCurrentStateIndex(int index) {
                // Load the states.
                ManagerEditorState._LoadProjectStates();
                
                // Check if the index is valid.
                if (index >= 0 && index < ManagerEditorState.States.Length) {
                    // Set the value of the index.
                    ManagerEditorState._msCurrentStateIndex = index;
                } else {
                    // Reset the index.
                    ManagerEditorState._msCurrentStateIndex = 0;
                }
                
                // Set it as the active state.
                ManagerEditorState._SetCurrentStateActive();
            }
            
            /// <summary>Forces the reloading of all <see cref="State"/> objects.</summary>
            public static void ForceReloadStates() { ManagerEditorState._LoadProjectStates(forceReload: true); }
            
            /// <summary>Creates a new <see cref="State"/> object.</summary>
            public static void CreateState() {
                // Create a new state instance.
                State newState = ScriptableObject.CreateInstance<State>();
                // Set the name of the state.
                newState.identifier = $"State_{ManagerEditorState.States.Length + 1}";
                newState.name = $"APP_STATE_{newState.identifier}";
                newState.isAppState = false;
                
                // Create the asset.
                Helper.CreateAsset(obj: newState, folder: $"Resources/{State.DEFAULT_PATH}");
                
                // Force reload the states.
                ManagerEditorState.ForceReloadStates();
            }
            
            /// <summary>Creates a new <see cref="Act"/> object.</summary>
            public static void CreateAct() {
                // Create a new act instance.
                Act newAct = ScriptableObject.CreateInstance<Act>();
                // Set the name of the act.
                newAct.identifier = $"Act_{ManagerEditorState.CurrentState.acts.Count + 1}";
                newAct.name = $"APP_ACT_{newAct.identifier}";
                
                // Add the act to the current state.
                ManagerEditorState.CurrentState.acts.Add(item: newAct);
                
                // Create the asset.
                Helper.CreateAsset(obj: newAct, folder: $"Resources/{Act.DEFAULT_PATH}");
            }
        // -- Protected Methods --
        // -- Private Methods --
            // - State Manipulation -
            /// <summary>
            /// Loads all the <see cref="State"/> found in the project.
            /// </summary>
            /// <param name="forceReload">If set, forces the reloading of all the <see cref="State"/>s.</param>
            private static void _LoadProjectStates(bool forceReload = false) {
                // Check if the states must be reloaded.
                if (forceReload || ManagerEditorState.States == null) {
                    // Get all the State assets.
                    string[] guids = AssetDatabase.FindAssets(filter: $"t:{nameof(State)}", searchInFolders: new [] { $"Assets/Resources/{State.DEFAULT_PATH}" });
                    
                    // Check if states were found.
                    if (guids.Length > 0) {
                        // Load all the states.
                        ManagerEditorState.States = guids
                            .Select(selector: AssetDatabase.GUIDToAssetPath)
                            .Select(selector: AssetDatabase.LoadAssetAtPath<State>)
                            .ToArray();
                            
                        // Load the current state's index.
                        ManagerEditorState._msCurrentStateIndex = 0;
                        for (int i = 0; i < ManagerEditorState.States.Length; i++) {
                            // If the state is activated.
                            if (ManagerEditorState.States[i].isAppState) {
                                // Store its index.
                                ManagerEditorState._msCurrentStateIndex = i;
                                break;
                            }
                        }
                    } else {
                        // Clear the array.
                        ManagerEditorState.States = new State[0];
                        
                        // Log a warning.
                        Debug.LogWarning(message: $"Could not find any State objects in the Resources/{State.DEFAULT_PATH} folder.");
                    }
                }
            }
            
            /// <summary>
            /// Marks the <see cref="CurrentState"/> as active.
            /// Also, disables the flag of all other <see cref="State"/>s.
            /// </summary>
            private static void _SetCurrentStateActive() {
                // Loop through all the states.
                for (int i = 0; i < ManagerEditorState.States.Length; i++) {
                    // Check if the state is the current one.
                    ManagerEditorState.States[i].isAppState = i == ManagerEditorState.CurrentStateIndex;
                }
            }
            
    // --- /Methods ---
}
}