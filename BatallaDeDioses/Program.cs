using Personajes;
using ArchivosJson;
using ConsumoAPI;
namespace Juego;

internal class Program
{   
    private static void Main(string[] args)
    {
        string ArchivoJson = "Json/personajes.json";
        string ArchivoNombres = "Personajes/DatosPersonajes.txt";
        string ArchivoTitulo = "ArchivosTexto/NombreJuego.txt";
        string IntroJuego = "ArchivosTexto/IntroJuego.txt";
        string ArchivoGanadores = "Json/ganadores.json";
        string YouLose = "ArchivosTexto/Perdiste.txt";
        string FinJuego = "ArchivosTexto/FinJuego.txt";
        string YouWin = "ArchivosTexto/Ganaste.txt";
        string ArchivoElegir = "ArchivosTexto/ElegirJugador.txt";

        var ListaDioses = new List<Personaje>();

        if (IniciarJuego(ArchivoJson, ArchivoNombres, ref ListaDioses))
        {
            bool programaEnUso = true;
            int jugadorElegido;
            string inputJugador;

            Helper.MostrarIntro(IntroJuego);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Helper.MostrarTitulo(ArchivoTitulo);
            Console.ResetColor();

            Helper.MostrarListaEnCuadros(ListaDioses);
            Helper.MostrarTitulo(ArchivoElegir);

            while (programaEnUso)
            {
                do
                {
                    Console.WriteLine("\nPresione 1,2,...,10 para elegir");
                    inputJugador = Console.ReadLine();
                } while (string.IsNullOrEmpty(inputJugador));

                bool resultado = int.TryParse(inputJugador, out jugadorElegido);

                if (resultado && IsValidPlayer(jugadorElegido))
                {
                    int indexPlayer1 = jugadorElegido - 1; // indice del jugador elegido
                    Personaje player1 = ListaDioses[indexPlayer1]; //guardar jugador 
                    ListaDioses.RemoveAt(indexPlayer1);//y quitar de la lista
                    Console.WriteLine("Dios elegido:" + player1.Nombre);

                    bool player1EnJuego = true;
                    int i = 1; //contador de peleas

                    while (ListaDioses.Count >= 1 && player1EnJuego)
                    {
                        int indexPlayer2 = FabricaPersonajes.ValorAleatorio(0, ListaDioses.Count);// indice aleatorio para rival
                        Personaje player2 = ListaDioses[indexPlayer2]; //elijo el rival
                        ListaDioses.RemoveAt(indexPlayer2); // remuevo el jugador perdido

                        Helper.PresentacionBatalla(i, player1, player2);
                        Console.WriteLine();

                        //consumo API
                        Console.ForegroundColor = player1.Color;
                        Console.WriteLine("{0}:{1}", player1.Nombre, MiConsumoAPI.ObtnerInsulto());
                        Console.ResetColor();

                        Console.ForegroundColor = player2.Color;
                        Console.WriteLine("\t\t\t\t\t\t\t\t\t\t{0}:{1}", player2.Nombre, MiConsumoAPI.ObtnerInsulto());
                        Console.ResetColor();

                        int k = 2;//variable para determinar posición de donde se muestra el ataque

                        while (PlayersAlives(player1, player2)) // Empieza pelea
                        {
                            RealizarAtaque(player1, player2, k); // mostrar ataque como chat, para eso sirve K
                            k++;
                            if (player2.IsALive())
                            {
                                RealizarAtaque(player2, player1, k);
                                k++;
                            }
                        }

                        if (player1.IsALive()) // si gana el player1
                        {
                            FabricaPersonajes.MejorarHabilidad(player1); //renueva vida y mejorar hab.

                            i++; // indice proxima pelea

                            Console.WriteLine("Dios:{0} eliminado!", player2.Nombre);
                            Helper.MostrarTitulo(YouWin);

                            if (ListaDioses.Count() > 0)
                            {
                                Helper.Escritura("Cargando proxima pelea...\n");
                            }

                        }
                        else
                        {
                            player1EnJuego = false;
                            Helper.MostrarTitulo(YouLose);
                        }
                    }

                    if (player1EnJuego)
                    {
                        Console.ForegroundColor = player1.Color;
                        Helper.Escritura(player1.Frase + "\n");
                        Console.ResetColor();

                        Helper.MostrarTitulo(FinJuego);

                        Helper.Escritura("Eres el campeón indiscutible de este torneo, y tu nombre quedará grabado para la historia.\n");
                        Ganadoresjson.GuardarGanadorEnJson(ArchivoGanadores, player1);
                        Helper.MostrarRanking(ArchivoGanadores);
                    }

                    Console.WriteLine("Para jugar de nuevo: presione una tecla");
                    Console.WriteLine("Presione 'Esc' para salir.");
                    ConsoleKeyInfo JugarDeNuevo = Console.ReadKey();

                    if (JugarDeNuevo.Key == ConsoleKey.Escape)
                    {
                        Helper.MostrarTitulo(ArchivoTitulo);
                        programaEnUso = false;
                    }
                    else
                    {
                        ListaDioses = PersonajesJson.LeerPersonajes(ArchivoJson);
                        Helper.MostrarListaEnCuadros(ListaDioses);
                        Helper.MostrarTitulo(ArchivoElegir);
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

    private static bool PlayersAlives(Personaje player1, Personaje player2)
    {
        return player1.Salud > 0 && player2.Salud > 0;
    }

    private static bool IsValidPlayer(int jugadorElegido)
    {
        return (1 <= jugadorElegido && jugadorElegido <= 10);
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
            if (Helper.ExisteArchivo(ArchivoNombres))//si existe y no esta vacio.
            {
                FabricaPersonajes.CargarListaPersonajes(ListaDioses, ArchivoNombres);
                PersonajesJson.GuardarPersonajes(ArchivoJson, ListaDioses);
                iniciar = true;
            }
        }
        return iniciar;
    }

    private static void RealizarAtaque(Personaje playerAtaca, Personaje playerDefiende, int k)
    {
        int cteAjuste = 500;
        int Ataque = (playerAtaca.Destreza * playerAtaca.Fuerza * playerAtaca.Nivel);
        int Efectividad = FabricaPersonajes.ValorAleatorio(90, 101);
        int Defensa = playerDefiende.Armadura * playerDefiende.Velocidad;
        double DañoProvocado = ((Ataque * Efectividad) - Defensa) / (double)cteAjuste;
        playerAtaca.Puntaje += DañoProvocado;
        playerDefiende.Salud -= DañoProvocado;
        if (playerDefiende.Salud < 0)
        {
            playerDefiende.Salud = 0;
        }

        Helper.MostrarAtque(playerAtaca, playerDefiende, k, DañoProvocado);
    }  
}