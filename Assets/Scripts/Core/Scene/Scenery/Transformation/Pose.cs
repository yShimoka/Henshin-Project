// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using Henshin.Components.Scene.Scenery;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery.Transformation {

/// <summary>
/// 
/// </summary>
[TransformationType(serializedType: typeof(Serialized))]
public class Pose: Base {
    // ---  Types ---
        // -- Public Types --
            /// <summary>
            /// Serialized representation of a <see cref="Pose"/> object.
            /// </summary>
            private new class Serialized: Base.Serialized {
                public Sprite Pose;
            }
    // --- /Types ---
    
    // ---  Attributes ---
        // -- Serialized Attributes --
        // -- Public Attributes --
            /// <summary>Stores the new pose that will be applied to the <see cref="ActorComponent"/></summary>
            public Sprite NewPose;
        // -- Protected Attributes --
        // -- Private Attributes --
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
        // -- Public Methods --
        // -- Protected Methods --
            /// <returns>The serialized representation of this transformation.</returns>
            protected override Base.Serialized _Serialize(Base.Serialized current = null) {
                // Check if the current object is set.
                if (current != null) {
                    // Create a new serialized object.
                    current = new Serialized {
                        Pose = this.NewPose
                    };
                } else {
                    // Downcast the current object and set the pose.
                    (current as Serialized).Pose = this.NewPose;
                }
                
                // Return the object.
                return current;
            }
            
            /// <summary>Deserializes the contents of the serialized object.</summary>
            protected override void _Deserialize(Base.Serialized serialized) {
                // Downcast the serialized object.
                Serialized current = (Serialized)serialized;
                
                // Parse the contents of the object.
                this.NewPose = current.Pose;
            }
            
            /// <summary>Applies the pose transformation.</summary>
            protected override void _ApplyTransformation() {
                // Update the actor's pose.
                this.ActorComponent.UpdatePose(to: this.NewPose);
                
                // Finish the action.
                this._Finish();
            }

        // -- Private Methods --
    // --- /Methods ---
}
}