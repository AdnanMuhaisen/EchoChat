using Serilog;

namespace EchoChat.Extentions;

public static class SerilogRegistration
{
    public static void AddSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.WriteTo.Console();
        });
    }
}