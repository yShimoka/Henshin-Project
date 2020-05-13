// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

/* Wrap the class within the local namespace. */

using Henshin.Runtime.Data;
using Henshin.Runtime.Directions.Act;
using Henshin.Runtime.Directions.Scene;
using Henshin.Runtime.Gameplay.Modes.Default;
using Henshin.Runtime.Gameplay.Modes.Holes;
using Henshin.Runtime.Gameplay.Modes.Snowball;
using JetBrains.Annotations;
using UnityEngine.Events;

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
            /// <param name="mode">The gameplay mode to prepare.</param>
            public static void PrepareMode(GameplayState.EGameplayMode mode, int gameplayIndex) {
                // Clear the current gameplay.
                GameplayController._Clear();
                
                // Update the gameplay mode.
                GameplayState.CurrentMode = mode;
                
                // Prepare the new game play.
                GameplayController._Prepare(gameplayIndex: gameplayIndex);
            }
            
            /// <summary>
            /// Starts playing the mode that was prepared with <see cref="PrepareMode"/>.
            /// </summary>
            /// <param name="callback">The method to invoke after the gameplay is done.</param>
            public static void Play([NotNull]UnityAction callback) {
                // Store the callback.
                GameplayState.Callback = callback;
                
                // Check the current gameplay mode.
                switch (GameplayState.CurrentMode) {
                case GameplayState.EGameplayMode.Holes:
                    // Call the holes method.
                    HolesController.Play();
                    break;
                case GameplayState.EGameplayMode.Snowball:
                    // Call the snowball method.
                    SnowballController.Play();
                    break;
                case GameplayState.EGameplayMode.None:
                    // Immediately call the callback.
                    callback.Invoke();
                    break;
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Prepares the new gameplay.
            /// Assumes that <see cref="_Clear"/> was already called
            /// and that <see cref="GameplayState.CurrentMode"/> is set to the correct value.
            /// </summary>
            private static void _Prepare(int gameplayIndex) {
                // Check the current gameplay mode.
                switch (GameplayState.CurrentMode) {
                case GameplayState.EGameplayMode.None:
                    // Load the static text.
                    DefaultController.Prepare(actIndex: ActState.Current.Index, sceneIndex: SceneState.Current.Index, gameplayIndex: gameplayIndex);
                    break;
                case GameplayState.EGameplayMode.Holes:
                    // Prepare the holes gameplay.
                    HolesController.Prepare(actIndex: ActState.Current.Index, sceneIndex: SceneState.Current.Index, gameplayIndex: gameplayIndex);
                    break;
                case GameplayState.EGameplayMode.Snowball:
                    // Call the snowball method.
                    SnowballController.Prepare(actIndex: ActState.Current.Index, sceneIndex: SceneState.Current.Index, gameplayIndex: gameplayIndex);
                    break;
                }
            }
            
            /// <summary>
            /// Clears all the artifacts generated for the current gameplay.
            /// </summary>
            private static void _Clear() {
                // Check the current gameplay mode.
                switch (GameplayState.CurrentMode) {
                case GameplayState.EGameplayMode.Snowball:
                    // Call the snowball method.
                    //SnowballController.Clear();
                    break;
                case GameplayState.EGameplayMode.Holes:
                    // Call the holes method.
                    //HolesController.Clear();
                    break;
                case GameplayState.EGameplayMode.None:
                    // Do nothing.
                    break;
                }
            }
    // --- /Methods ---
}
}