// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Henshin.Runtime.Application;
using Henshin.Runtime.Directions.Act;
using Henshin.Runtime.Directions.Scene;
using JetBrains.Annotations;
using UnityEngine;

/* Wrap the class within the local namespace. */
namespace Henshin.Runtime.Data {

/// <summary>
/// Controller class used to manipulate the GameData xml file.
/// Loads the file from disk and parses its contents.
/// In editor mode, auto-reloads the file on query. 
/// </summary>
public static class DataController {
    // ---  Attributes ---
        // -- Private Attributes --
            /// <summary>
            /// Instance of the MD5 hash generator.
            /// </summary>
            private static MD5 _msMd5;
            
            /// <summary>
            /// Defines the location of the file in the Resources folder.
            /// </summary>
            private const string _mFILE_LOCATION = "GameData/GameData";
    // --- /Attributes ---
    
    // ---  Methods ---
        // -- Public Methods --
            // - Scene Manipulation -
            /// <summary>
            /// Loads the <see cref="ActState.Current"/> and <see cref="SceneState.Current"/> indices.
            /// Loads the xml node at the specified location.
            /// Clears the selected gameplay.
            /// </summary>
            public static bool LoadScene(int act, int scene) {
                // Reload the file.
                DataController._ReloadFile();
                
                // Clear the current identifier.
                DataState.CurrentGameplay = null;
                
                // Build the xpath query.
                string xpath = $"henshin:root/henshin:act[{act + 1}]/henshin:scene[{scene + 1}]";
                
                // If we are in the editor, store the act and scene indices.
#if UNITY_EDITOR
                DataState.ActIndex = act;
                DataState.SceneIndex = scene;
#endif
                
                try {
                    // Get the node at the specified location.
                    DataState.CurrentScene = DataState.Document.SelectSingleNode(
                        xpath: xpath, 
                        nsmgr: DataState.NamespaceManager
                    );
                    
                    // Check if the scene node is set.
                    if (DataState.CurrentScene == null) {
                        throw new NullReferenceException(message: "The current scene could not be found.");
                    }
                    
                    // Return a success.
                    return true;
                } catch(Exception exception) {
                    // Show an error to the user.
                    ApplicationView.Error(
                        message: $"Could not find a node for Act-{act + 1} Scene-{scene + 1}", 
                        details: exception.Message
                    );
                    return false;
                }
            }
            
            // - Gameplay Manipulation -
            /// <summary>
            /// Loads the gameplay node with the specified identifier.
            /// Calls <see cref="ApplicationView.Error"/> if the loading is impossible.
            /// </summary>
            /// <param name="identifier">The identifier of the gameplay to load.</param>
            public static bool LoadGameplay(string identifier) {
                // Reload the file.
                DataController._ReloadFile();
                
                // Check if the gameplay is the same as the previous one.
                if (identifier == DataState.CurrentGameplay) {
                    // Do nothing.
                    return true;
                }
                
                // Check if the current scene node is loaded.
                if (DataState.CurrentScene == null) {
                    // Log an error.
                    ApplicationView.Error(message: "Tried to load a gameplay without loading its scene.");
                    return false;
                }
                
                try {
                    // Try to get the gameplay with the specified identifier.
                    XmlNode gameplayNode = DataState.CurrentScene.SelectSingleNode(
                        xpath: $"henshin:gameplay[@identifier=\"{identifier}\"]", 
                        nsmgr: DataState.NamespaceManager
                    );
                    
                    // If the node was not found or it has no attributes.
                    if (gameplayNode?.Attributes == null) {
                        throw new InvalidOperationException(
                            message: $"There is no gameplay node with the identifier {identifier}"
                        );
                    }
                    
                    // Get the gameplay kind.
                    DataState.Kind = gameplayNode.Attributes[name: "kind"]?.Value;
                    
                    // Reload the default size.
                    DataState.AnswerObjectSize = DataState.DEFAULT_ANSWER_OBJECT_SIZE;
                    DataState.DuplicateAnswers = false;
                    // Check if the gameplay's answer size was specified.
                    if (gameplayNode.SelectSingleNode(
                        xpath: "henshin:answer", 
                        nsmgr: DataState.NamespaceManager
                    ) is XmlNode size) {
                        // Load the size of the answer.
                        DataState.AnswerObjectSize = new Vector2(
                            x: int.Parse(s: size.Attributes?[name: "width"]?.Value 
                                ?? DataState.DEFAULT_ANSWER_OBJECT_SIZE.x.ToString(provider: CultureInfo.InvariantCulture)),
                            y: int.Parse(s: size.Attributes?[name: "height"]?.Value
                                ?? DataState.DEFAULT_ANSWER_OBJECT_SIZE.y.ToString(provider: CultureInfo.InvariantCulture))
                        );
                        
                        // Check if the duplicate attribute exists.
                        if (size.Attributes?[name: "duplicate"] != null) {
                            // Load the duplicate flag.
                            DataState.DuplicateAnswers = bool.Parse(value: size.Attributes[name: "duplicate"].Value);
                        }
                    }
                    
                    // Load the original and translated texts.
                    DataState.OriginalNodes   = gameplayNode.SelectNodes(
                        xpath: "henshin:original", 
                        nsmgr: DataState.NamespaceManager
                    );
                    DataState.TranslatedNodes = gameplayNode.SelectNodes(
                        xpath: "henshin:translated", 
                        nsmgr: DataState.NamespaceManager
                    );

                    // Seek the option nodes.
                    DataState.OptionsNodes = gameplayNode.SelectNodes(
                        xpath: "henshin:options", 
                        nsmgr: DataState.NamespaceManager
                    );
                    
                    // Store the current gameplay.
                    DataState.CurrentGameplay = identifier;
                    
                    // Return a success.
                    return true;
                } catch(Exception exception) {
                    // Show an error to the user.
                    ApplicationView.Error(
                        message: $"Could not load the gameplay node \"#{identifier}\" for the current scene.", 
                        details: exception.Message
                    );
                    return false;
                }
            }
            
#if UNITY_EDITOR
            /// <summary>
            /// Loads all the gameplay identifiers in the current scene's node.
            /// </summary>
            /// <returns>The list of all the gameplay identifiers in the scene.</returns>
            /// <exception cref="NullReferenceException">
            /// The scene node was not selected with <see cref="LoadScene"/>.
            /// </exception>
            public static string[] GetGameplayIdentifiers() {
                // Reload the file.
                DataController._ReloadFile();
                
                // Check if the scene is selected.
                if (DataState.CurrentScene == null) {
                    throw new NullReferenceException(message: "The current scene was not specified.");
                }
                
                // Get all the gameplay nodes in the current scene.
                XmlNodeList gameplayNodes = DataState.CurrentScene.SelectNodes(
                    xpath: "henshin:gameplay", 
                    nsmgr: DataState.NamespaceManager
                );
                
                // If the nodes were be found.
                if (gameplayNodes != null) {
                    // Get all of their identifiers.
                    return gameplayNodes
                        .Cast<XmlNode>()
                        .Select(selector: node => node.Attributes?[name: "identifier"]?.Value)
                        .Where(predicate: attr => attr != null)
                        .ToArray();
                } else {
                    // Return an empty list.
                    return new string[0];
                }
            }
#endif
            
            /// <summary>
            /// Returns the original <see cref="XmlNode"/> at the specified index.
            /// </summary>
            /// <param name="index">The index of the original node to query.</param>
            /// <returns>The found node object, or null if it is not found.</returns>
            [CanBeNull]
            public static XmlNode GetOriginalNode(int index) {
                // Check if the original nodes exist.
                if (DataState.OriginalNodes == null) {
                    return null;
                }
                
                // Check the bounds of the index.
                if (index < 0 || index >= DataState.OriginalNodes.Count) {
                    return null;
                }
                
                // Return the node instance.
                return DataState.OriginalNodes[i: index];
            }
            
            /// <summary>
            /// Returns the translated <see cref="XmlNode"/> at the specified index.
            /// </summary>
            /// <param name="index">The index of the translated node to query.</param>
            /// <returns>The found node object, or null if it is not found.</returns>
            [CanBeNull]
            public static XmlNode GetTranslatedNode(int index) {
                // Check if the translated nodes exist.
                if (DataState.TranslatedNodes == null) {
                    return null;
                }
                
                // Check the bounds of the index.
                if (index < 0 || index >= DataState.TranslatedNodes.Count) {
                    return null;
                }
                
                // Return the node instance.
                return DataState.TranslatedNodes[i: index];
            }
            
            /// <summary>
            /// Returns the list of option that belong to the Options node at the specified index.
            /// </summary>
            /// <param name="index">The index of the options node to query.</param>
            /// <returns>The found node objects, or null if they are not found.</returns>
            [NotNull]
            public static IEnumerable<XmlNode> GetOptionNodes(int index) {
                // Check if the options nodes exist.
                if (DataState.OptionsNodes == null) {
                    Debug.LogWarning(message: "Tried to get option nodes when none are loaded");
                    return new XmlNode[0];
                }
                
                // Check the bounds of the index.
                if (index < 0 || index >= DataState.OptionsNodes.Count) {
                    Debug.LogWarning(message: "Tried to get option nodes when none are loaded");
                    return new XmlNode[0];
                }
                
                // Return the node instance.
                return DataState.OptionsNodes[i: index]
                    .SelectNodes(xpath: "henshin:option", nsmgr: DataState.NamespaceManager)
                    ?.Cast<XmlNode>() ?? 
                    new XmlNode[0];
            }
        
        // -- Private Methods --
            // - File Management -
            /// <summary>
            /// Seeks the file on the file system and loads it
            /// in the <see cref="DataState.File"/> reference.
            /// </summary>
            private static void _FindFile() {
                // Seek the file in the resources.
                TextAsset xmlFile = Resources.Load<TextAsset>(path: DataController._mFILE_LOCATION);
                
                // Check if the file was found.
                if (xmlFile != null) {
                    // Store the file reference.
                    DataState.File = xmlFile;
                } else {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "Could not find the GameData file.");
                }
            }
            
            /// <summary>
            /// Loads the contents of the <see cref="DataState.File"/> into the <see cref="DataState.Document"/>.
            /// </summary>
            /// <exception cref="InvalidOperationException">
            /// The file object is unset or its contents are not in a valid format.
            /// </exception>
            private static void _LoadFileContents() {
                // Create the document instance.
                DataState.Document = new XmlDocument();
                
                try {
                    // Parse the contents of the file.
                    DataState.Document.PreserveWhitespace = false;
                    DataState.Document.LoadXml(xml: DataState.File.text);

                    // Load the namespace.
                    DataState.NamespaceManager = new XmlNamespaceManager(nameTable: DataState.Document.NameTable);
                    DataState.NamespaceManager.AddNamespace(prefix: "henshin", uri: "https://henshin.jb-caillaud.fr/");
                } catch (XmlException) {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "The contents of the GameData file are not valid.");
                } catch (NullReferenceException) {
                    // Throw an exception.
                    throw new InvalidOperationException(message: "Tried to load xml data without a file.");
                }
            }
            
            /// <summary>
            /// Reloads the contents of the xml file if necessary.
            /// In editor, compares MD5 hashes of the files to know if reloading is necessary.
            /// Otherwise, reloads only if the <see cref="DataState.File"/> object is <code>null</code>.
            /// </summary>
            private static void _ReloadFile() {
                // Check if the file is set.
                if (DataState.File == null) {
                    // Find the text file.
                    DataController._FindFile();
                    
                    // Load the file contents.
                    DataController._LoadFileContents();
#if UNITY_EDITOR
                    // Create the hash manager instance.
                    DataController._msMd5 = MD5.Create();
                    
                    // Get the hash of the file.
                    DataState.Hash = DataController._msMd5.ComputeHash(
                        buffer: Encoding.UTF8.GetBytes(s: DataState.File.text)
                    );
                } else {
                    // Get the hash of the file contents.
                    byte[] hash = DataController._msMd5.ComputeHash(
                        buffer: Encoding.UTF8.GetBytes(s: DataState.File.text)
                    );
                    
                    // Compare the hashes of the file.
                    if (!hash.SequenceEqual(second: DataState.Hash)) {
                        // Update the hash.
                        DataState.Hash = hash;
                        
                        // Reload the contents of the file.
                        DataController._LoadFileContents();
                        
                        // Reload the current scene and act.
                        DataController.LoadScene(act: DataState.ActIndex, scene: DataState.SceneIndex);
                    }
#endif
                }
            }
    // --- /Methods ---
}
}