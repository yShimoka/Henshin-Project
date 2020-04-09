// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using Henshin.Core.Scene.Scenery.Transformation;
using Henshin.Editor.App;
using Henshin.Editor.Misc;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Scene.Transformation {

/// <summary>
/// Editor window used to create and modify <see cref="Scene"/>, <see cref="Core.Scene.Directions.Line"/> and <see cref="Base"/> objects.
/// Renders the <see cref="Base"/> transformations into a blueprint-like structure.
/// </summary>
public class TransformationEditorWindow: EditorWindow {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>Unity event fired when the window should be rendered.</summary>
            private void OnGUI() {
                // Update the window rect.
                TransformationEditorState.WindowRect = this.position;
                
                // Draw the header.
                TransformationEditorHeader.OnGUI();
                
                // Draw the inspector.
                TransformationEditorInspector.OnGUI();
                
                // Draw the canvas.
                TransformationEditorCanvas.OnGUI();
            }
            
            /// <summary>Opens the transformation window.</summary>
            [MenuItem(itemName: "Henshin/Transformation Editor")]
            private static void _OpenEditor() {
                TransformationEditorWindow window;
                // Check if the window already exists.
                if (EditorWindow.HasOpenInstances<TransformationEditorWindow>()) {
                    // Get the editor window.
                    window = EditorWindow.GetWindow<TransformationEditorWindow>();
                    
                    // Take the focus.
                    window.Focus();
                } else {
                    // Create a new window.
                    window = EditorWindow.CreateWindow<TransformationEditorWindow>();

                    // Update the window's title and icon.
                    window.titleContent = new GUIContent {
                        text = nameof(TransformationEditorWindow),
                        tooltip = "Window used to edit the application's transformations.",
                        image = TextureStore.TransformationEditor
                    };
                }
            }
            
        // -- Public Methods --
            /// <summary>Repaints the window instance.</summary>
            public new static void Repaint() {
                // Check if the window already exists.
                if (EditorWindow.HasOpenInstances<TransformationEditorWindow>()) {
                    // Repaint the window.
                    ((EditorWindow) EditorWindow.GetWindow<TransformationEditorWindow>()).Repaint();
                }
            }
        // -- Private Methods --
    // --- /Methods ---
}
}