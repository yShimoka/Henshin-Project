// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay {

/// <summary>
/// Controller class used to manipulate the <see cref="GameplayState"/> object.
/// </summary>
public static class GameplayController {
    // ---  Methods ---
        // -- Public Methods --
            // - Mode Management -
            /// <summary>
            /// Prepares a new gameplay with the specified mode.
            /// This clears all data that was already created.
            /// </summary>
            /// <param name="mode"></param>
            public static void SetMode(GameplayState.EGameplayMode mode) {
                // Clear the current gameplay.
                GameplayController._Clear();
                
                // Update the gameplay mode.
                GameplayState.CurrentMode = mode;
                
                // Prepare the new game play.
                GameplayController._Prepare();
            }
            
        // -- Private Methods --
            /// <summary>
            /// Prepares the new gameplay.
            /// Assumes that <see cref="_Clear"/> was already called
            /// and that <see cref="GameplayState.CurrentMode"/> is set to the correct value.
            /// </summary>
            private static void _Prepare() {
                // Check the current gameplay mode.
                switch (GameplayState.CurrentMode) {
                case GameplayState.EGameplayMode.None:
                    // Do nothing.
                    break;
                }
            }
            
            /// <summary>
            /// Clears all the artifacts generated for the current gameplay.
            /// </summary>
            private static void _Clear() {
                // Check the current gameplay mode.
                switch (GameplayState.CurrentMode) {
                case GameplayState.EGameplayMode.None:
                    // Do nothing.
                    break;
                }
            }
    // --- /Methods ---
}
}