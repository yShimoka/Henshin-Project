// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEngine;
using UnityEditor;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.State.SceneEditor {

/// <summary>
/// Canvas area in which the <see cref="View.Graph.RenderArea"/> is drawn.\
/// Loads the canvas of the currently selected scene.
/// </summary>
public static class Canvas {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Constants -
            /// <summary>Ratio of the canvas relative to the overall window.</summary>
            public static readonly Rect RATIO = new Rect{ x = Inspector.RATIO.width, y = Header.RATIO.height, width = 1 - Inspector.RATIO.width, height = 1 - Header.RATIO.height };
            
            // - Render Area -
            /// <summary>Rect in which the canvas will be rendered.</summary>
            public static Rect Rect = new Rect();
            
            // - Rendered Items -
            /// <summary>Reference to the render area found within the current scene.</summary>
            public static State.Graph.RenderArea CurrentRenderArea;
            
            /// <summary>Contextual menu that is rendered when the user right-clicks on the screen.</summary>
            public static GenericMenu ContextMenu;
    // --- /Attributes ---
}
}