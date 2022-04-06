using Microsoft.Extensions.DependencyInjection;
using Translations.Contracts.Repositories;
using Translations.Contracts.Services;
using Translations.Repositories;
using Translations.Services;

namespace Translations;

public static class Extensions
{
    public static IServiceCollection AddTranslations(this IServiceCollection services)
    {
        services.AddScoped<ITranslationCategoriesRepository, TranslationCategoriesRepository>();
        services.AddScoped<ITranslationKeysRepository, TranslationKeysRepository>();
        services.AddScoped<ITranslationLanguagesRepository, TranslationLanguagesRepository>();
        services.AddScoped<ISelectListService, SelectListService>();
        services.AddScoped<ITranslationRowsRepository, TranslationRowsRepository>();
        
        services.AddScoped<ITranslationCategoriesService, TranslationCategoriesService>();
        services.AddScoped<ITranslationKeysService, TranslationKeysService>();
        services.AddScoped<ITranslationLanguagesService, TranslationLanguagesService>();
        services.AddScoped<ITranslationRowsService, TranslationRowsService>();
        
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IViewTranslationService, ViewTranslationService>();

        services.AddScoped<ICacheService, CacheService>();
        
        return services;
    }
}