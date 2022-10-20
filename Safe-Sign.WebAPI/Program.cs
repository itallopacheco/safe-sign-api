using System.Text;
using System.Reflection;

using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Safe_Sign.DAL;
using Safe_Sign.DAL.DAO;
using Safe_Sign.DAL.Models;
using Safe_Sign.Repository;
using Safe_Sign.DAL.DAO.Base;
using Safe_Sign.Repository.Interfaces;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v0.1",
        Title = "SafeSign API",
        Description = "",
    });
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// DB Context
builder.Services.AddTransient<SafeSignContext>();

// DAO
builder.Services.AddScoped<IDAO<DocumentFile>, BaseDAO<DocumentFile>>();
builder.Services.AddScoped<IDAO<LegalPerson>, BaseDAO<LegalPerson>>();
builder.Services.AddScoped<IDAO<Marker>, BaseDAO<Marker>>();
builder.Services.AddScoped<IDAO<Person>, BaseDAO<Person>>();
builder.Services.AddScoped<IDAO<Profile>, BaseDAO<Profile>>();
builder.Services.AddScoped<IDAO<Theme>, BaseDAO<Theme>>();
builder.Services.AddScoped<DocumentDAO>();
builder.Services.AddScoped<PersonDAO>();
builder.Services.AddScoped<ProfileDAO>();
builder.Services.AddScoped<SignatureDAO>();
builder.Services.AddScoped<UserDAO>();

// Repository
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<ILegalPersonRepository, LegalPersonRepository>();
builder.Services.AddScoped<IMarkerRepository, MarkerRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ISGPRepository, SGPRepository>();
builder.Services.AddScoped<ISignatureRepository, SignatureRepository>();
builder.Services.AddScoped<IThemeRepository, ThemeRepository>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "safe-sign-cors",
                        policy =>
                        {
                            policy.AllowAnyOrigin();
                            policy.AllowAnyHeader();
                            policy.AllowAnyMethod();
                        });
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    string? envKey = Environment.GetEnvironmentVariable("SAFE_SIGN_KEY");
    
    if (!string.IsNullOrEmpty(envKey))
    {
        byte[] key = Encoding.ASCII.GetBytes(envKey);
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
});

// App
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHealthChecks("/healthz");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("safe-sign-cors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
