using System.Net.WebSockets;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseWebSockets();
app.MapGet("/", () => "Hello World");
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var rand = new Random();

        while (true)
        {
            byte[] buf = new byte[1056];
            var result = await webSocket.ReceiveAsync(buf, CancellationToken.None);
            Console.WriteLine(Encoding.ASCII.GetString(buf, 0, result.Count));
            var now = DateTime.Now;
            byte[] data = Encoding.ASCII.GetBytes($"{now}");
            await webSocket.SendAsync(data, WebSocketMessageType.Text,
                true, CancellationToken.None);
            await Task.Delay(1000);

            long r = rand.NextInt64(0, 10000);

            if (r == 7)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "random closing", CancellationToken.None);

                return;
            }
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

app.Urls.Add("http://localhost:8603");
app.Urls.Add("https://localhost:8604");

app.Run();
