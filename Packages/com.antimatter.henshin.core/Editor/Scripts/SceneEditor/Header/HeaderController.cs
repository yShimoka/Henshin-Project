// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Editor.SceneEditor.Header {

/// <summary>
/// Controller class used to manipulate <see cref="HeaderState"/> objects.
/// Prepares the state for rendering and handles related events.
/// </summary>
public class HeaderController {
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - Initialization -
            /// <summary>
            /// Initializes the specified header state.
            /// Stores the owner into the state.
            /// </summary>
            /// <param name="header">The header object to initialize.</param>
            /// <param name="owner">The owner of this header.</param>
            public static void Initialize(HeaderState header, SceneEditorController owner) {
                // Store the reference to the owner in the header.
                header.Owner = owner.State;
            }
            
            // - Runtime Events -
            /// <summary>
            /// Prepares the header for rendering.
            /// </summary>
            /// <param name="header">The header state to manipulate.</param>
            public static void Prepare(HeaderState header) {
                // Prepare the rect of the header.
                Henshin.Editor.Skin.SkinState.RatioStruct.ApplyRatio(
                    from: header.Owner.WindowRect,
                    ratio: Henshin.Editor.Skin.SkinState.Ratios.SceneEditorHeaderRatio,
                    to: ref header.Rect
                );
            }
            
            /// <summary>
            /// Handles the latest event of the header.
            /// </summary>
            /// <param name="header">The header state to manipulate.</param>
            public static void HandleEvents(HeaderState header) {
                // Get the current event object.
                UnityEngine.Event ev = UnityEngine.Event.current;
            }
        // -- Private Methods --
    // --- /Methods ---
}
}