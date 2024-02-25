using System.Net.WebSockets;
using WebSocketService.Interfaces;

namespace WebSocketService.Handlers;

public class SocketServerFactory : ISocketServerFactory
{
	public IWebSocketServer CreateService(WebSocket webSocket)
	{
		return new WebSocketServer(webSocket);
	}
}
