using GeneracionPersonajes;

internal class Program
{

    const int cantJugadoresXteam = 10;
    static string[] NombreDioses = {"Thor","Zeus","Poseidon","Hercules","Shiva","Hades","Belzebu","Odin","Apolo","Anubis"};
    static string[] ApodoDioses = {"Dios del trueno","Dios de dioses","Rey de los mares","El hijo de Zeus","El destructor","El Señor del Inframundo","Principe de los demonios","El padre de todos","Dios del sol","El guardian de las almas"};
    /*static string[] NombreHumanos = {"Lu-bu","Adan","Kojiro Sasaki","Jack","Raiden Tameemon","Nikola Tesla","Michael Nostradamus","Rey Leonidas"};
    static string[] ApodoHumanos = {"El Dios de la Guerra","Padre de la humanidad","El Demonio de las Espadas","El destripador","Rikishi sin igual","El Genio de la Electricidad","El Profeta de los Siglos","El Leon Espartano"};*/
     private static void Main(string[] args)
    {
        string ArchivoJson = "personajes.json";
        var ListaDioses = new List<Personaje>();

        if(PersonajesJson.Existe(ArchivoJson))
        {
            ListaDioses=PersonajesJson.LeerPersonajes(ArchivoJson);

            if (ListaDioses.Count==0)
            {
                CargarListaPersonajes(cantJugadoresXteam,ListaDioses,TipoPersonaje.Dios,NombreDioses,ApodoDioses);
                PersonajesJson.GuardarPersonajes(ArchivoJson,ListaDioses);    
            }
        }else
        {
            CargarListaPersonajes(cantJugadoresXteam,ListaDioses,TipoPersonaje.Dios,NombreDioses,ApodoDioses);
            PersonajesJson.GuardarPersonajes(ArchivoJson,ListaDioses);
        }
        MostrarLista(ListaDioses);

    }

    private static void MostrarLista(List<Personaje> Lista)
    {
        Console.WriteLine("****** Listado de personajes *****");
        foreach (var Personaje in Lista)
        {
            Console.WriteLine("****** Datos *****");
            Console.WriteLine("Nombre:{0}|| Apodo:{1} || Edad:{2}", Personaje.Nombre, Personaje.Apodo, Personaje.Edad);
            Console.WriteLine("***** Caracteristicas *****");
            Console.WriteLine("-Velocidad:{0}\n-Destreza:{1}\n-Fuerza:{2}\n-Nivel:{3}\n-Armadura:{4}\n-Salud:{5}", Personaje.Velocidad, Personaje.Destreza, Personaje.Fuerza, Personaje.Nivel, Personaje.Armadura, Personaje.Salud);
            Console.WriteLine();
        }
    }

    private static void CargarListaPersonajes(int cantJugadoresXteam, List<Personaje> Lista,TipoPersonaje Tipo, string[] arregloNombres, string[] ArregloApodos)
    {
        for (int i = 0; i < cantJugadoresXteam; i++)
        {
            var Personaje = FabricaPersonajes.CrearPersonaje(Tipo,arregloNombres[i],ArregloApodos[i]);
            Lista.Add(Personaje);
        }
    }
  
}