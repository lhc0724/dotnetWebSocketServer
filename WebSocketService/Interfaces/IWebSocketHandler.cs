namespace WebSocketService.Interfaces;

public interface IWebSocketHandler
{
	Task Handle(IWebSocketService webSocketService);

	Task ProductCrawlingHandle(IWebSocketService webSocketService, string redisKey);
}

