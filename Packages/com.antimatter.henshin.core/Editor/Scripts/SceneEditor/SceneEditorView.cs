// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor {

/// <summary>
/// Class responsible for the viewing elements of the scene editor.
/// Renders the entire contents of the window.
/// </summary>
public class SceneEditorView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders the window's contents.
            /// </summary>
            /// <param name="state">The scene editor to render.</param>
            public static void Render(SceneEditorState state) {
                // Render the header.
                Header.HeaderView.Render(header: state.Header);
            }
    // --- /Methods ---
}
}