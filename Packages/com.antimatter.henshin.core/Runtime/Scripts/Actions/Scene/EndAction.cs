// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Directions.Scene;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Actions.Scene {

/// <summary>
/// Controller class used to manipulate <see cref="StartAction.State"/> objects.
/// This action marks the beginning of the specified action.
/// </summary>
[ActionControllerType(stateType: typeof(EndState)), ActionControllerCategory(category: EActionCategory.End)]
public class EndAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State class used to represent a <see cref="StartAction"/>.
            /// </summary>
            [Serializable]
            public class EndState: ActionState { }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
        // -- Protected Methods --
            /// <summary>
            /// Starts the scene.
            /// Moves on to the next scene in the current act.
            /// Does not call <see cref="ActionController.Finish"/>.
            /// </summary>
            protected override void Apply() {
#if UNITY_EDITOR
                // Check if the current scene is a debug scene.
                if (SceneState.Current.IsDebugScene) {
                    // Clear the debug flag.
                    SceneState.Current.IsDebugScene = false;
                    
                    // Stop execution of play mode.
                    EditorApplication.ExitPlaymode();
                    
                    // End the method.
                    return;
                }
#endif

                // Call the act's NextScene method.
                Directions.Act.ActController.NextScene();
            }
    
            // - Serialization Events -
            /// <inheritdoc cref="ActionController.SaveParameters"/>
            protected override void SaveParameters() { }

            /// <inheritdoc cref="ActionController.LoadParameters"/>
            protected override void LoadParameters() { }
        // -- Private Methods --
    // --- /Methods ---
}
}