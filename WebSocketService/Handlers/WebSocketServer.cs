using System.Net.WebSockets;
using System.Text;
using WebSocketService.Interfaces;

namespace WebSocketService.Handlers;

public class WebSocketServer : IWebSocketServer
{
	private readonly WebSocket _webSocket;

	public WebSocketServer(WebSocket webSocket)
	{
		_webSocket = webSocket;
	}

	public async Task<WebSocketCloseStatus?> KeepReceiving()
	{
		WebSocketReceiveResult message;
		do
		{
			using var memoryStream = new MemoryStream();
			message = await ReceiveMessage(memoryStream);
			if (message.Count > 0)
			{
				var receivedMessage = Encoding.UTF8.GetString(memoryStream.ToArray());
				if (receivedMessage == "ping")
				{
					await Send("pong");
				}
				else
				{
					// loopback message
					await Send(receivedMessage);
				}
			}
		} while (message.MessageType != WebSocketMessageType.Close);

		return message.CloseStatus;
	}

	public async Task<WebSocketReceiveResult> ReceiveMessage(Stream stream, CancellationToken cancellationToken = default)
	{
		var readBuffer = new ArraySegment<byte>(new byte[4 * 1024]);
		WebSocketReceiveResult result;
		do
		{
			result = await _webSocket.ReceiveAsync(readBuffer, cancellationToken);
			await stream.WriteAsync(readBuffer.Array, readBuffer.Offset, result.Count,
					CancellationToken.None);
		} while (!result.EndOfMessage);

		return result;
	}

	public async Task Send(string message)
	{
		var bytes = Encoding.UTF8.GetBytes(message);
		await _webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true,
				CancellationToken.None);
	}

	public async Task Close()
	{
		await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
	}

	public WebSocketState GetStatus()
	{
		return _webSocket.State;
	}

}
