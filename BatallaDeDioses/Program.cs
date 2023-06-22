using GeneracionPersonajes;

internal class Program
{

    
    static string[] NombreDioses = {"Thor","Zeus","Poseidon","Hercules","Shiva","Hades","Belzebu","Odin","Apolo","Anubis"};
    static string[] ApodoDioses = {"Dios del trueno","Dios de dioses","Rey de los mares","El hijo de Zeus","El destructor","El Señor del Inframundo","Principe de los demonios","El padre de todos","Dios del sol","El guardian de las almas"};
    static int cantidadJugadores = NombreDioses.Length;
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
                CargarListaPersonajes(cantidadJugadores,ListaDioses,TipoPersonaje.Dios,NombreDioses,ApodoDioses);
                PersonajesJson.GuardarPersonajes(ArchivoJson,ListaDioses);    
            }
        }else
        {
            CargarListaPersonajes(cantidadJugadores,ListaDioses,TipoPersonaje.Dios,NombreDioses,ApodoDioses);
            PersonajesJson.GuardarPersonajes(ArchivoJson,ListaDioses);
        }
        
        bool programaEnUso= true;
        int jugadorElegido;
        string inputJugador;

        while (programaEnUso)
        {
            Console.WriteLine("Elija un jugador: 1,2,...,10");
            MostrarLista(ListaDioses);
            do
            {
                inputJugador= Console.ReadLine();
            }while(string.IsNullOrEmpty(inputJugador));

            bool resultado = int.TryParse(inputJugador, out jugadorElegido);

            if (resultado && (1<=jugadorElegido && jugadorElegido <=10))
            {
                int indexPlayer1 = jugadorElegido - 1;
                Personaje player1 = ListaDioses[indexPlayer1];
                ListaDioses.RemoveAt(indexPlayer1);
                bool player1EnJuego = true;
                int i=1;
                while (ListaDioses.Count>=1 && player1EnJuego)
                {
                    int indexPlayer2 = FabricaPersonajes.ValorAleatorio(0,ListaDioses.Count);
                    Personaje player2 = ListaDioses[indexPlayer2];

                    Console.WriteLine("-----Duelo Nro:{0}------\nPlayer 1:{1} VS Player 2:{2}\n",i,player1.Nombre,player2.Nombre);

                    while (player1.Salud>0 && player2.Salud>0) // Empieza pelea
                    {
                        RealizarAtaque(player1,player2);
                        if (player2.Salud>0)
                        {
                            RealizarAtaque(player2,player1);
                        }
                    }

                    if (player1.Salud>0)
                    {
                        player1.Salud = 100; // renueva vida para la proxima ronda
                        ListaDioses.RemoveAt(indexPlayer2);
                        Console.WriteLine("Dios:{0} eliminado!",player2.Nombre);
                        Console.WriteLine("YOU WIN!\nCargando proxima pelea...");
                        Console.WriteLine();
                        //var aleatoria para aumentar poder
                        player1.Armadura +=100;
                        i++;
                        
                    }else
                    {
                        player1EnJuego = false;
                        Console.WriteLine("YOU LOSE\n*****GAME OVER*****");
                    }
                }
                if (player1EnJuego)
                {
                    Console.WriteLine("*********Felicidades Campeón!********\n");
                }
                
                Console.WriteLine("Jugar de nuevo: YES / NO");
                String JugarDeNuevo = Console.ReadLine();
                if (JugarDeNuevo == "no" || JugarDeNuevo == "NO" || JugarDeNuevo =="No")
                {
                    programaEnUso = false;
                    Console.WriteLine("Saliendo...");
                }else
                {
                    ListaDioses=PersonajesJson.LeerPersonajes(ArchivoJson);
                }
                
            }else
            {
                Console.WriteLine("Ingrese un jugador valido");
            }

        }
    }

    private static void CargarListaPersonajes(int cantidadJugadores, List<Personaje> Lista,TipoPersonaje Tipo, string[] arregloNombres, string[] ArregloApodos)
    {
        for (int i = 0; i < cantidadJugadores; i++)
        {
            var Personaje = FabricaPersonajes.CrearPersonaje(Tipo,arregloNombres[i],ArregloApodos[i]);
            Lista.Add(Personaje);
        }
    }
    
    private static void MostrarLista(List<Personaje> Lista)
    {
        Console.WriteLine("****** Listado de personajes *****");
        int i=1;
        foreach (var Personaje in Lista)
        {
            Console.WriteLine("****** Datos *****");
            Console.WriteLine("Dios Nro:"+i);
            Console.WriteLine("Nombre:{0}|| Apodo:{1} || Edad:{2}", Personaje.Nombre, Personaje.Apodo, Personaje.Edad);
            Console.WriteLine("***** Caracteristicas *****");
            Console.WriteLine("Velocidad:{0} || Destreza:{1}\nFuerza:{2}|| Nivel:{3}|| Armadura:{4}\n", Personaje.Velocidad, Personaje.Destreza, Personaje.Fuerza, Personaje.Nivel, Personaje.Armadura);
            Console.WriteLine();
            i++;
        }
    }

    private static void RealizarAtaque(Personaje playerAtaca,Personaje playerDefiende)
    {
        int cteAjuste = 500;
        int Ataque = (playerAtaca.Destreza*playerAtaca.Fuerza*playerAtaca.Nivel);
        int Efectividad = FabricaPersonajes.ValorAleatorio(50,100);
        int Defensa = playerDefiende.Armadura*playerDefiende.Velocidad;
        double DañoProvocado = ((Ataque*Efectividad)-Defensa)/(double)cteAjuste;
        playerDefiende.Salud = playerDefiende.Salud - DañoProvocado;
        if (playerDefiende.Salud < 0)
        {
            playerDefiende.Salud = 0;
        }
        Console.WriteLine("{0} ATACA A {1}", playerAtaca.Nombre,playerDefiende.Nombre);
        Console.WriteLine("Daño provocado a {0}:{1}\nSalud:{2}",playerDefiende.Apodo,DañoProvocado,playerDefiende.Salud);
        Console.WriteLine();
    }
}