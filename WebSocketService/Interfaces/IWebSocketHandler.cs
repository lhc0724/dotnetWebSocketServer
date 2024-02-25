namespace WebSocketService.Interfaces;

public interface IWebSocketHandler
{
	Task Handle(IWebSocketServer webSocketService);
}

