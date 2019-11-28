using SocketIO;
using UnityEngine;
[RequireComponent(typeof(SocketIOComponent))]
public class NetworkService : MonoBehaviour
{
    public SocketIOComponent socketIOComponent;
    // Start is called before the first frame update
    void Start()
    {
        socketIOComponent.Connect();
        socketIOComponent.On("connection", Connections);

        socketIOComponent.On("error", OnError);
    }

    private void OnError(SocketIOEvent obj)
    {
        Debug.Log("Error" + obj);
    }

    void Connections(SocketIOEvent socketIOEvent)
    {
        Debug.Log("Server Connected" + socketIOEvent);
    }
}
