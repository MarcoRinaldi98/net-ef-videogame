using net_ef_videogame.Database;
using net_ef_videogame.Models;
using static Azure.Core.HttpHeader;

internal class Program
{
    private static void Main(string[] args)
    {
        // creo un menu di scelta per l'utente
        Console.WriteLine("Benvenuto nel nostro sistema di gestione videogiochi!");

        while (true)
        {
            Console.WriteLine(@"
- 1: Inserire un nuovo videogioco;
- 2: Ricercare un videogioco per id;
- 3: Ricercare tutti i videogiochi aventi il nome contenente una determinata stringa inserita in input;
- 4: Inserisci una nuova software house;
- 5: Stampa tutti i videogiochi prodotti da una software house;
- 6: Cancellare un videogioco;
- 7: Chiudere il programma;
");

            Console.Write("Seleziona l'opzione desiderata: ");

            int selectedOption = int.Parse(Console.ReadLine());

            switch (selectedOption)
            {
                // Inserire un nuovo videogioco;
                case 1:
                    Console.WriteLine("Inserisci i dati del videogioco: ");
                    Console.Write("Inserisci il nome del videogioco: ");
                    string name = Console.ReadLine();

                    Console.Write("Inserisci la descrizione del videogioco: ");
                    string description = Console.ReadLine();

                    Console.Write("Inserisci la data di rilascio del videogioco (dd/mm/yyyy): ");
                    DateTime releaseDate = DateTime.Parse(Console.ReadLine());

                    Console.Write("Inserisci l'ID della Software House del videogioco: ");
                    long softwareHouseId = long.Parse(Console.ReadLine());

                    Videogame newVideogame = new Videogame()
                    {
                        Name = name,
                        Overview = description,
                        ReleaseDate = releaseDate,
                        SoftwareHouseId = softwareHouseId
                    };

                    using (VideogameContext db = new VideogameContext())
                    {
                        try
                        {
                            db.Add(newVideogame);
                            db.SaveChanges();

                            Console.WriteLine("Il videogioco è stato aggiunto!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Errore nell'aggiunta del videogioco!");
                        }
                    }
                    break;
                // Ricercare un videogioco per id;
                case 2:
                    Console.Write("Inserisci l'id del videogioco da cercare: ");
                    long idToSearch = long.Parse(Console.ReadLine());

                    Videogame videogameSerched = Videogame.SearchById(idToSearch);

                    if (videogameSerched == null)
                    {
                        Console.WriteLine($"Il videogioco con ID {idToSearch} non esiste!");
                    }
                    else
                    {
                        Console.WriteLine($"Il videogioco con ID {idToSearch} è: ");
                        Console.WriteLine($"- {videogameSerched}");
                    }
                    Console.WriteLine();
                    break;
                // Ricercare tutti i videogiochi aventi il nome contenente una determinata stringa inserita in input;
                case 3:
                    Console.Write("Inserisci la stringa: ");
                    string stringToSearch = Console.ReadLine();

                    List<Videogame> videogames = Videogame.GetVideogamesWithString(stringToSearch);

                    if (videogames.Count > 0)
                    {
                        Console.WriteLine($"Ecco la lista dei videogiochi che contengono \"{stringToSearch}\" nel nome:");
                        for (int i = 0; i < videogames.Count; i++)
                        {
                            Console.WriteLine(@$"
- {i + 1}:
{videogames[i]}
                    ");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Non ci sono videogiochi che contengono \"{stringToSearch}\" nel nome!");
                    }
                    Console.WriteLine();
                    break;
                // Inserisci una nuova software house;
                case 4:
                    Console.WriteLine("Inserisci i dati della Software House: ");
                    Console.Write("Inserisci il nome della software house: ");
                    string nameSoftwareHouse = Console.ReadLine();

                    Console.Write("Inserisci il taxID della software house: ");
                    string taxId = Console.ReadLine();

                    Console.Write("Inserisci la città della software house: ");
                    string city = Console.ReadLine();

                    Console.Write("Inserisci la nazione della software house: ");
                    string country = Console.ReadLine();

                    SoftwareHouse newSoftwareHouse = new SoftwareHouse()
                    {
                        Name = nameSoftwareHouse,
                        TaxId = taxId,
                        City = city,
                        Country = country
                    };

                    using (VideogameContext db = new VideogameContext())
                    {
                        try
                        {
                            db.Add(newSoftwareHouse);
                            db.SaveChanges();

                            Console.WriteLine("La Software House è stata aggiunta!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("Errore nell'aggiunta della software house!");
                        }
                    }
                    break;
                // Stampa tutti i videogiochi prodotti da una software house;
                case 5:
                    {
                        Console.Write("Inserisci l'id della software house della quale ne vuoi vedere i videogiochi: ");
                        if (long.TryParse(Console.ReadLine(), out long idSoftwareHouse))
                        {
                            using (VideogameContext db = new VideogameContext())
                            {
                                List<Videogame> gamesBySoftwareHouse = db.Videogames.Where(vg => vg.SoftwareHouseId == idSoftwareHouse).ToList();

                                foreach (var game in gamesBySoftwareHouse)
                                {
                                    Console.WriteLine($"- {game.Name} ({game.ReleaseDate})");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Valore non valido. Inserisci un numero intero per l'ID della Software House!");
                        }
                    }
                    break;
                // Cancellare un videogioco;
                case 6:
                    Console.Write("Inserisci l'id del videogioco che vuoi eliminare: ");
                    long idVideogameToDelete = long.Parse(Console.ReadLine());

                    Videogame.DeleteVideogame(idVideogameToDelete);
                    break;
                // Chiudere il programma;
                case 7:
                    Environment.Exit(0);
                    break;
                // In caso di errore di inserimento del numero visualizzo messaggio di errore e faccio ripetere la selezione all'utente
                default:
                    Console.WriteLine("Non hai selezionato un opzione valida!");
                    break;
            }
        }

    }
}