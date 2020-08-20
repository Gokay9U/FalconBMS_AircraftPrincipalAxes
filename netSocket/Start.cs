
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;


public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseWebSockets();

        app.Use(async (ctx, nextMsg) =>
        {
            Console.WriteLine("Web Socket is Listening");
            if (ctx.Request.Path == "/9U")
            {
                if (ctx.WebSockets.IsWebSocketRequest)
                {
                    var wSocket = await ctx.WebSockets.AcceptWebSocketAsync();
                    await Talk(ctx, wSocket);
                }
                else
                {
                    ctx.Response.StatusCode = 400;
                }

            }
            else
            {
                await nextMsg();
            }

        });
        app.UseFileServer();
    }
    private async Task Talk(HttpContext hContext, WebSocket wSocket)
    {
        netSocket.Falcon f16 = new netSocket.Falcon();

        while (wSocket.State == WebSocketState.Open)
        {
            //Sending flight data to client 
            double pitch = f16.sendPitch();
            string messageP = "Pitch:" + pitch.ToString();
            double roll = f16.sendRoll();
            string messageR = "Roll:" + roll.ToString();
            double yaw = f16.sendYaw();
            string messageY = "Yaw:" + yaw.ToString();

            byte[] outGoingMeesageP = Encoding.UTF8.GetBytes(messageP);
            byte[] outGoingMeesageR = Encoding.UTF8.GetBytes(messageR);
            byte[] outGoingMeesageY = Encoding.UTF8.GetBytes(messageY);
            await wSocket.SendAsync(new ArraySegment<byte>(outGoingMeesageP, 0, outGoingMeesageP.Length),WebSocketMessageType.Text,true,CancellationToken.None);
            await wSocket.SendAsync(new ArraySegment<byte>(outGoingMeesageR, 0, outGoingMeesageR.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            await wSocket.SendAsync(new ArraySegment<byte>(outGoingMeesageY, 0, outGoingMeesageY.Length), WebSocketMessageType.Text, true, CancellationToken.None);

        }





    }
}