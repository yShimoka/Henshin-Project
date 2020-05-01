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

                switch (ev.type) {
                // If the event is a context click.
                case EventType.ContextClick:
                    // Store the mouse's position.
                    Canvas._msLastMousePosition = State.SceneEditor.Canvas.CurrentRenderArea.scale *(ev.mousePosition - State.SceneEditor.Canvas.CurrentRenderArea.Position - State.SceneEditor.Canvas.Rect.position);
                    // Render the menu.
                    State.SceneEditor.Canvas.ContextMenu.ShowAsContext();
                    // Re-render the area.
                    Controller.Graph.RenderArea.ON_SHOULD_REPAINT.Invoke();
                    break;
                // If the mouse was released.
                case EventType.MouseUp:
                    // Clear the socket.
                    State.Graph.Socket.CurrentSource = null;
                    // Re-render the area.
                    Controller.Graph.RenderArea.ON_SHOULD_REPAINT.Invoke();
                    break;
                }
            }
            
            /// <summary>
            /// Centers the render area in its container rect.
            /// </summary>
            public static void Center(State.Graph.RenderArea area) {
                area.position = area.ContainerRect.center;
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
                menu.AddItem(content: new GUIContent{ text = "Pose" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Actor.Pose>(unique: false));
                menu.AddItem(content: new GUIContent{ text = "Flip" }, on: false, func: () => Canvas._CreateNode<Henshin.Controller.Directions.Transformations.Actor.Flip>(unique: false));
                
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
                    node.position -= State.Graph.Node.NODE_SIZE / 2f * current.scale;
                    node.position.Set(
                        newX: Mathf.FloorToInt(f: node.position.x / State.Graph.RenderArea.GUI_CELL_SIZE),  
                        newY: Mathf.FloorToInt(f: node.position.y / State.Graph.RenderArea.GUI_CELL_SIZE)
                    );
                    
                    // Add the node to the list
                    current.nodes.Add(item: node);
                    
                    // If there is a selected socket.
                    if (State.Graph.Socket.CurrentSource != null) {
                        // If the node has an input.
                        if (node.Input != null) {
                            // Bind the nodes.
                            State.Graph.Socket.CurrentSource.Owner.Output.Targets.Add(item: node);
                            node.Input.Targets.Add(item: State.Graph.Socket.CurrentSource.Owner);
                        }
                        
                        // Clear the current source.
                        State.Graph.Socket.CurrentSource = null;
                    }
                }
            }
    // --- /Methods ---
}
}