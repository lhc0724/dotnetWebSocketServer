using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using WebSocketService.Interfaces;

namespace WebSocketService.Controllers;

[Route("[controller]")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class SocketController : ControllerBase
{
	private readonly ILogger<SocketController> _logger;
	private readonly ISocketServerFactory _socketServerFactory;

	public SocketController(
		ILogger<SocketController> logger,
		ISocketServerFactory socketServerFactory)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_socketServerFactory = socketServerFactory ?? throw new ArgumentNullException(nameof(socketServerFactory));
	}

	[HttpPost]
	public async Task<IActionResult> ConnectionSocket()
	{
		var context = ControllerContext.HttpContext;
		WebSocket webSocket = context.WebSockets.IsWebSocketRequest ?
		 await context.WebSockets.AcceptWebSocketAsync()
		 : null;

		if (webSocket != null)
		{
			var socketServer = _socketServerFactory.CreateService(webSocket);
			try
			{
				await socketServer.KeepReceiving();
			}
			catch (Exception e)
			{
				_logger.LogError("{message}", e.Message);

				HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await HttpContext.Response.WriteAsync("message: internal server error");
			}
			finally
			{
				await socketServer.Close();
			}
		}

		return NoContent();
	}
}