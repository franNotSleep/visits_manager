using System;
using System.Collections.Generic;

namespace EntityLayer
{

    public class Building
    {
        public int Id;
        public string Name;
        public List<Classroom> Classrooms;
        public List<Visit> Visits;

        public Building(int id, string name, List<Classroom> classrooms, List<Visit> visits) 
        { 
            this.Id = id;
            this.Name = name;
            this.Visits = visits;
            this.Classrooms = classrooms;
        }
    }

    public class Classroom
    { 
    public int Id;
    public string Name;
    public Building Building;

    public Classroom(int id, string name, Building building)
    {
        Id = id;
        Name = name;
        Building = building;
    }
}

public class Visit
{
    public int Id;
    public string FirstName;
    public string LastName;
    public string FieldOfStudy;
    public string Email;
    public DateTime EntryTime;
    public DateTime ExitTime;
    public string Reason;
    public string Photos;
    public Building Building;

    public Visit(int id, string firstName, string lastName, string fieldOfStudy, string email,
                 DateTime entryTime, DateTime exitTime, string reason, string photos,
                 Building building)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        FieldOfStudy = fieldOfStudy;
        Email = email;
        EntryTime = entryTime;
        ExitTime = exitTime;
        Reason = reason;
        Photos = photos;
        Building = building;
    }
}
    public class User
    {
        public int UserId;
        public string Username;
        public string Email;
        public string Password;
        public string Role;

        public User(int userId, string username, string email, string password, string role)
        {
            UserId = userId;
            Username = username;
            Email = email;
            Password = password;
            Role = role;
        }
    }

}
