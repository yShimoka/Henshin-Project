// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Actor;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Application;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Actor {

/// <summary>
/// Class used to render the flip action.
/// </summary>
[ActionEditor(actionType: typeof(FlipAction))]
public class Flip: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a visible action.
                FlipAction.FlipState state = (FlipAction.FlipState)action;
                
                // Render the actor selector.
                Base.Render<Actor>(action: action, inspector: inspector);
                
                // Render the flip toggles.
                state.Vertical = EditorGUILayout.Toggle(label: "Vertical Flip", value: state.Vertical);
                state.Horizontal = EditorGUILayout.Toggle(label: "Horizontal Flip", value: state.Horizontal);
            }
    // --- /Methods ---
}
}