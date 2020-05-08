// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using Henshin.Runtime.Application;
using UnityEngine;
using UnityEngine.UI;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Gameplay.Components {

/// <summary>
/// Component that is used to manipulate the text box object.
/// The TextBox should be applied to an actor of the scene
/// and implements basic text manipulation routines.
/// </summary>
[AddComponentMenu(menuName: "Henshin/Gameplay/Text Box")]
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
            /// <summary>
            /// The font that will be used in this text box.
            /// </summary>
            public Font Font;
        // -- Private Attributes --
            // - References -
            /// <summary>
            /// Reference to the <see cref="RectTransform"/> component on this game object.
            /// </summary>
            private RectTransform _mRectTransform;
            
            // - Computed Values -
            /// <summary>
            /// Space taken by a single white character.
            /// </summary>
            private float _mBlankSpace = float.NaN;
            
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
            private void OnEnable() {
                // Get the rect transform component.
                this._mRectTransform = this.GetComponent<RectTransform>();
                
                // Update the generation parameters.
                this._mSettings.font = this.Font;
                
                // Set the limits of the rendering area.
                this._mSettings.generationExtents = this._mRectTransform.rect.size;
                
                // Update the container of this component.
                RectTransform parent = this.transform.parent.GetComponent<RectTransform>();
                parent.anchorMin = Vector2.zero;
                parent.anchorMax = Vector2.right;
                parent.pivot = Vector2.right / 2f;
                parent.sizeDelta = new Vector2(x: ApplicationView.VIEW_WIDTH - 100, y: 300);
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
                        )
                    );
                }
                
                // Return the width of the first line.
                return new Vector2(
                    x: this._mGenerator.GetPreferredWidth(str: text, settings: this._mSettings),
                    y: this._mGenerator.GetPreferredHeight(str: text, settings: this._mSettings)
                );
            }
            
            /// <summary>
            /// Loads the specified text into the <see cref="Text"/> component.
            /// </summary>
            /// <param name="text">The text to load in the component.</param>
            public void ParseText(string text) {
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
                
                // Get the size of the render area.
                int renderAreaWidth = Mathf.FloorToInt(f: this._mRectTransform.rect.width);
                
                // Loop until the copy is empty.
                while (copy.Length > 0) {
                    // Flag set to check if the text should be hidden. 
                    bool isHidden = false;
                    
                    // Seek a '{' character in the copy string.
                    int paramStartIndex;
                    
                    // If the item is a closing brace.
                    if (copy[index: 0] == '}') {
                        // Remove that brace.
                        copy = copy.Remove(startIndex: 0, count: 1);
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
                    
                    // Get the next clear text section.
                    string clear = copy.Substring(startIndex: 0, length: paramStartIndex);
                    
                    // Catch any TextTooLongException.
                    try {
                        // Get the size of the clear section.
                        Vector2 size = this.GetTextSize(text: clear, width: renderAreaWidth - position.x);
                        
                        // If the text is not hidden.
                        if (!isHidden) {
                            // Add a new text section.
                            this._AddTextSegment(text: clear, position: position, size: size);
                        } else {
                            // Draw simple lines.
                            this._AddTextSegment(
                                text: new string(c: '_', count: clear.Length), position: position, size: size
                            );
                        }
                        
                        // Update the position.
                        position.x += size.x;
                        
                        // Remove the segment from the copy.
                        copy = copy.Substring(startIndex: clear.Length);
                        
                    } catch (TextTooLongException size) {
                        // Get the full line segment.
                        string segment = copy.Substring(startIndex: 0, length: size.CharIndex - 1);
                        
                        // Remove the segment from the copy.
                        copy = copy.Substring(startIndex: size.CharIndex);
                        
                        // Add the segment to the text.
                        this._AddTextSegment(text: segment, position: position, size: new Vector2(
                            x: renderAreaWidth - position.x, 
                            y: size.Height
                        ));
                        
                        // Update the position.
                        position.Set(newX: 0, newY: position.y - size.Height);
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
    // --- /Methods ---
}
}