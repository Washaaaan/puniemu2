using Newtonsoft.Json;
using Puniemu.Src.Server.GameServer.DataClasses;

namespace Puniemu.Src.Server.GameServer.Requests.DefaultHandler.Logic
{
    public class DefaultHandler
    {
        public static async Task HandleAsync(HttpContext ctx)
        {
            try
            {
                var path = ctx.Request.Path.Value ?? "";

                // Si la petición es de HSP / Launching / L5ID (no .nhn), respondemos JSON PLANO (sin NHNCrypt)
                if (path.StartsWith("/hsp", StringComparison.OrdinalIgnoreCase) || !path.EndsWith(".nhn", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Response.ContentType = "application/json; charset=utf-8";
                    
                    // JSON estándar de HSP/Launching para que el cliente no arroje error
                    var hspResponse = new
                    {
                        result = 0,
                        launching = new
                        {
                            server_status = 1,
                            maintenance_message = ""
                        }
                    };

                    await ctx.Response.WriteAsync(JsonConvert.SerializeObject(hspResponse));
                    return;
                }

                // Si es una ruta .nhn no implementada, usamos la lógica de NHNCrypt original
                var formattedMsg = $"Unimplemented request:\n{path}";
                var msgStruct = new MsgBoxResponse(formattedMsg, DataManager.Logic.DataManager.ServerName ?? "Puniemu");
                var msgJson = JsonConvert.SerializeObject(msgStruct);
                var encrypted = NHNCrypt.Logic.NHNCrypt.EncryptResponse(msgJson);

                ctx.Response.ContentType = "application/json; charset=utf-8";
                await ctx.Response.WriteAsync(encrypted);
            }
            catch
            {
                ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await ctx.Response.WriteAsync("Internal server error");
            }
        }
    }
}