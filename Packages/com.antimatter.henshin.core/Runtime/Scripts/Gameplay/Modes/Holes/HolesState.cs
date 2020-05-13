// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Gameplay.Components;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Modes.Holes {

/// <summary>
/// Class used to describe the state of the holes gameplay.
/// </summary>
public static class HolesState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Parameters -
            /// <summary>
            /// Flag set if the answer to the question can be wrong.
            /// </summary>
            public static bool CanBeWrong;
            
            /// <summary>
            /// Flag set if the draggable components should be duplicated on drag.
            /// </summary>
            public static bool CreateDuplicates;
            
            // - References -
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