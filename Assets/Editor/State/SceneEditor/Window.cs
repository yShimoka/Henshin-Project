// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using UnityEditor;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.State.SceneEditor {

/// <summary>
/// Static state class for the scene editor window.
/// Made static since only one <see cref="Controller.SceneEditor.Window"/> is allowed to exist.
/// </summary>
public static class Window {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Rect of the window.</summary>
            /// <seealso cref="EditorWindow.position"/>
            public static Rect Rect;
            
            /// <summary>
            /// Reference to the <see cref="Graph.RenderArea"/> that is currently shown to the user.
            /// Loaded from the current selection's <see cref="Henshin.State.Directions.Scene"/>.
            /// </summary>
            public static Graph.RenderArea CurrentGraph;
            
            /// <summary>Flag set if the window's OnGUI method was called.</summary>
            public static bool WasRenderedOnce => View.SceneEditor.Header._msBackgroundTexture != null;
            
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
        // -- Private Methods --
    // --- /Methods ---
}
}