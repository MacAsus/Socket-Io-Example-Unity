using System.Collections;
using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine;

public class example : MonoBehaviour {
	public string serverURL = "http://localhost:3000";
	protected Socket socket = null;
	public string chatLog = "";

	// Use this for initialization
	void Start() {
		Debug.Log("Socket.IO Client Started");
		DoOpen();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	void Destroy() {
		DoClose();
	}

	void DoOpen() {
		if (socket == null) {
			socket = IO.Socket (serverURL);
			socket.On(Socket.EVENT_CONNECT, () => {
				lock(chatLog) {
					// Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
					chatLog = "Socket.IO Client connected.";
					Debug.Log(chatLog);
				}
			});
			socket.On ("chat", (data) => {
				string str = data.ToString();
				// Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
				lock(chatLog) {
					chatLog = str;
					Debug.Log(chatLog);
				}
			});
		}
	}

	void DoClose() {
		if (socket != null) {
			socket.Disconnect ();
			socket = null;
		}
	}
}