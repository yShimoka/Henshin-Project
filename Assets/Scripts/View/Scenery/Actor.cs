// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.


using System;
using System.Collections;
using Henshin.State;
using Henshin.Controller;
using UnityEngine;
using UnityEngine.Events;

/* Wrap the class within the local namespace. */
namespace Henshin.View.Scenery {

/// <summary>
/// Custom overload of the <see cref="UnityEvent{T0}"/> class.
/// </summary>
public class TickEvent: UnityEvent<float> {};

/// <summary>
/// Component attached to the <see cref="State.Scenery.Actor"/>'s GameObject in the scene.
/// </summary>
[AddComponentMenu(menuName: "Henshin/Scene/Actor", order: 111), RequireComponent(requiredComponent: typeof(SpriteRenderer))]
public class Actor: MonoBehaviour {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Unity event fired on each <see cref="Update"/> event call.
            /// Used by the <see cref="Controller.Directions.TimedTransformation"/>.
            /// </summary>
            [NonSerialized]
            public TickEvent OnTick;
            
        // -- Private Attributes --
            /// <summary>
            /// Reference the the <see cref="SpriteRenderer"/> of this actor.
            /// </summary>
            private SpriteRenderer _mSprite;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Unity event triggered when this object gets instanced in the scene.
            /// </summary>
            private void Awake() {
                // Seek the SpriteRenderer in the object.
                this._mSprite = this.GetComponent<SpriteRenderer>();
                
                // Initialize the onTick event.
                this.OnTick = new TickEvent();
            }
            
            /// <summary>
            /// Unity event triggered at the start of each frame.
            /// </summary>
            private void Update() {
                // Invoke the OnTick event.
                this.OnTick.Invoke(arg0: Time.deltaTime);
            }

        // -- Public Methods --
            // - SpriteRenderer Mutators -
            /// <summary>
            /// Updates the pose of the actor.
            /// </summary>
            /// <param name="to">The <see cref="Sprite"/> object used for the new actor's pose.</param>
            public void UpdatePose(Sprite to) { this._mSprite.sprite = to; }
            
            /// <summary>
            /// Updates the vertical flip of the <see cref="SpriteRenderer"/>.
            /// </summary>
            /// <param name="value">The new value of the vertical flip.</param>
            public void SetVerticalFlip(bool value) { this._mSprite.flipY = value; }
            
            /// <summary>
            /// Updates the horizontal flip of the <see cref="SpriteRenderer"/>.
            /// </summary>
            /// <param name="value">The new value of the horizontal flip.</param>
            public void SetHorizontalFlip(bool value) { this._mSprite.flipX = value; }
            
            /// <returns>The current <see cref="Color"/> of the <see cref="SpriteRenderer"/>.</returns>
            public Color GetColour() { return this._mSprite.color; }
            
            /// <summary>Sets the colour of the <see cref="SpriteRenderer"/>.</summary>
            /// <param name="colour">The new colour of the <see cref="SpriteRenderer"/>.</param>
            public void SetColour(Color colour) { this._mSprite.color = colour; }
            
            /// <returns>The current position of the actor, in local space.</returns>
            public Vector3 GetPosition() { return this.transform.localPosition; }
            
            /// <returns>The current rotation of the actor, in local space.</returns>
            public Quaternion GetRotation() { return this.transform.localRotation; }
            
            /// <returns>The current scale of the actor, in local space.</returns>
            public Vector3 GetScale() { return this.transform.localScale; }
            
            /// <summary>Sets the local position of the actor.</summary>
            /// <param name="position">The new position of the actor.</param>
            public void SetPosition(Vector3 position) { this.transform.localPosition = position; }
            
            /// <summary>Sets the local rotation of the actor.</summary>
            /// <param name="rotation">The new rotation of the actor.</param>
            public void SetRotation(Quaternion rotation) { this.transform.localRotation = rotation; }
            
            /// <summary>Sets the local scale of the actor.</summary>
            /// <param name="scale">The new scale of the actor.</param>
            public void SetScale(Vector3 scale) { this.transform.localScale = scale; }
            
            // - Coroutine Manipulator -
            /// <summary>
            /// Executes the specified method on the next <see cref="Update"/> call.
            /// Uses <see cref="MonoBehaviour.StartCoroutine(IEnumerator)"/> internally.
            /// </summary>
            /// <param name="method">The method to execute after a fixed update.</param>
            public void ExecuteOnNextUpdate(UnityAction method) {
                // Start the co routine.
                this.StartCoroutine(routine: this._ExecuteOnNextUpdate(method: method));
            }
            
        // -- Private Methods --
            /// <summary>
            /// Executes the specified method on the next <see cref="Update"/> call.
            /// </summary>
            /// <param name="method">The method to execute after a fixed update.</param>
            private IEnumerator _ExecuteOnNextUpdate(UnityAction method) {
                // Wait for the next fixed update.
                yield return new WaitForFixedUpdate();
                
                // Invoke the method.
                method.Invoke();
            }
    // --- /Methods ---
}
}