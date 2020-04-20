// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.SceneEditor {

/// <summary>
/// View class used to render the <see cref="Henshin.Editor.View.SceneEditor"/>'s inspector section.
/// Allows editing of the selected <see cref="Henshin.State.Directions.Scene"/> and <see cref="Henshin.State.Directions.Transformation"/>.
/// </summary>
public static class Inspector {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
            // - Constants -
            /// <summary></summary>
            private const float _mBACKGROUND_GRAY_LEVEL = 0.7f;
            
            // - Textures -
            /// <summary>Texture used for the inspector's background.</summary>
            private static Texture2D _msBackgroundTexture;
            
            // - Contents -
            
            // - Styles -
            /// <summary>Style used for the inspector's background.</summary>
            private static GUIStyle _msBackgroundStyle;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initialize the inspector view.</summary>
            public static void Initialize() {
                // Create all the required textures.
                Inspector._CreateTextures();
                // Create all the required GUIContent objects.
                Inspector._CreateContents();
                // Create all the required GUIStyle objects.
                Inspector._CreateStyles();
            }
            
            /// <summary>Renders the inspector window on the screen.</summary>
            public static void Render() {
                // Draw the inspector background.
                GUI.Box(position: State.SceneEditor.Inspector.Rect, content: GUIContent.none, style: Inspector._msBackgroundStyle);
                
                // Begin a new GUILayout area.
                GUILayout.BeginArea(screenRect: State.SceneEditor.Inspector.Rect);
                
                // Draw an info box.
                GUILayout.Space(pixels: 8);
                EditorGUILayout.HelpBox(message: "The inspector is not implemented yet.", type: MessageType.Info);
                
                // End the area.
                GUILayout.EndArea();
            }
            
        // -- Private Methods --
            /// <summary>Creates all the textures required for the inspector.</summary>
            private static void _CreateTextures() {
                // Create the background texture.
                Inspector._msBackgroundTexture = new Texture2D(width: 1, height: 1, textureFormat: TextureFormat.RGB24, mipChain: false);
                Inspector._msBackgroundTexture.SetPixel(
                    x: 0, y: 0, color: new Color(
                        r: Inspector._mBACKGROUND_GRAY_LEVEL, 
                        g: Inspector._mBACKGROUND_GRAY_LEVEL, 
                        b: Inspector._mBACKGROUND_GRAY_LEVEL 
                    )
                );
                Inspector._msBackgroundTexture.wrapMode = TextureWrapMode.Repeat;
                Inspector._msBackgroundTexture.filterMode = FilterMode.Point;
                Inspector._msBackgroundTexture.Apply();
            }
            
            /// <summary>Creates all the <see cref="GUIContent"/>s required for the inspector.</summary>
            private static void _CreateContents() {}
            
            /// <summary>Creates all the <see cref="GUIStyle"/>s required for the inspector.</summary>
            private static void _CreateStyles() {
                // Create the background style.
                Inspector._msBackgroundStyle = new GUIStyle {
                    normal = { background = Inspector._msBackgroundTexture },
                };
            }
    // --- /Methods ---
}
}