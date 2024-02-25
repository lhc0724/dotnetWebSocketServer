using System.Net.WebSockets;

namespace WebSocketService.Interfaces;

public interface ISocketServiceFactory
{
	IWebSocketService CreateService(WebSocket webSocket);
}

