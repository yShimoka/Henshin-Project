// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.




/* Wrap the class within the local namespace. */
namespace Henshin.Controller.Directions.Transformations.Actor {

/// <summary>
/// 
/// </summary>
[TransformationState(stateType: typeof(Henshin.State.Directions.Transformations.Actor.Pose))]
public class Pose: Transformation {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>State accessor.</summary>
            public new Henshin.State.Directions.Transformations.Actor.Pose State => (Henshin.State.Directions.Transformations.Actor.Pose)base.State;
            
            /// <summary>The pose of the actor.</summary>
            public State.Scenery.Actor.ActorPose ActorPose => this.State.actor.poses[index: this.State.PoseIndex];
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Class constructor.
            /// Creates a new <see cref="Pose"/> instance.
            /// Calls the base constructor.
            /// </summary>
            /// <param name="state">The state of the transformation.</param>
            public Pose(State.Directions.Transformation state) : base(state: state) {}
        
        // -- Protected Methods --
            /// <summary>Serialize the timer duration.</summary>
            protected override void _Serialize() {
                // Call the base serialization method.
                base._Serialize();
                
                // Store the index.
                this._AddSerializedString(serialized: this.State.PoseIndex.ToString(provider: System.Globalization.CultureInfo.InvariantCulture));
            }
            
            /// <summary>Deserialize the timer duration.</summary>
            protected override void _Deserialize() {
                // Call the base serialization method.
                base._Deserialize();
                
                // Load the pose index.
                this.State.PoseIndex = int.Parse(s: this._GetNextSerializedString());
            }

            protected override void _Apply() {
                // Update the pose.
                this.State.actor.ActorComponent.UpdatePose(to: this.ActorPose.sprite);
                
                // Finish the action.
                this._Finish();
            }
    // --- /Methods ---
}
}