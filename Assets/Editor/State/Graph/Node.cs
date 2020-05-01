// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using UnityEngine;
using System.Collections.Generic;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.State.Graph {

/// <summary>
/// State class used to represent individual nodes rendered on a <see cref="Henshin.Editor.State.Graph.RenderArea"/>.
/// This object can represent multiple types of <see cref="Henshin.State.Directions.Transformation"/>s.
/// </summary>
[Serializable]
public class Node {
    // ---  Attributes ---
        // -- Serialized Attributes --
            /// <summary>Position of the node in the canvas.</summary>
            public Vector2 position;
            
            /// <summary>Index of the transformation that is represented by this node.</summary>
            public int transformationIndex;
            
            /// <summary>Transformation described by this node.</summary>
            public Henshin.Controller.Directions.Transformation Transformation {
                get {
                    // Check if the index is valid.
                    if (this.transformationIndex >= 0 && this.transformationIndex < this.owner.describedScene.Transformations.Count) {
                        // Return the transformation reference.
                        return this.owner.describedScene.Transformations[index: this.transformationIndex];
                    } else {
                        // Throw a new IndexOutOfRange exception.
                        throw new IndexOutOfRangeException(message: $"A node references a transformation index ({this.transformationIndex}) that does not exist.");
                    }
                }
            }

    /// <summary>Reference to the render area that owns this node.</summary>
            public RenderArea owner;
            
            /// <summary>List of all the indices of this node's output socket targets.</summary>
            [SerializeField]
            public List<int> targetIndices = new List<int>();
            
        // -- Public Attributes --
            // - Constants -
            /// <summary>Size of the base node.</summary>
            public static readonly Vector2 BODY_SIZE  = new Vector2{ x = RenderArea.GUI_CELL_SIZE * 8, y = RenderArea.GUI_CELL_SIZE * 4 };
            /// <summary>Size of the base title.</summary>
            public static readonly Vector2 HEADER_SIZE = new Vector2{ x = Node.BODY_SIZE.x, y = RenderArea.GUI_CELL_SIZE * 2f };
            /// <summary>Size of full node.</summary>
            public static readonly Vector2 NODE_SIZE = new Vector2{ x = Node.BODY_SIZE.x, y = Node.BODY_SIZE.y + Node.HEADER_SIZE.y };
            
            // - Rects -
            /// <summary>Rect used for the node's header.</summary>
            [NonSerialized]
            public Rect HeaderRect = new Rect();
            /// <summary>Rect used for the node's body.</summary>
            [NonSerialized]
            public Rect BodyRect = new Rect();
            /// <summary>Rect used for the node's title.</summary>
            [NonSerialized]
            public Rect TextRect = new Rect();
            
            public Rect FullRect => new Rect( x: this.HeaderRect.x, y: this.HeaderRect.y, width: this.HeaderRect.width, height: this.HeaderRect.height + this.BodyRect.height);
            
            // - Flags -
            /// <summary>Flag set if the node is being dragged.</summary>
            [NonSerialized]
            public bool IsDragged;
            
            // - Sockets -
            /// <summary>Input node of the socket.</summary>
            [NonSerialized]
            public Socket Input;
            
            /// <summary>Output node of the socket.</summary>
            [NonSerialized]
            public Socket Output;
            
            /// <summary>Reference to the node that is currently selected.</summary>
            public static Node CurrentNode;
    // --- /Attributes ---
}
}