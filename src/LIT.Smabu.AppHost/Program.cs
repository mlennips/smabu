using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// API mit festem, benanntem Endpoint
var api = builder.AddProject("api", "../LIT.Smabu.API/LIT.Smabu.API.csproj")
                 .WithHttpEndpoint(name: "api-http", port: 5100);

// React Dev Server (Vite)
// Nutze (falls verfügbar) das Overload mit scriptName: "dev".
builder.AddNpmApp("web", "../lit.smabu.react", scriptName: "dev")
                 .WithHttpEndpoint(targetPort: 5173) // Vite lauscht strikt auf 5173 (siehe package.json)
                 .WithExternalHttpEndpoints()
                 .WithReference(api)
                 .WithEnvironment("VITE_API_URL", "http://localhost:5100")
                 .WaitFor(api);

builder.Build().Run();
