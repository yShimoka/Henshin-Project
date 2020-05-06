// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actions.Scene;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor {

/// <summary>
/// Editor class used to render the start action's editor.
/// </summary>
[ActionEditor(actionType: typeof(TimedAction))]
public class Timed: Base {
    // ---  Methods ---
        // -- Public Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState state, InspectorState inspector) {
                // Cast the state.
                TimedAction.TimedState timedState = (TimedAction.TimedState)state;
                
                // Draw the time of the delay.
                timedState.Time = EditorGUILayout.FloatField(label: "Time", value: timedState.Time);
            }
    // --- /Methods ---
}
}