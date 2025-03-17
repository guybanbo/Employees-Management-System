using BlazorApp1.Models;
using Microsoft.AspNetCore.Components.Forms;



namespace BlazorApp1.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task SaveToCsvAsync(List<Employee> employees, string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, fileName);

            var lines = employees.Select(f => $"{f.ID},{f.Name},{f.Title},{f.Salary},{f.ProfileImage}");
            await File.WriteAllLinesAsync(filePath, new[] { "ID,Name,Title,Salary,Profile Image" }.Concat(lines));
        }

        public async Task FileUpload(String ID, IBrowserFile? browserFile)
        {
            const int MAX_FILESIZE = 5000 * 1024; // 2 MB
            if (browserFile != null)
            {
                try
                {
                    var fileStream = browserFile.OpenReadStream(MAX_FILESIZE);
                    var newFile = ID;
                    var extension = Path.GetExtension(browserFile.Name);
                    var targetFilePath = Path.ChangeExtension(newFile, extension);//adding extension to newFile
                    //var finalPath = Path.Combine(config.GetValue<string>("FileStorage")!, targetFilePath);
                    var finalPath = Path.Combine("wwwroot/images"!, targetFilePath);
                    var destinationStream = new FileStream(finalPath, FileMode.Create);
                    await fileStream.CopyToAsync(destinationStream);
                    destinationStream.Close();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("upload failed");
                }
            }
        }

        public async Task<List<Employee>> LoadEmployees(string filePath)
        {
            var employeeList = new List<Employee>();

            try
            {
                // Read all lines from the CSV file
                var csvLines = await File.ReadAllLinesAsync(filePath);

                foreach (var line in csvLines.Skip(1)) // Skip header row
                {
                    var columns = line.Trim().Split(',');

                    if (columns.Length >= 5 && float.TryParse(columns[3], out var number))
                    {
                        employeeList.Add(new Employee
                        {
                            ID = columns[0],
                            Name = columns[1],
                            Title = columns[2],
                            Salary = number,
                            ProfileImage = columns[4]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV file: {ex.Message}");
            }

            return employeeList;
        }

    }
}
