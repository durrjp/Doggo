
using Doggo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Doggo.Repositories
{
    public class DogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogsWithOwner(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT d.Id AS DogId, d.[Name] AS DogName, OwnerId, Breed, Notes, ImageURL
                        FROM Dog d
                        JOIN Owner o ON d.OwnerId = o.Id
                        WHERE o.Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> owners = new List<Dog>();
                    while (reader.Read())
                    {
                        string notes = "";
                        string image = "";
                        if(!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if(!reader.IsDBNull(reader.GetOrdinal("ImageURL")))
                        {
                            image = reader.GetString(reader.GetOrdinal("ImageURL"));
                        }
                        Dog owner = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Name = reader.GetString(reader.GetOrdinal("DogName")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = notes,
                            ImageURL = image
                        };

                        owners.Add(owner);
                    }

                    reader.Close();

                    return owners;
                }
            }
        }
    }
}
