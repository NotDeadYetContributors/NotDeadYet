# Not Dead Yet

## .NET application health checking made easy

### Getting started

Getting the package:

    Install-Package NotDeadYet

In your code:

    var healthChecker = new HealthCheckerBuilder()
        .WithHealthChecksFromAssemblies(ThisAssembly)
        .Build();

### Doing a health check

    var results = healthChecker.Check();
    if (results.Status == HealthCheckStatus.Okay)
    {
        // Hooray!
    } else {
        // Boo!
    }

### Adding your own, custom health checks:

    public class NeverCouldGetTheHangOfThursdays : IHealthCheck
    {
        public string Description
        {
            get { return "This app doesn't work on Thursdays."; }
        }

        public void Check()
        {
            // Example: just throw if it's a Thursday
            if (DateTimeOffset.Now.DayOfWeek == DayOfWeek.Thursday)
            {
                throw new HealthCheckFailedException("I never could get the hang of Thursdays.");
            }

            // ... otherwise we're fine.
        }

        public void Dispose()
        {
        }
    }
    
Or a slightly more realistic example:
    
     public class CanConnectToSqlDatabase : IHealthCheck
    {
        public string Description
        {
            get { return "Our SQL Server database is available and we can run a simple query on it."; }
        }

        public void Check()
        {
            // We really should be using ConfigInjector here ;)
            var connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnectionString"].ConnectionString;

            // Do a really simple query to confirm that the server is up and we can hit our database            
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SELECT 1", connection);
                command.ExecuteScalar();
            }
        }

        public void Dispose()
        {
        }
    }

## Framework integration

### Integrating with MVC

In your Package Manager Console:

    Install-Package NotDeadYet.MVC4

Then, in your RouteConfig.cs:

    var thisAssembly = typeof (MvcApplication).Assembly;
    var notDeadYetAssembly = typeof (IHealthChecker).Assembly;
    
    var healthChecker = new HealthCheckerBuilder()
        .WithHealthChecksFromAssemblies(thisAssembly, notDeadYetAssembly)
        .Build();

    routes.RegisterHealthCheck(healthChecker);

### Integrating with Nancy

In your Package Manager Console:

    Install-Package NotDeadYet.MVC4

Then, in your bootstrapper:

    var thisAssembly = typeof (Bootstrapper).Assembly;
    var notDeadYetAssembly = typeof (IHealthChecker).Assembly;

    var healthChecker = new HealthCheckerBuilder()
        .WithHealthChecksFromAssemblies(thisAssembly, notDeadYetAssembly)
        .Build();

    container.Register(healthChecker);

## FAQ

### How do I query it?

Once you've hooked up your integration of choice (currently MVC or Nancy), just point your monitoring tool at `/healthcheck`.

That's it.

If you point a browser at it you'll observe a 200 response if all's well and a 503 if not. This plays nicely with load balancers (yes, including Amazon's Elastic Load Balancer) which, by default, expect a 200 response code from a monitoring endpoint before they'll add an instance to the pool.

### Does this work with X load balancer?

If your load balancer can be configured to expect a 200 response from a monitoring endpoint, then yes :)

### Can I change the monitoring endpoint?

Of course. In MVC land, it looks like this:

    var healthChecker = new HealthCheckerBuilder()
        .WithHealthChecksFromAssemblies(typeof(MvcApplication).Assembly)
        .Build();

    routes.RegisterHealthCheck(healthChecker, "/someCustomEndpoint");

and in Nancy land it looks like this:

    HealthCheckNancyModule.EndpointName = "/someCustomEndpoint";

### Does this with with my IoC container of choice?

NotDeadYet is designed to work both with and without an IoC container. There's a different configuration method on the `HealthCheckerBuilder` class called `WithHealthChecks` which takes a `Func<IHealthCheck[]>` parameter. This is designed so that you can wire it in to your container like so:

    public class HealthCheckModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(ThisAssembly, typeof (IHealthCheck).Assembly)
                .Where(t => t.IsAssignableTo<IHealthCheck>())
                .As<IHealthCheck>()
                .ExternallyOwned();

            builder.Register(CreateHealthChecker)
                .As<IHealthChecker>()
                .SingleInstance();
        }

        private static IHealthChecker CreateHealthChecker(IComponentContext c)
        {
            var componentContext = c.Resolve<IComponentContext>();

            return new HealthCheckerBuilder()
                .WithHealthChecks(componentContext.Resolve<IHealthCheck[]>)
                .WithLogger((ex, message) => componentContext.Resolve<ILogger>().Error(ex, message))
                .Build();
        }
    }

This example is for Autofac but you can easily see how to hook it up to your container of choice.

### Why don't the health checks show stack traces when they fail?

For the same reason that we usually try to avoid showing a stack trace on an error page.

### Can I log the stack traces to somewhere else, then?

You can wire in any logger you like. In this example below, we're using [Serilog](http://serilog.net/):

    var serilogLogger = new LoggerConfiguration()
        .WriteTo.ColoredConsole()
        .WriteTo.Seq("http://localhost:5341")
        .CreateLogger();

    return new HealthCheckerBuilder()
        .WithLogger((ex, message) => serilogLogger.Error(ex, message))
        .Build();
