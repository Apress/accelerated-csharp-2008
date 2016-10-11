using System;

public class Employee
{
    public string FullName { get; private set; }
    public string Id { get; set; }
}

public class AutoProps
{
    static void Main() {
        Employee emp = new Employee {
            FullName = "John Doe",
            Id = "111-11-1111"
        };
    }
}
