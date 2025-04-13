using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using Unity.Mathematics;

public class Connection : MonoBehaviour
{
    WebSocket websocket;
    public GameObject propPreFab;
    public GameObject propPreFab1;

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket("wss://www.sceneitbefore.org/ws");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
            websocket.SendText("Hello^Unity");
            websocket.SendText("Go^Stage1");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            //Debug.Log("OnMessage!");
            //Debug.Log(bytes);

            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            //Debug.Log("OnMessage! " + message);

            string[] parts = message.Split('^');

            if ("Create".Equals(parts[0])){

                string[] nameParts = parts[1].Split('|');

                GameObject prefab = "PropPreFab".Equals(nameParts[0]) ? propPreFab : propPreFab1;
                Vector3 pos = new Vector3(float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4]));
                GameObject newThing = Instantiate(prefab, pos, new Quaternion(float.Parse(parts[5]), float.Parse(parts[6]), float.Parse(parts[7]), float.Parse(parts[8])));
                newThing.transform.localScale = new Vector3(float.Parse(parts[9]), float.Parse(parts[10]), float.Parse(parts[11]));
                newThing.name = parts[1];

                //interperlation saves memory but costs CPU
                newThing.GetComponent<MeshLoader>().LoadMeshFromPath($"{Application.dataPath}/{nameParts[1]}.dat");
            }
            else if ("Move".Equals(parts[0])){
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (g.name.Equals(parts[1]))
                    {
                        Vector3 pos = new Vector3(float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4]));
                        g.transform.position = pos;
                        //Debug.Log("Move" + parts[2] + "^" + parts[3] + "^" + parts[4]);
                        break;
                    }
                }
            }
            else if ("Rotate".Equals(parts[0]))
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (g.name.Equals(parts[1]))
                    {
                        Quaternion rotate = new Quaternion(float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4]), float.Parse(parts[5]));
                        g.transform.rotation = Quaternion.identity;
                        g.transform.rotation *= rotate;
                        //Debug.Log("Rotate" + parts[2] + "^" + parts[3] + "^" + parts[4] + "^" + parts[5]);
                        break;
                    }
                }
            }
            else if ("Scale".Equals(parts[0]))
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (g.name.Equals(parts[1]))
                    {
                        Vector3 scale = new Vector3(float.Parse(parts[2]), float.Parse(parts[3]), float.Parse(parts[4]));
                        g.transform.localScale = scale;
                        //Debug.Log("Scale" + parts[2] + "^" + parts[3] + "^" + parts[4]);
                        break;
                    }
                }
            }
            else if ("Delete".Equals(parts[0]))
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (g.name.Equals(parts[1]))
                    {
                        Destroy(g);
                        //Debug.Log("Scale" + parts[2] + "^" + parts[3] + "^" + parts[4]);
                        break;
                    }
                }
            }
        };

        // Keep sending messages at every 0.3s
        // InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    public async void SendWebSocketMessage(string msg)
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            // await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText(msg);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}
