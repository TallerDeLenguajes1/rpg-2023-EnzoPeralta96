using Personajes;
using ConsumoAPI;
using ArchivosJson;
namespace Juego;


public static class Helper
{
    public static void MostrarIntro(string IntroJuego)
    {
        if (ExisteArchivo(IntroJuego))
        {
            using (var intro = new StreamReader(IntroJuego))
            {
                while (!intro.EndOfStream)
                {
                    string lineaIntro = intro.ReadLine();
                    Escritura(lineaIntro);
                }
                intro.Close();
                Console.WriteLine();
            }
        }
    }

    public static void MostrarTitulo(string ArchivoTitulo)
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
            }
        }
    }

    public static void MostrarListaEnCuadros(List<Personaje> Lista)
    {
        MostrarTitulo("ArchivosTexto/Listado.txt");
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
            Thread.Sleep(1500);

            Console.ResetColor();

            i++;
        }
       
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



    public static void PresentacionBatalla(int i, Personaje Dios1, Personaje Dios2)
    {
        switch (i)
        {
            case 1:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            case 2:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            case 3:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            case 4:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            case 5:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            case 6:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            case 7:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            case 8:
                MostrarTitulo("ArchivosTexto/batalla_" + i + ".txt");
                break;
            default:
                MostrarTitulo("ArchivosTexto/batalla_final.txt");
                break;
        }
        Console.ForegroundColor = Dios1.Color;
        MostrarTitulo("ArchivosTexto/" + Dios1.Nombre + ".txt");
        Console.ResetColor();

        MostrarTitulo("ArchivosTexto/vs.txt");

        Console.ForegroundColor = Dios2.Color;
        MostrarTitulo("ArchivosTexto/" + Dios2.Nombre + ".txt");
        Console.ResetColor();
    }

    public static void MostrarAtque(Personaje playerAtaca, Personaje playerDefiende, int k, double DañoProvocado)
    {
        int recuadroAncho = 50;
        Console.ForegroundColor = playerAtaca.Color;
        if ((k % 2) == 0)
        {
            Console.WriteLine(new string('=', recuadroAncho));
            Console.WriteLine("\t{0} ATACA A {1}", playerAtaca.Nombre, playerDefiende.Nombre);
            if (10 <= playerDefiende.Salud && playerDefiende.Salud <= 40)
            {
                Console.ForegroundColor = playerDefiende.Color;
                Console.WriteLine("\t{0}:{1}", playerDefiende.Nombre, MiConsumoAPI.ObtnerInsulto());
                Console.ForegroundColor = playerAtaca.Color;
            }
            Console.WriteLine("\tDaño provocado a {0}:{1:N2}", playerDefiende.Apodo, DañoProvocado);
            Console.ForegroundColor = playerDefiende.Color;
            Console.WriteLine("\tSalud {0}:{1:N2}", playerDefiende.Nombre, playerDefiende.Salud);
            Console.ResetColor();
            Console.ForegroundColor = playerAtaca.Color;
            Console.WriteLine(new string('=', recuadroAncho));
        }
        else
        {
            Console.WriteLine("\t\t\t\t\t\t\t\t\t\t" + new string('=', recuadroAncho));
            Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t{0} ATACA A {1}", playerAtaca.Nombre, playerDefiende.Nombre);
            if (10 <= playerDefiende.Salud && playerDefiende.Salud <= 40)
            {
                Console.ForegroundColor = playerDefiende.Color;
                Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t{0}:{1}", playerDefiende.Nombre, MiConsumoAPI.ObtnerInsulto());
                Console.ForegroundColor = playerAtaca.Color;
            }
            Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\tDaño provocado a {0}:{1:N2}", playerDefiende.Apodo, DañoProvocado, playerDefiende.Salud);
            Console.ForegroundColor = playerDefiende.Color;
            Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\tSalud {0}:{1:N2}", playerDefiende.Nombre, playerDefiende.Salud);
            Console.ResetColor();
            Console.ForegroundColor = playerAtaca.Color;
            Console.WriteLine("\t\t\t\t\t\t\t\t\t\t" + new string('=', recuadroAncho));
        }
        Console.ResetColor();
        Thread.Sleep(1500);
    }


    public static bool ExisteArchivo(string Archivo)
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

    public static void Escritura(string lineaIntro)
    {
        foreach (var caracter in lineaIntro)
        {
            Console.Write(caracter);
            Thread.Sleep(15);
        }
    }

    public static void MostrarRanking(string CampeonesJson)
    {
        var ListaGanadores = Ganadoresjson.LeerGanadores(CampeonesJson);
        MostrarTitulo("ArchivosTexto/Historial.txt");
        foreach (var ganador in ListaGanadores)
        {
            Console.WriteLine("\t"+ganador.NombreUsuario+"\t\t\t\t\t\t"+ganador.Ranking+"\t\t\t\t\t\t"+"{0:N2}",ganador.Puntaje);
        }
    }
}
