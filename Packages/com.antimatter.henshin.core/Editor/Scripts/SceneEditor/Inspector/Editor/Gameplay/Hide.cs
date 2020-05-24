// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Actions.Gameplay;
using Henshin.Runtime.Data;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Gameplay {

/// <summary>
/// Class used to render valid action in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(HideTextAction)), ActionEditor(actionType: typeof(HideToolsAction))]
public class Hide: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                Base.Render<Timed>(action: action, inspector: inspector);
            }
    // --- /Methods ---
}
}