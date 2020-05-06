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
/// Class used to render the layer action.
/// </summary>
[ActionEditor(actionType: typeof(LayerAction))]
public class Layer: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a visible action.
                LayerAction.LayerState state = (LayerAction.LayerState)action;
                
                // Render the actor selector.
                Base.Render<Actor>(action: action, inspector: inspector);
                
                // Render the layer index popup.
                int targetLayer = EditorGUILayout.Popup(
                    selectedIndex: state.LayerIndex,
                    displayedOptions: ApplicationView.SortingLayers.LayerNames,
                    label: "Layer"
                );
                state.LayerIndex = ApplicationView.SortingLayers.LayerIDs[targetLayer];
            }
    // --- /Methods ---
}
}