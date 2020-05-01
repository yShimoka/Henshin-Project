// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using UnityEditor;
using Enumerable = System.Linq.Enumerable;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.Graph {

/// <summary>
/// View class used to render <see cref="Henshin.Editor.State.Graph.Socket"/>s on the screen.
/// </summary>
public static class Socket {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
            // - Textures -
            /// <summary>Texture used by the empty socket objects.</summary>
            private static Texture2D _msEmptyTexture;
            /// <summary>Texture used by the filled socket objects.</summary>
            private static Texture2D _msFilledTexture;
            
            // - Contents -
            
            // - Styles -
            /// <summary>Style used to render the empty socket objects.</summary>
            private static GUIStyle _msEmptyStyle;
            /// <summary>Style used to render the filled socket objects.</summary>
            private static GUIStyle _msFilledStyle;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Initializes the view class.
            /// Loads all required objects.
            /// </summary>
            public static void Initialize() {
                // Create all the texture objects.
                Socket._CreateTextures();
                // Create all the content objects.
                Socket._CreateContents();
                // Create all the style objects.
                Socket._CreateStyles();
            }
            
            /// <summary>
            /// Renders the specified socket.
            /// </summary>
            /// <param name="socket">The socket object to render.</param>
            public static void RenderSocket(State.Graph.Socket socket) {
                // Draw a box where the socket is.
                GUI.Box(
                    position: socket.Rect,
                    content: GUIContent.none,
                    style: socket.Targets.Count > 0 || State.Graph.Socket.CurrentSource == socket ? Socket._msFilledStyle : Socket._msEmptyStyle
                );
            }
            
            /// <summary>
            /// Renders the specified socket's edges.
            /// </summary>
            /// <param name="socket">The socket to draw the edges of.</param>
            public static void RenderEdges(State.Graph.Socket socket) {
                // Draw a line to each of the targets.
                foreach (Vector2 inputPosition in Enumerable.Select(source: socket.Targets, selector: target => target.Input.Rect.center)) {
                    // Draw the line between the points.
                    Handles.DrawBezier(
                        startPosition: socket.Rect.center,
                        endPosition: inputPosition,
                        startTangent: socket.Rect.center + Vector2.right * 50 * (1 / socket.Owner.owner.scale),
                        endTangent: inputPosition + Vector2.left * 50 * (1 / socket.Owner.owner.scale),
                        color: Color.white, 
                        texture: Texture2D.whiteTexture, 
                        width: 3 * (1 / socket.Owner.owner.scale)
                    );
                }

                // If the socket is the current source.
                if (State.Graph.Socket.CurrentSource == socket) {
                    // Draw the line between the points.
                    Handles.DrawBezier(
                        startPosition: socket.Rect.center,
                        endPosition: Event.current.mousePosition,
                        startTangent: socket.Rect.center + Vector2.right * 50 * (1 / socket.Owner.owner.scale),
                        endTangent: Event.current.mousePosition + Vector2.left * 50 * (1 / socket.Owner.owner.scale),
                        color: Color.white, 
                        texture: Texture2D.whiteTexture, 
                        width: 3 * (1 / socket.Owner.owner.scale)
                    );
                }
            }
            
        // -- Private Methods --
            /// <summary>Creates all the required texture objects.</summary>
            private static void _CreateTextures() {
                // Load the empty texture.
                Socket._msEmptyTexture  = Resources.Load<Texture2D>(path: "Editor/Graph/UI_SOCKET_Empty");
                // Load the filled texture.
                Socket._msFilledTexture = Resources.Load<Texture2D>(path: "Editor/Graph/UI_SOCKET_Full");
            }
            
            /// <summary>Creates all the required <see cref="GUIContent"/> objects.</summary>
            private static void _CreateContents() {}
            
            /// <summary>Creates all the required <see cref="GUIStyle"/> objects.</summary>
            private static void _CreateStyles() {
                // Load the empty socket style.
                Socket._msEmptyStyle = new GUIStyle {
                    normal = { background = Socket._msEmptyTexture, textColor = Color.white }
                };
                // Load the filled socket style.
                Socket._msFilledStyle = new GUIStyle {
                    normal = { background = Socket._msFilledTexture, textColor = Color.white }
                };
            }
    // --- /Methods ---
}
}