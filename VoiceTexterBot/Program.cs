using VoiceTexterBot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using System.Text;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;
using VoiceTexterBot.Configuration;

Console.OutputEncoding = Encoding.Unicode;

var host = new HostBuilder().ConfigureServices((hostContext, services) => ConfigureServices(services)).UseConsoleLifetime().Build();

Console.WriteLine("Сервис запущен");
await host.RunAsync();
Console.WriteLine("Сервис остановлен");

void ConfigureServices(IServiceCollection services)
{
    AppSettings appSettings = BuildAppSettings();
    services.AddSingleton(BuildAppSettings());
    services.AddTransient<DefaultMessageController>();
    services.AddTransient<VoiceMessageController>();
    services.AddTransient<TextMessageController>();
    services.AddTransient<InlineKeyboardController>();

    services.AddSingleton<IStorage, MemoryStorage>();
    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
    services.AddSingleton<IFileHandler, AudioFileHandler>();
    services.AddHostedService<Bot>();
}

static AppSettings BuildAppSettings()
{
    return new AppSettings()
    {
        DownloadsFolder = @"D:\\Project\\Vsstudio\\skillfactory\\VoiceTexterBot\\download",
        BotToken = "7040667993:AAFlSRuM-aFDfxg7h7wykGMbn8VgTqFP1tY",
        OutputAudioFormat = "wav",
        AudioFileName = "audio",
        InputAudioFormat = "ogg",
        InputAudioBitrate = 48000
    };
}