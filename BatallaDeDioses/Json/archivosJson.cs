using Personajes;
using System.Text.Json;
using Juego;

namespace ArchivosJson
{
    public class PersonajesJson
    {
        static private void guardarPersonajes(string nombreArchivo, List<Personaje> Lista)
        {
            String formatoJson = JsonSerializer.Serialize(Lista);
            File.WriteAllText(nombreArchivo, formatoJson);
        }
        static public void GuardarPersonajes(string nombreArchivo, List<Personaje> Lista) => guardarPersonajes(nombreArchivo, Lista);



        static private List<Personaje> leerPersonajes(string NombreArchivo)
        {
            var ListadoPersonajes = new List<Personaje>();

            string TextoJson = File.ReadAllText(NombreArchivo);

            if (!string.IsNullOrEmpty(TextoJson))
            {
                ListadoPersonajes = JsonSerializer.Deserialize<List<Personaje>>(TextoJson)!;
            }

            return ListadoPersonajes;
        }

        static public List<Personaje> LeerPersonajes(string NombreArchivo) => leerPersonajes(NombreArchivo);

        static private bool existe(string NombreArchivo)
        {
            if (File.Exists(NombreArchivo))
            {
                var InfoArchivo = new FileInfo(NombreArchivo);

                if (InfoArchivo.Length > 0)
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
        static public bool Existe(string NombreArchivo) => existe(NombreArchivo);
    }

    public class Ganadoresjson
    {
        static private List<Ganadores> leerGanadores(string ArchivoGanadores)
        {
            string textoJson = File.ReadAllText(ArchivoGanadores);
            var ListaGanadores = JsonSerializer.Deserialize<List<Ganadores>>(textoJson);
            return ListaGanadores;
        }

        static public List<Ganadores> LeerGanadores(string ArchivoGanadores) => leerGanadores(ArchivoGanadores);


        static private void guardarGanadores(string nombreArchivo, List<Ganadores> ListaPlayer)
        {
            String formatoJson = JsonSerializer.Serialize(ListaPlayer);
            File.WriteAllText(nombreArchivo, formatoJson);
        }
        static public void GuardarGanadores(string nombreArchivo, List<Ganadores> Listaplayer) => guardarGanadores(nombreArchivo, Listaplayer);

        public static void GuardarGanadorEnJson(string ArchivoGanadores, Personaje player1) // mover a manejo de json
        {
            var ListaGanadores = new List<Ganadores>();

            string usuarioPlayer1;
            do
            {
                Console.WriteLine("Ingrese un nick:");
                usuarioPlayer1 = Console.ReadLine();
            } while (String.IsNullOrEmpty(usuarioPlayer1));

            var Ganador = new Ganadores(usuarioPlayer1, player1.Puntaje, 0);

            if (Helper.ExisteArchivo(ArchivoGanadores))
            {
                ListaGanadores = Ganadoresjson.LeerGanadores(ArchivoGanadores);
            }

            ListaGanadores.Add(Ganador);
            ListaGanadores = Ganadores.OrdenarListaGanadores(ListaGanadores);

            for (int i = 0; i < ListaGanadores.Count(); i++)
            {
                ListaGanadores[i].Ranking = i + 1;
            }

            Ganadoresjson.GuardarGanadores(ArchivoGanadores, ListaGanadores);
        }

       

    }


}


