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
/// Class used to render v action in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(InvalidAction))]
public class Invalid: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                EditorGUILayout.HelpBox(message: "There is nothing to edit on an invalid action", type: MessageType.Info);
            }
    // --- /Methods ---
}
}