using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Controllers.PageTemplates;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;


public class MvcApplication : HttpApplication
{
    protected void Application_Start()
    {
		// ADD THE BELOW TO YOUR APPLICATION_START
        #region "AutoFac Cache and other dependency injections"

        // Register AutoFac Items
        var builder = new ContainerBuilder();

        // Register Dependencies for Cache
        DependencyResolverConfig.Register(builder);

        // Set Autofac Dependency resolver to the builder
        DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

        #endregion
    }

	// BELOW IS OPTIONAL TO OVERRIDE VARYBYCUSTOMSTRING HANDLING
    /// <summary>
    /// When a OutputCache VaryByCustom string is passed, can define various options to this to alter the Cache Name.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="custom"></param>
    /// <returns></returns>
    public override string GetVaryByCustomString(HttpContext context, string custom)
    {
        // Creates the options object used to store individual cache key parts
        IOutputCacheKeyOptions options = OutputCacheKeyHelper.CreateOptions();

        // Selects a caching configuration according to the current custom string, allows semi-colon separate list
        foreach (string customItem in custom.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
        {
            switch (customItem.ToLowerInvariant())
            {
                case "default":
                    // Sets the variables that compose the cache key for the 'Default' VaryByCustom string
                    options
                        .VaryByHost()
                        .VaryByBrowser()
                        .VaryByUser();
                    break;

                case "onlinemarketing":
                    // Sets the variables that compose the cache key for the 'OnlineMarketing' VaryByCustom string
                    options
                        .VaryByCookieLevel()
                        .VaryByPersona()
                        .VaryByABTestVariant();
                    break;
            }
        }

        // Combines individual 'VaryBy' key parts into a cache key under which the output is cached
        string cacheKey = OutputCacheKeyHelper.GetVaryByCustomString(context, custom, options);

        // Returns the constructed cache key
        if (!String.IsNullOrEmpty(cacheKey))
        {
            return cacheKey;
        }

        // Calls the base implementation if the provided custom string does not match any predefined configurations
        return base.GetVaryByCustomString(context, custom);
    }
}
