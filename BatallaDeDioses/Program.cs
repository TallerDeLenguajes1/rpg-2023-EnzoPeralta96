using Personajes;
using ArchivosJson;
using ConsumoAPI;

// agregar insultos al cominezo y luego cuando un jugador se vaya debilitando.
// mostrar como en wp
internal class Program
{
    private static void Main(string[] args)
    {
        string ArchivoJson = "Json/personajes.json";
        string ArchivoNombres = "Personajes/DatosPersonajes.txt";
        string ArchivoTitulo = "ArchivosTexto/NombreJuego.txt";
        string IntroJuego = "ArchivosTexto/IntroJuego.txt";

        var ListaDioses = new List<Personaje>();


        if (IniciarJuego(ArchivoJson, ArchivoNombres, ref ListaDioses))
        {
            bool programaEnUso = true;
            int jugadorElegido;
            string inputJugador;

            while (programaEnUso)
            {
                MostrarIntro(IntroJuego);
                MostrarTitulo(ArchivoTitulo);
                MostrarListaEnCuadros(ListaDioses);

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
                        Personaje player2 = ListaDioses[indexPlayer2]; //elijo el rival
                        ListaDioses.RemoveAt(indexPlayer2); // remuevo el jugador perdido

                        Console.WriteLine("-----Duelo Nro:{0}------\nPlayer 1:{1} VS Player 2:{2}\n", i, player1.Nombre, player2.Nombre);//presento jugadores
                        Console.WriteLine("Player 1:{0}\nPlayer 2:{1}\n", MiConsumoAPI.ObtnerInsulto(), MiConsumoAPI.ObtnerInsulto()); // se bardeam

                        while (player1.Salud > 0 && player2.Salud > 0) // Empieza pelea
                        {
                            RealizarAtaque(player1, player2); // mostrar ataque como en wp
                            if (player2.Salud > 0)
                            {
                                RealizarAtaque(player2, player1);
                            }
                        }

                        if (player1.Salud > 0) // si gana el player1
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
                            Console.WriteLine("YOU LOSE\n|GAME OVER|");
                        }
                    }

                    if (player1EnJuego)
                    {
                        Console.WriteLine("|Felicidades Campeón!|\n"); // json lista de ganadores y mensaje
                        Console.WriteLine(player1.Frase);
                    }

                    Console.WriteLine("Para jugar de nuevo: presione una tecla");
                    Console.WriteLine("Presione 'Esc' para salir.");

                    ConsoleKeyInfo JugarDeNuevo = Console.ReadKey();

                    if (JugarDeNuevo.Key == ConsoleKey.Escape)
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

    private static bool IniciarJuego(string ArchivoJson, string ArchivoNombres, ref List<Personaje> ListaDioses)
    {
        bool iniciar = false;
        if (PersonajesJson.Existe(ArchivoJson)) //si existe y no esta vacio.
        {
            ListaDioses = PersonajesJson.LeerPersonajes(ArchivoJson);
            iniciar = true;
        }
        else
        {
            if (ExisteArchivo(ArchivoNombres))//si existe y no esta vacio.
            {
                CargarListaPersonajes(ListaDioses, ArchivoNombres);
                PersonajesJson.GuardarPersonajes(ArchivoJson, ListaDioses);
                iniciar = true;
            }
        }
        return iniciar;
    }

    private static void CargarListaPersonajes(List<Personaje> Lista, string NombresPersonajes)
    {
        var lineas = File.ReadAllLines(NombresPersonajes);
        foreach (var linea in lineas)
        {
            string[] contenidoLinea = linea.Split(';');
            string nombre = contenidoLinea[0];
            string apodo = contenidoLinea[1];
            int nroTipo = int.Parse(contenidoLinea[2]);
            string frase = contenidoLinea[3];
            ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), contenidoLinea[4]);
            var Personaje = FabricaPersonajes.CrearPersonaje(nroTipo, nombre, apodo, frase, color);
            Lista.Add(Personaje);
        }
    }
    private static void MostrarIntro(string IntroJuego)
    {
        if (ExisteArchivo(IntroJuego))
        {
            using (var intro = new StreamReader(IntroJuego))
            {
                while (!intro.EndOfStream)
                {
                    string lineaIntro = intro.ReadLine();
                    foreach (var caracter in lineaIntro)
                    {
                        Console.Write(caracter);
                        Thread.Sleep(15);
                    }
                }
                intro.Close();
                Console.WriteLine();
            }
        }
    }

    private static void MostrarTitulo(string ArchivoTitulo)
    {
        if (ExisteArchivo(ArchivoTitulo))
        {
            using (var tituloJuego = new StreamReader(ArchivoTitulo))
            {
                while (!tituloJuego.EndOfStream)
                {
                    string linea = tituloJuego.ReadLine();
                    Console.WriteLine(linea);
                }
                tituloJuego.Close();
                Console.WriteLine();
            }
        }
    }

    

    private static void MostrarListaEnCuadros(List<Personaje> Lista)
    {
        Console.WriteLine("|| Listado de personajes ||");
        int i = 1;
        int recuadroAncho = 35; // Ancho del recuadro (ajústalo según sea necesario)
        int anchoNombre = Lista.Max(p => p.Nombre.Length); // Calculamos el ancho máximo del nombre

        foreach (var personaje in Lista)
        {
            Console.ForegroundColor = personaje.Color;
            // Encabezado del recuadro
            Console.WriteLine(new string('=', recuadroAncho));
            Console.WriteLine("|" + CentrarTexto("DATOS", recuadroAncho - 2) + "|");
            Console.WriteLine(new string('=', recuadroAncho));

            // Datos del personaje
            Console.WriteLine("| Dios Nro: {0,-3} {1}    |", i, new string(' ', recuadroAncho - 21));
            Console.WriteLine("| Nombre: {0,-" + anchoNombre + "} {1} |", personaje.Nombre, new string(' ', recuadroAncho - 13 - anchoNombre));
            Console.WriteLine(new string('=', recuadroAncho));
            Console.WriteLine("|" + CentrarTexto("CARACTERISTICAS", recuadroAncho - 2) + "|");
            Console.WriteLine(new string('=', recuadroAncho));
            Console.WriteLine("| Velocidad: {0,-3} {1}       |", personaje.Velocidad, new string(' ', recuadroAncho - 25));
            Console.WriteLine("| Destreza: {0,-3} {1}       |", personaje.Destreza, new string(' ', recuadroAncho - 24));
            Console.WriteLine("| Fuerza: {0,-3} {1}       |", personaje.Fuerza, new string(' ', recuadroAncho - 22));
            Console.WriteLine("| Nivel: {0,-3} {1}       |", personaje.Nivel, new string(' ', recuadroAncho - 21));
            Console.WriteLine(new string('=', recuadroAncho));
            Console.WriteLine();

            Console.ResetColor();
            i++;
        }
        Console.WriteLine("Elija un jugador: 1,2,...,10");
    }

    private static string CentrarTexto(string text, int width)
    {
        if (text.Length >= width)
        {
            return text;
        }
        else
        {
            return text.PadLeft((width + text.Length) / 2).PadRight(width);
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
        Console.ForegroundColor = playerAtaca.Color;
        Console.WriteLine("{0} ATACA A {1}", playerAtaca.Nombre, playerDefiende.Nombre);
        if (10<= playerDefiende.Salud && playerDefiende.Salud <= 40 )
        {   
            Console.ForegroundColor = playerDefiende.Color;
            Console.WriteLine("Player 2:{0}", MiConsumoAPI.ObtnerInsulto());
            Console.ForegroundColor = playerAtaca.Color;
        }
        Console.WriteLine("Daño provocado a {0}:{1}\nSalud:{2}", playerDefiende.Apodo, DañoProvocado, playerDefiende.Salud);
        Console.WriteLine();
       
        Console.ResetColor();
        

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
    private static bool ExisteArchivo(string Archivo)
    {
        if (File.Exists(Archivo))
        {
            var info = new FileInfo(Archivo);
            if (info.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}