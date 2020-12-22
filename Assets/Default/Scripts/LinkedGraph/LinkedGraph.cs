using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A linked graph that shows the realationships between values. It can also be used to find links in a graph. <br></br>
/// K - Is the Key used to look up the Value <br></br>
/// V - The Value you want to store in the graph
/// </summary>
public class LinkedGraph<K, V>
{
    private Dictionary<K, LinkedNode> searchDictionary = new Dictionary<K, LinkedNode>();

    /// <summary>
    /// Used to show they relation between values 
    /// </summary>
    public LinkedGraph() { }

    /// <summary>
    /// Adds a node to the graph
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void addNode(K key, V value) {
        if (searchDictionary[key] == null) {
            throw new System.Exception("KeyExists: The key " + key.ToString() + " already exists in the Linked Graph");
        }
        searchDictionary.Add(key, new LinkedNode(value));
    }

    /// <summary>
    /// Links two nodes together based on their keys
    /// </summary>
    /// <param name="keyOne"></param>
    /// <param name="keyTwo"></param>
    public void link(K keyOne, K keyTwo) {
        searchDictionary[keyOne].addLink(searchDictionary[keyTwo]);
    }

    /// <summary>
    /// Returns all the Values that are linked to this value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public V[] getLinked(K key) {
        return this.searchDictionary[key].getLinks().Select(x => x.Value).ToArray();
    }

    /// <summary>
    /// Checks if there is a loop in any of the LinkedNodes
    /// </summary>
    /// <returns></returns>
    public bool loops() {
        this.markAllSearchValue(false);
        LinkedNode[] arr = searchDictionary.Values.ToArray();
        bool result = false;
        int counter = 0;
        while(!result && counter < arr.Length) {

            result = hasLoop(arr[counter], new Stack<LinkedNode>());
            counter++;
        }

        return result;
    }

    private void markAllSearchValue(bool searched) {
        LinkedNode[] nodes = searchDictionary.Values.ToArray();
        for(int i = 0; i < nodes.Length; i++) {
            nodes[i].searched = searched;
        }
    }

    /// <summary>
    /// recursivly calls to check to see if the LinkNode has a loop in it
    /// </summary>
    /// <param name="node"></param>
    /// <param name="stack"></param>
    /// <returns></returns>
    private bool hasLoop(LinkedNode node, Stack<LinkedNode> stack) {

        bool result = false;

        if (node.searched) {
            if (Array.Exists(stack.ToArray(), current => current.Equals(node))) {
                return true;
            }

            stack.Push(node);
            int counter = 0;
            LinkedNode[] linked = node.LinkedNodes;

            while (!result && counter < linked.Length) {

                result = hasLoop(linked[counter], stack);
                counter++;
            }
            stack.Pop();
        }

        node.searched = true;

        return result;
    }

    /// <summary>
    /// Get all the values in the graph
    /// </summary>
    public V[] AllValues {
        get {
            return searchDictionary.Values.Select(x => x.Value).ToArray();
        }
    }

    private class LinkedNode
    {
        private V value;
        public bool searched = false;

        private List<LinkedNode> linkedNodes = new List<LinkedNode>();
        public LinkedNode(V value) {
            this.value = value;
        }

        public void addLink(LinkedNode node) {
            linkedNodes.Add(node);
        }

        public LinkedNode[] getLinks() {
            return this.linkedNodes.ToArray();
        }

        public V Value {
            get {
                return value;
            }
        }

        public LinkedNode[] LinkedNodes {
            get {
                return linkedNodes.ToArray();
            }
        }
    }
}