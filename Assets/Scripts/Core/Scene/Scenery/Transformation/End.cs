// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using Henshin.Core.Scene.Directions;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery.Transformation {

/// <summary>
/// 
/// </summary>
[TransformationType(serializedType: nameof(SerializedEnd))]
public class End: Base {
    // ---  Types ---
        // -- Public Types --
            /// <summary>
            /// Serialized representation of a <see cref="Pose"/> object.
            /// </summary>
            [Serializable]
            public class SerializedEnd: Serialized {
                public SerializedEnd() { type = nameof(SerializedEnd); }
            }
    // --- /Types ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Reference to the <see cref="Line"/> object that owns this element.</summary>
            public Line Owner;
    // --- /Attributes ---
    
    // ---  Methods ---
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
            protected override Serialized _Serialize(Serialized current = null) {
                // Return a new serialized instance.
                return base._Serialize(current: new SerializedEnd());
            }
            
            /// <summary>Deserializes the instance.</summary>
            protected override void _Deserialize(Serialized serialized) {
                // Get a reference to the owner of this object.
                this.Owner = Line.Current; 
            }
        // -- Private Methods --
    // --- /Methods ---
}
}