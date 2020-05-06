// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Actor;
using Henshin.Runtime.Actions.Base;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Actor {

/// <summary>
/// Class used to render the selected visible action in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(VisibleAction))]
public class Visible: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a visible action.
                VisibleAction.VisibleState state = (VisibleAction.VisibleState)action;
                
                // Render the all actors toggle box.
                state.AllActors = EditorGUILayout.Toggle(label: "All Actors", value: state.AllActors);
                
                // If the all actors toggle is unset.
                if (!state.AllActors) {
                    // Render the actor selector.
                    Base.Render<Actor>(action: action, inspector: inspector);
                }
                
                // Render the set visible toggle box.
                state.SetVisible = EditorGUILayout.Toggle(label: "Set Visible", value: state.SetVisible);
            }
    // --- /Methods ---
}
}