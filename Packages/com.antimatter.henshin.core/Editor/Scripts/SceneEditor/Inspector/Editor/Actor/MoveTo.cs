// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Actor;
using Henshin.Runtime.Libraries;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Actor {

/// <summary>
/// Inspector editor used to manipulate the move to action.
/// </summary>
[ActionEditor(actionType: typeof(MoveToAction))]
public class MoveTo: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a MoveToAction state.
                MoveToAction.MoveToState state = (MoveToAction.MoveToState)action;
                
                // Call the base actor renderer.
                Base.Render<TimedActor>(action: action, inspector: inspector);
                
                // Render the position vector.
                state.Target = EditorGUILayout.Vector2Field(label: "Target", value: state.Target);
            }
    // --- /Methods ---
}
}