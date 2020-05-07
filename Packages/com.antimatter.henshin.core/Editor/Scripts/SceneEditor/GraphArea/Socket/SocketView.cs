// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Editor.Skin;
using Henshin.Runtime.Actions.Scene;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea.Socket {

/// <summary>
/// Static view class used to render <see cref="SocketState"/> objects.
/// </summary>
public static class SocketView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders the specified socket on screen.
            /// </summary>
            /// <param name="socket">The socket object to draw.</param>
            public static void Render(SocketState socket) {
                // Check if the socket should be rendered.
                if (socket.IsEnabled) {
                    // Draw the socket.
                    GUI.Box(
                        position: socket.Rect,
                        content: GUIContent.none,
                        style: SocketView._IsFilled(socket: socket) ?
                            SkinState.Styles.FilledSocket :
                            SkinState.Styles.EmptySocket
                    );
                }
            }
            
            /// <summary>
            /// Renders an edge of the graph.
            /// </summary>
            public static void RenderEdge(Vector2 from, Vector2 to, float scale) {
                // Draw the edge.
                Handles.DrawBezier(
                    startPosition: from,
                    endPosition: to,
                    startTangent: from + Vector2.right * 50 * scale,
                    endTangent: to + Vector2.left * 50 * scale,
                    color: Color.white,
                    texture: Texture2D.whiteTexture, 
                    width: 3 * scale
                );
            }
            
        // -- Private Methods --
    /// <summary>
            /// Checks if the socket should be filled or empty.
            /// </summary>
            /// <param name="socket">The socket to check.</param>
            /// <returns>True if the socket should be filled.</returns>
            private static bool _IsFilled(SocketState socket) {
                // Check the kind of the socket.
                if (socket.IsInput) {
                    // Check if any parent of the socket is set.
                    return socket.Owner.ParentList.Count > 0;
                } else {
                    // Check if any child of the socket is set.
                    return socket.Owner.ChildrenList.Count > 0;
                }
            }
    // --- /Methods ---
}
}