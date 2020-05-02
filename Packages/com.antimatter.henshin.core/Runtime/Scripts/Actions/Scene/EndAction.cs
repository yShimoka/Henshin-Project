// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Runtime.Actions.Scene {

/// <summary>
/// Controller class used to manipulate <see cref="StartAction.State"/> objects.
/// This action marks the beginning of the specified action.
/// </summary>
[ActionControllerType(stateType: typeof(EndState))]
public class EndAction: ActionController {
    // ---  SubObjects ---
        // -- Public Classes --
            /// <summary>
            /// State class used to represent a <see cref="StartAction"/>.
            /// </summary>
            [System.SerializableAttribute]
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
                if (Runtime.Directions.Scene.SceneState.Current.IsDebugScene) {
                    // Stop execution of play mode.
                    UnityEditor.EditorApplication.ExitPlaymode();
                    
                    // End the method.
                    return;
                }
#endif

                // Call the act's NextScene method.
                Runtime.Directions.Act.ActController.NextScene();
            }
    
            // - Serialization Events -
            /// <inheritdoc cref="ActionController._SaveParameters"/>
            protected override void _SaveParameters() { }

            /// <inheritdoc cref="ActionController._LoadParameters"/>
            protected override void _LoadParameters() { }
        // -- Private Methods --
    // --- /Methods ---
}
}