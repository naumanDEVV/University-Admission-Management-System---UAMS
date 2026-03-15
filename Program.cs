using System;
using System.Collections.Generic;

// ==================== Subject Class ====================
class Subject
{
    public string code;
    public string type;
    public int creditHours;
    public int subjectFee;

    public Subject(string code, string type, int creditHours, int subjectFee)
    {
        this.code = code;
        this.type = type;
        this.creditHours = creditHours;
        this.subjectFee = subjectFee;
    }
}

// ==================== DegreeProgram Class ====================
class DegreeProgram
{
    public string degreeName;
    public float degreeDuration;
    public int seats;
    public List<Subject> subjects;

    public DegreeProgram(string degreeName, float degreeDuration, int seats)
    {
        this.degreeName = degreeName;
        this.degreeDuration = degreeDuration;
        this.seats = seats;
        this.subjects = new List<Subject>();
    }

    public int calculateCreditHours()
    {
        int total = 0;
        for (int i = 0; i < subjects.Count; i++)
        {
            total += subjects[i].creditHours;
        }
        return total;
    }

    public bool isSubjectExists(Subject sub)
    {
        for (int i = 0; i < subjects.Count; i++)
        {
            if (subjects[i].code == sub.code)
                return true;
        }
        return false;
    }

    public bool addSubject(Subject s)
    {
        int currentCH = calculateCreditHours();
        if (currentCH + s.creditHours <= 20)
        {
            subjects.Add(s);
            return true;
        }
        return false;
    }
}

// ==================== Student Class ====================
class Student
{
    public string name;
    public int age;
    public double fscMarks;
    public double ecatMarks;
    public double merit;
    public List<DegreeProgram> preferences;
    public List<Subject> regSubjects;
    public DegreeProgram regDegree;

    public Student(string name, int age, double fscMarks, double ecatMarks, List<DegreeProgram> preferences)
    {
        this.name = name;
        this.age = age;
        this.fscMarks = fscMarks;
        this.ecatMarks = ecatMarks;
        this.preferences = preferences;
        this.regSubjects = new List<Subject>();
        this.regDegree = null;
        this.merit = 0;
    }

    public void calculateMerit()
    {
        // Merit = 70% FSC (out of 1100) + 30% ECAT (out of 400)
        merit = (fscMarks / 1100.0 * 70) + (ecatMarks / 400.0 * 30);
    }

    public int getCreditHours()
    {
        int total = 0;
        for (int i = 0; i < regSubjects.Count; i++)
        {
            total += regSubjects[i].creditHours;
        }
        return total;
    }

    public float calculateFee()
    {
        float total = 0;
        for (int i = 0; i < regSubjects.Count; i++)
        {
            total += regSubjects[i].subjectFee;
        }
        return total;
    }

    public bool registerSubject(Subject s)
    {
        int currentCH = getCreditHours();
        if (regDegree != null && regDegree.isSubjectExists(s) && currentCH + s.creditHours <= 9)
        {
            regSubjects.Add(s);
            return true;
        }
        return false;
    }
}

// ==================== Main Program ====================
class Program
{
    static void printLine()
    {
        Console.WriteLine("========================================");
    }

    static int showMenu()
    {
        Console.Clear();
        printLine();
        Console.WriteLine("            UAMS - University Admission");
        Console.WriteLine("           Management System");
        printLine();
        Console.WriteLine("  1. Add Degree Program");
        Console.WriteLine("  2. Add Student");
        Console.WriteLine("  3. Generate Merit & Give Admission");
        Console.WriteLine("  4. View Registered Students");
        Console.WriteLine("  5. View Students of a Specific Program");
        Console.WriteLine("  6. Register Subjects for a Student");
        Console.WriteLine("  7. Calculate Fees for All Students");
        Console.WriteLine("  8. Exit");
        printLine();
        Console.Write("  Enter Option: ");
        int opt;
        int.TryParse(Console.ReadLine(), out opt);
        return opt;
    }

    static void pause()
    {
        Console.WriteLine("\nPress any key to Continue...");
        Console.ReadKey();
    }

    // ---- Option 1: Add Degree Program ----
    static void addDegreeProgram(List<DegreeProgram> programList)
    {
        Console.Clear();
        printLine();
        Console.WriteLine("         Add Degree Program");
        printLine();

        Console.Write("Enter Degree Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Degree Duration (years): ");
        float duration;
        float.TryParse(Console.ReadLine(), out duration);

        Console.Write("Enter Number of Seats: ");
        int seats;
        int.TryParse(Console.ReadLine(), out seats);

        DegreeProgram dp = new DegreeProgram(name, duration, seats);

        Console.Write("Enter How Many Subjects to Add: ");
        int count;
        int.TryParse(Console.ReadLine(), out count);

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine("\n--- Subject " + (i + 1) + " ---");
            Console.Write("Enter Subject Code: ");
            string code = Console.ReadLine();

            Console.Write("Enter Subject Type: ");
            string type = Console.ReadLine();

            Console.Write("Enter Credit Hours: ");
            int ch;
            int.TryParse(Console.ReadLine(), out ch);

            Console.Write("Enter Subject Fee: ");
            int fee;
            int.TryParse(Console.ReadLine(), out fee);

            Subject s = new Subject(code, type, ch, fee);
            bool added = dp.addSubject(s);
            if (!added)
            {
                Console.WriteLine("** Cannot add subject: 20 credit hour limit exceeded. Subject skipped. **");
            }
            else
            {
                Console.WriteLine("Subject added successfully.");
            }
        }

        programList.Add(dp);
        Console.WriteLine("\nDegree Program '" + name + "' added successfully!");
        pause();
    }

    // ---- Option 2: Add Student ----
    static void addStudent(List<Student> studentList, List<DegreeProgram> programList)
    {
        Console.Clear();
        printLine();
        Console.WriteLine("              Add Student");
        printLine();

        if (programList.Count == 0)
        {
            Console.WriteLine("No degree programs available. Please add a program first.");
            pause();
            return;
        }

        Console.Write("Enter Student Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Student Age: ");
        int age;
        int.TryParse(Console.ReadLine(), out age);

        Console.Write("Enter FSC Marks (out of 1100): ");
        double fsc;
        double.TryParse(Console.ReadLine(), out fsc);

        Console.Write("Enter ECAT Marks (out of 400): ");
        double ecat;
        double.TryParse(Console.ReadLine(), out ecat);

        Console.WriteLine("\nAvailable Degree Programs:");
        for (int i = 0; i < programList.Count; i++)
        {
            Console.WriteLine("  " + (i + 1) + ". " + programList[i].degreeName);
        }

        Console.Write("\nEnter How Many Preferences to Enter: ");
        int prefCount;
        int.TryParse(Console.ReadLine(), out prefCount);

        List<DegreeProgram> prefs = new List<DegreeProgram>();
        for (int i = 0; i < prefCount; i++)
        {
            Console.Write("Enter Preference " + (i + 1) + " (Degree Name): ");
            string prefName = Console.ReadLine();
            bool found = false;
            for (int j = 0; j < programList.Count; j++)
            {
                if (programList[j].degreeName.ToLower() == prefName.ToLower())
                {
                    prefs.Add(programList[j]);
                    found = true;
                    break;
                }
            }
            if (!found)
                Console.WriteLine("  Program not found. Skipped.");
        }

        Student st = new Student(name, age, fsc, ecat, prefs);
        studentList.Add(st);
        Console.WriteLine("\nStudent '" + name + "' added successfully!");
        pause();
    }

    // ---- Option 3: Generate Merit & Give Admission ----
    static void generateMeritAndAdmission(List<Student> studentList)
    {
        Console.Clear();
        printLine();
        Console.WriteLine("       Generate Merit & Give Admission");
        printLine();

        if (studentList.Count == 0)
        {
            Console.WriteLine("No students found.");
            pause();
            return;
        }

        // Calculate merit for all students
        for (int i = 0; i < studentList.Count; i++)
        {
            studentList[i].calculateMerit();
        }

        // Sort students by merit (bubble sort - descending)
        for (int i = 0; i < studentList.Count - 1; i++)
        {
            for (int j = 0; j < studentList.Count - i - 1; j++)
            {
                if (studentList[j].merit < studentList[j + 1].merit)
                {
                    Student temp = studentList[j];
                    studentList[j] = studentList[j + 1];
                    studentList[j + 1] = temp;
                }
            }
        }

        // Give admission based on preferences and seats
        for (int i = 0; i < studentList.Count; i++)
        {
            Student st = studentList[i];
            bool admitted = false;

            for (int p = 0; p < st.preferences.Count; p++)
            {
                DegreeProgram dp = st.preferences[p];
                if (dp.seats > 0)
                {
                    st.regDegree = dp;
                    dp.seats--;
                    Console.WriteLine(st.name + " got Admission in " + dp.degreeName);
                    admitted = true;
                    break;
                }
            }

            if (!admitted)
            {
                Console.WriteLine(st.name + " did not get Admission");
            }
        }

        pause();
    }

    // ---- Option 4: View Registered Students ----
    static void viewRegisteredStudents(List<Student> studentList)
    {
        Console.Clear();
        printLine();
        Console.WriteLine("          Registered Students");
        printLine();

        bool found = false;
        Console.WriteLine(String.Format("{0,-15} {1,-8} {2,-8} {3,-5}", "Name", "FSC", "ECAT", "Age"));
        Console.WriteLine("----------------------------------------");

        for (int i = 0; i < studentList.Count; i++)
        {
            if (studentList[i].regDegree != null)
            {
                Student st = studentList[i];
                Console.WriteLine(String.Format("{0,-15} {1,-8} {2,-8} {3,-5}", st.name, st.fscMarks, st.ecatMarks, st.age));
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No registered students found.");

        pause();
    }

    // ---- Option 5: View Students of a Specific Program ----
    static void viewStudentsInDegree(List<Student> studentList)
    {
        Console.Clear();
        printLine();
        Console.WriteLine("      View Students of a Specific Program");
        printLine();

        Console.Write("Enter Degree Name: ");
        string degName = Console.ReadLine();

        Console.WriteLine();
        Console.WriteLine(String.Format("{0,-15} {1,-8} {2,-8} {3,-5}", "Name", "FSC", "ECAT", "Age"));
        Console.WriteLine("----------------------------------------");

        bool found = false;
        for (int i = 0; i < studentList.Count; i++)
        {
            Student st = studentList[i];
            if (st.regDegree != null && st.regDegree.degreeName.ToLower() == degName.ToLower())
            {
                Console.WriteLine(String.Format("{0,-15} {1,-8} {2,-8} {3,-5}", st.name, st.fscMarks, st.ecatMarks, st.age));
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No students found for degree: " + degName);

        pause();
    }

    // ---- Option 6: Register Subjects for a Student ----
    static void registerSubjectsForStudent(List<Student> studentList)
    {
        Console.Clear();
        printLine();
        Console.WriteLine("      Register Subjects for a Student");
        printLine();

        Console.Write("Enter Student Name: ");
        string sName = Console.ReadLine();

        Student found = null;
        for (int i = 0; i < studentList.Count; i++)
        {
            if (studentList[i].name.ToLower() == sName.ToLower())
            {
                found = studentList[i];
                break;
            }
        }

        if (found == null)
        {
            Console.WriteLine("Student not found.");
            pause();
            return;
        }

        if (found.regDegree == null)
        {
            Console.WriteLine("Student is not admitted in any program.");
            pause();
            return;
        }

        Console.WriteLine("\nAvailable Subjects in " + found.regDegree.degreeName + ":");
        Console.WriteLine(String.Format("{0,-10} {1,-15} {2,-5}", "Code", "Type", "CH"));
        Console.WriteLine("-------------------------------");

        List<Subject> subList = found.regDegree.subjects;
        for (int i = 0; i < subList.Count; i++)
        {
            Console.WriteLine(String.Format("{0,-10} {1,-15} {2,-5}", subList[i].code, subList[i].type, subList[i].creditHours));
        }

        Console.WriteLine("\nCurrent Credit Hours: " + found.getCreditHours() + " / 9");
        Console.Write("Enter Subject Code to Register: ");
        string subCode = Console.ReadLine();

        Subject toReg = null;
        for (int i = 0; i < subList.Count; i++)
        {
            if (subList[i].code.ToLower() == subCode.ToLower())
            {
                toReg = subList[i];
                break;
            }
        }

        if (toReg == null)
        {
            Console.WriteLine("Subject not found.");
        }
        else
        {
            bool success = found.registerSubject(toReg);
            if (success)
                Console.WriteLine("Subject '" + toReg.code + "' registered successfully for " + found.name + "!");
            else
                Console.WriteLine("Cannot register: Credit hour limit (9) exceeded or subject not in program.");
        }

        pause();
    }

    // ---- Option 7: Calculate Fees ----
    static void calculateFees(List<Student> studentList)
    {
        Console.Clear();
        printLine();
        Console.WriteLine("     Fee Calculation for Registered Students");
        printLine();

        bool any = false;
        Console.WriteLine(String.Format("{0,-15} {1,-20} {2,-10}", "Name", "Program", "Total Fee"));
        Console.WriteLine("----------------------------------------");

        for (int i = 0; i < studentList.Count; i++)
        {
            Student st = studentList[i];
            if (st.regDegree != null)
            {
                float fee = st.calculateFee();
                Console.WriteLine(String.Format("{0,-15} {1,-20} {2,-10}", st.name, st.regDegree.degreeName, fee));
                any = true;
            }
        }

        if (!any)
            Console.WriteLine("No registered students found.");

        pause();
    }

    // ==================== Main ====================
    static void Main(string[] args)
    {
        List<Student> studentList = new List<Student>();
        List<DegreeProgram> programList = new List<DegreeProgram>();

        int option;
        do
        {
            option = showMenu();

            if (option == 1)
                addDegreeProgram(programList);
            else if (option == 2)
                addStudent(studentList, programList);
            else if (option == 3)
                generateMeritAndAdmission(studentList);
            else if (option == 4)
                viewRegisteredStudents(studentList);
            else if (option == 5)
                viewStudentsInDegree(studentList);
            else if (option == 6)
                registerSubjectsForStudent(studentList);
            else if (option == 7)
                calculateFees(studentList);
            else if (option != 8)
            {
                Console.WriteLine("Invalid option. Try again.");
                pause();
            }

        } while (option != 8);

        Console.Clear();
        Console.WriteLine("Thank you for using UAMS. Goodbye!");
        Console.ReadKey();
    }
}

