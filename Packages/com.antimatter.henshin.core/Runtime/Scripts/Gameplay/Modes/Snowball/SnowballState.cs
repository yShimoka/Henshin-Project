// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Gameplay.Components;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Modes.Snowball {

/// <summary>
/// Class used to describe the state of the holes gameplay.
/// </summary>
public static class SnowballState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the text box that holds the original text.
            /// </summary>
            public static TextBoxComponent OriginalComponent;
            
            /// <summary>
            /// Reference to the text box that holds the translated text.
            /// </summary>
            public static TextBoxComponent TranslatedComponent;
            
            /// <summary>
            /// Reference to the text box that holds the tool box.
            /// </summary>
            public static ToolBoxComponent ToolboxComponent;
            
            /// <summary>
            /// List of all the words that should be filled in the scene.
            /// </summary>
            public static DropTargetComponent[] Words;
            
            /// <summary>
            /// Number of <see cref="DraggableComponent"/> that have reached their target.
            /// </summary>
            public static int CompletedWords;
            
            
    // --- /Attributes ---
}
}