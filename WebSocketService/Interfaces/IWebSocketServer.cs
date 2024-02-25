using System.Net.WebSockets;

namespace WebSocketService.Interfaces;

public interface IWebSocketServer
{
	Task<WebSocketCloseStatus?> KeepReceiving();

	Task<WebSocketReceiveResult> ReceiveMessage(Stream stream, CancellationToken cancellationToken = default);

	Task Send(string message);

	Task Close();

	WebSocketState GetStatus();
}

