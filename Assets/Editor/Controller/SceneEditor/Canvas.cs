// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.SceneEditor {

/// <summary>
/// 
/// </summary>
public static class Canvas {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initialize the canvas controller.</summary>
            public static void Initialize() {
                // Attach to the scene change event.
                State.SceneEditor.Header.OnSceneChange.AddListener(call: Canvas._ReloadRenderArea);
                
                // Reload the render area.
                Canvas._ReloadRenderArea();
                
                // Initialize the render area view.
                View.Graph.RenderArea.Initialize();
            }
            
            /// <summary>
            /// Updates the state of the canvas.
            /// Called right before the view's Render method.
            /// </summary>
            /// <param name="container">The container's rect</param>
            public static void BeforeRender(Rect container) {
                // Update the rect of the canvas.
                State.SceneEditor.Canvas.Rect.Set(
                    x:      State.SceneEditor.Canvas.RATIO.x      * container.width,
                    y:      State.SceneEditor.Canvas.RATIO.y      * container.height,
                    width:  State.SceneEditor.Canvas.RATIO.width  * container.width,
                    height: State.SceneEditor.Canvas.RATIO.height * container.height
                );
                
                // Update the render area's rect.
                Controller.Graph.RenderArea.UpdateRect(containerRect: State.SceneEditor.Canvas.Rect, area: State.SceneEditor.Canvas.CurrentRenderArea);
            }
            
            /// <summary>
            /// Handles unity's <see cref="Event.current"/> object.
            /// Called right after the Render method.
            /// </summary>
            public static void AfterRender() {
                // Handle the events for the render area.
                Controller.Graph.RenderArea.HandleEvent(ev: Event.current, area: State.SceneEditor.Canvas.CurrentRenderArea);
            }
            
        // -- Private Methods --
            /// <summary>
            /// Reloads the <see cref="Henshin.Editor.State.Graph.RenderArea"/>.
            /// Loads it from the currently selected scene.
            /// </summary>
            private static void _ReloadRenderArea() {
                // Load the render area from the scene.
                State.SceneEditor.Canvas.CurrentRenderArea = Controller.Graph.RenderArea.Load(
                    scene: State.SceneEditor.Header.CurrentScene
                );
                
                // Initialize the render area's rect.
                Controller.Graph.RenderArea.InitializeRect(containerRect: State.SceneEditor.Canvas.Rect, area: State.SceneEditor.Canvas.CurrentRenderArea);
            }
    // --- /Methods ---
}
}