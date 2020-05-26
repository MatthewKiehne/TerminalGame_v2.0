using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window {

	private int minWidth;
	private int minHeight;
	private string name;
	private WindowContent contents;

	public Window(string name, int minWidth, int minHeight, WindowContent contents){
		this.minWidth = minWidth;
		this.minHeight = minHeight;
		this.name = name;
		this.contents = contents;

        
	}

	public string Name {
		get {
			return name;
		}
	}

	public int MinWidth {
		get {
			return minWidth;
		}
	}

	public int MinHeight {
		get {
			return minHeight;
		}
	}

	public WindowContent Contents {
		get {
			return contents;
		}
	}
}
