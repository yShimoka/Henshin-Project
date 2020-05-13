// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actions.Gameplay;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Gameplay {

/// <summary>
/// Class used to render the selected prepare action in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(PrepareAction))]
public class Prepare: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a prepare state.
                PrepareAction.PrepareState state = (PrepareAction.PrepareState)action;
                
                // Load the possible gameplay names.
                Tuple<string, GameplayState.EGameplayMode>[] modes = XmlController.GetGameplayModes(
                    actIndex: inspector.Owner.Header.EditedActIndex, 
                    sceneIndex: inspector.Owner.Header.EditedSceneIndex
                );
                
                // Let the user chose.
                state.GameplayIndex = EditorGUILayout.Popup(
                    label: "Gameplay",
                    selectedIndex: state.GameplayIndex,
                    displayedOptions: modes.Select(selector: mode => mode.Item1).ToArray()
                );
                
                // Get the gameplay for the index.
                state.GameplayMode = modes[state.GameplayIndex].Item2;
            }
    // --- /Methods ---
}
}