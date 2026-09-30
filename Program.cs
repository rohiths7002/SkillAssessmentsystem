using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Data;
using SkillAssessmentSystem.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


// ===============================
// DATABASE MIGRATION + SEED DATA
// ===============================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apply pending migrations
    db.Database.Migrate();

    // Seed assessment if database is empty
    if (!db.Assessments.Any())
    {
        var assessment = new Assessment
        {
            Title = "C# Programming Fundamentals",
            Description = "Test your basic knowledge of C# programming and .NET.",
            DurationMinutes = 15,
            TotalQuestions = 5,
            CreatedAt = DateTime.Now
        };

        db.Assessments.Add(assessment);
        db.SaveChanges();


        // ===============================
        // QUESTIONS
        // ===============================

        var questions = new List<Question>
        {
            new Question
            {
                AssessmentId = assessment.AssessmentId,
                QuestionText = "Which keyword is used to define a class in C#?",
                OptionA = "class",
                OptionB = "Class",
                OptionC = "define",
                OptionD = "struct",
                CorrectAnswer = "class"
            },

            new Question
            {
                AssessmentId = assessment.AssessmentId,
                QuestionText = "Which symbol is used to end a statement in C#?",
                OptionA = ":",
                OptionB = ";",
                OptionC = ".",
                OptionD = ",",
                CorrectAnswer = ";"
            },

            new Question
            {
                AssessmentId = assessment.AssessmentId,
                QuestionText = "Which data type is used to store true or false?",
                OptionA = "int",
                OptionB = "string",
                OptionC = "bool",
                OptionD = "double",
                CorrectAnswer = "bool"
            },

            new Question
            {
                AssessmentId = assessment.AssessmentId,
                QuestionText = "Which method is the entry point of a C# console application?",
                OptionA = "Start()",
                OptionB = "Run()",
                OptionC = "Main()",
                OptionD = "Begin()",
                CorrectAnswer = "Main()"
            },

            new Question
            {
                AssessmentId = assessment.AssessmentId,
                QuestionText = "Which keyword is used to create an object in C#?",
                OptionA = "create",
                OptionB = "object",
                OptionC = "new",
                OptionD = "instance",
                CorrectAnswer = "new"
            }
        };

        db.Questions.AddRange(questions);
        db.SaveChanges();
    }
}


// ===============================
// HTTP REQUEST PIPELINE
// ===============================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();