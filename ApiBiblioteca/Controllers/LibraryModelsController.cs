using ApiBiblioteca.Db;
using ApiBiblioteca.Models;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using NuGet.Protocol.Plugins;

namespace ApiBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryModelsController : ControllerBase
    {
        private readonly TodoContexto _context;

        public LibraryModelsController(TodoContexto context)
        {
            _context = context;
        }


        [HttpGet]
        public List<LibraryModels> GetLibraryModels()
        {
            string DefaultConnection = "User = SYSDBA; Password = masterkey; database = C:\\Work.Luquetti\\LIBRARYBD.FDB; DataSource = localhost; Port = 3054";
            List<LibraryModels> libraryModelsList = [];
            using (FbConnection connection = new(DefaultConnection))
            {
                connection.Open();
                string query = "SELECT * FROM LIBRARYMODELS2";
                using FbCommand command = new(query, connection);
                using (FbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LibraryModels model = new()
                        {
                            Id = reader.GetInt32(0),
                            BookName = reader.GetString(1),
                            IsRent = reader.GetBoolean(2),
                        };
                        libraryModelsList.Add(model);
                    }
                }
            }
            return libraryModelsList;
        }

        
        [HttpGet("{Id}")]
        public LibraryModels GetById(int Id)
        {
            var book = new LibraryModels();
            try
            {
                string DefaultConnection = "User=SYSDBA;Password=masterkey;Database=C:\\Work.Luquetti\\LIBRARY.FDB;DataSource=localhost;Port=3054";
                using FbConnection connection = new FbConnection(DefaultConnection);
                connection.Open();
                string query = "SELECT * FROM LIBRARYMODELS2 WHERE ID=@Id";
                using FbCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Id", Id);
              using (FbDataReader reader=command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        book.Id = Convert.ToInt32(reader["Id"]);
                        book.BookName = (string)reader["BookName"];
                        book.IsRent = (bool)reader["IsRent"];
                    }
                }
            return book;
            }catch(Exception ex)
            {
                throw new Exception("Erro ao buscar item");
                return null;
            }
        }

       


        [HttpPut("{Id}")]
        public void Update (LibraryModels libraryModels)
        {
            try
            {
                string DefaultConnection = "User=SYSDBA;Password=masterkey;Database=C:\\Work.Luquetti\\LIBRARYBD.FDB;DataSource=localhost;Port=3054";
                using FbConnection connection = new(DefaultConnection);
                connection.Open();
                string query = "UPDATE LIBRARYMODELS2 SET BOOKNAME=@BookName,ISRENT= @IsRent WHERE ID = @Id";
                using FbCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Id", libraryModels.Id);
                command.Parameters.AddWithValue("BookName", libraryModels.BookName);
                command.Parameters.AddWithValue("@IsRent", libraryModels.IsRent);
                command.ExecuteNonQuery();
            }
            catch(Exception ex) 
            {
                throw new Exception("Erro ao alterar item");
            }
        }

        [HttpPost]
        public void Adicionar(LibraryModels libraryModels)
        {
            try
            {
                string DefaultConnection = "User=SYSDBA;Password=masterkey;Database=C:\\Work.Luquetti\\LIBRARYBD.FDB;DataSource=localhost;Port=3054";
                using FbConnection connection = new(DefaultConnection);
                connection.Open();
                string query = "INSERT INTO LIBRARYMODELS2(BOOKNAME,ISRENT) VALUES(@BookName,@IsRent)";
                using FbCommand command = new(query, connection);
                command.Parameters.AddWithValue("@BookName", libraryModels.BookName);
                command.Parameters.AddWithValue("@IsRent", libraryModels.IsRent);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir item" + ex.Message);
            }
        }

        [HttpDelete("{Id}")]
        public void Delete(int Id)
        {
            try
            {
                string DefaultConnection = "User=SYSDBA;Password=masterkey;Database=C:\\Work.Luquetti\\LIBRARYBD.FDB;DataSource=localhost;Port=3054";

                using (FbConnection connection = new(DefaultConnection))
                {
                    connection.Open();
                    string query = "DELETE FROM LIBRARYMODELS2 WHERE ID = @Id ";
                    using FbCommand command = new(query, connection);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar item" + ex.Message);
            }
        }



        private bool LibraryModelsExists(int id)
        {
            return _context.LibraryModels.Any(e => e.Id == id);
        }
    }
}
