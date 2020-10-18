using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGraph : GraphInteger<LightComponent>
{

    private List<PassiveComponent> passiveComponents = new List<PassiveComponent>();
    private List<LogicComponent> logicComponents = new List<LogicComponent>();
    private List<LinkComponent> linkComponents = new List<LinkComponent>();

    private int width;
    private int height;

    public const int MaxRayDistance = 1000;
    public const int maxBounces = 16;

    private LGUtilities LGUtilities;
    public LogicGraph logicGraph;

    public LightGraph(LogicGraph logicGraph, int width, int height) {
        this.width = width;
        this.height = height;
        this.LGUtilities = new LGUtilities(this);
        this.logicGraph = logicGraph;
    }

    public bool addComponent(LightComponent comp) {

        bool result = false;

        if (this.canPlace(comp)) {

            this.LGUtilities.disconnectGraph();

            result = true;

            if (comp.GetType().BaseType == typeof(LogicComponent)) {

                this.logicComponents.Add((LogicComponent)comp);
                result = true;

            } else if (comp.GetType().BaseType == typeof(PassiveComponent)) {

                this.passiveComponents.Add((PassiveComponent)comp);
                result = true;
            } else if (comp.GetType().BaseType == typeof(LinkComponent)) {

                result = true;

                this.linkComponents.Add((LinkComponent)comp);

                if (comp.GetType() == typeof(GraphInput)) {
                    this.logicGraph.addReceiveBridge((ReceiveBridge)((GraphInput)comp).getExtensionConnection());
                    

                } else if (comp.GetType() == typeof(GraphOutput)) {
                    this.logicGraph.addSendBridge((SendBridge)((GraphOutput)comp).getExtensionConnection());

                } else {
                    throw new System.Exception(comp.GetType() + " can not be added to the Logic Graph the Bridge type is not supported");
                }
            } else {
                throw new System.Exception(comp.GetType() + " can not be added to the Logic Graph becasue the Type does not match");
            }
        }

        if (result) {
            this.LGUtilities.connectGraph();
        }

        return result;
    }

    public List<LightComponent> getAllGraphComponents() {

        List<LightComponent> result = new List<LightComponent>();

        foreach (LogicComponent lc in this.logicComponents) {
            result.Add(lc);
        }
        foreach (PassiveComponent pc in this.passiveComponents) {
            result.Add(pc);
        }
        foreach (LinkComponent lc in this.linkComponents) {
            result.Add(lc);
        }

        return result;
    }

    public bool removeComponent(LightComponent component) {
        //removes the component from the graph
        bool result = false;

        List<LightComponent> all = this.getAllGraphComponents();
        int counter = 0;

        while (counter < all.Count && !result) {

            if (all[counter].Equals(component)) {

                if (component.GetType().IsSubclassOf(typeof(LogicComponent))) {
                    result = this.logicComponents.Remove((LogicComponent)component);
                } else if (component.GetType().IsSubclassOf(typeof(PassiveComponent))) {
                    result = this.passiveComponents.Remove((PassiveComponent)component);
                } else if (component.GetType().IsSubclassOf(typeof(LinkComponent))) {
                    if (component.GetType() == typeof(GraphInput)) {

                        GraphInput rec = (GraphInput)component;
                        rec.getExtensionConnection().clearConnections();
                        result = this.logicGraph.removeReceiveBridge((ReceiveBridge)rec.getExtensionConnection());
                        this.linkComponents.Remove(rec);

                    } else if (component.GetType() == typeof(GraphOutput)) {
                        GraphOutput send = (GraphOutput)component;
                        send.getExtensionConnection().clearConnections();
                        result = this.logicGraph.removeSendBridge((SendBridge)send.getExtensionConnection());
                        this.linkComponents.Remove(send);

                    } else {
                        throw new System.Exception(component.GetType() + " can not be removed to the Logic Graph");
                    }
                } else {
                    throw new Exception("TypeNotFound: type " + component.GetType() + " is not a subclass on the looked at classes");
                }

            }
            counter++;
        }

        if (result == true) {
            //redundent statement but i like it

            this.LGUtilities.disconnectGraph();
            this.LGUtilities.connectGraph();
        }

        return result;
    }

    /// <summary>
    /// Checks to see if the Component can be placed on the Graph
    /// </summary>
    public bool canPlace(LightComponent component) {
        //checks to see if the component can be placed on the graph
        bool result = true;
        int counter = 0;

        List<LightComponent> all = this.getAllGraphComponents();

        Rect dimentions = component.getDimentions();

        if (dimentions.position.x + dimentions.width > this.Width ||
            dimentions.position.y + dimentions.height > this.Height) {
            result = false;
        }

        while (result && counter < all.Count) {

            if (dimentions.Overlaps(all[counter].getDimentions())) {
                result = false;
            }

            counter++;
        }

        return result;
    }

    /// <summary>
    /// Returns a component at the passed position if it exits
    /// </summary>
    public LightComponent getComponentAt(Vector2Int position) {
        return getComponentAt(position.x, position.y);
    }

    /// <summary>
    /// Returns a component at the passed position if it exits
    /// </summary>
    public LightComponent getComponentAt(int x, int y) {
        //gets the logic component at the x and y position
        LightComponent result = null;

        List<LightComponent> all = this.getAllGraphComponents();
        int counter = 0;

        while (counter < all.Count && result == null) {

            if (all[counter].getDimentions().Overlaps(new Rect(x + .1f, y + .1f, .8f, .8f))) {
                result = all[counter];
            }

            counter++;
        }

        return result;
    }

    public List<InteractiveComponent> getAllInteractiveComponents() {
        //gets all the interactive components

        List<InteractiveComponent> result = new List<InteractiveComponent>();

        foreach (LogicComponent lc in this.logicComponents) {
            result.Add(lc);
        }
        foreach (LinkComponent link in this.linkComponents) {
            result.Add(link);
        }

        return result;
    }

    public LogicComponent getLogicComponentAt(int index) {
        return this.logicComponents[index];
    }

    public int getLogicComponentCount() {
        return this.logicComponents.Count;
    }

    public PassiveComponent getPassiveComponentAt(int index) {
        return this.passiveComponents[index];
    }

    public int getPassiveComponentCount() {
        return this.passiveComponents.Count;
    }

    public void setState() {
        foreach (LogicComponent lc in this.logicComponents) {
            lc.setState();
        }
    }

    public void sendSignal() {
        foreach (LogicComponent lc in this.logicComponents) {
            lc.SendSignal();
        }
    }

    public void clearReceivers() {
        List<InteractiveComponent> intComps = this.getAllInteractiveComponents();
        for (int i = 0; i < intComps.Count; i++) {
            intComps[i].clearRecievers();
        }
    }

    public int Width {
        get {
            return this.width;
        }
    }

    public int Height {
        get {
            return this.height;
        }
    }
}