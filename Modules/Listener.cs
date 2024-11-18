using System.Net;
using System.Net.Sockets;
using System.Text;
using WhoAreYou.Helpers;

namespace WhoAreYou.Modules;

internal class Listener
{
    private readonly int _port;

    /// <param name="port">
    ///     The port number to listen on.
    /// </param>
    public Listener(int port)
    {
        if (port < 1 || port > 65535)
            throw new ArgumentException("Invalid port number.", nameof(port));

        _port = port;
    }

    public async Task Listen()
    {
        try
        {
            var listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            Interface.PrintLine("!", "Press Ctrl + C to stop the listener.");
            Interface.PrintLine("~", $"Listening on port {_port}...");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                Interface.PrintLine("+", "Client connected.");
                _ = HandleClientAsync(client);
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error occurred while listening: {e.Message}");
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using var stream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead;

            var welcomeMessage = "WhoAreYou?!";
            var welcomeBytes = Encoding.UTF8.GetBytes(welcomeMessage);
            await stream.WriteAsync(welcomeBytes, 0, welcomeBytes.Length);

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                var received = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {received}");

                var response = $"Echo: {received}";
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Client handling error: {e.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}