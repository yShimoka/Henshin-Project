// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using Henshin.Components.Scene.Scenery;
using UnityEngine;
using UnityEngine.Serialization;

/* Wrap the class within the local namespace. */
namespace Henshin.Core.Scene.Scenery.Transformation {

/// <summary>
/// 
/// </summary>
[TransformationType(serializedType: nameof(SerializedPose))]
public class Pose: Base {
    // ---  Types ---
        // -- Public Types --
            /// <summary>
            /// Serialized representation of a <see cref="pose"/> object.
            /// </summary>
            [Serializable]
            public class SerializedPose: Serialized {
                public SerializedPose() { type = nameof(SerializedPose); }
                
                public Sprite pose;
            }
    // --- /Types ---
    
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>Stores the new pose that will be applied to the <see cref="ActorComponent"/></summary>
            public Sprite NewPose;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Protected Methods --
            /// <returns>The serialized representation of this transformation.</returns>
            protected override Serialized _Serialize(Serialized current = null) {
                // Check if the current object is set.
                if (current != null) {
                    // Create a new serialized object.
                    current = new SerializedPose {
                        pose = this.NewPose
                    };
                } else {
                    // Downcast the current object and set the pose.
                    (current as SerializedPose).pose = this.NewPose;
                }
                
                // Call the base method.
                base._Serialize(current: current);
                
                // Return the object.
                return current;
            }
            
            /// <summary>Deserializes the contents of the serialized object.</summary>
            protected override void _Deserialize(Serialized serialized) {
                // Downcast the serialized object.
                SerializedPose current = (SerializedPose)serialized;
                
                // Parse the contents of the object.
                this.NewPose = current.pose;
            }
            
            /// <summary>Applies the pose transformation.</summary>
            protected override void _ApplyTransformation() {
                // Update the actor's pose.
                this.ActorComponent.UpdatePose(to: this.NewPose);
                
                // Finish the action.
                this._Finish();
            }

    // --- /Methods ---
}
}