// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using Henshin.Editor.SceneEditor.GraphArea.Node;
using UnityEngine;
using Henshin.Runtime.Actions;
using Henshin.Runtime.Directions.Scene;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.GraphArea {

/// <summary>
/// State class used to represent a SceneEditor window's graph area.
/// Uses a visual scripting editor to modify the scene's <see cref="ActionState"/> objects.
/// </summary>
[Serializable]
public class GraphAreaState {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - References -
            /// <summary>
            /// Reference to the <see cref="SceneState"/> that owns this <see cref="GraphAreaState"/>.
            /// </summary>
            public int SceneIndex;
            
            /// <summary>
            /// List of all the nodes that are found on this graph area.
            /// </summary>
            public List<NodeState> NodeList = new List<NodeState>();
            
            // - Position Information -
            /// <summary>
            /// Current position of the graph area's contents.
            /// This value should be interpreted as the center of the graph area.
            /// </summary>
            public Vector2 Position;
            
            /// <summary>
            /// Ratio of the graph area relative to the window.
            /// This should only change in integer values.
            /// Bounded by <see cref="RATIO_MAX"/> and <see cref="RATIO_MIN"/>.
            /// </summary>
            public float Ratio = 1;
            
            /// <summary>
            /// Positions of the scrollbars, in normalized space.
            /// </summary>
            public Vector2 ScrollPosition;
            
        // -- Public Attributes --
            // - Rects -
            /// <summary>
            /// Rect within which the graph area should be rendered. 
            /// </summary>
            [NonSerialized]
            public Rect RenderRect;
            
            /// <summary>
            /// Rect used to render the entire grid object.
            /// </summary>
            [NonSerialized]
            public Rect GraphRect;
            
            /// <summary>
            /// UV rect used to render the graph area's texture.
            /// </summary>
            [NonSerialized]
            public Rect TextureRect;
            
            /// <summary>
            /// Reused rect that holds the positions of the scrollbars.
            /// Should always be 0 when not used.
            /// </summary>
            [NonSerialized]
            public Rect ScrollbarRect;
            
            // - References -
            /// <summary>
            /// Reference to the window that holds this <see cref="GraphAreaState"/> object.
            /// </summary>
            [NonSerialized]
            public SceneEditorController Owner;
            
            // - Event Properties -
            /// <summary>
            /// Flag set if the area is being dragged.
            /// </summary>
            [NonSerialized]
            public bool IsDragged;
            
            // - Helper Properties -
            /// <summary>
            /// Returns the position of the center of the canvas, in grid coordinates.
            /// </summary>
            public Vector2 Center => new Vector2(
                x: Mathf.Floor(f: GraphAreaState.CELL_COUNT_X / 2f), 
                y: Mathf.Floor(f: GraphAreaState.CELL_COUNT_Y / 2f)
            );
            
            /// <summary>
            /// Current scale of the <see cref="GraphAreaState"/>.
            /// </summary>
            public float Scale => Mathf.Pow(f: 2, p: this.Ratio / 2f);
            
            /// <summary>
            /// Returns a reference to the currently edited scene object.
            /// </summary>
            public SceneState Scene => this.Owner.State.Header.EditedScene; 
            
        // -- Public Constants --
            // - Render Info -
            /// <summary>
            /// Size of a single grid cell, in pixels.
            /// </summary>
            public const int CELL_SIZE = 16;
            
            /// <summary>
            /// Number of cells on the grid, in the horizontal direction.
            /// </summary>
            public const int CELL_COUNT_X = 192;
            
            /// <summary>
            /// Number of cells on the grid, in the vertical direction.
            /// </summary>
            public const int CELL_COUNT_Y = 96;
            
            /// <summary>
            /// Size of the graph along the x axis.
            /// </summary>
            public const int GRAPH_SIZE_X = GraphAreaState.CELL_SIZE * GraphAreaState.CELL_COUNT_X;
            
            /// <summary>
            /// Size of the graph along the y axis.
            /// </summary>
            public const int GRAPH_SIZE_Y = GraphAreaState.CELL_SIZE * GraphAreaState.CELL_COUNT_Y;
            
            // - Scale Limits -
            /// <summary>
            /// Maximum value that the <see cref="Ratio"/> can take.
            /// </summary>
            public const int RATIO_MAX = 3;
            
            /// <summary>
            /// Minimum value that the <see cref="Ratio"/> can take.
            /// </summary>
            public const int RATIO_MIN = -2;
            
    // --- /Attributes ---
}
}