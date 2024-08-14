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
                string query = "SELECT * FROM LIBRARYMODELS";
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

        // GET: api/LibraryModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LibraryModels>> GetLibraryModels(int id)
        {
            var libraryModels = await _context.LibraryModels.FindAsync(id);

            if (libraryModels == null)
            {
                return NotFound();
            }

            return libraryModels;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibraryModels(int id, LibraryModels libraryModels)
        {
            if (id != libraryModels.Id)
            {
                return BadRequest();
            }

            _context.Entry(libraryModels).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryModelsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LibraryModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LibraryModels>> PostLibraryModels(LibraryModels libraryModels)
        {
            _context.LibraryModels.Add(libraryModels);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibraryModels), new { id = libraryModels.Id }, libraryModels);
        }

        // DELETE: api/LibraryModels/5
        [HttpDelete("{id}")]
        public void Delete(int Id)
        {
            try
            {
                string DefaultConnection = "User=SYSDBA;Password=masterkey;Database=C:\\Work.Luquetti\\LIBRARYBD.FDB;DataSource=localhost;Port=3054";

                using (FbConnection connection = new(DefaultConnection))
                {
                    connection.Open();
                    string query = "DELETE FROM LIBRARYMODELS WHERE ID = @Id ";
                    using FbCommand command = new(query, connection);
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar bomba" + ex.Message);
            }
        }

    

        private bool LibraryModelsExists(int id)
        {
            return _context.LibraryModels.Any(e => e.Id == id);
        }
    }
}
