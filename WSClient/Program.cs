﻿using System.Net.WebSockets;
using System.Text;

Console.Title = "Client";

using var ws = new ClientWebSocket();

await ws.ConnectAsync(new Uri("ws://localhost:8603/ws"), CancellationToken.None);
byte[] buf = new byte[1056];

while (ws.State == WebSocketState.Open)
{
    var data = Encoding.UTF8.GetBytes("Halo World");
    await ws.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
    var result = await ws.ReceiveAsync(buf, CancellationToken.None);

    if (result.MessageType == WebSocketMessageType.Close)
    {
        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        Console.WriteLine(result.CloseStatusDescription);
    }
    else
    {
        Console.WriteLine(Encoding.ASCII.GetString(buf, 0, result.Count));
    }
}
