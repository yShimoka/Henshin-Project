// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Header {

/// <summary>
/// View class responsible for the rendering of the specified header state.
/// </summary>
public static class HeaderView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Renders the specified header object.
            /// </summary>
            /// <param name="header">The header object to render.</param>
            public static void Render(HeaderState header) {
                // DEBUG: Draw a debug box object.
                UnityEngine.GUI.Box(
                    position: header.Rect, 
                    content: UnityEngine.GUIContent.none, 
                    style: Henshin.Editor.Skin.SkinState.Styles.DebugBox
                );
            }
        // -- Private Methods --
    // --- /Methods ---
}
}