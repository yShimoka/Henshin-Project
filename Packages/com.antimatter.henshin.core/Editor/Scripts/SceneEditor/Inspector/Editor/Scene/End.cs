// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Scene;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Scene {

/// <summary>
/// Editor class used to render the start action's editor.
/// </summary>
[ActionEditor(actionType: typeof(EndAction))]
public class End: Base {
    // ---  Methods ---
        // -- Public Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState state, InspectorState inspector) {
                // Draw a help box.
                EditorGUILayout.HelpBox(message: "There is nothing to edit on an end action.", type: MessageType.Info);
            }
    // --- /Methods ---
}
}