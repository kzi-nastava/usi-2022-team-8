// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.GUI.LoginWindow;


/*
JsonSerializerOptions options = new JsonSerializerOptions
{
    Converters ={
        new JsonStringEnumConverter()
    }
};
var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(@"..\..\..\Data\JSON\users.json"), options);

foreach (User user in users)
{
    System.Console.WriteLine(user.username);
    //System.Console.WriteLine(user.type);
}*/
