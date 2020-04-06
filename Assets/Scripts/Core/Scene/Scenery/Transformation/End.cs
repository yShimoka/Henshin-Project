// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using Henshin.Core.Scene.Directions;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery.Transformation {

/// <summary>
/// 
/// </summary>
[TransformationType(serializedType: typeof(Serialized))]
public class End: Base {
    // ---  Types ---
        // -- Public Types --
            /// <summary>
            /// Serialized representation of a <see cref="Pose"/> object.
            /// </summary>
            private new class Serialized: Base.Serialized {}
    // --- /Types ---
    
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
        // -- Protected Methods --
            /// <summary>Marks the end of all transformations.</summary>
            protected override void _Finish() {
                // Check if there is a line.
                if (Line.Current != null) {
                    // Advance to the next line.
                    Line.Current.Advance();
                } else {
                    // Throw an exception.
                    throw new InvalidOperationException(message: $"Transformation \"#{this.Identifier}\" has finished but it is not part of a Line !");
                }
            }
            
            /// <summary>Applies the transformation.</summary>
            protected override void _ApplyTransformation() { this._Finish(); }

            /// <summary>Serializes the instance.</summary>
            protected override Base.Serialized _Serialize(Base.Serialized current = null) {
                // Return a new serialized instance.
                return new Serialized();
            }
            
            /// <summary>Deserializes the instance.</summary>
            protected override void _Deserialize(Base.Serialized serialized) {}
        // -- Private Methods --
    // --- /Methods ---
}
}