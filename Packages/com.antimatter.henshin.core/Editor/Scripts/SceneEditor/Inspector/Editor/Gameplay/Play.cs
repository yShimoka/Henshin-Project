// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actions.Gameplay;
using Henshin.Runtime.Gameplay;
using Henshin.Runtime.Libraries;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Gameplay {

/// <summary>
/// </summary>
[ActionEditor(actionType: typeof(PlayAction))]
public class Play: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Draw a help box.
                EditorGUILayout.HelpBox(message: "There is nothing to edit on a play action.", type: MessageType.Info);
            }
    // --- /Methods ---
}
}