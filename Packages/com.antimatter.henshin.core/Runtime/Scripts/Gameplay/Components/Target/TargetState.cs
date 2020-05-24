// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using Henshin.Runtime.Gameplay.Components.Answer;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Target {

/// <summary>
/// Class representing the current state of a target object.
/// Targets are what <see cref="AnswerState"/> look for in the scene.
/// </summary>
public class TargetState {
    // ---  Attributes ---
        // -- Public Attributes --
            // - References -
            /// <summary>
            /// Reference to the owner <see cref="TargetController"/>.
            /// </summary>
            public readonly TargetController Controller;
            
            // - Components -
            /// <summary>
            /// Reference to the <see cref="RectTransform"/> on this target's owner.
            /// </summary>
            public readonly RectTransform Transform;
            
            /// <summary>
            /// Reference to the <see cref="AnswerState"/> that is placed on this target.
            /// </summary>
            public AnswerState PlacedAnswer;
            
            // - Parameters -
            /// <summary>
            /// Stores the value of this target.
            /// Used for comparison against the <see cref="AnswerState.Value"/>.
            /// </summary>
            public string Value;
            
            // - Computed Values -
            /// <summary>
            /// Rect of this target, in world space.
            /// </summary>
            public Rect WorldRect = new Rect();
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="TargetState"/> instance.
            /// </summary>
            /// <param name="owner"></param>
            public TargetState(TargetController owner) {
                // Store the owner of the state.
                this.Controller = owner;
                
                // Get the rect transform of the answer.
                if (owner.GetComponent<RectTransform>() is RectTransform rectTransform) {
                    this.Transform = rectTransform;
                } else {
                    throw new MissingComponentException(message: "This TargetState's owner has no RectTransform !");
                }
                
                // Load a default string value. Used for debugging.
                this.Value = "__undefined_host__";
            }
    // --- /Methods ---
}
}