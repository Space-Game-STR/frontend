using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class TCPConnect : MonoBehaviour
{
    public TcpClient client;
    public static TCPConnect tcpClient;
    private NetworkStream stream;
    private Thread receiveThread;

    public string serverAddress = "127.0.0.1";
    public int serverPort = 8080;

    public List<string> requestsQueue = new List<string>();
    public List<DataReceivedCallback> requestsCallbacksQueue = new List<DataReceivedCallback>();

    public delegate void DataReceivedCallback(string data, string currentRequest);
    public event DataReceivedCallback OnDataReceived;

    private bool requesting = false;
    private string currentRequest = "";
    private DataReceivedCallback currentDataReceivedCallback = null;

    private void Awake()
    {
        if (tcpClient == null)
        {
            tcpClient = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (!requesting)
        {
            if (requestsQueue.Count > 0)
            {
                int lastIndex = requestsQueue.Count - 1;
                string reqMessage = requestsQueue[lastIndex];
                requestsQueue.RemoveAt(lastIndex);
                currentRequest = reqMessage;
                DataReceivedCallback reqCallback = requestsCallbacksQueue[lastIndex];
                requestsCallbacksQueue.RemoveAt(lastIndex);
                currentDataReceivedCallback = reqCallback;
                
                SendData(reqMessage);
            }
        }
    }

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverAddress, serverPort);
            stream = client.GetStream();

            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    private void ReceiveData()
    {
        byte[] data = new byte[2048];
        string responseData = string.Empty;
        string wholeResponse = "";

        try
        {
            while (true)
            {
                int bytes = stream.Read(data, 0, data.Length);
                if (bytes == 0)
                {
                    Debug.Log("Server has closed the connection");
                    break;
                }
                responseData = Encoding.ASCII.GetString(data, 0, bytes);

                wholeResponse += responseData;
                Debug.Log("Received: " + responseData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
        }
        finally
        {
            MainThreadDispatcher.ExecuteOnMainThread(() =>
            {
                OnDataReceived?.Invoke(wholeResponse, currentRequest);
                currentDataReceivedCallback(wholeResponse, currentRequest);
                CloseConnection();
            });
        }
    }

    public void addRequestToQueue(string request, DataReceivedCallback callback)
    {
        requestsQueue.Add(request);
        requestsCallbacksQueue.Add(callback);
    }

    private void SendData(string message)
    {
        requesting = true;
        ConnectToServer();
        if (stream == null)
        {
            Debug.LogError("Stream is null. Not connected to server.");
        }

        try
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

    private void CloseConnection()
    {
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }

        stream?.Close();

        client?.Close();

        requesting = false;
    }
}
