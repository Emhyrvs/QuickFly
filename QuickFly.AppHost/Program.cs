var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.QuickFly_Server>("quickfly-server");

builder.Build().Run();
