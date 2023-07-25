using Personajes;
using ArchivosJson;
using ConsumoAPI;
namespace Juego;

// mostrar listado historico
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

        var ListaDioses = new List<Personaje>();

        if (IniciarJuego(ArchivoJson, ArchivoNombres, ref ListaDioses))
        {
            bool programaEnUso = true;
            int jugadorElegido;
            string inputJugador;

            Helper.MostrarIntro(IntroJuego);

            Console.ForegroundColor = ConsoleColor.Gray;
            Helper.MostrarTitulo(ArchivoTitulo);
            Console.ResetColor();

            while (programaEnUso)
            {
                Helper.MostrarListaEnCuadros(ListaDioses);

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
                    Console.WriteLine("Dios elegido:"+player1.Nombre);

                    bool player1EnJuego = true;
                    int i = 1;

                    while (ListaDioses.Count >= 1 && player1EnJuego)
                    {
                        int indexPlayer2 = FabricaPersonajes.ValorAleatorio(0, ListaDioses.Count);
                        Personaje player2 = ListaDioses[indexPlayer2]; //elijo el rival
                        ListaDioses.RemoveAt(indexPlayer2); // remuevo el jugador perdido

                        Helper.PresentacionBatalla(i, player1, player2);
                        Console.WriteLine();

                        Console.ForegroundColor = player1.Color;
                            Console.WriteLine("{0}:{1}", player1.Nombre, MiConsumoAPI.ObtnerInsulto());
                        Console.ResetColor();

                        Console.ForegroundColor = player2.Color;
                            Console.WriteLine("\t\t\t\t\t\t\t\t\t\t{0}:{1}", player2.Nombre, MiConsumoAPI.ObtnerInsulto());
                        Console.ResetColor();

                        int k = 2;//variable para determinar posición de donde se muestra el ataque

                        while (player1.Salud > 0 && player2.Salud > 0) // Empieza pelea
                        {
                            RealizarAtaque(player1, player2, k); // mostrar ataque como en wp
                            k++;
                            if (player2.Salud > 0)
                            {
                                RealizarAtaque(player2, player1, k);
                                k++;
                            }
                        }

                        if (player1.Salud > 0) // si gana el player1
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
                            Helper.Escritura(player1.Frase+"\n");
                        Console.ResetColor();

                        Helper.MostrarTitulo(FinJuego);
                        
                        Helper.Escritura("Eres el campeón indiscutible de este torneo, y tu nombre quedará grabado para la historia.\n");
                        Ganadoresjson.GuardarGanadorEnJson(ArchivoGanadores, player1);
                        //mostrarListaGanadores
                    }

                    Console.WriteLine("Para jugar de nuevo: presione una tecla");
                    Console.WriteLine("Presione 'Esc' para salir.");
                    ConsoleKeyInfo JugarDeNuevo = Console.ReadKey();
                    
                    if (JugarDeNuevo.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Saliendo...");
                        programaEnUso = false;
                        
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