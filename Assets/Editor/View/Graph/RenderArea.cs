// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.Graph {

/// <summary>
/// Renders a specified <see cref="State.Graph.RenderArea"/> state object.
/// </summary>
public static class RenderArea {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Texture Constants -
            /// <summary>Size of a single cell on the render texture.</summary>
            public const int TEXTURE_CELL_SIZE = 16;
            
            /// <summary>Number of cells across the render texture.</summary>
            public const int TEXTURE_CELL_COUNT = 5;
            
            /// <summary>Level of gray used by the texture's separator.</summary>
            public const float TEXTURE_SEPARATOR_GRAY = 0.2f;
            
            /// <summary>Level of gray used by the texture's background.</summary>
            public const float TEXTURE_BACKGROUND_GRAY = 0.5f;
            
        // -- Private Attributes --
            // - Textures -
            /// <summary>Texture object used for the background of the render area.</summary>
            private static Texture2D _msBackgroundTexture;
            
            // - Rects -
            /// <summary>Rect object used for the UV coordinates of the texture.</summary>
            private static Rect _msTextureUv = new Rect{ 
                x = 0, y = 0, 
                width  = (float) State.Graph.RenderArea.GUI_CELL_COUNT / RenderArea.TEXTURE_CELL_COUNT, 
                height = (float) State.Graph.RenderArea.GUI_CELL_COUNT / RenderArea.TEXTURE_CELL_COUNT
            }; 
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
            /// <summary>
            /// Initializes the render area class.
            /// Creates all required textures and GUIStyle objects.
            /// This method should be called on the first OnGUI call of the owner window.
            /// </summary>
            /// <exception cref="InvalidOperationException">Thrown if the view object is already initialized.</exception>
            public static void Initialize() {
                // Create the texture objects.
                RenderArea._CreateTextures();
                
                // Create the gui contents.
                RenderArea._CreateContents();
                
                // Create the styles.
                RenderArea._CreateStyles();
                
                // Initialize the node view class.
                Node.Initialize();
            }
            
            /// <summary>
            /// Renders the area using the <see cref="GUI"/> class.
            /// Renders all child node objects as well. 
            /// </summary>
            /// <param name="rendered">The state of the area to render.</param>
            public static void Render(State.Graph.RenderArea rendered) {
                // Render the texture.
                GUI.DrawTextureWithTexCoords(
                    position: rendered.Rect,
                    image: RenderArea._msBackgroundTexture,
                    texCoords: RenderArea._msTextureUv
                );
                
                // Render each of the nodes.
                foreach (State.Graph.Node node in rendered.nodes) {
                    View.Graph.Node.Render(node: node);
                }
                
                // Get all of the node's sockets.
                State.Graph.Socket[] sockets = rendered.nodes
                    .SelectMany(selector: node => new[] { node.Input, node.Output })
                    .Where(predicate: socket => socket != null)
                    .ToArray();
                // Get all the output sockets.
                State.Graph.Socket[] outputs = sockets.Where(predicate: socket => !socket.IsInput).ToArray();
                
                // Render the node's edges.
                foreach (State.Graph.Socket socket in outputs) {
                    View.Graph.Socket.RenderEdges(socket: socket);
                }
                // Render the node's sockets.
                foreach (State.Graph.Socket socket in sockets) {
                    View.Graph.Socket.RenderSocket(socket: socket);
                }
            }
            
        // -- Private Methods --
            // - Initialization -
            /// <summary>Creates all the <see cref="Texture"/>s required by the view.</summary>
            private static void _CreateTextures() {
                // Get the size of the texture object.
                const int TEX_SIZE = RenderArea.TEXTURE_CELL_SIZE * RenderArea.TEXTURE_CELL_COUNT;
                 
                // Create the background texture object.
                RenderArea._msBackgroundTexture = new Texture2D(
                    width: TEX_SIZE,
                    height: TEX_SIZE,
                    textureFormat: TextureFormat.RGB24,
                    mipChain: false
                ) {
                    wrapMode = TextureWrapMode.Repeat,
                    filterMode = FilterMode.Trilinear
                };
                
                // Create the separator and background textures.
                Color sepColor = new Color{ r = RenderArea.TEXTURE_SEPARATOR_GRAY , g = RenderArea.TEXTURE_SEPARATOR_GRAY , b = RenderArea.TEXTURE_SEPARATOR_GRAY  }; 
                Color bgColor  = new Color{ r = RenderArea.TEXTURE_BACKGROUND_GRAY, g = RenderArea.TEXTURE_BACKGROUND_GRAY, b = RenderArea.TEXTURE_BACKGROUND_GRAY }; 
                // Create all the pixels of the texture.
                for (int x = 0; x < TEX_SIZE; x++) for (int y = 0; y < TEX_SIZE; y++) {
                    // Check if the pixel is on the left border.
                    if (x == 1 || y == 1) {
                        RenderArea._msBackgroundTexture.SetPixel(x: x, y: y, color: sepColor);
                    // Check if the pixel is on a cell border.
                    } else if (x % RenderArea.TEXTURE_CELL_SIZE == 0 || y % RenderArea.TEXTURE_CELL_SIZE == 0) {
                        RenderArea._msBackgroundTexture.SetPixel(x: x, y: y, color: sepColor);
                    } else {
                        RenderArea._msBackgroundTexture.SetPixel(x: x, y: y, color: bgColor);
                    }
                }
                
                // Apply the texture info.
                RenderArea._msBackgroundTexture.Apply();
            }
            
            /// <summary>Creates all the <see cref="GUIContent"/>s required by the view.</summary>
            private static void _CreateContents() {}
            
            /// <summary>Creates all the <see cref="GUIStyle"/>s required by the view.</summary>
            private static void _CreateStyles() {}
    // --- /Methods ---
}
}