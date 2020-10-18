using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GraphInteger<T> where T : GraphComponent
{
    /// <summary>
    /// Adds a GraphComponent to the graph
    /// </summary>
    bool addComponent(T component);

    /// <summary>
    /// Removes a Graph Component from the Graph
    /// </summary>
    bool removeComponent(T component);

    /// <summary>
    /// Checks to see if the GraphComponent can be placed on the Graph
    /// </summary>
    bool canPlace(T component);

    /// <summary>
    /// Attempts to get the GraphComponent at the position
    /// </summary>
    T getComponentAt(int x, int y);

    /// <summary>
    /// A list of all GraphComponents
    /// </summary>
    List<T> getAllGraphComponents();
}