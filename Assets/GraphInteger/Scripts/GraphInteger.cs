using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GraphInteger<T>
{

    bool addComponent(T component);
    bool removeComponent(T component);
    bool canPlace(T component);
    T getComponentAt(int x, int y);
    List<T> getAllGraphComponents();

}