using Newtonsoft.Json;
using WebSocketService.Interfaces;
using System.Net.WebSockets;

namespace WebSocketService.Handlers;

public class WebSocketHandler : IWebSocketHandler
{
	public WebSocketHandler()
	{
	}

	/// <summary>
	/// PingPong Test Handler
	/// </summary>
	/// <param name="webSocketService"></param>
	/// <returns></returns>
	public async Task Handle(IWebSocketServer webSocketService)
	{

		#region receive from client test code
		//MemoryStream ms = new MemoryStream();

		//_ = await webSocketService.ReceiveMessage(ms);

		//var receivedMessage = Encoding.UTF8.GetString(ms.ToArray());
		//if(receivedMessage == "exit")
		//{
		//	await webSocketService.Close();
		//	return;
		//}else if(receivedMessage == "run")
		//{
		//	await webSocketService.Send("get redis data");
		//}
		#endregion
		await webSocketService.KeepReceiving();
		await webSocketService.Close();
	}

}

