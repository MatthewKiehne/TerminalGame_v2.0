using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGUtilities
{
    private LightGraph lightGraph;
    public LGUtilities(LightGraph lightGraph) {
        this.lightGraph = lightGraph;
    }

    public void connectGraph() {

        List<InteractiveComponent> interComps = this.lightGraph.getAllInteractiveComponents();

        for (int i = 0; i < interComps.Count; i++) {

            InteractiveComponent currentComp = interComps[i];

            for (int s = 0; s < currentComp.senderCount(); s++) {

                Sender sender = currentComp.getSenderAt(s);
                int direction = currentComp.Rotation;

                //gets the connections for the sender
                sender.setTargets(getConnections(sender, direction));
            }
        }
    }

    public void disconnectGraph() {
        //disconnects all the components from each other

        for (int c = 0; c < this.lightGraph.getLogicComponentCount(); c++) {
            LogicComponent l = this.lightGraph.getLogicComponentAt(c);
            for (int s = 0; s < l.senderCount(); s++) {
                l.getSenderAt(s).clearTargets();
            }
        }
    }

    /// <summary>
    /// Retruns the dirction the ray will be going after being reflected
    /// </summary>
    private Direction reflect(Direction incomingDirection, int componentRotation, bool flipped) {

        int realRotation = componentRotation;
        if (flipped) {
            realRotation = ((realRotation + 1) % 4);
        }

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

        return outgoingDirection;
    }

    private List<Receiver> getConnections(ComponentPiece currentPiece, int rotation) {
        //gets the connections for the sender
        return this.getConnections(0, LightGraph.maxBounces, currentPiece, rotation);
    }

    private List<Receiver> getConnections(int currentBounces, int maxBounces, ComponentPiece currentPiece, int rotation) {
        //passes in a piece and the sees it it connects to a reciever eventually.

        //stores all of the recievers connected to
        List<Receiver> result = new List<Receiver>();


        List<LightComponent> components = this.lightGraph.getAllGraphComponents();

        //default to rotation 0
        Func<ComponentPiece, ComponentPiece, bool> selector = (closestPiece, checkingPiece) =>
        {
            //gets the smallest y value
            bool isCloser = false;
            if (closestPiece == null || (checkingPiece.Rect.y < closestPiece.Rect.y)) {
                isCloser = true;
            }
            return isCloser;
        };

        if (rotation == 1) {
            selector = (closestPiece, checkingPiece) =>
            {
                //gets the smallest x value
                bool isCloser = false;
                if (closestPiece == null || (checkingPiece.Rect.x < closestPiece.Rect.x)) {
                    isCloser = true;
                }
                return isCloser;
            };

        } else if (rotation == 2) {
            selector = (closestPiece, checkingPiece) =>
            {
                //gets the biggest y value
                bool isCloser = false;
                if (closestPiece == null || (checkingPiece.Rect.y > closestPiece.Rect.y)) {
                    isCloser = true;
                }
                return isCloser;
            };

        } else if (rotation == 3) {
            selector = (closestPiece, checkingPiece) =>
            {
                //gets the biggest x value
                bool isCloser = false;
                if (closestPiece == null || (checkingPiece.Rect.x > closestPiece.Rect.x)) {
                    isCloser = true;
                }
                return isCloser;
            };
        }

        Rect ray = getRayRect((Direction)rotation, currentPiece.Rect, new Vector2(.2f, getMaxRayLength()));

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
            }
        }

        if (currentClosestPiece != null && currentBounces < LightGraph.maxBounces) {

            if (currentClosestPiece.GetType() == typeof(Mirror)) {

                //reflect off of the mirror
                Direction newDirection = reflect((Direction)rotation, currentClosestComponent.Rotation, currentClosestComponent.Flipped);

                //adds the result from the next beam to this list
                result.AddRange(getConnections(currentBounces++, LightGraph.maxBounces, currentClosestPiece, (int)newDirection));

            } else if (currentClosestPiece.GetType() == typeof(PassingMirror)) {

                //goes through the passing mirror
                result.AddRange(getConnections(currentBounces++, LightGraph.maxBounces, currentClosestPiece, rotation));

                //reflects off the passing mirror
                Direction newDirection = reflect((Direction)rotation, currentClosestComponent.Rotation, currentClosestComponent.Flipped);
                result.AddRange(getConnections(currentBounces++, LightGraph.maxBounces, currentClosestPiece, (int)newDirection));

            } else if (currentClosestPiece.GetType().IsSubclassOf(typeof(Receiver))) {
                result.Add((Receiver)currentClosestPiece);
            }

        }

        return result;
    }

    /// <summary>
    /// Returns the Rect for a Ray
    /// </summary>
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

    /// <summary>
    /// Returns the length of the longest possible ray
    /// </summary>
    public int getMaxRayLength() {
        int result = this.lightGraph.Width;
        if(result < this.lightGraph.Height) {
            result = this.lightGraph.Height;
        }
        return result;
    }
}