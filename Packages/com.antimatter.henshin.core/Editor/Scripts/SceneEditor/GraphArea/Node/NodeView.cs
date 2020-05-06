// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Editor.SceneEditor.GraphArea.Socket;
using Henshin.Editor.Skin;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Actions.Scene;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea.Node {

/// <summary>
/// Class used to render the <see cref="NodeState"/> objects.
/// </summary>
public static class NodeView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders the specified <see cref="NodeState"/> object.
            /// </summary>
            /// <param name="node">The node object to render.</param>
            public static void Render(NodeState node) {
                // Draw the node's header.
                GUI.Box(
                    position: node.HeaderRect,
                    content: GUIContent.none,
                    style: NodeView._FindHeaderStyle(action: node.Action)
                );
                
                // Draw the node's body.
                GUI.Box(
                    position: node.BodyRect,
                    content: GUIContent.none,
                    style: SkinState.Styles.NodeBody
                );
                
                // Draw the node's name.
                SkinState.Styles.NodeHeaderText.fontSize = Mathf.FloorToInt(f: 12 * node.Owner.Scale);
                GUI.Label(
                    position: node.HeaderRect,
                    text: node.Action?.ControllerType.Name.Replace(oldValue: "Action", newValue: ""),
                    style: SkinState.Styles.NodeHeaderText
                );
                
                // Draw the node's sockets.
                SocketView.Render(socket: node.Input);
                SocketView.Render(socket: node.Output);
            }
            
        // -- Private Methods --
            /// <summary>
            /// Finds the <see cref="GUIStyle"/> object that corresponds to the specified controller name.
            /// </summary>
            /// <param name="action">The action to get the style for.</param>
            /// <returns>The style for the specified controller type.</returns>
            private static GUIStyle _FindHeaderStyle(ActionState action) {
                try {
                    // Check the type of the action.
                    switch (ActionController.GetCategory(action: action)) {
                    case ActionController.EActionCategory.Start:
                        return SkinState.Styles.StartNodeHeader;
                    case ActionController.EActionCategory.End:
                        return SkinState.Styles.EndNodeHeader;
                    case ActionController.EActionCategory.Gameplay:
                        return SkinState.Styles.GameplayNodeHeader;
                    case ActionController.EActionCategory.Actor:
                        return SkinState.Styles.ActorNodeHeader;
                    case ActionController.EActionCategory.Scene:
                        return SkinState.Styles.SceneNodeHeader;
                    default:
                        // Log an error.
                        Debug.LogError(message: $"Could not find the style to apply to the type {action.ActionControllerName}");
                        return GUIStyle.none;
                    }
                } catch(InvalidOperationException) {
                    // Log an error.
                    Debug.LogError(message: $"Could not find the style to apply to the type {action.ActionControllerName}");
                    return GUIStyle.none;
                } catch(NullReferenceException) {
                    // Log an error.
                    Debug.LogError(message: "Tried to get the header style for a null action.");
                    return GUIStyle.none;
                }
            }
    // --- /Methods ---
}
}