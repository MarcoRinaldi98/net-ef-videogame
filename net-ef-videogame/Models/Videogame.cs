using Microsoft.EntityFrameworkCore;
using net_ef_videogame.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net_ef_videogame.Models
{
    public class Videogame
    {
        // ATTRIBUTI
        public long VideogameId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }

        // relazione 1 a n con SoftwareHouse
        public long SoftwareHouseId { get; set; }
        public SoftwareHouse SoftwareHouse { get; set; }

        // METODI
        public override string ToString()
        {
            return @$"
Nome: {Name};
Software house: {SoftwareHouse.Name};
Data di Rilascio: {ReleaseDate.ToString("dd/MM/yyyy")};
Descrizione: {Overview};
            ";
        }

        public static Videogame SearchById(long id)
        {
            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    Videogame risultato = db.Videogames.Where(Videogames => Videogames.VideogameId == id).First();

                    return risultato;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    return null;
                }
            }
        }

        public static List<Videogame> GetVideogamesWithString(string name)
        {
            using (VideogameContext db = new VideogameContext())
            {
                List<Videogame> risultati = db.Videogames.Where(Videogame => Videogame.Name.Contains(name)).Include(Videogames => Videogames.SoftwareHouse).ToList();

                return risultati;
            }
        }

        public static void DeleteVideogame(long id)
        {
            using (VideogameContext db = new VideogameContext())
            {
                try
                {
                    Videogame risultato = db.Videogames.Where(Videogames => Videogames.VideogameId == id).First();
                    db.Remove(risultato);
                    db.SaveChanges();

                    Console.WriteLine($"Videogioco id numero:{id} è stato eliminato!");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Console.WriteLine($"Il videogioco con id numero {id} non esiste!");
                }
            }
        }
    }
}
