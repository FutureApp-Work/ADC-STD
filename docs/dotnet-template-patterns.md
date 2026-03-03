# ADC-STD-api .NET Template Architectural Patterns

## 1. Project Structure and Folder Organization

```
ADC-STD-api/
├── dotnet.template.sln           # Solution file
├── dotnet.webapi/                # Web API entry point (Host project)
│   ├── Controllers/
│   │   ├── base/                 # Base controller classes
│   │   │   └── APIBaseController.cs
│   │   ├── V1/                   # Version 1 controllers
│   │   │   └── SampleController.cs
│   │   └── V2/                   # Version 2 controllers
│   │       └── SampleController.cs
│   ├── Extensions/               # Service configuration extensions
│   ├── Services/                 # Web-specific services (ConfigurationService)
│   ├── Properties/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Program.cs
├── dotnet.core/                  # Core shared library
│   ├── Attributes/               # Action/Result/Exception filters
│   ├── Exceptions/               # Custom exception classes
│   ├── Extensions/               # Core extensions (ServiceRegistration, Enum)
│   ├── Models/                   # Core models and interfaces
│   │   ├── Enums/                # Enum definitions (StateEnum)
│   │   ├── PagedData.cs          # Pagination wrapper
│   │   ├── ResponseViewModel.cs  # API response wrapper
│   │   ├── Entity.cs             # Base entity class
│   │   ├── Identity.cs           # IIdentity interface
│   │   ├── SessionContext.cs     # ISessionContext interface
│   │   ├── CreatedInfo.cs        # ICreatedInfo interface
│   │   ├── UpdatedInfo.cs        # IUpdatedInfo interface
│   │   ├── CodeState.cs          # ICodeState interface
│   │   ├── CodeSorting.cs        # ICodeSorting interface
│   │   └── PagedInfo.cs          # Pagination metadata
│   ├── Providers/                # Model binders and JSON converters
│   ├── Resources/                # Localization resources (.resx)
│   └── Services/                 # Base service classes
├── dotnet.services.testing/      # Example service layer (UserService)
│   ├── UserService.cs
│   └── TestingDBContext.cs       # EF DbContext
└── dotnet.models.testing/        # Example models layer
    ├── UserEntity.cs             # Database entity
    ├── Enums/GenderEnum.cs       # Domain enums
    └── ViewModels/
        └── UserViewModel.cs      # DTO/ViewModel
```

### Multi-Project Architecture Pattern

| Project | Purpose | Dependencies |
|---------|---------|--------------|
| `dotnet.webapi` | Entry point, controllers, middleware | dotnet.core, dotnet.services.testing, dotnet.models.testing |
| `dotnet.core` | Shared core: base classes, interfaces, filters, exceptions | None (base library) |
| `dotnet.services.testing` | Business logic services, DbContext | dotnet.core, dotnet.models.testing |
| `dotnet.models.testing` | Entities, ViewModels, Enums | dotnet.core |

---

## 2. Controller Patterns

### Base Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public abstract class APIBaseController<QModel, VModel, TKey> : ControllerBase
    where QModel : class, Core::IIdentity<TKey>, new()
    where VModel : class, Core::IIdentity<TKey>, new()
{
    // Standard CRUD endpoints:
    [HttpGet("{id}")]          virtual Task<VModel> GetAsync(TKey id)
    [HttpPost("list")]         virtual Task<Core::PagedData<VModel>> ListAsync(PagedData<QModel> model)
    [HttpPost]                 virtual Task<VModel> CreateAsync(QModel model)
    [HttpPut]                  virtual Task<VModel> UpdateAsync(QModel model)
    [HttpDelete("{id}")]       virtual Task<VModel> DeleteAsync(TKey id)
}
```

### Controller Implementation Pattern

```csharp
[ApiVersion("1.0")]                           // API versioning attribute
[ServiceFilter<DevelopActionFilterAttribute>]  // Action filter injection
public class SampleController(UserService user)   // Primary constructor DI
    : APIBaseController<UserViewModel, UserViewModel, ulong>
{
    protected readonly UserService _user = user;  // Store injected service

    public override async Task<UserViewModel> CreateAsync([FromBody] UserViewModel model)
    {
        // Validation pattern
        Core::ModelValidationException.ThrowIfInvalid(ModelState);
        
        // Business logic call
        var result = await _user.CreateUserAsync(model);
        
        // Exception pattern
        Core::DatabaseException.ThrowIfAccessFailed(() => !result);
        
        return model;
    }
}
```

### Versioning Pattern

Controllers use inheritance for versioning:
```csharp
// V2 extends V1
[ApiVersion("2.0")]
public class SampleController(UserService user) : V1::SampleController(user)
{
    // Inherits all V1 behavior, can override specific methods
}
```

---

## 3. Dependency Injection Patterns

### Marker Interface Pattern for Service Registration

```csharp
// Registration interfaces (no implementation needed)
public interface IOperationInjection { }
public interface IOperationSingleton : IOperationInjection { }
public interface IOperationScoped : IOperationInjection { }
public interface IOperationTransient : IOperationInjection { }
```

### Automatic Service Registration

```csharp
public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var tp in assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(IOperationInjection))))
            {
                var face = tp.GetInterfaces()
                    .Except(_exceptTypes)
                    .Where(x => !string.IsNullOrEmpty(x.FullName) && !x.FullName.ToUpper().StartsWith("MICROSOFT"))
                    .FirstOrDefault();

                if (tp.IsAssignableTo(typeof(IOperationSingleton)))
                    services.AddSingleton(face ?? tp, tp);
                else if (tp.IsAssignableTo(typeof(IOperationScoped)))
                    services.AddScoped(face ?? tp, tp);
                else if (tp.IsAssignableTo(typeof(IOperationTransient)))
                    services.AddTransient(face ?? tp, tp);
            }
        }
        return services;
    }
}
```

### Usage in Services

```csharp
// Mark with lifetime interface
public class UserService(TestingDBContext dbContext, ISessionContext session)
    : BaseService<UserEntity, ulong>(dbContext, session), IOperationScoped
```

### Extension Method Chaining Pattern

```csharp
// Program.cs - chained extension calls
builder.Services
    .AddModelBindingProvider()      // Custom model binders
    .AddFilterAttribute()           // Global filters
    .AddJsonSerializerOption()      // JSON options
    .AddServiceRegistration()       // Auto DI registration
    .AddSupportedCulture(builder.Configuration)  // Localization
    .AddAPIVersioning()             // API versioning
    .AddMvc();
```

---

## 4. Database/Entity Framework Patterns

### Entity Base Class Hierarchy

```csharp
// Base entity (marker class)
public abstract class Entity { }

// Entity with all auditing interfaces
[Table(name: "user")]
public class UserEntity : Entity, 
    IIdentity<ulong>,      // Id property
    ICodeState,            // State enum (Y/N/D)
    ICodeSorting,          // Sorting integer
    ICreatedInfo,          // CreatedAt, CreaterId
    IUpdatedInfo           // UpdatedAt, UpdaterId
{
    [Column(name: "id_user")]
    public ulong Id { get; set; }
    
    [Column(name: "name_user")]
    public string Name { get; set; } = string.Empty;
    
    [Column(name: "gender")]
    public GenderEnum Gender { get; set; }
    
    // ... more properties
}
```

### Entity Auditing Interfaces

| Interface | Properties | Purpose |
|-----------|------------|---------|
| `IIdentity<TKey>` | `TKey Id` | Primary key |
| `ICodeState` | `StateEnum State` | Soft delete/status (Y=Active, N=Inactive, D=Deleted) |
| `ICodeSorting` | `int Sorting` | Display order |
| `ICreatedInfo` | `CreatedAt, CreaterId` | Creation audit |
| `IUpdatedInfo` | `UpdatedAt, UpdaterId` | Update audit |

### DbContext Pattern

```csharp
public class TestingDBContext(DbContextOptions<TestingDBContext> options) : DbContext(options)
{
    public virtual DbSet<UserEntity> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Store enums as strings in database
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType.IsEnum);
            
            foreach (var prop in properties)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property(prop.Name)
                    .HasConversion<string>();
            }
        }
    }
}
```

### BaseService Pattern (Generic CRUD)

```csharp
public class BaseService<TEntity, TKey>(DbContext dbContext, ISessionContext session)
    : IDisposable
    where TEntity : Entity
    where TKey : IComparable<TKey>
{
    protected Task<bool> CreateAsync(TEntity entity)
    protected Task<bool> UpdateAsync(TEntity entity)  
    protected Task<bool> DeleteAsync(TEntity entity)   // Soft delete via State=D
    protected Task<TEntity?> GetByIdAsync(TKey id)
    protected IQueryable<TEntity> GetAvailable()       // Excludes State=D
}
```

### Database Configuration (Program.cs)

```csharp
builder.Services.AddDbContext<TestingDBContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("TestingDB"),
        MariaDbServerVersion.Create(new Version(11, 8), ServerType.MariaDb));
});
```

---

## 5. Configuration Patterns

### appsettings.json Structure

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "SupportedCultures": ["en-US", "zh-TW"],
  "ConnectionStrings": {
    "TestingDB": "Server=127.0.0.1;Database=testing_db;uid=root;password=...;"
  }
}
```

### Configuration Service Pattern

```csharp
// Interface in dotnet.core
public interface IConfigurationService
{
    bool IsDevelopment { get; }
    TValue? Get<TValue>(string key, TValue defa);
}

// Abstract base in dotnet.core
public abstract class ConfigurationService(IConfiguration config) : IConfigurationService
{
    protected readonly IConfiguration _config = config;
    public virtual bool IsDevelopment { get => true; }
    public TValue? Get<TValue>(string key, TValue defa) => _config.GetValue(key, defa);
}

// Implementation in dotnet.webapi
public class ConfigurationService(IConfiguration config) : Core::ConfigurationService(config), Core::IOperationSingleton
{
    public override bool IsDevelopment { get => _config.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Development"; }
}
```

---

## 6. Middleware and Startup Patterns (Program.cs)

### Full Program.cs Structure

```csharp
var builder = WebApplication.CreateBuilder(args);

// 1. Database context registration
builder.Services.AddDbContext<TestingDBContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("TestingDB"),
        MariaDbServerVersion.Create(new Version(11, 8), ServerType.MariaDb));
});

// 2. Custom extension chain
builder.Services
    .AddModelBindingProvider()      // Custom enum binding
    .AddFilterAttribute()           // Global action/result/exception filters
    .AddJsonSerializerOption()      // JSON serialization options
    .AddServiceRegistration()       // Auto DI scanning
    .AddSupportedCulture(builder.Configuration)  // i18n
    .AddAPIVersioning()             // Versioning support
    .AddMvc();

// 3. Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.UseRequestLocalization();  // Must be after MapControllers

app.Run();
```

### Global Filter Registration Pattern

```csharp
public static class FilterAttributeExtension
{
    public static IServiceCollection AddFilterAttribute(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<APIResultFilterAttribute>();   // Wraps all results
            options.Filters.Add<ExceptionFilterAttribute>();   // Global exception handling
        });
        return services;
    }
}
```

### API Versioning Configuration

```csharp
public static class APIVersioningExtension
{
    public static IServiceCollection AddAPIVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine([
                new HeaderApiVersionReader(["x-api-version", "api-version"]),
                new UrlSegmentApiVersionReader(),
            ]);
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
```

---

## 7. DTO/Model Naming Conventions

### Naming Conventions

| Type | Suffix | Example | Location |
|------|--------|---------|----------|
| Database Entity | `Entity` | `UserEntity` | dotnet.models.{domain}/ |
| API Input/Output | `ViewModel` | `UserViewModel` | dotnet.models.{domain}/ViewModels/ |
| Enums | `Enum` | `GenderEnum`, `StateEnum` | dotnet.models.{domain}/Enums/ or dotnet.core/Models/Enums/ |
| Response Wrapper | `ResponseViewModel<T>` | `ResponseViewModel<UserViewModel>` | dotnet.core/Models/ |
| Pagination Wrapper | `PagedData<T>` | `PagedData<UserViewModel>` | dotnet.core/Models/ |
| Base Classes | (no suffix) | `Entity`, `BaseService` | dotnet.core/ |

### Response Wrapper Pattern

All API responses are wrapped in a standard format via `APIResultFilterAttribute`:

```json
{
  "code": 0,
  "message": "",
  "timestamp": 1709452800000,
  "data": { ... },           // For single object
  "list": [ ... ],           // For arrays
  "paged": {
    "index": 1,
    "size": 20,
    "count": 100
  }
}
```

### Pagination Pattern

```csharp
// Request (POST /api/sample/list)
{
  "paged": { "index": 1, "size": 20 },
  "data": { /* filter criteria */ }
}

// Response
{
  "code": 0,
  "list": [ /* results */ ],
  "paged": { "index": 1, "size": 20, "count": 100 }
}
```

### StateEnum Values

```csharp
public enum StateEnum
{
    None = 0,    // Not set
    Y = 3,       // Active/Enabled
    N = 5,       // Inactive/Disabled
    D = 9,       // Deleted (soft delete)
}
```

---

## 8. Testing Structure

### Test Project Organization

The template uses "testing" as an example domain:

```
dotnet.services.testing/      # Service layer for "testing" domain
├── UserService.cs            # Business logic
└── TestingDBContext.cs       # EF DbContext

dotnet.models.testing/        # Models for "testing" domain
├── UserEntity.cs             # DB entity
├── Enums/
│   └── GenderEnum.cs         # Domain enums
└── ViewModels/
    └── UserViewModel.cs      # DTO
```

### No Dedicated Test Project

The template does **not** include a unit test project. Testing is expected to be:
1. Integration testing via the controller layer
2. Or add a separate `dotnet.tests` project following xUnit/MSTest/NUnit conventions

### Development-Only Controller Protection

```csharp
// Controllers can be marked for development only
[ServiceFilter<DevelopActionFilterAttribute>]
public class SampleController : APIBaseController<...>
```

The `DevelopActionFilterAttribute` throws `NotImplementedException` if not in Development environment.

---

## Summary: Key Patterns for Backend Rewrite

1. **Multi-project architecture**: Separate projects for WebAPI, Core, Services, and Models
2. **Generic BaseController**: APIBaseController<QModel, VModel, TKey> with standard CRUD
3. **Marker Interface DI**: IOperationSingleton/Scoped/Transient for auto-registration
4. **Entity Interfaces**: IIdentity, ICodeState, ICodeSorting, ICreatedInfo, IUpdatedInfo
5. **Soft Delete**: StateEnum.D marks deleted records, GetAvailable() filters them out
6. **Auto-Registration**: Assembly scanning for services implementing IOperationInjection
7. **API Versioning**: Inheritance-based versioning (V2 extends V1)
8. **Response Wrapper**: Unified JSON format via APIResultFilterAttribute
9. **Pagination**: PagedData<T> with POST /list endpoints
10. **Exception Handling**: Custom exceptions with HTTP status codes via attributes
