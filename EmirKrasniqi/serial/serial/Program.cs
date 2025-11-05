using serial;
using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        Character gerard = new Character
        {
            FirstName = "Gérard",
            LastName = "Dupont",
            Description = "Cuisinier passionné mais un peu têtu. Dirige la cafétéria de l'école avec Maya.",
            PlayedBy = null
        };
        Actor Maya = new Actor
        {
            FirstName = "Maya",
            LastName = "Dupont",
            BirthDate = DateTime.Now,
            Country = "Italy",
            IsAlive = true,
        };

        // Sérialisation en JSON
        string jsonPerson = JsonSerializer.Serialize(gerard, new JsonSerializerOptions { WriteIndented = true });
        string jsonActor = JsonSerializer.Serialize(gerard, new JsonSerializerOptions { WriteIndented = true });

        // Sauvegarde dans un fichier 
        File.WriteAllText("./personnages/gerard.json", jsonPerson);
        File.WriteAllText("./personnages/maya.json", jsonActor);

        Console.WriteLine("Fichier JSON créé : gerard.json");
        Console.WriteLine("Fichier JSON créé : maya.json");
        Console.WriteLine(jsonPerson);
        Console.WriteLine(jsonActor);

        // Récuperer les fichier des autres et les déserialiser
        string folderPath = "./personnages";
        string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");

        List<Character> characters = new List<Character>();
        List<Actor> actors = new List<Actor>();

        foreach (string file in jsonFiles)
        {
            string content = File.ReadAllText(file);
            Character? person = JsonSerializer.Deserialize<Character>(content);
            Actor? actor = JsonSerializer.Deserialize<Actor>(content);

            if (person != null )
            {
                characters.Add(person);
                Console.WriteLine($"Chargé : {person.FirstName} {person.LastName} - {person.Description}");
            }
            if (actor != null )
            {
                actors.Add(actor);
                Console.WriteLine($"Chargé : {actor.FirstName} {actor.LastName} born at {actor.BirthDate} in {actor.Country} is alive {actor.IsAlive} ");
            }
        }
        Console.WriteLine($"\nNombre total de personnages : {characters.Count}");

    }
}
