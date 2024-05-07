﻿using XResults;

namespace App;

public sealed class StudentController
{
    private readonly SchoolContext _context;
    private readonly StudentRepository _repository;

    public StudentController(SchoolContext context)
    {
        _context = context;
        _repository = new StudentRepository(context);
    }

    public string CheckStudentFavoriteCourse(long studentId, long courseId)
    {
        Student? student = _repository.GetById(studentId);
        if (student == null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course == null)
            return "Course not found";

        return student.FavoriteCourse == course ? "Yes" : "No";
    }

    public string EnrollStudent(long studentId, long courseId, Grade grade)
    {
        Student? student = _repository.GetById(studentId);
        if (student == null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course == null)
            return "Course not found";

        string result = student.EnrollIn(course, grade);

        _context.SaveChanges();

        return result;
    }

    public string DisenrollStudent(long studentId, long courseId)
    {
        Student? student = _repository.GetById(studentId);
        if (student == null)
            return "Student not found";

        Course? course = Course.FromId(courseId);
        if (course == null)
            return "Course not found";

        student.Disenroll(course);

        _context.SaveChanges();

        return "OK";
    }

    public string? RegisterStudent(
        string firstName,
        string lastName,
        long nameSuffixId,
        string email,
        long favoriteCourseId,
        Grade favoriteCourseGrade
    )
    {
        Course? favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse == null)
            return "Course not found";

        Suffix? suffix = Suffix.FromId(nameSuffixId);
        if (suffix == null)
            return "Suffix not found";

        Result<Email> emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return emailResult.ErrorMessage;

        Result<Name> nameResult = Name.Create(firstName, lastName, suffix);
        if (nameResult.IsFailure)
            return nameResult.ErrorMessage;

        var student = new Student(
            nameResult.Value,
            emailResult.Value,
            favoriteCourse,
            favoriteCourseGrade
        );
        _repository.Save(student);

        _context.SaveChanges();

        return "OK";
    }

    public string? EditPersonalInfo(
        long studentId,
        string firstName,
        string lastName,
        long nameSuffixId,
        string email,
        long favoriteCourseId
    )
    {
        Student? student = _repository.GetById(studentId);
        if (student == null)
            return "Student not found";

        Course? favoriteCourse = Course.FromId(favoriteCourseId);
        if (favoriteCourse == null)
            return "Course not found";

        Suffix? suffix = Suffix.FromId(nameSuffixId);
        if (suffix == null)
            return "Suffix not found";

        Result<Email> emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return emailResult.ErrorMessage;

        Result<Name> nameResult = Name.Create(firstName, lastName, suffix);
        if (nameResult.IsFailure)
            return nameResult.ErrorMessage;

        student.EditPersonalInfo(nameResult.Value, emailResult.Value, favoriteCourse);

        _context.SaveChanges();

        return "OK";
    }
}
