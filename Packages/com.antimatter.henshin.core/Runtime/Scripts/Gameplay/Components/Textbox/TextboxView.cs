// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Henshin.Runtime.Application;
using Henshin.Runtime.Data;
using Henshin.Runtime.Gameplay.Components.Target;
using Henshin.Runtime.Libraries;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Selectable = Henshin.Runtime.Libraries.Selectable;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components.Textbox {

/// <summary>
/// Static class used to update the rendering of the Textbox object.
/// Works with the <see cref="TextboxState.Instance"/>.
/// </summary>
public static class TextboxView {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>
            /// Color used for the texts of the text box.
            /// </summary>
            private static Color _msTextColor;
            
            /// <summary>
            /// Flag set if the text should be displayed in italic.
            /// </summary>
            private static bool _msIsItalic;
            
            /// <summary>
            /// Flag set if the text should be displayed in bold.
            /// </summary>
            private static bool _msIsBold;
            
            /// <summary>
            /// Text generator instance.
            /// </summary>
            private static readonly TextGenerator _msTEXT_GENERATOR = new TextGenerator();
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            /// <summary>
            /// Reveals the textbox on the screen.
            /// </summary>
            /// <param name="callback">The callback to trigger once the textbox is revealed.</param>
            /// <param name="time">The time it should take of the textbox to be visible.</param>
            public static void Reveal(UnityAction callback, float time) {
                if (time == 0) {
                    TextboxView._Reveal(callback: callback, time: time, reveal: true).MoveNext();
                } else {
                    ApplicationView.Root.StartCoroutine(routine: TextboxView._Reveal(callback: callback, time: time, reveal: true));
                }
            }
            
            /// <summary>
            /// Hides the textbox on the screen.
            /// </summary>
            /// <param name="callback">The callback to trigger once the textbox is hidden.</param>
            /// <param name="time">The time it should take of the textbox to be invisible.</param>
            public static void Hide(UnityAction callback, float time) {
                if (time == 0) {
                    TextboxView._Reveal(callback: callback, time: time, reveal: false).MoveNext();
                } else {
                    ApplicationView.Root.StartCoroutine(routine: TextboxView._Reveal(callback: callback, time: time, reveal: false));
                }
            }
        
            /// <summary>
            /// Parses the contents of the <see cref="DataController"/>.
            /// </summary>
            /// <param name="index">The index of the data to query.</param>
            public static void Parse(int index) {
                // Clear the textbox.
                TextboxView.Clear();
                
                // Clear the variables.
                TextboxView._msTextColor = GameplayState.Own.TextColor;
                TextboxView._msIsBold = false;
                TextboxView._msIsItalic = false;
                
                // Try to get the original and translated text nodes.
                XmlNode original   = DataController.GetOriginalNode(index: index);
                XmlNode translated = DataController.GetTranslatedNode(index: index);
                
                // Check if the original text is set.
                if (original != null && TextboxState.Instance.Original != null) {
                    // Parse the original node.
                    TextboxView._ParseNode(container: TextboxState.Instance.Original, node: original);
                }
                
                // Check if the translated text is set.
                if (translated != null && TextboxState.Instance.Translated != null) {
                    // Parse the translated node.
                    TextboxView._ParseNode(container: TextboxState.Instance.Translated, node: translated);
                }
            }
            
            /// <summary>
            /// Clears the textbox from all generated text objects.
            /// </summary>
            public static void Clear() {
                // Clear all the children in the original container.
                if (TextboxState.Instance.Original is Transform original) {
                    while(original.childCount > 0) {
                        Object.DestroyImmediate(obj: original.GetChild(index: 0).gameObject);
                    }
                }
                
                // Clear all the children in the translated container.
                if (TextboxState.Instance.Translated is Transform translated) {
                    while(translated.childCount > 0) {
                        Object.DestroyImmediate(obj: translated.GetChild(index: 0).gameObject);
                    }
                }
            }
            
        // -- Private Methods --
            // - Text Parser -
            /// <summary>
            /// Parses the specified xml node.
            /// </summary>
            /// <param name="container">The container of all the created text objects.</param>
            /// <param name="node">The text node to parse.</param>
            private static void _ParseNode([NotNull]RectTransform container, [NotNull]IEnumerable node) {
                // Loop through the contents of the text. 
                foreach (object element in node) {
                    // Check the type of the element.
                    switch (element) {
                        // If the element is a modifier. 
                        case XmlElement modifier:
                            // Handle the modifier.
                            TextboxView._ParseModifier(container: container, modifier: modifier);
                            break;
                        // If the element is a simple text.
                        case XmlText text:
                            // Split the string along the newline characters.
                            string[] split = text.Value.Split('\n');
                            
                            // If there were no newline characters.
                            string joined;
                            if (split.Length == 1) {
                                // Get the whole line.
                                joined = split[0];
                            } else {
                                // Remove the whitespace after the newlines.
                                joined = split
                                    .Select(selector: s => 
                                        s.Length > 0 && char.IsWhiteSpace(c: s[index: 1]) ? 
                                        s.TrimStart() : 
                                        s
                                    )
                                    .Aggregate(seed: "", func: (output, next) => output += next);
                            }
                            
                            // Handle the text.
                            TextboxView._ParseText(
                                container: container, 
                                text: joined 
                            );
                            break;
                    }
                }
            }
            
            /// <summary>
            /// Parses a specified <see cref="XmlNode"/> object.
            /// Handles the rich text definitions and valid/invalid responses.
            /// </summary>
            /// <param name="container">The container to place the generated elements into.</param>
            /// <param name="modifier">The modifier node to apply to the current state.</param>
            /// <exception cref="MissingReferenceException">
            /// Thrown if the <see cref="container"/> has no Valid or Invalid child component.
            /// </exception>
            private static void _ParseModifier([NotNull]RectTransform container, [NotNull]XmlNode modifier) {
                // Check the type of the modifier.
                switch (modifier.LocalName) {
                    // If the text should be displayed in italic.
                    case "i":
                        // Set the italic flag.
                        TextboxView._msIsItalic = true;
                        // Parse the node contents.
                        TextboxView._ParseNode(container: container, node: modifier);
                        // Unset the italic flag.
                        TextboxView._msIsItalic = false;
                        break;
                        
                    // If the text should be displayed in bold.
                    case "b":
                        // Set the bold flag.
                        TextboxView._msIsBold = true;
                        // Parse the node contents.
                        TextboxView._ParseNode(container: container, node: modifier);
                        // Unset the bold flag.
                        TextboxView._msIsBold = false;
                        break;
                        
                    // If the text should be displayed in color.
                    case "c":
                        // Get the color from the node.
                        TextboxView._msTextColor = new Color(
                            r: float.Parse(s: modifier.Attributes?[name: "r"]?.Value ?? "0"),
                            g: float.Parse(s: modifier.Attributes?[name: "g"]?.Value ?? "0"),
                            b: float.Parse(s: modifier.Attributes?[name: "b"]?.Value ?? "0"),
                            a: float.Parse(s: modifier.Attributes?[name: "a"]?.Value ?? "1")
                        );
                        // Parse the node contents.
                        TextboxView._ParseNode(container: container, node: modifier);
                        // Reset the color.
                        TextboxView._msTextColor = GameplayState.Own.TextColor;
                        break;
                    
                    // If the text is a drop target.
                    case "t":
                        // Parse the target.
                        TextboxView._ParseTarget(container: container, text: modifier.InnerText);
                        break;
                        
                    // If the text is a subtext target.
                    case "s":
                        // Parse the target.
                        TextboxView._ParseSubtext(
                            container: container, 
                            text: modifier.InnerText, 
                            subtext: modifier.Attributes?[name: "text"].Value ?? "__undefined__"
                        );
                        break;
                        
                    // If the text is a valid text.
                    case "valid":
                        // Try to get the valid container.
                        if (!(container.Find(n: "Valid") is RectTransform validContainer)) {
                            // Create the children.
                            validContainer = TextboxView._CreateValidChildren(container: container).valid;
                        }
                        // Wrap the text parser with the valid option.
                        TextboxView._ParseNode(container: validContainer, node: modifier);
                        
                        // Get all the children text objects.
                        TextboxState.Instance.ValidTexts = validContainer.GetComponentsInChildren<Text>();
                        break;
                        
                    // If the text is an invalid text.
                    case "invalid":
                        // Try to get the invalid container.
                        if (!(container.Find(n: "Invalid") is RectTransform invalidContainer)) {
                            // Create the children.
                            invalidContainer = TextboxView._CreateValidChildren(container: container).invalid;
                        }
                        // Wrap the text parser with the valid option.
                        TextboxView._ParseNode(container: invalidContainer, node: modifier);
                        
                        // Get all the children text objects.
                        TextboxState.Instance.InvalidTexts = invalidContainer.GetComponentsInChildren<Text>();
                        break;
                    
                    // Handle unknown types.
                    default:
                        Debug.LogWarning(message: $"Unknown modifier type {modifier.LocalName}");
                        break;
                }
            }
            
            /// <summary>
            /// Parses the specified target text.
            /// Creates a new <see cref="TargetController"/> instance in the textbox.
            /// </summary>
            /// <param name="container">The container to create the target into.</param>
            /// <param name="text">The text to display.</param>
            private static void _ParseTarget([NotNull]RectTransform container, string text) {
                // Check if the container has any other children.
                if (container.childCount > 0) {
                    // Get the container's last child.
                    RectTransform last = (RectTransform)container.GetChild(index: container.childCount - 1);
                    
                    // Check if the component does not overflow.
                    if (last.rect.position.x + last.rect.size.x + GameplayState.AnswerObjectSize.x > container.rect.size.x) {
                        // Create a new line game object.
                        TextboxView._CreateNewline(container: container);
                    }
                }
                
                // Instantiate a new target prefab.
                TargetState component = TargetController.Instantiate(parent: container);
                
                // Store the value of the component.
                component.Value = text;
                
                // Set the color of the target's drawables.
                foreach (Graphic graphic in component.Controller
                    .GetComponents<Graphic>()
                    .Union(second: component.Controller.GetComponentsInChildren<Graphic>())
                ) {
                    Color color = graphic.color;
                    if (TextboxState.Instance.Background != null) {
                        color.a = TextboxState.Instance.Background.color.a; 
                    }
                    graphic.color = color;
                }
                
                // Set the anchor of the component.
                TextboxView._SetupAnchors(element: component.Transform);
                    
                // Attach the component to the new line.
                TextboxView._AppendToLast(container: container, child: component.Transform);
            }
            
            /// <summary>
            /// Parses a node with a subtext target.
            /// </summary>
            /// <param name="container">The container to place the text into.</param>
            /// <param name="text">The text to display.</param>
            /// <param name="subtext">The value for the subtext target.</param>
            private static void _ParseSubtext([NotNull]RectTransform container, string text, string subtext) {
                // Draw the text normally.
                TextboxView._ParseText(container: container, text: text);
                
                // Get the new text instance's transform.
                RectTransform newText = (RectTransform)container.GetChild(index: container.childCount - 1);
                
                // Create a new target object.
                TargetState target = TargetController.Instantiate(parent: container);
                // Set its anchors up.
                TextboxView._SetupAnchors(element: target.Transform);
                
                // Place the target object below the new text.
                target.Transform.anchoredPosition = new Vector2(
                    x: newText.anchoredPosition.x + (newText.sizeDelta.x / 2 - target.Transform.sizeDelta.x / 2),
                    y: newText.anchoredPosition.y - GameplayState.AnswerObjectSize.y
                );
                
                // Check if the box stays in the rect.
                if (target.Transform.anchoredPosition.x < 0) {
                    target.Transform.anchoredPosition = new Vector2(x: 0, y: target.Transform.anchoredPosition.y);
                }
                if (target.Transform.anchoredPosition.x + target.Transform.sizeDelta.x > container.rect.size.x) {
                    target.Transform.anchoredPosition = new Vector2(
                        x: container.rect.size.x - target.Transform.sizeDelta.x, 
                        y: target.Transform.anchoredPosition.y
                    );
                }
                
                // Set the target's value.
                target.Value = subtext;
                
                // Place the text after the target in the hierarchy.
                newText.SetAsLastSibling();
            }
            
            /// <summary>
            /// Parses the specified text and creates the appropriate <see cref="Text"/> components.
            /// </summary>
            /// <param name="container">The container that should hold the text.</param>
            /// <param name="text">The text to render.</param>
            private static void _ParseText([NotNull]RectTransform container, string text) {
                // Create the text object.
                Text component = TextboxView._CreateText(container: container);
                // Set the text settings.
                component.fontStyle = 
                    TextboxView._msIsItalic && TextboxView._msIsBold ? FontStyle.BoldAndItalic :
                    TextboxView._msIsItalic ? FontStyle.Italic : 
                    TextboxView._msIsBold ? FontStyle.Bold : 
                    FontStyle.Normal;
                
                // Get the text's color.
                Color color = TextboxView._msTextColor;
                if (TextboxState.Instance.Background != null) {
                    color.a = TextboxState.Instance.Background.color.a;
                }
                component.color = color;
                component.raycastTarget = false;
            
                // Get the text generator settings.
                TextGenerationSettings settings = component.GetGenerationSettings(extents: Vector2.positiveInfinity);
                // Get the space left in the container.
                float space = container.rect.size.x - component.rectTransform.anchoredPosition.x;
                
                // Loop until the width of the text fits in the container.
                string line = text; string carry = "";
                float width;
                do {
                    // Set the text temporarily.
                    component.text = line;
                    // Get the size of the current string.
                    width = LayoutUtility.GetPreferredWidth(rect: component.rectTransform);
                    
                    // If the line is greater than the space left.
                    if (width > space) {
                        // Remove the last word from the line.
                        int lastWordIndex = line.LastIndexOf(value: ' ');
                        
                        // If there is no space in the word.
                        if (lastWordIndex == -1) {
                            // Carry over the entire line.
                            carry = line;
                            line = "";
                        } else {
                            // Copy the word in the carry string.
                            carry = line.Substring(startIndex: lastWordIndex) + carry;
                            line = line.Substring(startIndex: 0, length: lastWordIndex);
                        }
                    }
                } while (width > space && !string.IsNullOrEmpty(value: line));
                
                // Parse the contents of the line.
                if (!string.IsNullOrEmpty(value: line)) {
                    // Set the component's size.
                    component.rectTransform.sizeDelta = new Vector2(
                        x: width,
                        y: GameplayState.AnswerObjectSize.y
                    );
                    
                    // Set its text.
                    component.text = line;
                    component.gameObject.name = line;
                } else {
                    // Destroy the component.
                    Object.DestroyImmediate(obj: component.gameObject);
                }
                
                // Parse the carry over string.
                if (!string.IsNullOrEmpty(value: carry) && carry.Length > 1) {
                    // Create a new line game object.
                    TextboxView._CreateNewline(container: container);
                    
                    // Remove any white space character.
                    carry = carry.TrimStart();
                    
                    // Recurse.
                    // ReSharper disable once TailRecursiveCall
                    TextboxView._ParseText(container: container, text: carry);
                }
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
            
            /// <summary>
            /// Attaches the <see cref="child"/> to the end of the <see cref="container"/>'s last child.
            /// </summary>
            /// <param name="container">The container to append the child to.</param>
            /// <param name="child">The child to append to the container.</param>
            private static void _AppendToLast([NotNull]Transform container, [NotNull]RectTransform child) {
                // Set the anchors of the child.
                TextboxView._SetupAnchors(element: child);
                
                // If there already are objects in the container.
                if (container.childCount > 1) {
                    // Get the container's last child.
                    RectTransform last = (RectTransform)container.GetChild(index: container.childCount - 2);
                    Vector2 lastPos = last.anchoredPosition;
                    
                    // Place the component at the correct position.
                    child.anchoredPosition = new Vector2(
                        x: lastPos.x + last.sizeDelta.x,
                        y: lastPos.y
                    );
                }
            }
            
            /// <summary>
            /// Adds a newline game object to the specified container.
            /// </summary>
            /// <param name="container">The container to add a newline to.</param>
            private static void _CreateNewline([NotNull]Transform container) {
                // Compute the y position of the new line object.
                float baseY = 0;
                if (container.childCount > 0) {
                    // Find the lowest child.
                    baseY = container
                        .Cast<RectTransform>()
                        .Aggregate(seed: 0f, func: (output, child) => {
                            if (child.anchoredPosition.y < output) {
                                output = child.anchoredPosition.y;
                            }
                            return output;
                        }) 
                        - GameplayState.AnswerObjectSize.y;
                }
                
                // Create a new line game object.
                RectTransform newLine = new GameObject(name: "__newline__", components: typeof(RectTransform))
                    .GetComponent<RectTransform>();
                
                // Attach the new line to the container.
                newLine.SetParent(parent: container, worldPositionStays: false);
                
                // Set the anchors of the new line.
                TextboxView._SetupAnchors(element: newLine);
                // Set the position of the new line.
                newLine.anchoredPosition = new Vector2(
                    x: 0,
                    y: baseY
                );
                // Set the size of the new line.
                newLine.sizeDelta = Vector2.up * GameplayState.AnswerObjectSize;
            }
            
            /// <summary>
            /// Creates a new Text object in the specified container.
            /// </summary>
            /// <param name="container">The container to create the text in.</param>
            /// <returns>The created text instance.</returns>
            /// <exception cref="MissingReferenceException">
            /// The TextboxText prefab is not set on the <see cref="GameplayState"/> object.
            /// </exception>
            /// <exception cref="MissingComponentException">
            /// The TextboxText prefab on the <see cref="GameplayState"/> object has no <see cref="Text"/> component.
            /// </exception>
            private static Text _CreateText([NotNull]Transform container) {
                // Check if the prefab is set.
                if (GameplayState.Own.TextboxTextPrefab == null) {
                    throw new MissingReferenceException(message: "There is no TextboxText prefab in the GameplayState.");
                }
                
                // Instantiate the text prefab.
                GameObject text = Object.Instantiate(
                    original: GameplayState.Own.TextboxTextPrefab,
                    parent: container,
                    position: Vector3.zero,
                    rotation: Quaternion.identity
                );
                
                // Check if there is a Text component on the object.
                if (!(text.GetComponent<Text>() is Text component)) {
                    throw new MissingComponentException(
                        message: "There is no Text component on the GameplayState's TextboxText prefab."
                    );
                }
                
                // Ensure that the z position of the object is correct.
                component.transform.localPosition = Vector3.zero;
                
                // Set the components position.
                TextboxView._AppendToLast(container: container, child: component.rectTransform);
                
                // Return the component.
                return component;
            }
            
            /// <summary>
            /// Creates the Valid and Invalid children transforms.
            /// </summary>
            /// <param name="container">The container to create the elements into.</param>
            private static (RectTransform valid, RectTransform invalid) _CreateValidChildren([NotNull]Transform container) {
                // Randomize the text boxes.
                Vector2 vAMin; Vector2 iAMin;
                Vector2 vAMax; Vector2 iAMax;
                if (ApplicationController.Random.NextDouble() > 0.5) {
                    vAMin = new Vector2(x: 0.0f, y: 0.5f); vAMax = new Vector2(x: 1.0f, y: 1.0f);
                    iAMin = new Vector2(x: 0.0f, y: 0.0f); iAMax = new Vector2(x: 1.0f, y: 0.5f);
                } else {
                    iAMin = new Vector2(x: 0.0f, y: 0.5f); iAMax = new Vector2(x: 1.0f, y: 1.0f);
                    vAMin = new Vector2(x: 0.0f, y: 0.0f); vAMax = new Vector2(x: 1.0f, y: 0.5f);
                }
            
                // Create a new Valid container.
                RectTransform valid = new GameObject(name: "Valid", components: new[] { typeof(Selectable) })
                    .GetComponent<RectTransform>();
                
                // Attach it to the container.
                valid.SetParent(parent: container, worldPositionStays: false);
                valid.localPosition = Vector3.zero;
                
                // Set the containers anchors.
                valid.anchorMin = vAMin;
                valid.anchorMax = vAMax;
                valid.pivot = new Vector2(x: 0.5f, y: 1f);
                valid.anchoredPosition = Vector2.zero;
                valid.sizeDelta = Vector2.zero;
                
                // Create the arrow.
                Image validArrow = new GameObject(name: "Arrow", components: new []{ typeof(Image) }).GetComponent<Image>();
                validArrow.sprite = GameplayState.Own.Arrow;
                Color color = Color.white;
                if (TextboxState.Instance.Background != null) {
                    color.a = TextboxState.Instance.Background.color.a;
                }
                validArrow.color = color;
                validArrow.rectTransform.sizeDelta = Vector2.one * GameplayState.AnswerObjectSize.y;
                validArrow.transform.SetParent(parent: valid, worldPositionStays: false);
                TextboxView._SetupAnchors(element: validArrow.rectTransform);
                TextboxState.Instance.ValidArrow = validArrow;
                
                // Create a new Invalid container.
                RectTransform invalid = new GameObject(name: "Invalid", components: new[] { typeof(Selectable) })
                    .GetComponent<RectTransform>();
                // Attach it to the container.
                invalid.SetParent(parent: container, worldPositionStays: false);
                invalid.localPosition = Vector3.zero;
                
                // Set the containers anchors.
                invalid.anchorMin = iAMin;
                invalid.anchorMax = iAMax;
                invalid.pivot = new Vector2(x: 0.5f, y: 1f);
                invalid.anchoredPosition = Vector2.zero;
                invalid.sizeDelta = Vector2.zero;
                
                // Create the arrow.
                Image invalidArrow = new GameObject(name: "Arrow", components: new []{ typeof(Image) }).GetComponent<Image>();
                invalidArrow.sprite = GameplayState.Own.Arrow;
                color = Color.white;
                if (TextboxState.Instance.Background != null) {
                    color.a = TextboxState.Instance.Background.color.a;
                }
                invalidArrow.color = color;
                invalidArrow.rectTransform.sizeDelta = Vector2.one * GameplayState.AnswerObjectSize.y; 
                invalidArrow.transform.SetParent(parent: invalid, worldPositionStays: false);
                TextboxView._SetupAnchors(element: invalidArrow.rectTransform);
                TextboxState.Instance.InvalidArrow = invalidArrow;
                
                // Return the objects.
                return (valid, invalid);
            }
            
            // - Rendering -
            /// <summary>
            /// Slowly reveals the textbox over a few seconds.
            /// </summary>
            /// <param name="callback">The callback called once the operation is finished.</param>
            /// <param name="time">The time it should take for the transition.</param>
            /// <param name="reveal">If true, reveals the textbox. Otherwise hides it.</param>
            private static IEnumerator _Reveal(UnityAction callback, float time, bool reveal = true) {
                // Create a new timer object.
                float timer = reveal ? 0 : time;
                
                // Get all the drawable children objects.
                IEnumerable<Graphic> drawables = TextboxState.ControllerInstance
                    .GetComponentsInChildren<Graphic>()
                    .Union(second: TextboxState.ControllerInstance.GetComponents<Graphic>())
                    .ToArray();
                
                // Loop until the reveal time has elapsed.
                while (reveal ? timer <= time : timer >= 0) {
                    // Increment the timer.
                    timer += reveal ? Time.deltaTime : -Time.deltaTime;
                    
                    // Get the normalized time.
                    float normBg = time == 0 ? reveal ? 1 : 0 : timer / time;
                    
                    // Update the color of the background.
                    foreach (Graphic drawable in drawables) {
                        Color copy = drawable.color;
                        copy.a = Mathf.Clamp(value: normBg, min: 0, max: 1);
                        drawable.color = copy;
                    }
                    
                    // Wait for the next frame.
                    if (time != 0) {
                        yield return null;
                    }
                }
                
                // Call the callback.
                callback?.Invoke();
            }
    // --- /Methods ---
}
}