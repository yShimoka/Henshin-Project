// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Editor.SceneEditor.GraphArea.Node;
using Henshin.Editor.SceneEditor.GraphArea.Socket;
using Henshin.Editor.Skin;
using UnityEditor;
using UnityEngine;

namespace Henshin.Editor.SceneEditor.GraphArea {

/// <summary>
/// Static class used to render the <see cref="GraphAreaState"/> objects.
/// Draws all the the <see cref="GraphAreaState"/>'s nodes and sockets as well.
/// </summary>
public static class GraphAreaView {
    // ---  Attributes ---
        // -- Public Constants --
            // - Texture Components -
            /// <summary>
            /// Number of cells on a single texture.
            /// </summary>
            public const int TEXTURE_CELL_COUNT = 4;
            
            /// <summary>
            /// Scale of the texture.
            /// </summary>
            public const int TEXTURE_SCALE = 2;
            
            /// <summary>
            /// Total size of the texture.
            /// </summary>
            public const int TEXTURE_SIZE = GraphAreaView.TEXTURE_CELL_COUNT * GraphAreaState.CELL_SIZE * GraphAreaView.TEXTURE_SCALE;
            
        // -- Private Attributes --
            // - Texture -
            /// <summary>
            /// The texture used to draw the background of the 
            /// </summary>
            private static Texture2D _msAreaTexture;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - Renderers -
            /// <summary>
            /// Render the graph area.
            /// </summary>
            public static void Render(GraphAreaState graphArea) {
                // Check if the texture is generated.
                GraphAreaView._GenerateTexture();
                
                // Ensure that the area is set.
                if (graphArea != null) {
                    // Start a new area.
                    GUILayout.BeginArea(screenRect: graphArea.RenderRect);
                    
                    // Draw the background texture.
                    GUI.DrawTextureWithTexCoords(
                        position: graphArea.GraphRect,
                        image: GraphAreaView._msAreaTexture,
                        texCoords: graphArea.TextureRect
                    );
                    
                    // Draw all the nodes.
                    foreach (NodeState nodeState in graphArea.NodeList) {
                        NodeView.Render(node: nodeState);
                    }
                    
                    // Draw all the edges.
                    foreach (NodeState nodeState in graphArea.NodeList) {
                        // Loop through all the node's children
                        foreach (NodeState child in nodeState.ChildrenList) {
                            SocketView.RenderEdge(
                                from: nodeState.Output.Rect.center, 
                                to: child.Input.Rect.center,
                                scale: graphArea.Scale
                            );
                        }
                    }
                    // Draw the current edge.
                    if (SocketState.Manipulated != null) {
                        // Check if the socket is an input or an output.
                        if (SocketState.Manipulated.IsInput) {
                            // Draw the edge from the mouse.
                            SocketView.RenderEdge(
                                from: GraphAreaController.CurrentMousePosition, 
                                to: SocketState.Manipulated.Rect.center,
                                scale: graphArea.Scale
                            );
                        } else {
                            // Draw the edge to the mouse.
                            SocketView.RenderEdge(
                                from: SocketState.Manipulated.Rect.center, 
                                to: GraphAreaController.CurrentMousePosition,
                                scale: graphArea.Scale
                            );
                        }
                    }
                    
                    // Draw the scroll bars.
                    GraphAreaView._DrawScrollbars(graphArea: graphArea);
                    
                    // End the area.
                    GUILayout.EndArea();
                } else {
                    // Draw an error message.
                    EditorGUILayout.HelpBox(message: "There is no GraphArea to render here !", type: MessageType.Error);
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Generates the texture used for the background of the GraphArea.
            /// </summary>
            private static void _GenerateTexture() {
                // Check if the texture is already set.
                if (GraphAreaView._msAreaTexture != null) {
                    // Stop the method.
                    return;
                }
                
                // Create the texture object.
                Texture2D tex = new Texture2D(
                    width:  GraphAreaView.TEXTURE_SIZE, 
                    height: GraphAreaView.TEXTURE_SIZE,
                    textureFormat: TextureFormat.RGB24, 
                    mipChain: false
                );
                
                // Set the pixels of the texture.
                for (int x = 0; x < GraphAreaView.TEXTURE_SIZE; x++) 
                for (int y = 0; y < GraphAreaView.TEXTURE_SIZE; y++) {
                    // Check if we are on a border.
                    if (
                        x / GraphAreaView.TEXTURE_SCALE % GraphAreaState.CELL_SIZE == 0 ||
                        y / GraphAreaView.TEXTURE_SCALE % GraphAreaState.CELL_SIZE == 0 ||
                        x / GraphAreaView.TEXTURE_SCALE == 1 ||
                        y / GraphAreaView.TEXTURE_SCALE == 1
                    ) {
                        // Set the pixel as a border pixel.
                        tex.SetPixel(x: x, y: y, color: SkinState.Textures.SceneGraphSeparatorColor);
                    } else {
                        // Set the pixel as a bg pixel.
                        tex.SetPixel(x: x, y: y, color: SkinState.Textures.SceneGraphBackgroundColor);
                    }
                }
                
                // Set the filtering mode of the texture.
                tex.filterMode = FilterMode.Bilinear;
                tex.wrapMode = TextureWrapMode.Repeat;
                
                // Apply the changes.
                tex.Apply();
                
                // Save the texture.
                GraphAreaView._msAreaTexture = tex;
            }
            
            /// <summary>
            /// Draws the scrollbars of the specified <see cref="GraphAreaState"/>.
            /// </summary>
            /// <param name="graphArea">The graph area to render.</param>
            private static void _DrawScrollbars(GraphAreaState graphArea) {
                const int BG_SIZE = 10;
                const int BAR_WIDTH = BG_SIZE - 4;
                const int BAR_LENGTH = 64;
                
                // Compute the rect of the right scrollbar background.
                graphArea.ScrollbarRect.Set(
                    x: graphArea.RenderRect.width - BG_SIZE, 
                    y: 0, 
                    width: BG_SIZE, 
                    height: graphArea.RenderRect.height
                );
                // Draw the right scrollbar background.
                GUI.Box(
                    position: graphArea.ScrollbarRect, 
                    content: GUIContent.none, 
                    style: SkinState.Styles.SceneGraphScrollbarBackground
                );
                
                // Compute the rect of the right scrollbar.
                graphArea.ScrollbarRect.Set(
                    x: graphArea.RenderRect.width - BG_SIZE + 2,
                    y: graphArea.ScrollPosition.y * (graphArea.RenderRect.height - BAR_LENGTH - BG_SIZE),
                    width: BAR_WIDTH,
                    height: BAR_LENGTH
                );
                // Draw the right scrollbar.
                GUI.Box(
                    position: graphArea.ScrollbarRect, 
                    content: GUIContent.none, 
                    style: SkinState.Styles.SceneGraphScrollbar
                );
                
                // Compute the rect of the bottom scrollbar background.
                graphArea.ScrollbarRect.Set(
                    x: 0, 
                    y: graphArea.RenderRect.height - BG_SIZE, 
                    width: graphArea.RenderRect.width - BG_SIZE,
                    height: BG_SIZE 
                );
                // Draw the bottom scrollbar background.
                GUI.Box(
                    position: graphArea.ScrollbarRect, 
                    content: GUIContent.none, 
                    style: SkinState.Styles.SceneGraphScrollbarBackground
                );
                
                // Compute the rect of the bottom scrollbar.
                graphArea.ScrollbarRect.Set(
                    x: graphArea.ScrollPosition.x * (graphArea.RenderRect.width - BAR_LENGTH - BG_SIZE),
                    y: graphArea.RenderRect.height - BG_SIZE + 2,
                    width: BAR_LENGTH,
                    height: BAR_WIDTH
                );
                // Draw the bottom scrollbar.
                GUI.Box(
                    position: graphArea.ScrollbarRect, 
                    content: GUIContent.none, 
                    style: SkinState.Styles.SceneGraphScrollbar
                );
                
                // Reset the scrollbar rect.
                graphArea.ScrollbarRect.Set(x: 0, y: 0, width: 0, height: 0);
            }
    // --- /Methods ---
}
}