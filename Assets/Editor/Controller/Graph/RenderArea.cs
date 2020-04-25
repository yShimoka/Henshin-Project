// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Linq;
using System.Collections.Generic;
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
                area.ContainerRect.Set(x: 0, y: 0, width: containerRect.width, height: containerRect.height);
            }
            
            /// <summary>Updates the rect of the specfiied state..</summary>
            /// <param name="containerRect">The rect of the container into which the area should be rendered.</param>
            /// <param name="area">The render area to initialize.</param>
            public static void BeforeRender(Rect containerRect, State.Graph.RenderArea area) {
                // Check the bounds of the area.
                if (area.position.x > +area.Rect.width / 2f) {
                    area.position.x = +area.Rect.width / 2f;
                }
                if (area.position.x < -area.Rect.width / 2f + area.ContainerRect.width) {
                    area.position.x = -area.Rect.width / 2f + area.ContainerRect.width;
                }
                if (area.position.y > +area.Rect.height / 2f) {
                    area.position.y = +area.Rect.height / 2f;
                }
                if (area.position.y < -area.Rect.height / 2f + area.ContainerRect.height) {
                    area.position.y = -area.Rect.height / 2f + area.ContainerRect.height;
                }
                
                // Set the position and size of the container area.
                area.Rect.center = area.position;
                        
                area.Rect.size = Vector2.one * (1 / area.scale) * State.Graph.RenderArea.SIZE;
                area.ContainerRect.Set(x: 0, y: 0, width: containerRect.width, height: containerRect.height);
                
                // Update all the nodes.
                foreach (State.Graph.Node node in area.nodes) {
                    Controller.Graph.Node.BeforeRender(canvasPosition: area.Position, node: node);
                }
            }
            
            /// <summary>
            /// Called right after the render area is rendered.
            /// Handles the specified event object.
            /// Calls the HandleEvent method on all children elements of the specified area object.
            /// Consumes the event if it should not be propagated downwards.
            /// </summary>
            /// <param name="area">The state object representing the area to manipulate.</param>
            public static void AfterRender(State.Graph.RenderArea area) {
                // Get the current event.
                Event ev = Event.current;
                
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
                        area.position += ev.delta;
                        
                        // Fire the repaint event.
                        RenderArea.ON_SHOULD_REPAINT.Invoke();
                        
                        // Use the event.
                        ev.Use();
                    } 
                } else {
                    // Check if the scroll wheel was moved.
                    if (ev.type == EventType.ScrollWheel) {
                        // Update the scale of the canvas.
                        const float FACTOR = 1.15f;
                        switch (Mathf.RoundToInt(f: Mathf.Sign(f: ev.delta.y))) {
                            case +1:
                                area.scale *= FACTOR;
                                break;
                            case -1:
                                area.scale /= FACTOR;
                                break;
                        }
                        
                        // Clamp the area's scale.
                        area.scale = Mathf.Clamp(value: area.scale, min: 0.5f, max: 2f);
                        
                        // Fire the repaint event.
                        RenderArea.ON_SHOULD_REPAINT.Invoke();
                    }
                    
                    // Check if the right mouse button was pressed, with the alt key.
                    if ((ev.modifiers & EventModifiers.Alt) != 0 && ev.type == EventType.MouseDown && ev.button == (int)MouseButton.LeftMouse) {
                        // Set the dragging flag.
                        area.IsDragged = true;
                        
                        // Use the event.
                        ev.Use();
                    }
                }
                
                // Loop through the nodes in reverse order.
                for (int i = area.nodes.Count; i > 0; i--) {
                    // Call the after render method.
                    Node.AfterRender(ev: ev, node: area.nodes[index: i - 1]);
                }
                
                // If there is a selected socket.
                if (State.Graph.Socket.CurrentSource != null) {
                    switch (ev.type) {
                    // If the user released the mouse button.
                    case EventType.MouseUp:
                        // Clear the current source.
                        State.Graph.Socket.CurrentSource = null;
                        goto case EventType.MouseDrag;
                    // If the user moved the 
                    case EventType.MouseDrag:
                        // Repaint the window.
                        Controller.Graph.RenderArea.ON_SHOULD_REPAINT.Invoke();
                        break;
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
                // Serialize the nodes in the list.
                foreach (State.Graph.Node node in area.nodes) {
                    Node.Serialize(node: node, allNodes: area.nodes);
                }
                        
                // Check if the scene object is set.
                if (area.describedScene != null) {
                    // If there are deserialized transformations.
                    if (area.describedScene.Transformations != null) {
                        // Rebuild the transformation tree.
                        area.describedScene.RootTransformation = Henshin.Controller.Directions.Transformation.RebuildTree(from: area.describedScene.Transformations);
                    }
                
                    // Check if the area is already stored in an asset.
                    if (string.IsNullOrEmpty(value: AssetDatabase.GetAssetPath(assetObject: area))) {
                        // Add the area object to the scene.
                        AssetDatabase.AddObjectToAsset(objectToAdd: area, assetObject: area.describedScene);
                    } else {
                        AssetDatabase.ForceReserializeAssets();
                    }
                } else {
                    // Throw an error.
                    Debug.LogError(message: "Cannot save a RenderArea that does no refer to a scene !");
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
                    Debug.LogWarning(message: $"Could not find a render area in the scene {scene.identifier}");
                    // Create a new state object.
                    state = ScriptableObject.CreateInstance<State.Graph.RenderArea>();
                    state.describedScene = scene;
                    state.name = scene.identifier + " - RenderArea";
                } else {
                    // Deserialize the nodes in the list.
                    List<State.Graph.Node> deserializedNodes = state.nodes
                        .Select(selector: (node, index) => Node.CreateNode(type: node.Transformation.GetType(), transformationIndex: index, at: node.position, owner: state, nodeIndices: node.targetIndices))
                        .ToList();
                    
                    // Deserialize the individual nodes and their links.
                    foreach (State.Graph.Node node in deserializedNodes) {
                        Node.Deserialize(node: node, allNodes: deserializedNodes);
                    }
                    
                    // Store the node list.
                    state.nodes = deserializedNodes;
                }
                
                // Return the state object.
                return state;
            }
    // --- /Methods ---
}
}