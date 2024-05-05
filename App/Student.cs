﻿namespace App;

public class Student : Entity
{
    public string Name { get; set; }
    public Email Email { get; set; }
    public virtual Course FavoriteCourse { get; set; }
    public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();
    private readonly List<Enrollment> _enrollments = new List<Enrollment>();

    protected Student() { }

    public Student(string name, Email email, Course favoriteCourse, Grade favoriteCourseGrade)
        : this()
    {
        Name = name;
        Email = email;
        FavoriteCourse = favoriteCourse;

        EnrollIn(favoriteCourse, favoriteCourseGrade);
    }

    public string EnrollIn(Course course, Grade grade)
    {
        if (_enrollments.Any(x => x.Course == course))
            return $"Already enrolled in course '{course.Name}'";

        var enrollment = new Enrollment(course, this, grade);
        _enrollments.Add(enrollment);

        return "OK";
    }

    public void Disenroll(Course course)
    {
        Enrollment? enrollment = _enrollments.FirstOrDefault(x => x.Course == course);

        if (enrollment == null)
            return;

        _enrollments.Remove(enrollment);
    }
}
