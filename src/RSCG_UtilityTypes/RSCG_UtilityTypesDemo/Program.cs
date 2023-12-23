var p = new PersonWithSalary();
p.FirstName = "Andrei";
p.LastName = "Ignat";
p.Salary = 1000;
Console.WriteLine(p.Salary);
var p1 = (Person)p;
Console.WriteLine(p1.FirstName);


