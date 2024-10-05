using Microsoft.EntityFrameworkCore;
using RESTful_API_project_Uni.Data;
using RESTful_API_project_Uni.Models;


//Create database container with docker-compose up -d in the folder of docker-compose.yml
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    db.Database.EnsureCreated();  // Ensures the database is created if it doesn't exist
                                  // Check if database already has data
    if (!db.Schools.Any())
    {
        // Create some sample schools
        var school1 = new School
        {
            Name = "Springfield High School",
            Address = "123 Main St, Springfield",
            Teachers = new List<Teacher>(),
            Students = new List<Student>()
        };

        var school2 = new School
        {
            Name = "Shelbyville Elementary",
            Address = "456 Elm St, Shelbyville",
            Teachers = new List<Teacher>(),
            Students = new List<Student>()
        };

        db.Schools.AddRange(school1, school2);

        // Add teachers to the schools
        var teacher1 = new Teacher
        {
            Name = "Ms. Frizzle",
            Subject = "Science",
            School = school1
        };

        var teacher2 = new Teacher
        {
            Name = "Mr. Smith",
            Subject = "Mathematics",
            School = school1
        };

        db.Teachers.AddRange(teacher1, teacher2);

        // Add students to the schools
        var student1 = new Student
        {
            Name = "Bart Simpson",
            Age = 10,
            School = school1
        };

        var student2 = new Student
        {
            Name = "Lisa Simpson",
            Age = 8,
            School = school1
        };

        var student3 = new Student
        {
            Name = "Ralph Wiggum",
            Age = 9,
            School = school2
        };

        db.Students.AddRange(student1, student2, student3);

        // Create a class and assign the teacher and students
        var class1 = new Class
        {
            ClassName = "Introduction to Science",
            Teacher = teacher1,
            Students = new List<Student> { student1, student2 }
        };

        db.Classes.Add(class1);

        // Save changes to the database
        db.SaveChanges();
    }

}


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
