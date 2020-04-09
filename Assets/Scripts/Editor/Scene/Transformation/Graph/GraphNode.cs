// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using Henshin.Core.Scene.Directions;
using Henshin.Core.Scene.Scenery.Transformation;
using Henshin.Editor.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.Scene.Transformation.Graph {

/// <summary>
/// 
/// </summary>
public class GraphNode {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Stores a reference to the represented transformation object.</summary>
            public readonly Base Referenced;
            
            /// <summary>Size of the transformation element's box.</summary>
            public static readonly Vector2 Size = new Vector2(x: 150f, y: 100f);
            
        // -- Private Attributes --
            /// <summary>Flag set if the object is grabbed.</summary>
            private bool _mGrabbed;
            
            /// <summary>Rect of this object.</summary>
            private Rect _mRect;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Creates a new <see cref="GraphNode"/> instance.
            /// Uses the data in the <see cref="Base"/> transformation.
            /// </summary>
            /// <param name="referenced">The referenced transformation object.</param>
            public GraphNode(Base referenced) {
                // Store the referenced objects.
                this.Referenced = referenced;
            }
        // -- Public Methods --
            public void Draw() {
                // If the referenced object is invalid, do nothing.
                if (this.Referenced == null) { return; }
                
                // Compute the rect of the node.
                this._mRect = new Rect{ position = this.Referenced.Position + TransformationEditorCanvas.CanvasPosition - TransformationEditorCanvas.CellSize / 2, size = GraphNode.Size };
                
                // Draw the box.
                GUI.Box(position: this._mRect,  text: "", style: StyleStore.GraphNode);
                
                // Handle mouse events.
                this._HandleMouseEvents();
            }
            
        // -- Private Methods --
            private void _HandleMouseEvents() {
                // Check for a context click.
                if (Event.current.type == EventType.ContextClick) {
                    // Create the context menu.
                    GenericMenu menu = new GenericMenu();
                    
                    // Check the type of the referenced object.
                    if (this.Referenced is Start || this.Referenced is End) {
                        // Add the option to delete the line.
                        menu.AddItem(content: new GUIContent{ text = "Delete line" }, on: false, func: () => {
                            // Get a reference to the line owner.
                            Line owner = null;
                            switch (this.Referenced) {
                            case Start start:
                                owner = start.Owner;
                                break;
                            case End end:
                                owner = end.Owner;
                                break;
                            }

                            // Delete the line.
                            TransformationEditorState.CurrentScene.lines.Remove(item: owner);
                            
                            // Reload the graph nodes.
                            TransformationEditorCanvas.OnSceneChanged();
                        });
                    } else {
                        // Add the option to delete the transformation.
                        menu.AddItem(content: new GUIContent{ text = "Delete" }, on: false, func: () => {
                            
                        });
                    }
                    
                    // Show the menu.
                    menu.ShowAsContext();
                    
                    // Mark the event as used.
                    Event.current.Use();
                
                // If the user clicked in the box.
                } else if (
                    Event.current.type == EventType.MouseDown && 
                    Event.current.button == (int)MouseButton.LeftMouse &&
                    this._mRect.Contains(point: Event.current.mousePosition)
                ) {
                    // Set the grabbed flag.
                    this._mGrabbed = true;
                    
                    // Consume the event.
                    Event.current.Use();
                    
                // If the user released the mouse button.
                } else if (Event.current.type == EventType.MouseUp && Event.current.button == (int)MouseButton.LeftMouse) {
                    // Unset the grabbed flag.
                    this._mGrabbed = false;
                    
                    // Consume the event.
                    Event.current.Use();
                
                // If the user moved their mouse.
                } else if (
                    Event.current.type == EventType.MouseDrag && 
                    this._mGrabbed
                ) {
                    // Move the element.
                    this.Referenced.Position = Event.current.mousePosition;
                    
                    // Lock the position to the grid.
                    float cellSize = TransformationEditorCanvas.CellSize.x; 
                    this.Referenced.Position.x = (int)(this.Referenced.Position.x / cellSize) * cellSize;
                    this.Referenced.Position.y = (int)(this.Referenced.Position.y / cellSize) * cellSize;
                    
                    // Consume the event.
                    Event.current.Use();
                } else {
                    Debug.Log(Event.current.type);
                }
            }
    // --- /Methods ---
}
}