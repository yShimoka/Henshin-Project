// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.SceneEditor {

/// <summary>
/// Controller class used to manipulate <see cref="State.SceneEditor.Window"/> objects.
/// Inherits from <see cref="EditorWindow"/>, allowing creation and manipulation of a Unity Editor window.
/// </summary>
public class Window: EditorWindow {
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>Unity event fired when the window should be rendered.</summary>
            private void OnGUI() {
                // If this is the first call to OnGUI.
                if (!State.SceneEditor.Window.WasRenderedOnce) {
                    // Initialize the repaint callback.
                    Graph.RenderArea.ON_SHOULD_REPAINT.AddListener(call: this.Repaint);
                    
                    // Initialize the header.
                    Header.Initialize();
                    // Initialize the inspector.
                    Inspector.Initialize();
                    // Initialize the canvas.
                    Canvas.Initialize();
                    
                    // Initialize the view.
                    View.SceneEditor.Window.Initialize();
                }
                
                // If the canvas is unset.
                if (State.SceneEditor.Canvas.CurrentRenderArea == null) {
                    Controller.SceneEditor.Canvas.ReloadRenderArea();
                }
                
                // Update the state's rect.
                State.SceneEditor.Window.Rect.Set(x: this.position.x, y: this.position.y, width: this.position.width, height: this.position.height);
                
                // Update the header's rect.
                Header.UpdateRect(container: State.SceneEditor.Window.Rect);
                // Prepare the inspector.
                Inspector.BeforeRender(container: State.SceneEditor.Window.Rect);
                // Prepare the canvas.
                Canvas.BeforeRender(container: State.SceneEditor.Window.Rect);
                
                // Render the view.
                View.SceneEditor.Window.Render();
                
                // Handle the canvas events.
                Canvas.AfterRender(ev: Event.current);
            }
            
            /// <summary>Opens the window, if it does not already exist.</summary>
            [MenuItem(itemName: "Window/Henshin Scene Editor")]
            private static void OpenWindow() {
                // Check if there already is an opened window.
                if (EditorWindow.HasOpenInstances<Window>()) {
                    // Load the window and put it in focus.
                    EditorWindow.GetWindow<Window>().Focus();
                } else {
                    // Create a new window instance.
                    Window window = EditorWindow.CreateWindow<Window>(title: null, typeof(SceneView));
                    
                    // Set its title.
                    window.titleContent = new GUIContent {
                        text = "Henshin Scene Editor",
                        image = Resources.Load<Texture2D>(path: "Editor/Icons/UI_EDITOR_Editor"),
                        tooltip = "The Henshin Project's visual scene editor."
                    };
                }
            }
    // --- /Methods ---
}
}