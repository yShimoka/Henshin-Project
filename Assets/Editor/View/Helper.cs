// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View {

/// <summary>
/// Helper class used by all the view classes.
/// </summary>
public static class Helper {
    // ---  Attributes ---
        // -- Public Attributes --
            public static GUIStyle NoWidthExpansion  = new GUIStyle{ stretchWidth  = false };
            public static GUIStyle NoHeightExpansion = new GUIStyle{ stretchHeight = false };
    // --- /Attributes ---

    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Generates a new texture with rounded edges.
            /// </summary>
            /// <returns>The newly created texture.</returns>
            public static Texture2D MakeRoundedRectangleTexture(Vector2 size, float radius, Color content) {
                // Wrap the make rounded rectangle border method.
                return Helper.MakeRoundedRectangleTexture(size: size, radius: radius, content: content, border: 0, borderColor: Color.red);
            }
            
            public static Texture2D MakeRoundedRectangleTexture(Vector2 size, float radius, Color content, int border, Color borderColor) {
                // Generate a new texture.
                Texture2D texture = new Texture2D(width: (int) size.x, height: (int) size.y, textureFormat: TextureFormat.ARGB32, mipChain: false);
                
                // Create some handy helper variables.
                int w = (int) size.x;
                int h = (int) size.y;
                float r = radius;
                int b = border; 
                
                // Set all the pixels of the texture.
                for (int x = 0; x < w; x++) for (int y = 0; y < h; y++) {
                    // If we are in the bottom left corner.
                    if (x < r && y < r) {
                        // Get the value of x + y.
                        float d = Mathf.Sqrt(f: Mathf.Pow(f: r - x, p: 2) + Mathf.Pow(f: r - y, p: 2));
                        
                        // Set the pixel of the texture.
                        texture.SetPixel(x: x, y: y, color: d <= r - b ? content : d < r ? borderColor : Color.clear);
                        
                    // If we are in the bottom right corner.
                    } else if (x > w - r && y < r) {
                        // Get the value of x + y.
                        float d = Mathf.Sqrt(f: Mathf.Pow(f: r - (w - x), p: 2) + Mathf.Pow(f: r - y, p: 2));
                        
                        // Set the pixel of the texture.
                        texture.SetPixel(x: x, y: y, color: d <= r - b ? content : d < r ? borderColor : Color.clear);
                        
                    // If we are in the top left corner.
                    } else if (x < r && y > h - r) {
                        // Get the value of x + y.
                        float d = Mathf.Sqrt(f: Mathf.Pow(f: r - x, p: 2) + Mathf.Pow(f: r - (h - y), p: 2));
                        
                        // Set the pixel of the texture.
                        texture.SetPixel(x: x, y: y, color: d <= r - b ? content : d < r ? borderColor : Color.clear);
                        
                    // If we are in the top right corner.
                    } else if (x > w - r && y > h - r) {
                        // Get the value of x + y.
                        float d = Mathf.Sqrt(f: Mathf.Pow(f: r - (w - x), p: 2) + Mathf.Pow(f: r - (h - y), p: 2));
                        
                        // Set the pixel of the texture.
                        texture.SetPixel(x: x, y: y, color: d <= r - b ? content : d < r ? borderColor : Color.clear);
                        
                    // If we are on the border.
                    } else if (x < b || x >= w - b || y < b || y >= h - b) {
                        // Set the pixel of the texture.
                        texture.SetPixel(x: x, y: y, color: borderColor);
                        
                    // If whe are in the center.
                    } else {
                        // Set the pixel of the texture.
                        texture.SetPixel(x: x, y: y, color: content);
                    }
                }
                // Apply the changes.
                texture.Apply();
                
                // Return the texture.
                return texture;
            }
        // -- Private Methods --
    // --- /Methods ---
}
}