// Copyright 2020 © Caillaud Jean-Baptiste. All rights reserved.

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Henshin.Runtime.Application;
using Henshin.Runtime.Directions.Act;
using Henshin.Runtime.Directions.Scene;
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
                        // Check if the x and y attributes exist.
                        if (size.Attributes?[name: "width"] != null && size.Attributes?[name: "height"] != null) {
                            // Load the size of the answer.
                            DataState.AnswerObjectSize = new Vector2(
                                x: int.Parse(s: size.Attributes[name: "width"].Value),
                                y: int.Parse(s: size.Attributes[name: "height"].Value)
                            );
                            
                        }
                        
                        // Check if the duplicate attribute exists.
                        if (size.Attributes?[name: "duplicate"] != null) {
                            // Load the duplicate flag.
                            DataState.DuplicateAnswers = bool.Parse(value: size.Attributes[name: "duplicate"].Value);
                        }
                    }
                    
                    // Load the original and translated texts.
                    DataState.Original   = DataController._GetNodeValues(parent: gameplayNode, name: "henshin:original");
                    DataState.Translated = DataController._GetNodeValues(parent: gameplayNode, name: "henshin:translated");
                    
                    // Seek the option nodes.
                    XmlNodeList optionsNodes = gameplayNode.SelectNodes(
                         xpath: "henshin:options", 
                        nsmgr: DataState.NamespaceManager
                    );
                    
                    // If no option was found.
                    if (optionsNodes == null) {
                        // Clear the array.
                        DataState.Options = new string[0][];
                    } else {
                        // Create the data array.
                        DataState.Options = new string[optionsNodes.Count][];
                        
                        // Loop through all the options.
                        for (int i = 0; i < optionsNodes.Count; i++) {
                            // Load the options list.
                            XmlNode optionsNode = optionsNodes[i: i];
                            
                            // Load its children nodes.
                            XmlNodeList optionNodes = optionsNode.SelectNodes(
                                xpath: "henshin:option", 
                                nsmgr: DataState.NamespaceManager
                            );
                            
                            // If the nodes were found.
                            if (optionNodes != null) {
                                // Create the data array.
                                DataState.Options[i] = new string[optionNodes.Count];
                                
                                // Loop through the nodes.
                                for (int j = 0; j < optionNodes.Count; j++) {
                                    // Load the data at the specified index.
                                    DataState.Options[i][j] = optionNodes[i: j].InnerText;
                                }
                            } else {
                                // Clear the data array.
                                DataState.Options[i] = new string[0];
                            }
                        }
                    }
                    
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
        
        // -- Private Methods --
            // - Xml Management -
            /// <summary>
            /// Loads all the node texts found in the specified nodes.
            /// </summary>
            /// <param name="parent">The parent of the nodes to parse.</param>
            /// <param name="name">The name of the children nodes to parse.</param>
            /// <returns>The list of string found in the children.</returns>
            /// <exception cref="InvalidOperationException">
            /// There is no node with the specified <see cref="name"/> in the <see cref="parent"/>.
            /// </exception>
            private static string[] _GetNodeValues(XmlNode parent, string name) {
                // Seek the nodes.
                XmlNodeList children = parent.SelectNodes(xpath: name, nsmgr: DataState.NamespaceManager);
                
                // Check if the children nodes were found.
                if (children == null || children.Count <= 0) {
                    throw new InvalidOperationException(message: $"Could not find a '{children}' node.");
                }
                
                // Load its contents.
                return children.Cast<XmlNode>().Select(selector: child => child.InnerText).ToArray();
            }
            
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
                        // Reload the contents of the file.
                        DataController._LoadFileContents();
                        
                        // Reload the current scene and act.
                        DataController.LoadScene(act: DataState.ActIndex, scene: DataState.SceneIndex);
                    
                        // Update the hash.
                        DataState.Hash = hash;
                    }
#endif
                }
            }
    // --- /Methods ---
}
}