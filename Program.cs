using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Student
{
    public string Name { get; set; }
    public int Zone { get; set; }
    public bool IsAssigned { get; set; }

    public Student(string name, int zone)
    {
        Name = name;
        Zone = zone;
        IsAssigned = false;
    }
}

public class Bus
{
    public int Id { get; set; }
    public int Capacity { get; set; }
    public List<Student> Students { get; private set; }

    public Bus(int id, int capacity)
    {
        Id = id;
        Capacity = capacity;
        Students = new List<Student>();
    }

    public bool AddStudent(Student student)
    {
        if (Students.Count < Capacity)
        {
            Students.Add(student);
            student.IsAssigned = true;
            return true;
        }
        return false;
    }

    public int RemainingCapacity => Capacity - Students.Count;
}

public class BusAssignmentSystem
{
    private List<Bus> Buses { get; set; }
    private List<Student> Students { get; set; }

    public BusAssignmentSystem(List<Bus> buses, List<Student> students)
    {
        Buses = buses.OrderBy(b => b.Id).ToList();
        Students = students.OrderBy(s => s.Zone).ToList();
    }

    public void AssignStudents()
    {
        foreach (var zone in Students.Select(s => s.Zone).Distinct().OrderBy(z => z))
        {
            var zoneStudents = Students.Where(s => s.Zone == zone && !s.IsAssigned).ToList();

            foreach (var bus in Buses)
            {
                var studentsForBus = zoneStudents.Take(bus.RemainingCapacity).ToList();

                foreach (var student in studentsForBus)
                {
                    bus.AddStudent(student);
                }
            }
        }

        var unassignedStudents = Students.Where(s => !s.IsAssigned).ToList();
        foreach (var student in unassignedStudents)
        {
            var availableBus = Buses.FirstOrDefault(b => b.RemainingCapacity > 0);
            if (availableBus != null)
            {
                availableBus.AddStudent(student);
            }
        }
    }

    public void PrintManifests()
    {
        Console.WriteLine("Bus Assignment Manifests:");

        foreach (var bus in Buses)
        {
            Console.WriteLine($"Bus {bus.Id} (Capacity: {bus.Capacity}, Assigned: {bus.Students.Count}):");

            foreach (var zone in bus.Students.Select(s => s.Zone).Distinct())
            {
                Console.WriteLine($"  Zone {zone} Students:");
                foreach (var student in bus.Students.Where(s => s.Zone == zone))
                {
                    Console.WriteLine($"    - {student.Name}");
                }
            }
            Console.WriteLine();
        }

        var unassignedStudents = Students.Where(s => !s.IsAssigned).ToList();
        if (unassignedStudents.Count > 0)
        {
            Console.WriteLine("Unassigned Students:");
            foreach (var student in unassignedStudents)
            {
                Console.WriteLine($"- {student.Name} (Zone: {student.Zone})");
            }
        }
    }
}

namespace School_Bus_Route_Optimizer__Zones_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var buses = new List<Bus>
        {
            new Bus(1, 5),
            new Bus(2, 4),
            new Bus(3, 6)
        };

            var students = new List<Student>
        {
            new Student("Alice", 1),
            new Student("Bob", 1),
            new Student("Charlie", 1),
            new Student("David", 2),
            new Student("Eve", 2),
            new Student("Frank", 2),
            new Student("Grace", 3),
            new Student("Hank", 3),
            new Student("Ivy", 1),
            new Student("Jack", 2),
            new Student("Kelly", 4)  
        };

            var system = new BusAssignmentSystem(buses, students);
            system.AssignStudents();
            system.PrintManifests();
        }
    }
}
