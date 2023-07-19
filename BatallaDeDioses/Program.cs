using Personajes;
using ArchivosJson;
// agregar insultos al cominezo y luego cuando un jugador se vaya debilitando.
// mostrar como en wp
// agregar color a cada personaje
internal class Program
{


    private static void Main(string[] args)
    {
        string ArchivoJson = "personajes.json";
        string ArchivoNombres = "DatosPersonajes.txt";
        bool iniciar = false;


        var ListaDioses = new List<Personaje>();

        if (PersonajesJson.Existe(ArchivoJson)) //si existe y no esta vacio.
        {
            ListaDioses = PersonajesJson.LeerPersonajes(ArchivoJson);

        }
        else
        {
            CargarListaPersonajes(ListaDioses, ArchivoNombres);
            if (ListaDioses.Count > 0)
            {
                PersonajesJson.GuardarPersonajes(ArchivoJson, ListaDioses);
                iniciar = true;
            }
        }

        if (iniciar)
        {
            bool programaEnUso = true;
            int jugadorElegido;
            string inputJugador;

            while (programaEnUso)
            {
                MostrarLista(ListaDioses);
                Console.WriteLine("Elija un jugador: 1,2,...,10");
                do
                {
                    inputJugador = Console.ReadLine();
                } while (string.IsNullOrEmpty(inputJugador));

                bool resultado = int.TryParse(inputJugador, out jugadorElegido);

                if (resultado && (1 <= jugadorElegido && jugadorElegido <= 10))
                {
                    int indexPlayer1 = jugadorElegido - 1;
                    Personaje player1 = ListaDioses[indexPlayer1];
                    ListaDioses.RemoveAt(indexPlayer1);
                    bool player1EnJuego = true;
                    int i = 1;

                    while (ListaDioses.Count >= 1 && player1EnJuego)
                    {
                        int indexPlayer2 = FabricaPersonajes.ValorAleatorio(0, ListaDioses.Count);
                        Personaje player2 = ListaDioses[indexPlayer2];
                        ListaDioses.RemoveAt(indexPlayer2); // remuevo el jugador perdido

                        Console.WriteLine("-----Duelo Nro:{0}------\nPlayer 1:{1} VS Player 2:{2}\n", i, player1.Nombre, player2.Nombre);

                        while (player1.Salud > 0 && player2.Salud > 0) // Empieza pelea
                        {
                            RealizarAtaque(player1, player2); // mostrar ataque como en wp
                            if (player2.Salud > 0)
                            {
                                RealizarAtaque(player2, player1);
                            }
                        }

                        if (player1.Salud > 0)
                        {
                            player1.Salud = 100; // renueva vida para la proxima ronda
                            MejorarHabilidad(player1);
                            i++;
                            Console.WriteLine("Dios:{0} eliminado!", player2.Nombre);
                            Console.WriteLine("YOU WIN!\nCargando proxima pelea...");
                            Console.WriteLine();
                        }
                        else
                        {
                            player1EnJuego = false;
                            Console.WriteLine("YOU LOSE\n*****GAME OVER*****");
                        }
                    }
                    if (player1EnJuego)
                    {
                        Console.WriteLine("*********Felicidades Campeón!********\n"); // json lista de ganadores y mensaje
                        Console.WriteLine(player1.Frase);
                    }

                    Console.WriteLine("Jugar de nuevo: YES / NO");
                    String JugarDeNuevo = Console.ReadLine();
                    if (JugarDeNuevo == "no" || JugarDeNuevo == "NO" || JugarDeNuevo == "No")
                    {
                        programaEnUso = false;
                        Console.WriteLine("Saliendo...");
                    }
                    else
                    {
                        ListaDioses = PersonajesJson.LeerPersonajes(ArchivoJson);
                    }

                }
                else
                {
                    Console.WriteLine("Ingrese un jugador valido");
                }

            }
        }
        else
        {
            Console.WriteLine("No se pudo cargar el juego");
        }


    }

    private static void MejorarHabilidad(Personaje player1)
    {
        int habilidadMejora = FabricaPersonajes.ValorAleatorio(1, 6);

        switch (habilidadMejora)
        {
            case 1:
                if (player1.Velocidad < 10)
                {
                    player1.Velocidad += 1;
                }
                break;
            case 2:
                if (player1.Destreza < 5)
                {
                    player1.Destreza += 1;
                }
                break;
            case 3:
                if (player1.Fuerza < 10)
                {
                    player1.Fuerza += 1;
                }
                break;
            case 4:
                if (player1.Nivel < 10)
                {
                    player1.Nivel += 1;
                }
                break;
            case 5:
                if (player1.Armadura < 10)
                {
                    player1.Armadura += 1;
                }
                break;
        }
    }

    private static void CargarListaPersonajes(List<Personaje> Lista, string NombresPersonajes)
    {
        if (PersonajesJson.Existe(NombresPersonajes)) // Se utiliza para determinar si el archivo de nombres existe y no esta vacio
        {
            var lineas = File.ReadAllLines(NombresPersonajes);
            foreach (var linea in lineas)
            {
                string[] contenidoLinea = linea.Split(';');
                string nombre = contenidoLinea[0];
                string apodo = contenidoLinea[1];
                int nroTipo = int.Parse(contenidoLinea[2]);
                string frase = contenidoLinea[3];
                var Personaje = FabricaPersonajes.CrearPersonaje(nroTipo, nombre, apodo, frase);
                Lista.Add(Personaje);
            }
        }
        else
        {
            Console.WriteLine("No se pudo cargar personajes");
        }
    }


    private static void MostrarLista(List<Personaje> Lista)
    {
        Console.WriteLine("****** Listado de personajes *****");
        int i = 1;
        foreach (var Personaje in Lista)
        {
            Console.WriteLine("****** Datos *****");
            Console.WriteLine("Dios Nro:" + i);
            Console.WriteLine("Nombre:{0}|| Apodo:{1} || Edad:{2}", Personaje.Nombre, Personaje.Apodo, Personaje.Edad);
            Console.WriteLine("***** Caracteristicas *****");
            Console.WriteLine("Velocidad:{0} || Destreza:{1}\nFuerza:{2}|| Nivel:{3}|| Armadura:{4}\n", Personaje.Velocidad, Personaje.Destreza, Personaje.Fuerza, Personaje.Nivel, Personaje.Armadura);
            Console.WriteLine();
            i++;
        }
    }

    private static void RealizarAtaque(Personaje playerAtaca, Personaje playerDefiende)
    {
        int cteAjuste = 500;
        int Ataque = (playerAtaca.Destreza * playerAtaca.Fuerza * playerAtaca.Nivel);
        int Efectividad = FabricaPersonajes.ValorAleatorio(50, 101);
        int Defensa = playerDefiende.Armadura * playerDefiende.Velocidad;
        double DañoProvocado = ((Ataque * Efectividad) - Defensa) / (double)cteAjuste;
        playerDefiende.Salud = playerDefiende.Salud - DañoProvocado;
        if (playerDefiende.Salud < 0)
        {
            playerDefiende.Salud = 0;
        }
        Console.WriteLine("{0} ATACA A {1}", playerAtaca.Nombre, playerDefiende.Nombre);
        Console.WriteLine("Daño provocado a {0}:{1}\nSalud:{2}", playerDefiende.Apodo, DañoProvocado, playerDefiende.Salud);
        Console.WriteLine();
    }
}