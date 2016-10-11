using System;
using System.Linq;
using System.Collections.Generic;

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Decimal Salary { get; set; }
    public DateTime StartDate { get; set; }
}

public class SimpleQuery
{
    static void Main() {
        // Create our database of employees.
        var employees = new List<Employee> {
            new Employee {
                  FirstName = "Joe",
                  LastName = "Bob",
                  Salary = 94000,
                  StartDate = DateTime.Parse("1/4/1992") },
            new Employee {
                  FirstName = "Jane",
                  LastName = "Doe",
                  Salary = 123000,
                  StartDate = DateTime.Parse("4/12/1998") },
            new Employee {
                  FirstName = "Milton",
                  LastName = "Waddams",
                  Salary = 1000000,
                  StartDate = DateTime.Parse("12/3/1969") }
        };

        var query = from employee in employees
                    where employee.Salary > 100000
                    orderby employee.LastName, employee.FirstName
                    select new { LastName = employee.LastName,
                                 FirstName = employee.FirstName };

        Console.WriteLine( "Highly paid employees:" );
        foreach( var item in query ) {
            Console.WriteLine( "{0}, {1}",
                               item.LastName,
                               item.FirstName );
        }
                    
    }
}
