// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections.Generic;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.State.Graph {

/// <summary>
/// State class used to represent a graph render area.
/// </summary>
/// <seealso cref="Controller.Graph.RenderArea"/>
/// <seealso cref="View.Graph.RenderArea"/>
[Serializable]
public class RenderArea: ScriptableObject {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Contents -
            /// <summary>List of all the nodes that are rendered on the graph.</summary>
            public List<Node> nodes = new List<Node>();
            
            // - References -
            /// <summary>Reference to the <see cref="Henshin.State.Directions.Scene"/> represented by this <see cref="RenderArea"/>.</summary>
            public Henshin.State.Directions.Scene describedScene;
            
            /// <summary>Position of the canvas.</summary>
            public Vector2 position;
            
            /// <summary>Current scale of the render area.</summary>
            public float scale = 1f;
            
        // -- Public Attributes --
            // - Constants -
            /// <summary>Size of a single cell on the GUI-rendered object.</summary>
            public const int GUI_CELL_SIZE = 16;  
            
            /// <summary>Number of cells in total across the entire GUI rendered object.</summary>
            public const int GUI_CELL_COUNT = 128;
            
            // - Render Info -
            /// <summary>
            /// Rect area of the container window.
            /// Should be updated before calling <see cref="View.Graph.RenderArea.Render"/>.
            /// </summary>
            [NonSerialized]
            public Rect ContainerRect;
            
            /// <summary>
            /// Rect area of the rendered area.
            /// </summary>
            [NonSerialized]
            public Rect Rect;
            
            /// <summary>Size of the canvas.</summary>
            public const int SIZE = State.Graph.RenderArea.GUI_CELL_COUNT * State.Graph.RenderArea.GUI_CELL_SIZE;
            
            /// <summary>
            /// Position of the area on the screen.
            /// Used as the (0, 0) reference.
            /// </summary>
            public Vector2 Position => this.Rect.center;
            
            // - Control Info -
            /// <summary>Flag set if the <see cref="Controller.Graph.RenderArea"/> is dragging the area around.</summary>
            [NonSerialized]
            public bool IsDragged;
            
    // --- /Attributes ---
}
}