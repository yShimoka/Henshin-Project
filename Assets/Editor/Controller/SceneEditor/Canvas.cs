// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System.Linq;
using UnityEngine;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.SceneEditor {

/// <summary>
/// 
/// </summary>
public static class Canvas {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>Stores the last mouse's position.</summary>
            private static Vector2 _msLastMousePosition; 
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initialize the canvas controller.</summary>
            public static void Initialize() {
                // Attach to the scene change event.
                State.SceneEditor.Header.OnSceneChange.AddListener(call: Canvas.ReloadRenderArea);
                
                // Attach to the end of the edit mode event.
                EditorApplication.playModeStateChanged += change => {
                    if (change == PlayModeStateChange.ExitingEditMode) {
                        if (State.SceneEditor.Canvas.CurrentRenderArea != null ) {
                            Debug.Log("Saving");
                            Controller.Graph.RenderArea.Save(area: State.SceneEditor.Canvas.CurrentRenderArea);
                        } else {
                            Debug.LogError(message: "Current render area was null on edit exit.");
                        }
                    }
                };
                
                // Reload the render area.
                Canvas.ReloadRenderArea();
                
                // Initialize the generic menu.
                Canvas._CreateContextMenu();
                
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
                
                // If the current render area is unset.
                if (State.SceneEditor.Canvas.CurrentRenderArea == null) {
                    // Reload the render area.
                    //Canvas._ReloadRenderArea();
                }
                
                // Update the render area's rect.
                Controller.Graph.RenderArea.BeforeRender(containerRect: State.SceneEditor.Canvas.Rect, area: State.SceneEditor.Canvas.CurrentRenderArea);
            }
            
            /// <summary>
            /// Handles unity's <see cref="Event.current"/> object.
            /// Called right after the Render method.
            /// </summary>
            /// <param name="ev">The event to handle.</param>
            public static void AfterRender(Event ev) {
                // Handle the events for the render area.
                Controller.Graph.RenderArea.AfterRender(area: State.SceneEditor.Canvas.CurrentRenderArea);
                
                // If the event is a context click.
                if (ev.type == EventType.ContextClick) {
                    // Store the mouse's position.
                    Canvas._msLastMousePosition = ev.mousePosition - State.SceneEditor.Canvas.CurrentRenderArea.Position - State.SceneEditor.Canvas.Rect.position;
                    // Render the menu.
                    State.SceneEditor.Canvas.ContextMenu.ShowAsContext();
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Reloads the <see cref="Henshin.Editor.State.Graph.RenderArea"/>.
            /// Loads it from the currently selected scene.
            /// </summary>
            public static void ReloadRenderArea() {
                // Load the render area from the scene.
                State.SceneEditor.Canvas.CurrentRenderArea = Controller.Graph.RenderArea.Load(
                    scene: State.SceneEditor.Header.CurrentScene
                );
                
                // Initialize the render area's rect.
                Controller.Graph.RenderArea.InitializeRect(containerRect: State.SceneEditor.Canvas.Rect, area: State.SceneEditor.Canvas.CurrentRenderArea);
            }
            
            /// <summary>
            /// Creates the <see cref="GenericMenu"/> instance.
            /// </summary>
            private static void _CreateContextMenu() {
                // Create the instance.
                GenericMenu menu = new GenericMenu();
                
                // Allow transformation creation.
                menu.AddSeparator(path: "- Scene Transformations");
                menu.AddItem(content: new GUIContent{ text = "Start" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Scene.Start>(unique: true));
                menu.AddItem(content: new GUIContent{ text = "End" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Scene.End>(unique: true));
                menu.AddItem(content: new GUIContent{ text = "Delay" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Scene.Delay>(unique: false));
                menu.AddSeparator(path: "- Actor Transformations");
                menu.AddItem(content: new GUIContent{ text = "Move To" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Actor.MoveTo>(unique: false));
                menu.AddItem(content: new GUIContent{ text = "Scale" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Actor.Scale>(unique: false));
                menu.AddItem(content: new GUIContent{ text = "Colour" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Actor.Colour>(unique: false));
                menu.AddItem(content: new GUIContent{ text = "Visible" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Actor.Visible>(unique: false));
                
                // Store the instance.
                State.SceneEditor.Canvas.ContextMenu = menu;
            }
            
            /// <summary>
            /// Creates a new node object.
            /// </summary>
            private static void _CreateNode<T>(bool unique = false) where T: Henshin.Controller.Directions.Transformation {
                // Ensure that there is not another instance.
                if (unique && State.SceneEditor.Canvas.CurrentRenderArea.nodes.FirstOrDefault(predicate: node => node.Transformation.GetType() == typeof(T)) != null) {
                    // Log a warning.
                    Debug.LogWarning(message: "Duplicate instance found !");
                } else {
                    Henshin.Editor.State.Graph.RenderArea current = State.SceneEditor.Canvas.CurrentRenderArea;
                     
                    // Create a new transformation in the list.
                    current.describedScene.Transformations.Add(item: Henshin.Controller.Directions.Transformation.CreateTransformation<T>());
                    // Create a new node at the mouse position.
                    State.Graph.Node node = Controller.Graph.Node.CreateNode<T>(
                        transformationIndex: current.describedScene.Transformations.Count - 1, 
                        at: Canvas._msLastMousePosition, 
                        owner: current
                    );
                    node.position -= node.FullRect.size / 2;
                    
                    // Add the node to the list
                    current.nodes.Add(item: node);
                }
            }
    // --- /Methods ---
}
}