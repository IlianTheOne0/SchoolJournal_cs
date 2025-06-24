namespace Contracts;

using Database.Repositories.Grades;
using Database.Repositories.Supabase;
using Contracts.Interfaces.Mediators.Auth;
using Contracts.Interfaces.Mediators.Users;
using Contracts.Interfaces.Mediators.Grades;
using Contracts.Mediators.Auth;
using Contracts.Mediators.Grades;
using Contracts.Mediators.Users;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection Services)
    {
        RepositoriesSupabase RepositorySupabase = new RepositoriesSupabase();

        Services.AddSingleton<InterfacesMediatorsAuth>(
            new MediatorsAuth(RepositorySupabase)
        );
        Services.AddSingleton<InterfacesMediatorsUsers>(
            new MediatorsUsers(RepositorySupabase)
        );
        Services.AddSingleton<InterfacesMediatorsGrades>(
            new MediatorsGrades(
                new RepositoriesGrades(RepositorySupabase)
            )
        );

        return Services;
    }
}