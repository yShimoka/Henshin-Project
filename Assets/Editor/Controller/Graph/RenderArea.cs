// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Controller.Graph {

/// <summary>
/// Manipulates <see cref="State.Graph.RenderArea"/> objects.
/// </summary>
public static class RenderArea {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Event that gets triggered by the controller when the container window should be repainted.</summary>
            public static readonly UnityEvent ON_SHOULD_REPAINT = new UnityEvent();
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>Initializes the render area controller.</summary>
            /// <param name="containerRect">The rect of the container into which the area should be rendered.</param>
            /// <param name="area">The render area to initialize.</param>
            public static void InitializeRect(Rect containerRect, State.Graph.RenderArea area) {
                // Set the position and size of the area.
                area.Rect.Set(
                    x:       containerRect.width  / 2 - (float)State.Graph.RenderArea.GUI_CELL_COUNT * State.Graph.RenderArea.GUI_CELL_SIZE / 2, 
                    y:       containerRect.height / 2 - (float)State.Graph.RenderArea.GUI_CELL_COUNT * State.Graph.RenderArea.GUI_CELL_SIZE / 2, 
                    width:   State.Graph.RenderArea.GUI_CELL_COUNT * State.Graph.RenderArea.GUI_CELL_SIZE,
                    height:  State.Graph.RenderArea.GUI_CELL_COUNT * State.Graph.RenderArea.GUI_CELL_SIZE
                );
                area.ContainerRect.Set(x: 0, y: 0, width: containerRect.width, height: containerRect.height );
            }
            
            /// <summary>Updates the rect of the specfiied state..</summary>
            /// <param name="containerRect">The rect of the container into which the area should be rendered.</param>
            /// <param name="area">The render area to initialize.</param>
            public static void UpdateRect(Rect containerRect, State.Graph.RenderArea area) {
                // Set the position and size of the container area.
                area.ContainerRect.Set(x: 0, y: 0, width: containerRect.width, height: containerRect.height );
            }
            
            /// <summary>
            /// Handles the specified event object.
            /// Calls the HandleEvent method on all children elements of the specified area object.
            /// Consumes the event if it should not be propagated downwards.
            /// </summary>
            /// <param name="ev">The event object to parse.</param>
            /// <param name="area">The state object representing the area to manipulate.</param>
            public static void HandleEvent(Event ev, State.Graph.RenderArea area) {
                // Check if the area's drag flag is set.
                if (area.IsDragged) {
                    // Check if the mouse or alt key was released.
                    if (ev.type == EventType.MouseUp || ev.type == EventType.KeyUp && (ev.modifiers & EventModifiers.Alt) == 0) {
                        // Unset the dragging flag.
                        area.IsDragged = false;
                    }
                    
                    // Check if the mouse dragged on the screen.
                    if (ev.type == EventType.MouseDrag) {
                        // Move the area around.
                        area.Rect.position += ev.delta;
                        
                        // Ensure that the position of the area is valid.
                        if (area.Rect.x > 0) {
                            area.Rect.x = 0;
                        }
                        if (area.Rect.y > 0) {
                            area.Rect.y = 0;
                        }
                        if (area.Rect.x + area.Rect.width < area.ContainerRect.width) {
                            area.Rect.x = area.ContainerRect.width - area.Rect.width;
                        }
                        if (area.Rect.y + area.Rect.height < area.ContainerRect.height) {
                            area.Rect.y = area.ContainerRect.height - area.Rect.height;
                        }
                        
                        // Fire the repaint event.
                        RenderArea.ON_SHOULD_REPAINT.Invoke();
                    } 
                } else {
                    // Check if the right mouse button was pressed, with the alt key.
                    if ((ev.modifiers & EventModifiers.Alt) != 0 && ev.type == EventType.MouseDown && ev.button == (int)MouseButton.RightMouse) {
                        // Set the dragging flag.
                        area.IsDragged = true;
                    }
                }
            }
            
            // - Serialization -
            /// <summary>
            /// Saves the state of the specified <see cref="Henshin.Editor.State.Graph.RenderArea"/>.
            /// Stores it into the corresponding <see cref="Henshin.State.Directions.Scene"/> asset.
            /// </summary>
            /// <param name="area">The area object that should be saved.</param>
            /// <exception cref="InvalidOperationException">Thrown if the <see cref="area"/> object has no <see cref="Henshin.State.Directions.Scene"/> reference.</exception>
            public static void Save(State.Graph.RenderArea area) {
                // Check if the scene object is set.
                if (area.describedScene != null) {
                    // Check if the area is already stored in an asset.
                    if (string.IsNullOrEmpty(value: AssetDatabase.GetAssetPath(assetObject: area))) {
                        // Add the area object to the scene.
                        AssetDatabase.AddObjectToAsset(objectToAdd: area, assetObject: area.describedScene);
                    } else {
                        // Save the asset.
                        AssetDatabase.SaveAssets();
                    }
                } else {
                    // Throw an error.
                    throw new InvalidOperationException(message: $"Cannot save a RenderArea that does no refer to a scene !");
                }
            }
            
            /// <summary>
            /// Loads the <see cref="State.Graph.RenderArea"/> object from the specified <see cref="scene"/>'s asset.
            /// If there is no <see cref="Henshin.Editor.State.Graph.RenderArea"/> object in the asset, creates a new one.
            /// </summary>
            /// <param name="scene">The scene to load the render area from.</param>
            /// <returns>The loaded <see cref="Henshin.Editor.State.Graph.RenderArea"/>, or a newly created one if none is found.</returns>
            public static State.Graph.RenderArea Load(Henshin.State.Directions.Scene scene) {
                // Try to load the asset from the scene.
                State.Graph.RenderArea state = AssetDatabase.LoadAssetAtPath<State.Graph.RenderArea>(assetPath: AssetDatabase.GetAssetPath(assetObject: scene));
                
                // If the state could not be loaded.
                if (state == null) {
                    // Create a new state object.
                    state = new State.Graph.RenderArea{ describedScene = scene };
                }
                
                // Return the state object.
                return state;
            }
    // --- /Methods ---
}
}