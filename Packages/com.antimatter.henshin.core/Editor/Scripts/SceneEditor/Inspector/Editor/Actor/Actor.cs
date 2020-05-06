// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Base;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Actor {

/// <summary>
/// Class used to render the selected actor in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(ActorAction))]
public class Actor: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to an actor state.
                ActorAction.ActorState state = (ActorAction.ActorState)action;
                
                // Get the actor list from the current scene.
                state.ActorIndex = EditorGUILayout.Popup(
                    selectedIndex: state.ActorIndex,
                    displayedOptions: inspector.Owner.Header.EditedScene
                        .ActorList.Select(selector: actor => actor.Identifier).ToArray(),
                    label: "Actor"
                );
            }
    // --- /Methods ---
}
}