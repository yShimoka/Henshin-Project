// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components {

/// <summary>
/// Component that is used to manipulate the text box object.
/// The TextBox should be applied to an actor of the scene
/// and implements basic text manipulation routines.
/// </summary>
[AddComponentMenu(menuName: "Henshin/Gameplay/Text Box", order: 10)]
public class TextBoxComponent: MonoBehaviour {
    // ---  SubObjects ---
        // -- Private Classes --
            /// <summary>
            /// Exception used internally to catch texts that would overload the rendering area.
            /// </summary>
            private class TextTooLongException: Exception {
                // ---  Attributes ---
                    // -- Public Methods --
                        /// <summary>
                        /// Index of the character that caused the overload.
                        /// </summary>
                        public int CharIndex;
                        
                        /// <summary>
                        /// Height of the generated line.
                        /// </summary>
                        public int Height;
                // --- /Attributes ---
                
                // ---  Methods ---
                    // -- Constructor --
                        /// <summary>
                        /// Creates a new <see cref="TextTooLongException"/> instance.
                        /// Stores the index of the character where the text becomes too long.
                        /// </summary>
                        /// <param name="charIndex">The index of the char that causes the overload.</param>
                        /// <param name="nextLineY">The y position of the next line in the text.</param>
                        public TextTooLongException(int charIndex, int height) {
                            // Store the values.
                            this.CharIndex = charIndex;
                            this.Height = height;
                        }
                // --- /Methods ---
            }
    // --- /SubObjects ---
    
    // ---  Attributes ---
        // -- Serialized Attributes --
            // - References -
            /// <summary>
            /// The font that will be used in this text box.
            /// </summary>
            public Font Font;
            
            /// <summary>
            /// Prefab object used when instantiating a drop target.
            /// </summary>
            public GameObject DropTargetPrefab;
            
        // -- Private Attributes --
            // - References -
            /// <summary>
            /// Reference to the <see cref="RectTransform"/> component on this game object.
            /// </summary>
            private RectTransform _mRectTransform;
            
            // - Computed Values - 
            /// <summary>
            /// Instance of the generator used to compute the size of the text.
            /// </summary>
            private TextGenerator _mGenerator = new TextGenerator();
            
            /// <summary>
            /// Settings that will be applied to all the text generated for this text box.
            /// </summary>
            private TextGenerationSettings _mSettings = new TextGenerationSettings {
                color = Color.black, font = null, pivot = Vector2.zero, fontSize = 44,
                fontStyle = FontStyle.Normal, horizontalOverflow = HorizontalWrapMode.Wrap, 
                lineSpacing = 1, richText = false, scaleFactor = 1, textAnchor = TextAnchor.UpperLeft,
                verticalOverflow = VerticalWrapMode.Truncate 
            };
            
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Unity Events --
            /// <summary>
            /// Called right after instantiation in the scene.
            /// </summary>
            private void Awake() {
                // Get the rect transform component.
                this._mRectTransform = this.GetComponent<RectTransform>();
            }
            
            /// <summary>
            /// Called right after the object gets enabled in the scene.
            /// </summary>
            private void OnEnable() {
                // Update the generation parameters.
                this._mSettings.font = this.Font;
                
                // Set the limits of the rendering area.
                this._mSettings.generationExtents = this._mRectTransform.rect.size;
                
                ApplicationController.OnNextTick.AddListener(call: _ => {
                    // Update the container of this component.
                    RectTransform parent = this.transform.parent.GetComponent<RectTransform>();
                    parent.sizeDelta = new Vector2(x: ApplicationView.VIEW_WIDTH - 100, y: 300);
                    parent.anchoredPosition = Vector2.down * ApplicationView.VIEW_HEIGHT;
                });
            }

        // -- Public Methods --
            /// <summary>
            /// Gets the size of the specified text using the generator instance.
            /// </summary>
            /// <param name="text">The text to get the size for.</param>
            /// <param name="width">The allocated width for the text.</param>
            /// <returns>The expected rendered width of the string.</returns>
            public Vector2 GetTextSize(string text, float width) {
                // If the text is empty, return a 0.
                if (string.IsNullOrEmpty(value: text)) return Vector2.zero;
                
                // Update the generator's width.
                this._mSettings.generationExtents.x = width;
                
                // Generate the text with the specified parameters.
                if (!this._mGenerator.PopulateWithErrors(str: text, settings: this._mSettings, context: this.gameObject)) {
                    // Log an error.
                    Debug.LogError(message: $"Could not populate the generator with the text {text}");
                    // Return a 0.
                    return Vector2.zero;
                }
                
                // Check if there is more than one generated line.
                if (this._mGenerator.lines.Count > 1) {
                    // Get the number of characters in the first line.
                    throw new TextTooLongException(
                        charIndex: this._mGenerator.lines[index: 1].startCharIdx, 
                        height: Mathf.CeilToInt(
                            f: this._mGenerator.GetPreferredHeight(str: "I", settings: this._mSettings)
                        ) + 1
                    );
                }
                
                // Return the width of the first line.
                return new Vector2(
                    x: this._mGenerator.GetPreferredWidth(str: text, settings: this._mSettings) + 1,
                    y: this._mGenerator.GetPreferredHeight(str: text, settings: this._mSettings) + 1
                );
            }
            
            /// <summary>
            /// Loads the specified text into the <see cref="Text"/> component.
            /// </summary>
            /// <param name="text">The text to load in the component.</param>
            public void ParseText(string text) {
                // Clear any children components.
                while (this.transform.childCount > 0) {
                    Object.DestroyImmediate(obj: this.transform.GetChild(index: 0).gameObject);
                }
                
                // Replace newline characters.
                text = text.Replace(oldValue: "\\n", newValue: "\n");
                
                // Check if the font is set.
                if (this.Font == null) {
                    // Throw an exception.
                    throw new NullReferenceException(
                        message: $"The Font is not set on this game object: {this.gameObject}"
                    );
                }
                
                // Copy the text into a work variable.
                string copy = text;
                
                // Prepare the position store.
                Vector2 position = Vector2.zero;
                
                // Flag set if the next line should be skipped.
                bool skipNextLine = false;
                
                // Get the size of the render area.
                int renderAreaWidth = Mathf.FloorToInt(f: this._mRectTransform.rect.width);
                
                // Loop until the copy is empty.
                while (copy.Length > 0) {
                    // Flag set to check if the text should be hidden. 
                    bool isHidden = false;
                    // Set to the value of the segment that is expected below the current word.
                    string below = null;
                    
                    // Seek a '{' character in the copy string.
                    int paramStartIndex;
                    
                    // If the item is a closing brace.
                    if (copy[index: 0] == '}') {
                        // Remove that brace.
                        copy = copy.Remove(startIndex: 0, count: 1);
                        
                        // If the string is empty, stop the loop.
                        if (copy.Length == 0) break;
                    }
                    
                    // If we are at the end of a hidden section.
                    if (copy[index: 0] == '{') {
                        // Remove that brace.
                        copy = copy.Remove(startIndex: 0, count: 1);
                        
                        // Get the index of the closing brace.
                        paramStartIndex = copy.IndexOf(value: '}');
                        
                        // Set the hidden flag.
                        isHidden = true;
                    } else {
                        // Seek a '{' character in the copy string.
                        paramStartIndex = copy.IndexOf(value: '{');
                    }
                    
                    // If the character was not found.
                    if (paramStartIndex == -1) {
                        // Get the whole string.
                        paramStartIndex = copy.Length;
                    }
                    
                    // Get the next text section.
                    string section = copy.Substring(startIndex: 0, length: paramStartIndex);
                    
                    // If the word is expected to be hidden.
                    if (isHidden) {
                        // Check if there is a semi colon in the clear section.
                        int semiColonIndex = section.IndexOf(value: ':');
                        if (semiColonIndex != -1) {
                            // Get the word that is expected below the current word.
                            below = section.Substring(
                                startIndex: semiColonIndex == section.Length ? section.Length : semiColonIndex + 1, 
                                length: section.Length - semiColonIndex - 1
                            );
                            
                            // Get the word that is supposed to appear in clear.
                            section = section.Substring(startIndex: 0, length: semiColonIndex > 0 ? semiColonIndex : 0);
                            
                            // Clear the hidden flag.
                            isHidden = false;
                        }
                    }
                    
                    // Catch any TextTooLongException.
                    try {
                        // Get the size of the clear section.
                        Vector2 size = this.GetTextSize(text: section, width: renderAreaWidth - position.x);
                        
                        // If the text is not hidden.
                        if (!isHidden) {
                            // If there is a text below the word.
                            if (!string.IsNullOrEmpty(value: below)) {
                                // Get the size of the word to render below.
                                Vector2 belowSize = this.GetTextSize(text: below, width: renderAreaWidth - position.x);
                                
                                // Draw the expected word below.
                                this._AddDropTarget(
                                    text: below, 
                                    position: new Vector2(x: position.x - belowSize.x / 2 + size.x / 2, y: position.y - size.y), 
                                    size: belowSize
                                );
                                
                                // Update the text color.
                                this._mSettings.color = Color.blue;
                                
                                // Skip the next line.
                                skipNextLine = true;
                            }
                            
                            // Add a new text section.
                            this._AddTextSegment(text: section, position: position, size: size);
                        } else {
                            // Draw simple lines.
                            this._AddDropTarget(
                                text: section, position: position, size: size
                            );
                        }
                        
                        // Reset the color.
                        this._mSettings.color = Color.black;
                        
                        // Update the position.
                        position.x += size.x;
                        
                        // Remove the segment from the copy.
                        copy = copy.Substring(startIndex: section.Length);
                        
                        // If there was a word below.
                        if (!string.IsNullOrEmpty(value: below)) {
                            // Remove the segment and the semi colon.
                            copy = copy.Substring(startIndex: below.Length + 1);
                        }
                        
                    } catch (TextTooLongException size) {
                        // If the line has some characters.
                        if (size.CharIndex > 1) {
                            // Get the line segment.
                            string segment = copy.Substring(startIndex: 0, length: size.CharIndex - 1);
                            
                            // Remove the segment from the copy.
                            copy = copy.Substring(startIndex: size.CharIndex);
                            
                            // Add the segment to the text.
                            this._AddTextSegment(text: segment, position: position, size: new Vector2(
                                x: renderAreaWidth - position.x, 
                                y: size.Height
                            ));
                        }
                        
                        // Update the position.
                        position.Set(newX: 0, newY: position.y - (skipNextLine ? 2 : 1 ) * size.Height);
                        
                        // Reset the skip.
                        skipNextLine = false;
                    }
                }
            }
            
        // -- Private Methods --
            /// <summary>
            /// Adds a new text segment on the screen.
            /// </summary>
            /// <param name="text">The text to add on the segment.</param>
            /// <param name="position">The top-left position of the segment.</param>
            /// <param name="size">The size of the generated segment.</param>
            private void _AddTextSegment(string text, Vector2 position, Vector2 size) {
                // Create a new text game object.
                Text segment = new GameObject(
                    name: "Text Segment",
                    components: new []{ typeof(Text) }
                ).GetComponent<Text>();
                segment.gameObject.name = $"{text}";
                
                // Get the rect transform reference.
                RectTransform transform = segment.rectTransform;
                
                // Attach the segment to ourselves.
                transform.SetParent(parent: this.transform, worldPositionStays: false);
                
                // Set the parameters of the transform.
                transform.anchorMin = Vector2.up;
                transform.anchorMax = Vector2.up;
                transform.pivot = Vector2.up;
                transform.anchoredPosition = position;
                transform.sizeDelta = size;
                
                // Set the parameters of the text renderer.
                segment.font = this.Font;
                segment.fontSize = this._mSettings.fontSize;
                segment.alignment = this._mSettings.textAnchor;
                segment.fontStyle = this._mSettings.fontStyle;
                segment.horizontalOverflow = this._mSettings.horizontalOverflow;
                segment.verticalOverflow = this._mSettings.verticalOverflow;
                segment.lineSpacing = this._mSettings.lineSpacing;
                segment.color = this._mSettings.color;
                
                // Set the text of the segment.
                segment.text = text;
            }
            
            /// <summary>
            /// Adds a new drop target on the screen.
            /// </summary>
            /// <param name="text">The identifier text used for the target.</param>
            /// <param name="position">The top-left position of the target.</param>
            /// <param name="size">The size of the generated target.</param>
            private void _AddDropTarget(string text, Vector2 position, Vector2 size) {
                // Check if the prefab instance is set.
                if (this.DropTargetPrefab == null) {
                    // Throw an exception.
                    throw new MissingReferenceException(message: "Text box has no drop target prefab.");
                }
                
                // Instantiate the prefab.
                GameObject target = Object.Instantiate(
                    original: this.DropTargetPrefab,
                    parent: this.transform,
                    worldPositionStays: false
                );
                target.name = $"{text} - Target";
                
                // Try to find the drop target component.
                if (!(target.GetComponent<DropTargetComponent>() is DropTargetComponent component)) {
                    // Throw an exception.
                    throw new MissingComponentException(message: "The drop target has no DropTargetComponent !");
                }
                
                // Get the rect transform reference.
                RectTransform transform = component.GetComponent<RectTransform>();
                
                // Set the parameters of the transform.
                transform.anchoredPosition = position;
                transform.sizeDelta = size;
                
                // Set the parameters of the component.
                component.ExpectedIdentifier = text;
            }
    // --- /Methods ---
}
}