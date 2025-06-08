using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DelegatesLinQ.Homework
{
    // Data models for the homework
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
        public DateTime EnrollmentDate { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public double Grade { get; set; } // 0-4.0 scale
        public string Semester { get; set; }
        public string Instructor { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    /// <summary>
    /// Homework 3: LINQ Data Processor
    /// 
    /// This is the most challenging homework requiring students to:
    /// 1. Use complex LINQ operations with multiple data sources
    /// 2. Implement custom extension methods
    /// 3. Create generic delegates and expressions
    /// 4. Handle advanced scenarios like pivot operations, statistical analysis
    /// 5. Implement caching and performance optimization
    /// 
    /// Advanced Requirements:
    /// - Custom LINQ extension methods
    /// - Expression trees and dynamic queries
    /// - Performance comparison between different approaches
    /// - Statistical calculations and reporting
    /// - Data transformation and pivot operations
    /// </summary>
    public class LinqDataProcessor
    {
        private List<Student> _students;

        public LinqDataProcessor()
        {
            _students = GenerateSampleData();
        }

        // BASIC REQUIREMENTS (using techniques from sample codes)

        public void BasicQueries()
        {
            // TODO: Implement basic LINQ queries similar to 6_8_LinQObject

            // 1. Find all students with GPA > 3.5
            // 2. Group students by major
            // 3. Calculate average GPA per major
            // 4. Find students enrolled in specific courses
            // 5. Sort students by enrollment date

            Console.WriteLine("=== BASIC LINQ QUERIES ===");
            // Implementation needed by students
            var highGpaStudents = _students.Where(p => p.GPA > 3.5).ToList();
            Console.WriteLine("Students with GPA > 3.5:");
            foreach (var s in highGpaStudents)
            {
                Console.WriteLine($"{s.Name} - GPA: {s.GPA}");
            }
            var major = _students.GroupBy(p => p.Major).ToList();
            Console.WriteLine("Group students by major");
            foreach (var s in major)
            {
                Console.WriteLine($"{s.Key}");
                foreach (var student in s)
                {
                    Console.WriteLine($"{student.Name}");
                }
            }
            var avg = _students.GroupBy(p => p.Major).Select(g => new
            {
                Major = g.Key,
                x = g.Average(e => e.GPA)
            });
            Console.WriteLine("Calculate average GPA per major");
            foreach (var s in avg)
            {
                Console.WriteLine($"{s.Major} - {s.x}");
            }
            var enrolled = _students.SelectMany(x => x.Courses).Select(c => c.Name).Distinct().ToList();
            Console.WriteLine("Find students enrolled in specific courses");
            foreach (var s in enrolled)
            {
                Console.WriteLine($"{s}");
            }
            var sort = _students.OrderBy(p => p.EnrollmentDate).ToList();
            Console.WriteLine("Sort students by enrollment date");
            foreach (var s in sort)
            {
                Console.WriteLine($"{s.Name} - Entrolled on: {s.EnrollmentDate}");
            }
        }

        // Challenge 1: Create custom extension methods
        public void CustomExtensionMethods()
        {
            Console.WriteLine("=== CUSTOM EXTENSION METHODS ===");

            // TODO: Implement extension methods
            // 1. CreateAverageGPAByMajor() extension method
            // 2. FilterByAgeRange(int min, int max) extension method  
            // 3. ToGradeReport() extension method that creates formatted output
            // 4. CalculateStatistics() extension method

            // Example usage (students need to implement the extensions):
            Console.WriteLine("\nStudents aged 20 to 22:");
            var AgeStudents = _students.FilterByAgeRange(20, 25).Where(s => s.GPA > 3.5);
            foreach (var item in AgeStudents)
            {
                Console.WriteLine($"{item.Name}: {item.Age}");
            }
            Console.WriteLine("Average GPA by Major:");
            var gpaMajor = _students.AverageGPAByMajor();
            foreach (var item in gpaMajor)
            {
                Console.WriteLine($"major : {item.Key} GPA : {item.Value:F2}");
            }
            Console.WriteLine("\nGrade Reports:");
            foreach (var s in _students)
            {
                s.ToGradeReport();
            }
            var stats = _students.CalculateStatistics();
            Console.WriteLine("CalculateStatistics");
            Console.WriteLine(stats.ToString());
        }

        // Challenge 2: Dynamic queries using Expression Trees
        private Expression<Func<Student, bool>> BuildDynamicFilter(string propertyName, string op, object value)
        {
            var propertyInfo = typeof(Student).GetProperty(propertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"Thuộc tính '{propertyName}' không tồn tại.");

            var parameter = Expression.Parameter(typeof(Student), "s");
            var property = Expression.Property(parameter, propertyInfo);
            var constant = Expression.Constant(Convert.ChangeType(value, propertyInfo.PropertyType));

            Expression comparison;
            switch (op.ToLower())
            {
                case ">": comparison = Expression.GreaterThan(property, constant); break;
                case ">=": comparison = Expression.GreaterThanOrEqual(property, constant); break;
                case "<": comparison = Expression.LessThan(property, constant); break;
                case "<=": comparison = Expression.LessThanOrEqual(property, constant); break;
                case "==": comparison = Expression.Equal(property, constant); break;
                default: throw new ArgumentException($"Toán tử '{op}' không được hỗ trợ.");
            }

            return Expression.Lambda<Func<Student, bool>>(comparison, parameter);
        }

        private IQueryable<Student> DynamicSort(IQueryable<Student> query, string propertyName, bool ascending = true)
        {
            var parameter = Expression.Parameter(typeof(Student), "s");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(Student), property.Type);

            return (IQueryable<Student>)method.Invoke(null, new object[] { query, lambda });
        }
        public void DynamicQueries()
        {
            Console.WriteLine("=== DYNAMIC QUERIES ===");

            // TODO: Research Expression Trees
            // Implement a method that builds LINQ queries dynamically based on user input
            // Example: BuildDynamicFilter("GPA", ">", "3.5")
            // This requires understanding of Expression<Func<T, bool>>

            // Students should implement:
            // 1. Dynamic filtering based on property name and value
            // 2. Dynamic sorting by any property
            // 3. Dynamic grouping operations
            var filter1 = BuildDynamicFilter("GPA", ">", 3.5);
            var highGpaStudents = _students.AsQueryable().Where(filter1);
            Console.WriteLine("Dynamic filter - GPA > 3.5:");
            foreach (var student in highGpaStudents)
            {
                Console.WriteLine($"{student.Name} - GPA: {student.GPA}");
            }

            var filter2 = BuildDynamicFilter("Age", "<=", 21);
            var youngStudents = _students.AsQueryable().Where(filter2);
            Console.WriteLine("\nDynamic filter - Age <= 21:");
            foreach (var student in youngStudents)
            {
                Console.WriteLine($"{student.Name} - Age: {student.Age}");
            }

            // Dynamic sorting
            var sortedByName = DynamicSort(_students.AsQueryable(), "Name");
            Console.WriteLine("\nDynamic sort by Name:");
            foreach (var student in sortedByName.Take(3))
            {
                Console.WriteLine($"{student.Name}");
            }

            var sortedByGPA = DynamicSort(_students.AsQueryable(), "GPA", false);
            Console.WriteLine("\nDynamic sort by GPA (descending):");
            foreach (var student in sortedByGPA)
            {
                Console.WriteLine($"{student.Name} - GPA: {student.GPA}");
            }

        }

        // Challenge 3: Statistical Analysis with Complex Aggregations
        public void StatisticalAnalysis()
        {
            Console.WriteLine("=== STATISTICAL ANALYSIS ===");

            // TODO: Implement complex statistical calculations
            // 1. Calculate median GPA (requires custom logic)
            // 2. Calculate standard deviation of GPAs
            // 3. Find correlation between age and GPA
            // 4. Identify outliers using statistical methods
            // 5. Create percentile rankings

            // This requires research into statistical formulas and advanced LINQ usage
            var gpas = _students.Select(s => s.GPA).ToList();
            var ages = _students.Select(s => s.Age).ToList();
            var sortedGPAs = gpas.OrderBy(x => x).ToList();
            double median = sortedGPAs.Count % 2 == 0
                ? (sortedGPAs[sortedGPAs.Count / 2 - 1] + sortedGPAs[sortedGPAs.Count / 2]) / 2
                : sortedGPAs[sortedGPAs.Count / 2];

            // 2. Standard deviation
            double mean = gpas.Average();
            double variance = gpas.Select(x => Math.Pow(x - mean, 2)).Average();
            double stdDev = Math.Sqrt(variance);

            // 3. Correlation between age and GPA
            double correlation = CalculateCorrelation(ages.Select(x => (double)x).ToList(), gpas);

            // 4. Outliers (using IQR method)
            var q1 = CalculatePercentile(sortedGPAs, 25);
            var q3 = CalculatePercentile(sortedGPAs, 75);
            var iqr = q3 - q1;
            var lowerBound = q1 - 1.5 * iqr;
            var upperBound = q3 + 1.5 * iqr;
            var outliers = _students.Where(s => s.GPA < lowerBound || s.GPA > upperBound).ToList();

            // 5. Percentile rankings
            var percentileRankings = _students.Select(s => new
            {
                Student = s,
                Percentile = CalculatePercentileRank(gpas, s.GPA)
            }).OrderByDescending(x => x.Percentile);

            Console.WriteLine($"Mean GPA: {mean:F3}");
            Console.WriteLine($"Median GPA: {median:F3}");
            Console.WriteLine($"Standard Deviation: {stdDev:F3}");
            Console.WriteLine($"Correlation (Age vs GPA): {correlation:F3}");
            Console.WriteLine($"Number of GPA outliers: {outliers.Count}");

            Console.WriteLine("\nPercentile Rankings:");
            foreach (var item in percentileRankings)
            {
                Console.WriteLine($"{item.Student.Name}: {item.Percentile:F1}th percentile (GPA: {item.Student.GPA})");
            }
        }

        // Challenge 4: Data Pivot Operations
        public void PivotOperations()
        {
            Console.WriteLine("=== PIVOT OPERATIONS ===");

            // TODO: Research pivot table concepts
            // Create pivot tables showing:
            // 1. Students by Major vs GPA ranges (3.0-3.5, 3.5-4.0, etc.)
            // 2. Course enrollment by semester and major
            // 3. Grade distribution across instructors

            // This requires understanding of GroupBy with multiple keys and complex projections
            var gpaRanges = _students
           .GroupBy(s => s.Major)
           .Select(majorGroup => new
           {
               Major = majorGroup.Key,
               GpaRanges = majorGroup
                   .GroupBy(s => GetGpaRange(s.GPA))
                   .ToDictionary(g => g.Key, g => g.Count())
           });

            Console.WriteLine("Students by Major vs GPA Ranges:");
            foreach (var major in gpaRanges)
            {
                Console.WriteLine($"\n{major.Major}:");
                foreach (var range in major.GpaRanges)
                {
                    Console.WriteLine($"  {range.Key}: {range.Value} students");
                }
            }

            // 2. Course enrollment by semester and major
            var courseEnrollment = _students
                .SelectMany(s => s.Courses.Select(c => new { Student = s, Course = c }))
                .GroupBy(x => new { x.Course.Semester, x.Student.Major })
                .Select(g => new
                {
                    Semester = g.Key.Semester,
                    Major = g.Key.Major,
                    Count = g.Count(),
                    Courses = g.Select(x => x.Course.Name).Distinct().ToList()
                });

            Console.WriteLine("\nCourse Enrollment by Semester and Major:");
            foreach (var enrollment in courseEnrollment.OrderBy(x => x.Semester).ThenBy(x => x.Major))
            {
                Console.WriteLine($"{enrollment.Semester} - {enrollment.Major}: {enrollment.Count} enrollments");
                Console.WriteLine($"  Courses: {string.Join(", ", enrollment.Courses)}");
            }

            // 3. Grade distribution across instructors
            var gradeDistribution = _students
                .SelectMany(s => s.Courses)
                .GroupBy(c => c.Instructor)
                .Select(g => new
                {
                    Instructor = g.Key,
                    AverageGrade = g.Average(c => c.Grade),
                    CourseCount = g.Count(),
                    GradeRanges = g.GroupBy(c => GetGradeRange(c.Grade))
                                   .ToDictionary(gr => gr.Key, gr => gr.Count())
                });

            Console.WriteLine("\nGrade Distribution by Instructor:");
            foreach (var instructor in gradeDistribution.OrderByDescending(x => x.AverageGrade))
            {
                Console.WriteLine($"\n{instructor.Instructor} (Avg: {instructor.AverageGrade:F2}, Courses: {instructor.CourseCount}):");
                foreach (var range in instructor.GradeRanges)
                {
                    Console.WriteLine($"  {range.Key}: {range.Value} grades");
                }
            }
        }
        private double CalculateCorrelation(List<double> x, List<double> y)
        {
            if (x.Count != y.Count) return 0;

            double meanX = x.Average();
            double meanY = y.Average();

            double numerator = x.Zip(y, (xi, yi) => (xi - meanX) * (yi - meanY)).Sum();
            double denominatorX = Math.Sqrt(x.Select(xi => Math.Pow(xi - meanX, 2)).Sum());
            double denominatorY = Math.Sqrt(y.Select(yi => Math.Pow(yi - meanY, 2)).Sum());

            return numerator / (denominatorX * denominatorY);
        }

        private double CalculatePercentile(List<double> sortedValues, double percentile)
        {
            double index = (percentile / 100.0) * (sortedValues.Count - 1);
            int lowerIndex = (int)Math.Floor(index);
            int upperIndex = (int)Math.Ceiling(index);

            if (lowerIndex == upperIndex)
                return sortedValues[lowerIndex];

            return sortedValues[lowerIndex] + (index - lowerIndex) * (sortedValues[upperIndex] - sortedValues[lowerIndex]);
        }

        private double CalculatePercentileRank(List<double> values, double value)
        {
            int countBelow = values.Count(v => v < value);
            int countEqual = values.Count(v => Math.Abs(v - value) < 0.001);
            return (countBelow + 0.5 * countEqual) / values.Count * 100;
        }

        private string GetGpaRange(double gpa)
        {
            if (gpa >= 3.7) return "3.7-4.0 (A)";
            if (gpa >= 3.3) return "3.3-3.6 (B+)";
            if (gpa >= 3.0) return "3.0-3.2 (B)";
            if (gpa >= 2.7) return "2.7-2.9 (C+)";
            return "Below 2.7 (C or lower)";
        }

        private string GetGradeRange(double grade)
        {
            if (grade >= 3.7) return "A (3.7-4.0)";
            if (grade >= 3.3) return "B+ (3.3-3.6)";
            if (grade >= 3.0) return "B (3.0-3.2)";
            if (grade >= 2.7) return "C+ (2.7-2.9)";
            return "C or lower (< 2.7)";
        }


        // Sample data generator
        private List<Student> GenerateSampleData()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1, Name = "Alice Johnson", Age = 20, Major = "Computer Science",
                    GPA = 3.8, EnrollmentDate = new DateTime(2022, 9, 1),
                    Email = "alice.j@university.edu",
                    Address = new Address { City = "Seattle", State = "WA", ZipCode = "98101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS101", Name = "Intro to Programming", Credits = 3, Grade = 3.7, Semester = "Fall 2022", Instructor = "Dr. Smith" },
                        new Course { Code = "MATH201", Name = "Calculus II", Credits = 4, Grade = 3.9, Semester = "Fall 2022", Instructor = "Prof. Johnson" }
                    }
                },
                new Student
                {
                    Id = 2, Name = "Bob Wilson", Age = 22, Major = "Mathematics",
                    GPA = 3.2, EnrollmentDate = new DateTime(2021, 9, 1),
                    Email = "bob.w@university.edu",
                    Address = new Address { City = "Portland", State = "OR", ZipCode = "97201" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "MATH301", Name = "Linear Algebra", Credits = 3, Grade = 3.3, Semester = "Spring 2023", Instructor = "Dr. Brown" },
                        new Course { Code = "STAT101", Name = "Statistics", Credits = 3, Grade = 3.1, Semester = "Spring 2023", Instructor = "Prof. Davis" }
                    }
                },
                // Add more sample students...
                new Student
                {
                    Id = 3, Name = "Carol Davis", Age = 19, Major = "Computer Science",
                    GPA = 3.9, EnrollmentDate = new DateTime(2023, 9, 1),
                    Email = "carol.d@university.edu",
                    Address = new Address { City = "San Francisco", State = "CA", ZipCode = "94101" },
                    Courses = new List<Course>
                    {
                        new Course { Code = "CS102", Name = "Data Structures", Credits = 4, Grade = 4.0, Semester = "Fall 2023", Instructor = "Dr. Smith" },
                        new Course { Code = "CS201", Name = "Algorithms", Credits = 3, Grade = 3.8, Semester = "Fall 2023", Instructor = "Prof. Lee" }
                    }
                }
            };
        }
    }

    // TODO: Implement these extension methods
    public static class StudentExtensions
    {
        // Challenge: Implement custom extension methods
         public static IEnumerable<Student> FilterByAgeRange(this IEnumerable<Student> students, int minAge, int maxAge) {
             return students.Where(s => s.Age > minAge && s.Age <= maxAge);
        }
         public static Dictionary<string, double> AverageGPAByMajor(this IEnumerable<Student> students) {
            return students.GroupBy(x => x.Major).ToDictionary(g => g.Key , g => g.Average(x => x.GPA)); 
        }
         public static string ToGradeReport(this Student student) {
            return $"{student.Name} - {student.Major} - {student.GPA}";
        }
        public static StudentStatistics CalculateStatistics(this IEnumerable<Student> students) {
            var gpas = students.Select(x => x.GPA).ToList();
            double mean = gpas.Average();
            double median = gpas.OrderBy(p => p).ElementAt(gpas.Count / 2);
            double std = Math.Sqrt(gpas.Select(x => Math.Pow(x - mean, 2)).Average());
            return new StudentStatistics(mean, median, std);
        }
    }

    // TODO: Define this class for statistical operations
    public class StudentStatistics
    {
        // Properties for mean, median, mode, standard deviation, etc.
        public double Mean { get; set; }
        public double Median { get; set; }
        public double StandardDeviation { get; set; }
public StudentStatistics(double mean, double median, double standardDeviation)
        {
            Mean = mean;
            Median = median;
            StandardDeviation = standardDeviation;
        }

        public override string ToString()
        {
            return $"Mean: {Mean:F2}\nMedian: {Median:F2}\nStd Dev: {StandardDeviation:F2}\n";
        }
    }

    public class LinqDataProcessor1
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 3: LINQ DATA PROCESSOR ===");
            Console.WriteLine();

            Console.WriteLine("BASIC REQUIREMENTS:");
            Console.WriteLine("1. Implement basic LINQ queries (filtering, grouping, sorting)");
            Console.WriteLine("2. Use SelectMany for course data");
            Console.WriteLine("3. Calculate averages and aggregations");
            Console.WriteLine();

            Console.WriteLine("ADVANCED REQUIREMENTS:");
            Console.WriteLine("1. Create custom LINQ extension methods");
            Console.WriteLine("2. Implement dynamic queries using Expression Trees");
            Console.WriteLine("3. Perform statistical analysis (median, std dev, correlation)");
            Console.WriteLine("4. Create pivot table operations");
            Console.WriteLine();

            LinqDataProcessor processor = new LinqDataProcessor();

            // Students should implement all these methods
            processor.BasicQueries();
            processor.CustomExtensionMethods();
            processor.DynamicQueries();
            processor.StatisticalAnalysis();
            processor.PivotOperations();

            Console.ReadKey();
        }
    }
}