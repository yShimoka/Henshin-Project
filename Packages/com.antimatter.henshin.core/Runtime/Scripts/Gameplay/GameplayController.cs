// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay.Modes;
using UnityEngine.Events;


/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay {

/// <summary>
/// Controller class used to manipulate the <see cref="GameplayState"/> object.
/// </summary>
public static class GameplayController {
    // ---  Methods ---
        // -- Public Methods --
            // - Initialization -
            /// <summary>
            /// Initializes the specified gameplay state.
            /// </summary>
            /// <param name="gameplay">The state object to initialize.</param>
            public static void Initialize(GameplayState gameplay) {}
            
            /// <summary>
            /// Loads the specified gameplay from the <see cref="DataController"/>.
            /// Initializes the gameplay controller for the specified gameplay type.
            /// </summary>
            /// <param name="identifier">The XML identifier of the gameplay to load.</param>
            public static void Load(string identifier) {
                // If a new gameplay item was loaded.
                if (DataState.CurrentGameplay != identifier) {
                    // Clear the gameplay counter.
                    GameplayState.Own.CurrentIndex = 0;
                }
                
                // Load the data controller.
                if (DataController.LoadGameplay(identifier: identifier)) {
                    // Prepare the gameplay depending on the gameplay kind.
                    switch (DataState.Kind) {
                    case "questions": case "gaps": case "none": Gaps.Load(); break;
                    case "snowball" : Snowball.Load(); break;
                    case "comparison" : Comparison.Load(); break;
                    
                    case null: ApplicationView.Error(message: $"Could not load the gameplay \"#{identifier}\"'s kind"); break;
                    default: ApplicationView.Error(message: $"Unknown gameplay kind {DataState.Kind}"); break;
                    }
                }
            }
            
            
            /// <summary>
            /// Plays the currently loaded gameplay.
            /// </summary>
            /// <param name="callback">The callback that is triggered once the gameplay sequence is over.</param>
            public static void Play(UnityAction callback) {
                // Store the callback in the state.
                GameplayState.Own.Callback = callback;
                
                // Play the gameplay depending on the gameplay kind.
                switch (DataState.Kind) {
                // Start the gaps gameplay.
                case "questions": case "gaps": case "none": Gaps.Play(); break;
                case "snowball": Snowball.Play(); break;
                case "comparison" : Comparison.Play(); break;
                
                // If the kind is unhandled.
                default: ApplicationView.Error(message: $"Cannot play gameplay kind {DataState.Kind}"); break;
                }
            }
            
            /// <summary>
            /// Shows the correction for the current gameplay.
            /// </summary>
            public static void Correct() {
                // Play the gameplay depending on the gameplay kind.
                switch (DataState.Kind) {
                // Correct the gaps gameplay.
                case "questions": case "gaps": case "none": Gaps.Correct(); break;
                case "snowball": Snowball.Correct(); break;
                case "comparison" : Comparison.Correct(); break;
                
                // If the kind is unhandled.
                default: ApplicationView.Error(message: $"Cannot play gameplay kind {DataState.Kind}"); break;
                }
            }
            
        // -- Private Methods --
    // --- /Methods ---
}
}