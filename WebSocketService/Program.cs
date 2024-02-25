using System.Net.Mime;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using WebSocketService.Handlers;
using WebSocketService.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
	options.RespectBrowserAcceptHeader = true;

	//  increase IQueryable,IEnumerable buffer size
	options.MaxIAsyncEnumerableBufferLimit = 16386;
})
.AddNewtonsoftJson(options =>
{
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
	options.SerializerSettings.ContractResolver = new DefaultContractResolver();
	options.SerializerSettings.Converters.Add(new StringEnumConverter());
})
.AddJsonOptions(options =>
{
	options.JsonSerializerOptions.PropertyNamingPolicy = null;
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
})
.ConfigureApiBehaviorOptions(options =>
{
	options.InvalidModelStateResponseFactory = context =>
	{
		var result = new BadRequestObjectResult(context.ModelState);
		result.ContentTypes.Add(MediaTypeNames.Application.Json);

		return result;
	};
});

#region WebSocket

builder.Services.AddScoped<ISocketServerFactory, SocketServerFactory>();
builder.Services.AddScoped<IWebSocketHandler, WebSocketHandler>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

var wsOptions = new WebSocketOptions()
{
	KeepAliveInterval = TimeSpan.FromSeconds(600),  //default 2 minute
};

app.UseWebSockets(wsOptions);
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();