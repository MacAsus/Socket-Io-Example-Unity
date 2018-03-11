using System.Collections;
using System.Collections.Generic;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine;

public class example : MonoBehaviour {
	public string serverURL = "http://localhost:3000";
	protected Socket socket = null;
	public string chatLog = "";
	public int total = 0;

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
		Debug.Log ("Destroy");
	}

	void DoOpen() {
		if (socket == null) {
			socket = IO.Socket (serverURL);
			socket.On(Socket.EVENT_CONNECT, () => {
				lock(chatLog) {
					// Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
					chatLog = "Conenct Success!!";
					Debug.Log(chatLog);

					for(int i=0; i < 10; i++) {
						socket.Emit("chat", "hi");
					}
				}
			});

			socket.On ("chat", (data) => {
				 string str = data.ToString();
				 long unixTimestamp = (long)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalMilliseconds;
				 long dist = unixTimestamp - long.Parse(str);
				 Debug.Log(dist);
				// Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
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