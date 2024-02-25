using System.Net.WebSockets;

namespace WebSocketService.Interfaces;

public interface ISocketServerFactory
{
	IWebSocketServer CreateService(WebSocket webSocket);
}

