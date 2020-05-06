// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Scene;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Scene {

/// <summary>
/// Editor class used to render the start action's editor.
/// </summary>
[ActionEditor(actionType: typeof(DelayAction))]
public class Delay: Base {
    // ---  Methods ---
        // -- Public Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState state, InspectorState inspector) {
                // Render the timed action.
                Base.Render<Timed>(action: state, inspector: inspector);
            }
    // --- /Methods ---
}
}