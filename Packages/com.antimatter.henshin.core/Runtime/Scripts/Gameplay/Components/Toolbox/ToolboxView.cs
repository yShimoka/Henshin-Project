// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay.Components.Answer;
using Henshin.Runtime.Gameplay.Components.Textbox;
using Henshin.Runtime.Libraries;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Toolbox {

/// <summary>
/// Static class used to update the rendering of the Toolbox object.
/// Works with the <see cref="ToolboxState.Instance"/>.
/// </summary>
public static class ToolboxView {
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Shows the toolbox on the screen.
            /// </summary>
            public static void Show() {}
            
            /// <summary>
            /// Parses the contents of the <see cref="DataController"/>.
            /// </summary>
            /// <param name="index">The index of the data to query.</param>
            public static void Parse(int index) {
                // Clear all previous children.
                ToolboxView.Clear();
                
                // Get the options node.
                foreach (XmlNode node in DataController.GetOptionNodes(index: index)) {
                    // Parse the option.
                    ToolboxView._ParseOption(container: ToolboxState.Instance.Container, option: node.InnerText);
                }
            }
            
            /// <summary>
            /// Clears the contents of the toolbox.
            /// </summary>
            public static void Clear() {
                // Clear all the children of the transform.
                while (ToolboxState.Instance.Container.childCount > 0) {
                    Object.DestroyImmediate(obj: ToolboxState.Instance.Container.GetChild(index: 0).gameObject);
                }
            }
            
            /// <summary>
            /// Reveals the toolbox on the screen.
            /// </summary>
            /// <param name="callback">The callback to trigger once the toolbox is revealed.</param>
            /// <param name="time">The time it should take of the toolbox to be visible.</param>
            public static void Reveal(UnityAction callback, float time) {
                ApplicationView.Root.StartCoroutine(routine: ToolboxView._Reveal(callback: callback, time: time, reveal: true));
            }
            
            /// <summary>
            /// Hides the toolbox on the screen.
            /// </summary>
            /// <param name="callback">The callback to trigger once the toolbox is hidden.</param>
            /// <param name="time">The time it should take of the toolbox to be invisible.</param>
            public static void Hide(UnityAction callback, float time) {
                ApplicationView.Root.StartCoroutine(routine: ToolboxView._Reveal(callback: callback, time: time, reveal: false));
            }
            
        // -- Private Methods --
            // - Option Parser -
            private static void _ParseOption([NotNull]Transform container, string option) {
                // If the toolbox already has options.
                float baseY = 0;
                if (container.childCount > 0) {
                    // Get the previous child.
                    RectTransform lastChild = (RectTransform)container.GetChild(index: container.childCount - 1);
                    
                    // Get its y position.
                    baseY = lastChild.anchoredPosition.y - lastChild.sizeDelta.y - 4;
                }
                
                // Add a new answer in the toolbox.
                AnswerState answer = AnswerController.Instantiate(parent: container);
                // Set its value.
                AnswerView.UpdateText(answer: answer, text: option);
                
                // Set the anchors of the answer.
                ToolboxView._SetupAnchors(element: answer.Transform);
                
                // Set the position of the answer.
                answer.Transform.anchoredPosition = Vector2.up * baseY; 
            }
            
            // - Layout -
            /// <summary>
            /// Setup the anchors of the specified object.
            /// </summary>
            /// <param name="element">The object ot set the anchors of.</param>
            private static void _SetupAnchors([NotNull]RectTransform element) {
                // Set the anchor of the element.
                element.anchorMin = Vector2.up;
                element.anchorMax = Vector2.up;
                element.pivot = Vector2.up;
                element.anchoredPosition = Vector2.zero;
            }
            
            // - Rendering -
            /// <summary>
            /// Slowly reveals the toolbox over a few seconds.
            /// </summary>
            /// <param name="callback">The callback called once the operation is finished.</param>
            /// <param name="time">The time it should take for the transition.</param>
            /// <param name="reveal">If true, reveals the textbox. Otherwise hides it.</param>
            private static IEnumerator _Reveal(UnityAction callback, float time, bool reveal = true) {
                // Create a new timer object.
                float timer = reveal ? time : 0;
                
                // Loop until the reveal time has elapsed.
                while (reveal ? timer >= 0 : timer <= time) {
                    // Increment the timer.
                    timer += reveal ? -Time.deltaTime : Time.deltaTime;
                    
                    // Get the normalized time.
                    float n = time == 0 ? 1 : timer / time;
                    n = EasingFunction.EaseInOutQuad(start: 0, end: 1, value: n);
                    
                    // Set the position of the toolbox.
                    ToolboxState.Instance.Transform.anchoredPosition = new Vector2(
                        x: ToolboxState.Instance.Transform.sizeDelta.x * n,
                        y: 0
                    );
                    
                    // Wait for the next frame.
                    yield return new WaitForFixedUpdate();
                }
                
                // Call the callback.
                callback?.Invoke();
            }
    // --- /Methods ---
}
}