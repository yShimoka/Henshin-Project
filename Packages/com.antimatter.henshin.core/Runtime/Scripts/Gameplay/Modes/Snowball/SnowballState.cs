// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using UnityEngine.Events;

namespace Henshin.Runtime.Gameplay.Modes.Snowball {

/// <summary>
/// Static class used to control the state of the snowballing gameplay.
/// Used only when the <see cref="GameplayController.SetMode"/> is called
/// with the <see cref="GameplayState.EGameplayMode.Snowball"/>.
/// </summary>
public static class SnowballState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - Flags -
            /// <summary>
            /// Flag set if the state is active.
            /// </summary>
            public static bool Active;
            
            // - References -
            /// <summary>
            /// Action that is triggered once the current snowball step is over.
            /// </summary>
            public static UnityAction Callback;
            
    // --- /Attributes ---
}
}