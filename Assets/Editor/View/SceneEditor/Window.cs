// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.View.SceneEditor {

/// <summary>
/// View class used to draw the contents of the <see cref="State.SceneEditor.Window"/> state.
/// </summary>
public static class Window {
    // ---  Attributes ---
        // -- Public Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initialize the window's view objects.</summary>
            public static void Initialize() {
                // Initialize the header.
                Header.Initialize();
                
                // Initialize the inspector.
                Inspector.Initialize();
                
                // Initialize the canvas.
                Canvas.Initialize();
            }
            
            /// <summary>Renders the window's contents according to the <see cref="State.SceneEditor.Window"/> object.</summary>
            public static void Render() {
                // If the header found no scene.
                if (State.SceneEditor.Header.SceneList.Count == 0) {
                    // Draw an error message.
                    EditorGUILayout.HelpBox(message: "There is no Scene object in the project !", type: MessageType.Info);
                } else {
                    // Render the header.
                    Header.Render();
                    
                    // Render the inspector.
                    Inspector.Render();
                    
                    // Render the canvas.
                    Canvas.Render();
                }
            }
        // -- Private Methods --
    // --- /Methods ---
}
}