// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Editor.SceneEditor.GraphArea;
using Henshin.Editor.SceneEditor.Inspector;

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor {

/// <summary>
/// Class responsible for the viewing elements of the scene editor.
/// Renders the entire contents of the window.
/// </summary>
public static class SceneEditorView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders the window's contents.
            /// </summary>
            /// <param name="state">The scene editor to render.</param>
            public static void Render(SceneEditorState state) {
                // Render the graph area.
                GraphAreaView.Render(graphArea: state.GraphArea);
                
                // Render the inspector.
                InspectorView.Render(inspector: state.Inspector);
                
                // Render the header.
                Header.HeaderView.Render(header: state.Header);
            }
    // --- /Methods ---
}
}