using Personajes;
using System.Text.Json;
namespace ArchivosJson;

public class PersonajesJson
    {
        static public void GuardarPersonajes(string nombreArchivo,List<Personaje> Lista)
        {
            String formatoJson = JsonSerializer.Serialize(Lista);
            File.WriteAllText(nombreArchivo, formatoJson);
        }
        static public List<Personaje> LeerPersonajes(string NombreArchivo){
            var ListadoPersonajes = new List<Personaje>();

            string TextoJson = File.ReadAllText(NombreArchivo);

            if (!string.IsNullOrEmpty(TextoJson))
            {
                ListadoPersonajes = JsonSerializer.Deserialize<List<Personaje>>(TextoJson)!;
            }
            
            return ListadoPersonajes;
        }
        static public bool Existe(string NombreArchivo)
        {
            if (File.Exists(NombreArchivo))
            {
                var InfoArchivo = new FileInfo(NombreArchivo);

                if (InfoArchivo.Length>0)
                {
                    return true;
                }else
                {
                    return false;
                }
            }else
            {
                return false;
            }
        }
    }