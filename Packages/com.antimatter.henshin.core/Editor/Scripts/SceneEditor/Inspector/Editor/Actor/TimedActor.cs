// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Linq;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Base;
using Henshin.Runtime.Libraries;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Inspector.Editor.Actor {

/// <summary>
/// Class used to render the selected actor in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(TimedActorAction))]
public class TimedActor: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to an actor state.
                TimedActorAction.TimedActorState state = (TimedActorAction.TimedActorState)action;
                
                // Render the time.
                Base.Render<Timed>(action: action, inspector: inspector);
                
                // Check if the time is 0.
                if (state.Time != 0) {
                    // Render the easing function.
                    state.EasingMode = (EasingFunction.Ease)EditorGUILayout.EnumPopup(
                        selected: state.EasingMode, label: "Easing Function"
                    );
                } else {
                    // Set the easing mode to linear.
                    state.EasingMode = EasingFunction.Ease.Linear;
                }
                
                // Get the actor list from the current scene.
                state.ActorIndex = EditorGUILayout.Popup(
                    selectedIndex: state.ActorIndex,
                    displayedOptions: inspector.Owner.Header.EditedScene
                        .ActorList.Select(selector: actor => actor.Identifier).ToArray(),
                    label: "Actor"
                );
            }
    // --- /Methods ---
}
}