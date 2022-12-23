using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Tasks.Extensions;

public static class WebAppExtension
{
      private static bool IsInKubernetes(this WebApplication webApplication)
        {
            var cfg = webApplication.Services.GetService<IConfiguration>();
            var orchestratorType = cfg.GetValue<string>("OrchestratorType");
            return orchestratorType?.ToUpper() == "K8S";
        }

        public static WebApplication MigrateDbContext<TContext>(this WebApplication host) where TContext : DbContext
        {
            var underK8S = host.IsInKubernetes();
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<TContext>();

            try
            {
                if (underK8S)
                {
                    context?.Database.Migrate();
                }
                else
                {
                    Policy.Handle<SqliteException>()
                        .WaitAndRetry(new[]
                            {
                                TimeSpan.FromSeconds(4),
                                TimeSpan.FromSeconds(3),
                                TimeSpan.FromSeconds(2)
                            },
                            (ex, span) =>
                            {
                            })
                        .Execute(() => context?.Database.Migrate());
                }
            }
            catch (Exception ex)
            {
                if (underK8S)
                {
                    throw; // Rethrow under k8s because we rely on k8s to re-run the pod
                }
            }

            return host;
        }
}