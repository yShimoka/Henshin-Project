// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Editor.SceneEditor.GraphArea.Socket;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Directions.Scene;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea.Node {

/// <summary>
/// Class used to represent a single node in the <see cref="GraphAreaState"/>.
/// Every node refers to a specified <see cref="ActionState"/> object.
/// </summary>
[Serializable]
public class NodeState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - References -
            /// <summary>
            /// Reference to the targeted action's index in the
            /// <see cref="Owner"/>'s <see cref="SceneState.ActionList"/>.
            /// </summary>
            public int ActionIndex;
            
            /// <summary>
            /// Position of the node in the graph.
            /// This value is in graph space, relative to its center.
            /// </summary>
            public Vector2 Position;
            
            // - Tree Behaviour -
            /// <summary>
            /// List of all the indices that are targeted by this <see cref="NodeState"/>.
            /// Aka. all of its children.
            /// </summary>
            public List<int> ChildrenIndexList = new List<int>();
            
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the owner of this node.
            /// </summary>
            [NonSerialized]
            public GraphAreaState Owner;
            
            /// <summary>
            /// List of all the parents of this node.
            /// </summary>
            [NonSerialized]
            public List<NodeState> ParentList = new List<NodeState>();
            
            /// <summary>
            /// List of all the childrens of this node.
            /// </summary>
            [NonSerialized]
            public List<NodeState> ChildrenList = new List<NodeState>();
            
            /// <summary>
            /// Index of the node it its owner's <see cref="GraphAreaState.NodeList"/>.
            /// </summary>
            [NonSerialized]
            public int NodeIndex;
            
            /// <summary>
            /// Reference to the input socket of this node.
            /// </summary>
            [NonSerialized]
            public SocketState Input;
            
            /// <summary>
            /// Reference to the output socket of this node.
            /// </summary>
            [NonSerialized]
            public SocketState Output;
            
            // - Rects -
            /// <summary>
            /// Rect used to describe the header position of the node.
            /// </summary>
            [NonSerialized]
            public Rect HeaderRect = new Rect();
            
            /// <summary>
            /// Rect used to describe the body position of the node.
            /// </summary>
            [NonSerialized]
            public Rect BodyRect = new Rect();
            
            // - Handling Attributes -
            /// <summary>
            /// Flag set if the user is dragging the node object.
            /// </summary>
            [NonSerialized]
            public bool IsDragged;
            
            /// <summary>
            /// Offset (in pixels) from the mouse to the top left corner of the box.
            /// This value is valid only when <see cref="IsDragged"/> is set.
            /// </summary>
            [NonSerialized]
            public Vector2 MouseOffset;
        
            // - Accessors -
            /// <summary>
            /// Accessor to the targeted <see cref="ActionState"/> object.
            /// </summary>
            [CanBeNull]
            public ActionState Action {
                get {
                    // If the owner is unset.
                    if (this.Owner == null) {
                        Debug.Log(message: $"The node at #{this.NodeIndex} has no owner !");
                        return null;
                    }
                    
                    // Check the index.
                    if (this.ActionIndex >= 0 && this.ActionIndex < this.Owner.Scene.ActionList.Count) {
                        return this.Owner.Scene.ActionList[index: this.ActionIndex];
                    } else {
                        // Return a null reference.
                        Debug.Log(message: $"Could not access the action with index {this.ActionIndex}");
                        return null;
                    }
                }
            }
        
        // -- Public Constants --
            /// <summary>
            /// Width of the node in graph space.
            /// </summary>
            public const int WIDTH_CELL = 6;
            
            /// <summary>
            /// Height of the header in graph space.
            /// </summary>
            public const int HEADER_HEIGHT_CELL = 1;
            
            /// <summary>
            /// Height of the body in graph space.
            /// </summary>
            public const int BODY_HEIGHT_CELL = 4;
            
            /// <summary>
            /// Width of the node in pixels.
            /// </summary>
            public const int WIDTH = NodeState.WIDTH_CELL * GraphAreaState.CELL_SIZE;
            
            /// <summary>
            /// Height of the header in pixels.
            /// </summary>
            public const int HEADER_HEIGHT = NodeState.HEADER_HEIGHT_CELL * GraphAreaState.CELL_SIZE;
            
            /// <summary>
            /// Height of the body in pixels.
            /// </summary>
            public const int BODY_HEIGHT = NodeState.BODY_HEIGHT_CELL * GraphAreaState.CELL_SIZE;
    // --- /Attributes ---
}
}