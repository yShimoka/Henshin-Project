// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Xml;
using Henshin.Runtime.Gameplay;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Data {

/// <summary>
/// Controller class used to manipulate the game data XML object.
/// </summary>
public class XmlController {
    // ---  Attributes ---
        // -- Public Attributes --
            /// <summary>
            /// Returns the contents of the currently selected Original node.
            /// </summary>
            public static string Original => XmlController._GetInstance()._GetOriginal();
            
            /// <summary>
            /// Returns the contents of the currently selected Text nodes.
            /// </summary>
            public static string[] Translated => XmlController._GetInstance()._GetTranslated();
            
            /// <summary>
            /// Returns the contents of the currently selected Options nodes.
            /// </summary>
            public static string[] Options => XmlController._GetInstance()._GetOptions();
            
            /// <summary>
            /// Defines the index of the Text node that is used by
            /// <see cref="Original"/>, <see cref="Translated"/> and <see cref="Options"/>.
            /// </summary>
            public static int TextIndex {
                    get => XmlController._GetInstance()._mTextNodeIndex; 
                    set => XmlController._GetInstance().SetTextIndex(index: value);
            }
        // -- Private Attributes --
            // - Static References -
            /// <summary>
            /// Reference to the singleton instance of the controller.
            /// </summary>
            [CanBeNull]
            private static XmlController _msInstance;
            
            // - Xml References -
            /// <summary>
            /// Document loaded from disk.
            /// Holds all the game data.
            /// </summary>
            [NotNull]
            private XmlDocument _mDocument;
            
            /// <summary>
            /// Stores a reference to the currently handled xml element.
            /// </summary>
            [CanBeNull]
            private XmlNode _mCurrent;
            
#if UNITY_EDITOR
            [CanBeNull]
            private XmlNode _mCurrentScene;
            
            private int _mCurrentSceneIndex = 0;
            private int _mCurrentActIndex = 0;
#endif
            
            /// <summary>
            /// Stores the index of the accessed Text node.
            /// </summary>
            private int _mTextNodeIndex;
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Constructor --
            /// <summary>
            /// Private constructor.
            /// Loads the file from disk.
            /// </summary>
            /// <exception cref="MissingReferenceException">The game data file is not found.</exception>
            /// <exception cref="InvalidOperationException">The contents of the game data file are not valid xml.</exception>
            private XmlController() {
                // Seek the game data file.
                if (!(Resources.Load<TextAsset>(path: "GameData/GameData") is TextAsset document)) {
                    throw new MissingReferenceException(message: "There is no GameData file to load !");
                }
                
                try {
                    // Load the xml contents of the file.
                    this._mDocument = new XmlDocument();
                    this._mDocument.LoadXml(xml: document.text);
                } catch (XmlException) {
                    // Throw an invalid operation exception.
                    throw new InvalidOperationException(message: "The contents of the GameData xml file were invalid.");
                }
            }
        // -- Public Methods --
            // - Xml Reader -
            /// <summary>
            /// Selects the Text node with the specified parameters.
            /// </summary>
            /// <param name="actIndex">The index of the Act node.</param>
            /// <param name="sceneIndex">The index of the Scene node in the parent.</param>
            /// <param name="gameplayIndex">The index of the Gameplay node in the parent.</param>
            public static void SelectText(int actIndex, int sceneIndex, int gameplayIndex) {
                // Get the controller instance.
                XmlController instance = XmlController._GetInstance();
                
                // Search the act node in the document.
                if (!(
                    instance._mDocument.SelectSingleNode(xpath: $"root/Act[{actIndex + 1}]")
                    is XmlNode actNode
                )) {
                    // Throw an exception.
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(actIndex), 
                        message: $"There is no Act node with index {actIndex}"
                    );
                }
                
                // Search the scene node in the document.
                if (!(
                    actNode.SelectSingleNode(xpath: $"Scene[{sceneIndex + 1}]")
                    is XmlNode sceneNode
                )) {
                    // Throw an exception.
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(sceneIndex), 
                        message: $"There is no Scene node with index {sceneIndex} in the {actIndex}'th Act node."
                    );
                }
                
                // Search the gameplay node in the document.
                if (!(
                    sceneNode.SelectSingleNode(xpath: $"Gameplay[{gameplayIndex + 1}]")
                    is XmlNode gameplayNode
                )) {
                    // Throw an exception.
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(gameplayIndex), 
                        message: $"There is no Gameplay node with index {gameplayIndex} in the {sceneIndex}'th Scene node."
                    );
                }
                
                // Store the text node.
                instance._mCurrent = gameplayNode;
                
                // Set the text index.
                XmlController.TextIndex = 0;
            }
            
#if UNITY_EDITOR
            public static Tuple<string, GameplayState.EGameplayMode>[] GetGameplayModes(int actIndex, int sceneIndex) {
                XmlController instance = XmlController._GetInstance();
                
                // Check if the node is valid.
                if (instance._mCurrentScene == null || instance._mCurrentActIndex != actIndex || instance._mCurrentSceneIndex != sceneIndex) {
                    // Load the node.
                    instance._mCurrentScene = instance._mDocument
                        .SelectSingleNode(xpath: $"/root/Act[{actIndex + 1}]/Scene[{sceneIndex + 1}]");
                        
                    // Store the indices.
                    instance._mCurrentActIndex = actIndex;
                    instance._mCurrentSceneIndex = sceneIndex;
                }
                
                // Get all the gameplay nodes.
                XmlNodeList gameplayNodes = instance._mCurrentScene?.SelectNodes(xpath: "Gameplay");
                
                // If no nodes are found, return an empty lit.
                if (gameplayNodes == null) {
                    return new Tuple<string, GameplayState.EGameplayMode>[0];
                }
                
                // Loop through the nodes.
                Tuple<string, GameplayState.EGameplayMode>[] modes = new Tuple<string, GameplayState.EGameplayMode>[gameplayNodes.Count];
                for (int i = 0; i < gameplayNodes.Count; i++) {
                    // Get the name of the node.
                    modes[i] = new Tuple<string, GameplayState.EGameplayMode>(
                        item1: gameplayNodes[i: i].Attributes[name: "name"].Value,
                        item2: (GameplayState.EGameplayMode)Enum.Parse(
                            enumType: typeof(GameplayState.EGameplayMode), 
                            value: gameplayNodes[i: i].Attributes[name: "kind"].Value
                        )
                    );
                }
                
                // REturn the modes.
                return modes;
            }
            
            public static void Invalidate() {
                // Clear the instance.
                XmlController._msInstance = null;
            }
#endif
            
        // -- Private Methods --
            // - Singleton Methods -
            /// <summary>
            /// Returns the singleton instance of the controller.
            /// If it was no instantiated yet, create a new object.
            /// </summary>
            /// <returns>The singleton instance of the controller.</returns>
            [NotNull]
            private static XmlController _GetInstance() {
                // Check if the instance is created, then return it.
                return XmlController._msInstance ?? (XmlController._msInstance = new XmlController());
            }
            
            // - Xml Manipulation -
            /// <summary>
            /// Seeks the text value of the Original node in the currently selected Text node.
            /// </summary>
            /// <returns>The text contents of the Original node.</returns>
            /// <exception cref="NullReferenceException">The current node was not selected through <see cref="SelectText"/></exception>
            /// <exception cref="InvalidOperationException">The current node has no Original child.</exception>
            private string _GetOriginal() {
                // If there is no current node.
                if (this._mCurrent == null) {
                    // Throw an exception.
                    throw new NullReferenceException(message: "There is no node selected. Did you call SelectText ?");
                }
                
                // Check if there is an original text in the node.
                if(!(this._mCurrent.SelectSingleNode(xpath: $"Text[{this._mTextNodeIndex + 1}]/Original") is XmlNode original)) {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "The current node has no Original child.");
                }
                
                // Return the contents of the node.
                return original.InnerText;
            }
            
            /// <summary>
            /// Seeks all the text values of the Translated nodes in the currently selected Text node.
            /// </summary>
            /// <returns>All the text contents of the Translated nodes.</returns>
            /// <exception cref="NullReferenceException">The current node was not selected through <see cref="SelectText"/></exception>
            /// <exception cref="InvalidOperationException">The current node has no Translated child.</exception>
            private string[] _GetTranslated() {
                // If there is no current node.
                if (this._mCurrent == null) {
                    // Throw an exception.
                    throw new NullReferenceException(message: "There is no node selected. Did you call SelectText ?");
                }
                
                // Check if there is an translated text in the node.
                if(!(this._mCurrent.SelectNodes(xpath: $"Text[{this._mTextNodeIndex + 1}]/Translated") is XmlNodeList translated)) {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "The current node has no Translated child.");
                }
                
                // Return the contents of the nodes.
                return translated.Cast<XmlNode>().Select(selector: node => node.InnerText).ToArray();
            }
            
            /// <summary>
            /// Seeks all the text values of the Option nodes in the currently selected Text node.
            /// </summary>
            /// <returns>All the text contents of the Option nodes.</returns>
            /// <exception cref="NullReferenceException">The current node was not selected through <see cref="SelectText"/></exception>
            /// <exception cref="InvalidOperationException">The current node has no option child.</exception>
            private string[] _GetOptions() {
                // If there is no current node.
                if (this._mCurrent == null) {
                    // Throw an exception.
                    throw new NullReferenceException(message: "There is no node selected. Did you call SelectText ?");
                }
                
                // Check if there is an translated text in the node.
                if(!(this._mCurrent.SelectNodes(xpath: $"Text[{this._mTextNodeIndex + 1}]/Option") is XmlNodeList translated)) {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "The current node has no Option child.");
                }
                
                // Return the contents of the nodes.
                return translated.Cast<XmlNode>().Select(selector: node => node.InnerText).ToArray();
            }
            
            private void SetTextIndex(int index) {
                // If there is no current node.
                if (this._mCurrent == null) {
                    // Throw an exception.
                    throw new NullReferenceException(message: "There is no node selected. Did you call SelectText ?");
                }
                
                // If the index is -1.
                if (index == -1) {
                    // Unset the node index
                    this._mTextNodeIndex = -1;
                    return;
                }
                
                // Check if there is an translated text in the node.
                if(this._mCurrent.SelectSingleNode(xpath: $"Text[{index + 1}]") is null) {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "The current node has no Text child.");
                }
                
                // Store the index.
                this._mTextNodeIndex = index;
            }
    // --- /Methods ---
}
}