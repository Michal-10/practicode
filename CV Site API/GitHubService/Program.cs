// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Service;

Console.WriteLine("Hello, World!");
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IGitHubService>(sp =>
    {
        var accessToken = Configuration["GitHub:AccessToken"];  // טוקן מה-config
        return new GitHubService(accessToken, sp.GetRequiredService<IMemoryCache>());
    });

    services.AddControllers();
}