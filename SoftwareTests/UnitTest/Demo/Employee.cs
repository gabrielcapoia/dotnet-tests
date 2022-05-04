using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Person
    {
        public string Name { get; protected set; }
        public string Nickname { get; set; }
    }

    public class Employee : Person
    {
        public double Salary { get; private set; }
        public ProfissionalLevel ProfissionalLevel { get; private set; }
        public IList<string> Skills { get; private set; }

        public Employee(string name, double salary)
        {
            Name = string.IsNullOrEmpty(name) ? "Fulano" : name;
            SetSalary(salary);
            SetSkills();
        }

        public void SetSalary(double salary)
        {
            if (salary < 500) throw new Exception("Lower salary than allowed");

            Salary = salary;
            if (salary < 2000) ProfissionalLevel = ProfissionalLevel.Junior;
            else if (salary >= 2000 && salary < 8000) ProfissionalLevel = ProfissionalLevel.Mid;
            else if (salary >= 8000) ProfissionalLevel = ProfissionalLevel.Senior;
        }

        private void SetSkills()
        {
            var basicSkills = new List<string>()
            {

                "Programming logic",
                "OOP"
            };

            Skills = basicSkills;

            switch (ProfissionalLevel)
            {
                case ProfissionalLevel.Mid:
                    Skills.Add("Tests");
                    break;
                case ProfissionalLevel.Senior:
                    Skills.Add("Tests");
                    Skills.Add("Microservices");
                    break;
            }
        }
    }

    public enum ProfissionalLevel
    {
        Junior,
        Mid,
        Senior
    }

    public class EmployeeFactory
    {
        public static Employee Create(string name, double salary)
        {
            return new Employee(name, salary);
        }
    }
}
