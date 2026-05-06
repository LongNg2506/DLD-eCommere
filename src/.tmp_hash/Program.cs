using BCrypt.Net;

var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin123");
var staffHash = BCrypt.Net.BCrypt.HashPassword("Staff123");
var customerHash = BCrypt.Net.BCrypt.HashPassword("Customer123");

Console.WriteLine("Admin123    => " + adminHash);
Console.WriteLine("Staff123    => " + staffHash);
Console.WriteLine("Customer123 => " + customerHash);