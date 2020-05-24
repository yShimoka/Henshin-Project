// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Gameplay.Components.Answer;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Toolbox {

/// <summary>
/// Class used to store the state of the game's Toolbox object.
/// Used by the <see cref="ToolboxController"/> and <see cref="ToolboxView"/> to manipulate the object.
/// </summary>
public class ToolboxState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the <see cref="ToolboxController"/> that owns this state.
            /// </summary>
            public readonly ToolboxController Controller;
            
            /// <summary>
            /// Reference to the transform of the <see cref="Controller"/>.
            /// </summary>
            public RectTransform Transform;
            
            
            /// <summary>
            /// Reference to the container that will hold all the <see cref="AnswerController"/>.
            /// </summary>
            public RectTransform Container => this.Controller.Container;
            
            // - Static References -
            /// <summary>
            /// Reference to the box instance in the application.
            /// This value should be set once during the entire game.
            /// </summary>
            public static ToolboxState Instance { get; private set; }
            
            /// <summary>
            /// Static accessor to the <see cref="ToolboxController"/> instance.
            /// Simply wraps to <see cref="Instance"/>.<see cref="Controller"/>.
            /// </summary>
            public static ToolboxController ControllerInstance => ToolboxState.Instance.Controller;
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="ToolboxState"/> instance.
            /// Loads some parameters from the specified <see cref="owner"/>.
            /// </summary>
            /// <param name="owner">The <see cref="ToolboxController"/> that owns this state.</param>
            /// <exception cref="InvalidOperationException">There already is an instance of the state.</exception>
            public ToolboxState(ToolboxController owner) {
                // Check if the instance is not already set.
                if (ToolboxState.Instance != null) {
                    throw new InvalidOperationException(message: "Tried to instantiate a second ToolboxState");
                }
                
                // Store the text box instance.
                ToolboxState.Instance = this;
                this.Controller = owner;
                
                // Get the transform of the owner.
                this.Transform = owner.GetComponent<RectTransform>();
            }
    // --- /Methods ---
}
}