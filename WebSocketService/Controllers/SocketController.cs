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
}