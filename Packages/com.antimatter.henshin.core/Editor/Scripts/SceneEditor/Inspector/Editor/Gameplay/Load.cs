// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Gameplay;
using Henshin.Runtime.Data;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Gameplay {

/// <summary>
/// Class used to render load action in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(LoadAction))]
public class Load: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a load state.
                LoadAction.LoadState state = (LoadAction.LoadState)action;
                
                // Check if the loaded scene is correct.
                if (
                    DataState.ActIndex != inspector.Owner.Header.EditedActIndex || 
                    DataState.SceneIndex != inspector.Owner.Header.EditedSceneIndex
                ) {
                    // Load the current scene.
                    DataController.LoadScene(
                        act: inspector.Owner.Header.EditedActIndex,
                        scene: inspector.Owner.Header.EditedSceneIndex
                    );
                }
                // Get all the gameplay identifiers.
                string[] identifiers = DataController.GetGameplayIdentifiers();
                
                // Get the index of the current identifier.
                int current = Array.IndexOf(array: identifiers, value: state.Identifier);
                if (current == -1) current = 0;
                
                // Get the actor list from the current scene.
                state.Identifier = identifiers[EditorGUILayout.Popup(
                    selectedIndex: current,
                    displayedOptions: DataController.GetGameplayIdentifiers(),
                    label: "Gameplay"
                )];
            }
    // --- /Methods ---
}
}