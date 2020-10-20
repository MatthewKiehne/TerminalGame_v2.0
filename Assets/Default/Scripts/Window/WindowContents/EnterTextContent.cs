using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterTextContent : WindowContent {

    private List<CheckError> errorList = new List<CheckError>();
    private Action<string> confirmAction;
    private Action cancelAction;

    private string instructions;

    private InputField inputField;
    private string startText = "";
    private int maxCharacters;

    private Transform ErrorPanel;
    private Text errorText;

    private class CheckError {

        private Func<string, bool> function;
        private string errorMessage;

        public CheckError(Func<string, bool> function, string errorMessage) {
            this.function = function;
            this.errorMessage = errorMessage;
        }

        public bool checkError(string input) {
            return this.function.Invoke(input);
        }

        public string ErrorMessage {
            get {
                return this.errorMessage;
            }
        }
    }

    public EnterTextContent(string instructions, Action<string> confirmAction, Action cancelAction, int maxCharacters) {

        this.instructions = instructions;
        this.maxCharacters = maxCharacters;

        this.confirmAction = confirmAction;
        this.cancelAction = cancelAction;
    }

    public EnterTextContent(string instructions, Action<string> confirmAction, Action cancelAction, int maxCharacters, string startingText) {

        this.instructions = instructions;
        this.maxCharacters = maxCharacters;

        this.confirmAction = confirmAction;
        this.cancelAction = cancelAction;

        this.startText = startingText;
    }

    public void addErrorCheck(Func<string, bool> function, string errorMessage) {

        this.errorList.Add(new CheckError(function, errorMessage));
    }

    public bool checkErrors() {
        //checks to see if the input passes all the error checks
        //displays the error if it occurs

        bool passed = true;
        int counter = 0;

        while(counter < this.errorList.Count && passed) {

            passed = this.errorList[counter].checkError(this.inputField.text);

            counter++;
        }

        if (!passed) {

            this.ErrorPanel.gameObject.SetActive(true);
            this.errorText.text = this.errorList[counter - 1].ErrorMessage;
        } 

        return passed;
    }

    public override void spawnContents(WindowController windowController, Transform contentPanel, Canvas canvas) {

        GameObject fields = GameObject.Instantiate((GameObject)SceneResouces.SceneObjects["Default"][typeof(GameObject)]["EnterTextPanel"]);
        fields.transform.SetParent(contentPanel.transform, false);

        this.inputField = fields.transform.Find("InputField").GetComponent<InputField>();
        this.inputField.characterLimit = this.maxCharacters;
        this.inputField.Select();
        this.inputField.text = this.startText;

        fields.transform.Find("Instructions").GetComponent<Text>().text = this.instructions;

        this.ErrorPanel = fields.transform.Find("ErrorPanel");
        this.errorText = this.ErrorPanel.Find("ErrorText").GetComponent<Text>();

        Transform bottomPanel = fields.transform.Find("BottomPanel");

        bottomPanel.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(() => {

            if (this.checkErrors()) {
                
                this.confirmAction(this.inputField.text);
                WindowManager.Instance.destroyWindow(windowController);
            }
        });

        bottomPanel.Find("CancelButton").GetComponent<Button>().onClick.AddListener(() => {
            //exits the window
            WindowManager.Instance.destroyWindow(windowController);
        });
    }

    public override void changeWindowSize(int width, int height) {
        
    }

    public override bool sameContent(WindowContent content) {
        return content.GetType() == this.GetType() && ((EnterTextContent)content).instructions.Equals(this.instructions);
    }

    public override void receiveBroadcast(string message) {
        //does nothing
    }

    protected override void destroyContent() {
        //does nothing
    }
}