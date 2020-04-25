// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.SceneEditor {

/// <summary>
/// View class used to render the <see cref="Henshin.Editor.View.SceneEditor"/>'s graph canvas.
/// Draws the <see cref="Henshin.Editor.View.Graph.RenderArea"/> corresponding to the current scene.
/// </summary>
public static class Canvas {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initialize the canvas view.</summary>
            public static void Initialize() {
                // Create all the required textures.
                Canvas._CreateTextures();
                // Create all the required GUIContent objects.
                Canvas._CreateContents();
                // Create all the required GUIStyle objects.
                Canvas._CreateStyles();
            }
            
            /// <summary>Renders the canvas view on the screen.</summary>
            public static void Render() {
                // Start a new GUI area.
                GUILayout.BeginArea(screenRect: State.SceneEditor.Canvas.Rect);
                
                // Render the render area.
                if (State.SceneEditor.Canvas.CurrentRenderArea != null) {
                    View.Graph.RenderArea.Render(rendered: State.SceneEditor.Canvas.CurrentRenderArea);
                } else {
                    EditorGUILayout.HelpBox(message: "There is no area to render !", type: MessageType.Error);
                }
                
                // End the gui area.
                GUILayout.EndArea();
            }
            
        // -- Private Methods --
            /// <summary>Creates all the textures required for the canvas.</summary>
            private static void _CreateTextures() { }
            
            /// <summary>Creates all the <see cref="GUIContent"/>s required for the canvas.</summary>
            private static void _CreateContents() {}
            
            /// <summary>Creates all the <see cref="GUIStyle"/>s required for the canvas.</summary>
            private static void _CreateStyles() { }
    // --- /Methods ---
}
}