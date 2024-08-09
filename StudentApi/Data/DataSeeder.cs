using CsvHelper;
using CsvHelper.Configuration;
using StudentApi.Models;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StudentApi.Data
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public DataSeeder(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SeedData()
        {
            if (!_context.Students.Any())
            {
                var csvFilePath = _configuration["CsvFilePath"];
                var students = GetStudentsFromCsv(csvFilePath);
                await _context.Students.AddRangeAsync(students);
            }

            if (!_context.Users.Any())
            {
                var user = new User
                {
                    Username = "admin",
                    Password = "admin"
                };
                await _context.Users.AddAsync(user);
            }

            await _context.SaveChangesAsync();
        }

        private Student[] GetStudentsFromCsv(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                var records = csv.GetRecords<CsvStudent>().ToArray();
                var students = records.Select((record, index) => new Student
                {
                    Nome = record.Nome,
                    Idade = record.Idade,
                    Serie = record.Serie,
                    NotaMedia = record.NotaMedia,
                    Endereco = record.Endereco,
                    NomePai = record.NomePai,
                    NomeMae = record.NomeMae,
                    DataNascimento = record.DataNascimento
                }).ToArray();
                return students;
            }
        }

        private class CsvStudent
        {
            public string Nome { get; set; }
            public int Idade { get; set; }
            public int Serie { get; set; }
            public double NotaMedia { get; set; }
            public string Endereco { get; set; }
            public string NomePai { get; set; }
            public string NomeMae { get; set; }
            public DateTime DataNascimento { get; set; }
        }
    }
}