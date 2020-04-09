// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using Henshin.Core.Scene.Directions;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery.Transformation {

/// <summary>
/// 
/// </summary>
[TransformationType(serializedType: nameof(SerializedStart))]
public class Start: Base {
    // ---  Types ---
        // -- Public Types --
            /// <summary>
            /// Serialized representation of a <see cref="Start"/> object.
            /// </summary>
            [Serializable]
            public class SerializedStart: Serialized {
                public SerializedStart() { type = nameof(SerializedStart); }
            }
    // --- /Types ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Reference to the <see cref="Line"/> object that owns this element.</summary>
            public Line Owner;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <summary>Applies the transformation.</summary>
            protected override void _ApplyTransformation() { this._Finish(); }

            /// <summary>Serializes the instance.</summary>
            protected override Serialized _Serialize(Serialized current = null) {
                // Return a new serialized instance.
                return base._Serialize(current: new SerializedStart());
            }
            
            /// <summary>Deserializes the instance.</summary>
            protected override void _Deserialize(Serialized serialized) {
                // Get a reference to the owner of this object.
                this.Owner = Line.Current; 
            }
    // --- /Methods ---
}
}