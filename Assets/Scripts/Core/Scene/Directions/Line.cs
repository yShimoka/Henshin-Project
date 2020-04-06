// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Henshin.Core.Scene.Scenery;
using Henshin.Core.Scene.Scenery.Transformation;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Directions {

/// <summary>
/// This is the structure that describes a single line of dialogue of the play.
/// This is analogous to lines in a theatre play but are not strictly the same.
/// Lines are used to describe a single action that takes part on the screen.
/// It is made of an <see cref="Actor"/> object and a list of <see cref="Base"/>.
/// It can also update the GUI text if needed.
/// </summary>
[Serializable]
public class Line: ISerializationCallbackReceiver {
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - Line Info -
            /// <summary>
            /// Unique identifier of the <see cref="Line"/> instance.
            /// Used primarily for debugging purposes.
            /// </summary>
            public string identifier;
            
            // - Serialization Info -
            //[HideInInspector]
            public List<Base.Serialized> serializedTransformations;
            
            
        // -- Public Attributes --
            // - Static References -
            /// <summary>Static reference to the line currently being played.</summary>
            public static Line Current { get; private set; }
            
            // - Serialization Results -
            /// <summary>First <see cref="Base"/> that should be applied in the <see cref="Line"/>.</summary>
            /// <seealso cref="Base"/>
            [NonSerialized]
            public Base RootTransformation;
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Unity event fired just before the object gets serialized.
            /// </summary>
            public void OnBeforeSerialize() {
                // Create a new list of transformations.
                List<Base> unfolded = new List<Base>();
                
                // Serialize the root object.
                this.RootTransformation.Unfold(unfoldedList: unfolded);
                this.serializedTransformations = unfolded.Select(selector: transformation => transformation.Serialize(unfoldedTree: unfolded)).ToList();
            }
            
            /// <summary>
            /// Unity event fired right after this object was deserialized.
            /// </summary>
            public void OnAfterDeserialize() {
                // Deserialize the entire object list.
                List<Base> deserialized = this.serializedTransformations.Select(selector: Base.Deserialize).ToList();
                
                // Fold the tree.
                this.RootTransformation = Base.Fold(unfoldedTree: this.serializedTransformations, deserialized: deserialized);
            }
            
        // -- Public Methods --
            /// <summary>
            /// Starts the play of the line.
            /// Triggers the <see cref="Base"/> chain.
            /// </summary>
            public void Play() {
                // Store the current line.
                Line.Current = this;
                
                // Ensure that there is at least one transformation.
                if (this.RootTransformation != null) {
                    // Apply the first transformation to the actor.
                    this.RootTransformation.Apply(); 
                } else {
                    // Go to the next line.
                    this.Advance();
                }
            }
            
            /// <summary>
            /// Advances onto the next line.
            /// </summary>
            public void Advance() {
                // Remove the current line reference.
                Line.Current = null;
                
                // Check if the current scene is set.
                if (Scene.Current.HasValue) {
                    // Go to the next line.
                    Scene.Current.Value.NextLine();
                } else {
                    // Throw an exception.
                    throw new InvalidOperationException(message: $"Line \"#{this.identifier}\" has finished but it is not part of a Scene !");
                }
            }
            
        // -- Private Methods --
    // --- /Methods ---
}
}