using DigitalTwin.Common.AppsettingsModels;
using DigitalTwin.Common.Constants;
using Microsoft.AspNetCore.Rewrite;

namespace DigitalTwin.Api.Middlewares
{
    public static class RewriteUrl
    {
        public static IApplicationBuilder UseUrlRewriter(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var rewritingOptions = configuration.GetSection(Appsettings.RewritingOptions)
                                                     .Get<RewritingOptions>();
            RewriteOptions rewriteOptions = new RewriteOptions();
            if (rewritingOptions?.RewritingOptionList != null)
            {
                foreach (var rewriting in rewritingOptions.RewritingOptionList)
                {
                    if (rewriting.Regex != null && rewriting.Replacement != null)
                    {
                        rewriteOptions.AddRewrite(rewriting.Regex, rewriting.Replacement, true);
                    }
                }
            }
            return builder.UseRewriter(rewriteOptions);
        }
    }
}
