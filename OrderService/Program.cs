using Messaging.Kafka.Producer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProducer<Order>(builder.Configuration.GetSection("KafkaSettings:Order"), builder.Configuration);
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/", async (IKafkaProducer<Order> producer, CancellationToken token) =>
{
    await producer.ProduceAsync(new Order
    {
        Id = Guid.NewGuid(),
        Name = "Чебупели",
    },token);
});

app.Run();
