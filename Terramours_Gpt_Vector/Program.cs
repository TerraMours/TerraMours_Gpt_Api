using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Terramours_Gpt_Vector.Commons;
using Terramours_Gpt_Vector.IService;
using Terramours_Gpt_Vector.Service;

var builder = WebApplication.CreateBuilder(args);



var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
IConfiguration configuration = builder.Configuration;
var connectionString = configuration.GetValue<string>("DbConnectionString");
//ע�����
builder.Services.AddScoped<IVectorService,VectorService>();
//��ʼ��
builder.Services.AddTransient<DbInitialiser>();
//���ݿ�
builder.Services.AddDbContext<EFCoreDbContext>(opt => {
    //�������ļ��л�ȡkey,���ַ�����Ҫ����һ������֮��Ӧ

    //var connStr = $"Host=localhost;Database=TerraMours;Username=postgres;Password=root";
    var connStr = connectionString;
    opt.UseNpgsql(connStr, o => o.UseVector());
    //����EFĬ��AsNoTracking,EF Core�� ����
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    if (isDev)
    {
        //���ô�ѡ���EF Core������־�а����������ݣ�����ʵ�������ֵ������ڵ��Ժ��Ų�����ǳ����á�
        opt.EnableSensitiveDataLogging();
    }

    opt.EnableDetailedErrors();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
//��ʼ�����ݿ�

var initialiser = services.GetRequiredService<DbInitialiser>();

initialiser.Run();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
