// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Directions.Scene;
using UnityEngine.Events;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay {

/// <summary>
/// State class used to describe the current gameplay state.
/// Used by the <see cref="SceneState"/> class.
/// </summary>
public class GameplayState {
    // ---  SubObjects ---
        // -- Public Enumerators --
            /// <summary>
            /// List of all the available gameplay modes.
            /// </summary>
            public enum EGameplayMode {
                /// <summary>
                /// Default value for the <see cref="EGameplayMode"/> type.
                /// Marks that there is no gameplay selected.
                /// </summary>
                None = 0,
                Snowball,
                Holes,
                Comparison,
                Question,
                Link
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the current 
            /// </summary>
            public static UnityAction Callback;
            
            // - Mode -
            /// <summary>
            /// Stores the current gameplay mode.
            /// </summary>
            public static EGameplayMode CurrentMode;
            
    // --- /Attributes ---
}
}