using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGraph : TExtension, GraphInteger<LightComponent>  {

    private List<PassiveComponent> passiveComponents = new List<PassiveComponent>();
    private List<LogicComponent> logicComponents = new List<LogicComponent>();

    private int width;
    private int height;

    public const int MaxRayDistance = 1000;
    public const int maxBounces = 16;

    public LogicGraph(int width, int height, string name, Vector2Int position) : base(name,position) {

        this.width = width;
        this.height = height;
    }

    #region classSpecific

    public void addComponentAndConnect(LightComponent component) {
        //clears all the components and then reconnects them all with the new component

        this.disconnectGraph();
        addComponent(component);
        this.connectGraph();
    }

    public void connectGraph() {

        List<InteractiveComponent> interComps = this.getAllInteractiveComponents();

        for (int i = 0; i < interComps.Count; i++) {

            InteractiveComponent currentComp = interComps[i];

            for (int s = 0; s < currentComp.senderCount(); s++) {

                //Debug.Log(s);

                Sender sender = currentComp.getSenderAt(s);
                int direction = currentComp.Rotation;

                //Debug.Log("Gate: " + logicComponent.GetType() + " Sender: " + s + " direction: " + direction);

                //gets the connections for the sender
                sender.setTargets(getConnections(sender, direction));
                //Debug.Log("Number of connects: " + sender.Targets);
            }
        }
    }

    public void disconnectGraph() {
        //disconnects all the components from each other

        for (int c = 0; c < this.getLogicComponentCount(); c++) {
            LogicComponent l = this.getLogicComponentAt(c);
            for (int s = 0; s < l.senderCount(); s++) {
                l.getSenderAt(s).clearTargets();
                //Debug.Log("After Clear: " + this.getLogicComponentAt(c).getSenderAt(s).getTargetCount());
            }
        }
    }

    private Direction reflect(Direction incomingDirection, int componentRotation, bool flipped) {

        int realRotation = componentRotation;
        if (flipped) {
            realRotation = ((realRotation + 1) % 4);
        }

        //Debug.Log("real rotation: " + realRotation);
        Direction outgoingDirection = Direction.North;

        if (realRotation == 0) {

            if (incomingDirection == Direction.North) {
                outgoingDirection = Direction.East;

            } else if (incomingDirection == Direction.West) {
                outgoingDirection = Direction.South;

            } else if (incomingDirection == Direction.South) {
                outgoingDirection = Direction.West;

            } else if (incomingDirection == Direction.East) {
                outgoingDirection = Direction.North;
            }

        } else if (realRotation == 1) {

            if (incomingDirection == Direction.North) {
                outgoingDirection = Direction.West;

            } else if (incomingDirection == Direction.East) {
                outgoingDirection = Direction.South;

            } else if (incomingDirection == Direction.South) {
                outgoingDirection = Direction.East;

            } else if (incomingDirection == Direction.West) {
                outgoingDirection = Direction.North;
            }

        } else if (realRotation == 2) {

            if (incomingDirection == Direction.East) {
                outgoingDirection = Direction.North;

            } else if (incomingDirection == Direction.South) {
                outgoingDirection = Direction.West;

            } else if (incomingDirection == Direction.West) {
                outgoingDirection = Direction.South;

            } else if (incomingDirection == Direction.North) {
                outgoingDirection = Direction.East;
            }

        } else if (realRotation == 3) {

            if (incomingDirection == Direction.South) {
                outgoingDirection = Direction.East;

            } else if (incomingDirection == Direction.West) {
                outgoingDirection = Direction.North;

            } else if (incomingDirection == Direction.North) {
                outgoingDirection = Direction.West;

            } else if (incomingDirection == Direction.East) {
                outgoingDirection = Direction.South;
            }
        }

        //Debug.Log("incoming direction: " + incomingDirection + " outgoingDirection: " + outgoingDirection);

        return outgoingDirection;
    }

    private List<Receiver> getConnections(ComponentPiece currentPiece, int rotation) {
        //gets the connections for the sender
        return this.getConnections(0, LogicGraph.maxBounces, currentPiece, rotation);
    }

    private List<Receiver> getConnections(int currentBounces, int maxBounces, ComponentPiece currentPiece, int rotation) {
        //passes in a piece and the sees it it connects to a reciever eventually.

        //stores all of the recievers connected to
        List<Receiver> result = new List<Receiver>();


        List<LightComponent> components = this.getAllGraphComponents();

        //default to rotation 0
        Func<ComponentPiece, ComponentPiece, bool> selector = (closestPiece, checkingPiece) => {
            //gets the smallest y value
            bool isCloser = false;
            if (closestPiece == null || (checkingPiece.Rect.y < closestPiece.Rect.y)) {
                isCloser = true;
            }
            return isCloser;
        };

        //rotation 1
        if (rotation == 1) {
            selector = (closestPiece, checkingPiece) => {
                //gets the smallest x value
                bool isCloser = false;
                if (closestPiece == null || (checkingPiece.Rect.x < closestPiece.Rect.x)) {
                    isCloser = true;
                }
                return isCloser;
            };

        } else if (rotation == 2) {
            selector = (closestPiece, checkingPiece) => {
                //gets the biggest y value
                bool isCloser = false;
                if (closestPiece == null || (checkingPiece.Rect.y > closestPiece.Rect.y)) {
                    isCloser = true;
                }
                return isCloser;
            };

        } else if (rotation == 3) {
            selector = (closestPiece, checkingPiece) => {
                //gets the biggest x value
                bool isCloser = false;
                if (closestPiece == null || (checkingPiece.Rect.x > closestPiece.Rect.x)) {
                    isCloser = true;
                }
                return isCloser;
            };
        }

        Rect ray = getRayRect((Direction)rotation, currentPiece.Rect, new Vector2(.2f, MaxRayDistance));
        //Debug.Log(ray);

        GraphComponent currentClosestComponent = null;
        ComponentPiece currentClosestPiece = null;

        //gets the closest piece it collides with
        //goes through each component
        for (int c = 0; c < components.Count; c++) {

            GraphComponent currentComponent = components[c];
            List<ComponentPiece> pieces = currentComponent.ComponentPieces;

            //loops though all the pieces
            for (int p = 0; p < pieces.Count; p++) {

                //checks if overlaps with ray and is closer
                if (ray.Overlaps(pieces[p].Rect)) {

                    if (selector(currentClosestPiece, pieces[p])) {
                        currentClosestPiece = pieces[p];
                        currentClosestComponent = currentComponent;
                    }
                }
                /*
                if (currentClosestPiece != null) {
                    Debug.Log(pieces[p].GetType() + " at " + pieces[p].Rect.center + " is closer than " + currentClosestPiece.GetType() + " at " + currentClosestPiece.Rect.center);
                } else {
                    Debug.Log(pieces[p].GetType() + " is the first hit");
                }
                */
            }
        }

        if (currentClosestPiece != null && currentBounces < LogicGraph.maxBounces) {
            //Debug.Log("the closest is: " + currentClosestPiece.GetType() + " at " + currentClosestPiece.Rect.center);

            if (currentClosestPiece.GetType() == typeof(Mirror)) {

                //reflect off of the mirror
                Direction newDirection = reflect((Direction)rotation, currentClosestComponent.Rotation, currentClosestComponent.Flipped);

                //adds the result from the next beam to this list
                result.AddRange(getConnections(currentBounces++, LogicGraph.maxBounces, currentClosestPiece, (int)newDirection));

            } else if (currentClosestPiece.GetType() == typeof(PassingMirror)) {

                //goes through the passing mirror
                result.AddRange(getConnections(currentBounces++, LogicGraph.maxBounces, currentClosestPiece, rotation));

                //reflects off the passing mirror
                Direction newDirection = reflect((Direction)rotation, currentClosestComponent.Rotation, currentClosestComponent.Flipped);
                result.AddRange(getConnections(currentBounces++, LogicGraph.maxBounces, currentClosestPiece, (int)newDirection));

            } else if (currentClosestPiece.GetType().IsSubclassOf(typeof(Receiver))) {
                result.Add((Receiver)currentClosestPiece);
            }

        } else {
            //Debug.Log("Did not strike gold");
        }

        //Debug.Log("Connections List Size: " + result.Count);
        return result;
    }

    private Rect getRayRect(Direction outgoingDirection, Rect piece, Vector2 rayVertical) {

        Vector2 raySize = rayVertical;

        float x = Mathf.Floor(piece.x);
        float y = Mathf.Floor(piece.y);

        if (outgoingDirection == Direction.North) {
            x += ((1 - raySize.x) / 2f);
            y += 1;

        } else if (outgoingDirection == Direction.East) {
            raySize = new Vector2(raySize.y, raySize.x);
            x += 1;
            y += ((1 - raySize.y) / 2f);

        } else if (outgoingDirection == Direction.South) {
            x += ((1 - raySize.x) / 2f);
            y -= raySize.y;

        } else if (outgoingDirection == Direction.West) {
            raySize = new Vector2(raySize.y, raySize.x);
            y += ((1 - raySize.y) / 2f);
            x -= raySize.x;
        }

        Rect ray = new Rect(new Vector2(x, y), raySize);
        return ray;
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

    public List<InteractiveComponent> getAllInteractiveComponents() {
        //gets all the interactive components

        List<InteractiveComponent> result = new List<InteractiveComponent>();

        foreach (LogicComponent lc in this.logicComponents) {
            result.Add(lc);
        }
        foreach (SendBridge sb in this.sendBridges) {
            result.Add(sb);
        }
        foreach (ReceiveBridge rb in this.receiveBridges) {
            result.Add(rb);
        }

        return result;
    }

    #endregion

    #region GraphInteger

    public bool addComponent(LightComponent comp) {

        bool result = false;

        if (this.canPlace(comp)) {

            result = true;

            if (comp.GetType().BaseType == typeof(LogicComponent)) {

                this.logicComponents.Add((LogicComponent)comp);
                result = true;

            } else if (comp.GetType().BaseType == typeof(PassiveComponent)) {

                this.passiveComponents.Add((PassiveComponent)comp);
                result = true;
            } else if (comp.GetType().BaseType == typeof(BridgeComponent)) {

                result = true;

                if (comp.GetType() == typeof(ReceiveBridge)) {
                    this.ReceiveBridges.Add((ReceiveBridge)comp);

                } else if (comp.GetType() == typeof(SendBridge)) {
                    this.sendBridges.Add((SendBridge)comp);

                } else {
                    throw new System.Exception(comp.GetType() + " can not be added to the Logic Graph the Bridge type is not supported");
                }
            } else {
                throw new System.Exception(comp.GetType() + " can not be added to the Logic Graph becasue the Type does not match");
            }
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
        foreach (SendBridge sb in this.sendBridges) {
            result.Add(sb);
        }
        foreach (ReceiveBridge rb in this.receiveBridges) {
            result.Add(rb);
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
                } else if (component.GetType().IsSubclassOf(typeof(BridgeComponent))) {
                    if (component.GetType() == typeof(ReceiveBridge)) {
                        result = this.ReceiveBridges.Remove((ReceiveBridge)component);
                    } else if (component.GetType() == typeof(SendBridge)) {
                        result = this.sendBridges.Remove((SendBridge)component);
                    } else {
                        throw new System.Exception(component.GetType() + " can not be removed to the Logic Graph");
                    }
                } else {
                    throw new Exception("TypeNotFound: type " + component.GetType() + " is not a subclass on the looked at classes");
                }

                //Debug.Log("Logic Graph -> removeComponent(): " + result);
            }
            counter++;
        }

        if (result == true) {
            //redundent statement but i like it

            this.disconnectGraph();
            this.connectGraph();
        }

        return result;
    }

    public bool canPlace(LightComponent component) {
        //checks to see if the component can be placed on the graph
        bool result = true;
        int counter = 0;

        List<LightComponent> all = this.getAllGraphComponents();

        while (result && counter < all.Count) {

            if (component.getDimentions().Overlaps(all[counter].getDimentions())) {
                result = false;
            }

            counter++;
        }

        return result;
    }

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

    #endregion

    #region TExtension

    public override void setState() {
        foreach (LogicComponent lc in this.getAllInteractiveComponents()) {
            lc.setState();
            //Debug.Log("set state:" + lc.GetType().ToString() + " " + lc.Position + " state:" + lc.getState());
        }
    }

    public override void sendSignal() {

        foreach (LogicComponent lc in this.logicComponents) {
            lc.SendSignal();
        }
    }

    public override void clearReceivers() {
        //clears all the receiver bridges and the interactive components

        for (int i = 0; i < this.receiveBridges.Count; i++) {
            this.receiveBridges[i].clearState();
        }

        List<InteractiveComponent> intComps = this.getAllInteractiveComponents();
        for (int i = 0; i < intComps.Count; i++) {
            intComps[i].clearRecievers();
        }
    }

    #endregion

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
