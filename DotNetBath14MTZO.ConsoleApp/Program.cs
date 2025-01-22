// See https://aka.ms/new-console-template for more information

using DotNetBath14MTZO.ConsoleApp;

BlogService blogService = new BlogService();
 blogService.Read();

//await blogService.GetBlog("129a89f9-6140-46fb-9a1f-7e3396606550");
Console.ReadLine();