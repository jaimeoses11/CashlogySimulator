using LogsSimulador;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Cashlogy.SocketOPOS
{
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 8192;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }

    public class ServerOPOS
    {
        public int Port;
        public string Host;
        IPEndPoint endPoint;

        Socket Servidor;
        Socket Cliente;

        RequestOPOS request;
        ResponseOPOS response;
        Logs logs = new Logs();

        private bool acceptMoreConnections;

        public delegate ResponseOPOS onDataReceived(RequestOPOS request);
        public event onDataReceived OnDataReceived;

        public ServerOPOS(string host, int port)
        {
            Host = host;
            Port = port;
            endPoint = new IPEndPoint(IPAddress.Any, port);
        }

        public void DisposeServer()
        {
            Servidor.Close();
            Servidor.Dispose();
            Servidor = null;
            if (Cliente != null)
            {
                Cliente.Close();
                Cliente.Dispose();
                Cliente = null;
            }

            endPoint = null;
        } 

        #region Metodos del Servidor
        public void StartListening()
        {
            Servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Servidor.Bind(endPoint);
                Servidor.Listen(100);

                Servidor.BeginAccept(new AsyncCallback(AcceptCallback), Servidor);
            }
            catch(Exception) {  }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                acceptMoreConnections = true;

                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                Cliente = handler;

                string autentication = "<?xml version=\"1.0\" encoding=\"utf-8\"?><challenge>ezgxNjU5MTQyLTM4YmQtNGM2Yy05YjhkLTE4YzYxYjY0MjcyOX0=</challenge>";
                Send(Cliente, autentication);

                // Create the state object
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception)
            {
                // El Socket se ha desconectado
                acceptMoreConnections = false;
                CloseNode(acceptMoreConnections);
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                    List<string> content = new List<string>();
                    GetReceivedData(state.sb, ref content);

                    bool close = true;
                    for (int i = 0; i < content.Count; i++)
                    {
                        // Check for end-of-file tag. If it is not there, read more data.
                        var n = content[i].IndexOf("</request>");
                        var k = content[i].IndexOf("<keepalive/>");
                        var a = content[i].IndexOf("</authentication>");
                        if (n > -1)
                        {
                            request = new RequestOPOS(content[i]);
                            logs.Add(request);
                            response = OnDataReceived(request);
                            Send(Cliente, response.Response);
                            logs.Add(response);
                        }
                        else if (k > -1)
                        {
                            string keepalive = "<?xml version=\"1.0\" encoding=\"utf-8\"?><keepalive/>";
                            Send(Cliente, keepalive);
                        }
                        else if (a > -1)
                        {
                            // Recibe la respuesta al challenge, yo no hago nada (supongo challenge OK)
                        }
                        else close = false; // Al ser TCP, si falta un trozo del paquete no llega el siguiente (for n=1)
                    }
                    if(close) CloseNode(acceptMoreConnections);
                    else
                    {
                        // Not all data received. Get more.
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }
                }
                else
                {
                    // El Socket se ha desconectado
                    acceptMoreConnections = false;
                    CloseNode(acceptMoreConnections);
                }
            }
            catch (Exception)
            {
                // El Socket se ha desconectado
                acceptMoreConnections = false;
                CloseNode(acceptMoreConnections);
            }
        }

        private void Send(Socket handler, string data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.UTF8.GetBytes(data);

                // Begin sending the data to the remote device.  
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch (Exception)
            {
                // El Socket se ha desconectado
                acceptMoreConnections = false;
                CloseNode(acceptMoreConnections);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
            }
            catch (Exception)
            {
                // El Socket se ha desconectado
                acceptMoreConnections = false;
                CloseNode(acceptMoreConnections);
            }
        }

        private void CloseNode(bool acceptMoreConnections)
        {
            try
            {
                if (acceptMoreConnections)
                {
                    StateObject state = new StateObject();
                    state.workSocket = Cliente;
                    Cliente.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
                else
                {
                    if (Cliente != null)
                    {
                        Cliente.Shutdown(SocketShutdown.Both);
                        Cliente.Close();
                        Cliente.Dispose();
                        Cliente = null;

                        Servidor.BeginAccept(new AsyncCallback(AcceptCallback), Servidor);
                    }
                }
            }
            catch (Exception)
            {
                // El Socket se ha desconectado
                acceptMoreConnections = false;
                CloseNode(acceptMoreConnections);
            }
        }
        #endregion

        private int GetReceivedData(StringBuilder sb, ref List<string> receivedData)
        {
            receivedData = new List<string>();
            string s = sb.ToString();
            if (s.IndexOf("<?xml", 0) == -1)
            {
                receivedData.Add(s);
                return -1;
            }

            bool stop = false;
            int startIndex = 0;
            int endIndex;
            while (!stop)
            {
                endIndex = (s.IndexOf("<?xml", startIndex + 1));
                if (endIndex == -1)
                {
                    string str = s.Substring(startIndex);
                    receivedData.Add(str);
                    stop = true;
                }
                else
                {
                    string str = s.Substring(startIndex, endIndex - startIndex);
                    receivedData.Add(str);
                    startIndex = endIndex;
                    stop = false;
                }
            }

            return 0;
        }
    }
}