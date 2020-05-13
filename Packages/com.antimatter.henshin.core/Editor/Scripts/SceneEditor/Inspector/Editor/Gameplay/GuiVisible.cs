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
/// Class used to render the selected gui visible action in the inspector.
/// </summary>
[ActionEditor(actionType: typeof(GuiVisibleAction))]
public class GuiVisible: Base {
    // ---  Methods ---
        // -- Protected Methods --
            /// <inheritdoc cref="Base._Render"/>
            protected override void _Render(ActionState action, InspectorState inspector) {
                // Cast the action to a gui visible state.
                GuiVisibleAction.GuiVisibleState state = (GuiVisibleAction.GuiVisibleState)action;
                
                // Show the time.
                state.Time = EditorGUILayout.FloatField(label: "Time", value: state.Time);
                
                // If the value is greater than 0.
                if (state.Time > 0) {
                    // Show the easing function.
                    state.EaseMode = (EasingFunction.Ease)EditorGUILayout.EnumPopup(label: "Easing Function", selected: state.EaseMode);
                } else {
                    // Use the linear easing.
                    state.EaseMode = EasingFunction.Ease.Linear;
                }
                
                // Show the actor selector list.
                state.Actor = (GuiVisibleAction.FActorType)EditorGUILayout.EnumFlagsField(label: "Actor", enumValue: state.Actor);
                
                // Show the visible toggle.
                state.Visible = EditorGUILayout.Toggle(label: "Visible", value: state.Visible);
            }
    // --- /Methods ---
}
}