using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicComponentController : InteractiveComponentController {

    private LogicComponent logicComponent;

    public override void setUp(GraphComponent logicComponent) {

        this.graphComponent = logicComponent;

        this.logicComponent = (LogicComponent)logicComponent;


        //updates the sprite of the logic component
        string gateName = "";
        if (logicComponent.GetType() == typeof(AndGate)) {
            gateName = "And";
        } else if (logicComponent.GetType() == typeof(NotGate)) {
            gateName = "Not";
        } else if (logicComponent.GetType() == typeof(OrGate)) {
            gateName = "Or";
        } else if (logicComponent.GetType() == typeof(XorGate)) {
            gateName = "Xor";
        } else if (logicComponent.GetType() == typeof(XnorGate)) {
            gateName = "Xnor";
        } else if (logicComponent.GetType() == typeof(BufferGate)) {
            gateName = "Buffer";
        } else if (logicComponent.GetType() == typeof(NorGate)) {
            gateName = "Nor";
        } else if (logicComponent.GetType() == typeof(NandGate)) {
            gateName = "Nand";
        }

        if (gateName.Equals("")) {
            throw new System.Exception(logicComponent.GetType() + " could not be found amoung the sprites");
        }

        this.formBasicShape(this.logicComponent);
        this.displaySprite(gateName);
    }

    public LogicComponent LogicComponent {
        get {
            return this.logicComponent;
        }
    }
}
