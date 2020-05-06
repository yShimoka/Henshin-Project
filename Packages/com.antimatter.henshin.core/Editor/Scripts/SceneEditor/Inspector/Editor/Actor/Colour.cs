// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Actor;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Actor {

/// <summary>
/// Inspector editor used to manipulate the colour actions.
/// </summary>
[ActionEditor(actionType: typeof(ColourAction))]
public class Colour: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a RotateAction state.
                ColourAction.ColourState state = (ColourAction.ColourState)action;
                
                // Call the base actor renderer.
                Base.Render<TimedActor>(action: action, inspector: inspector);
                
                // Render the target colour.
                state.Target = EditorGUILayout.ColorField(label: "Target", value: state.Target);
            }
    // --- /Methods ---
}
}