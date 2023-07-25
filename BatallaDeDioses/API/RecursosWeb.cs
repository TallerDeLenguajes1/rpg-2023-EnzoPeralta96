using System.Net;
using System.Text.Json;
namespace ConsumoAPI
{
    class MiConsumoAPI
    {
        private static FraseAPI obtenerDatosAPI()
        {
            var url = $"https://evilinsult.com/generate_insult.php?lang=es&type=json";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader != null)
                        {
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                //// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
                                string responseBody = objReader.ReadToEnd();
                                return JsonSerializer.Deserialize<FraseAPI>(responseBody);
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error al acceder a la API web: " + ex.Message);
            }
            return null;
        }
        //public static FraseAPI ObtnerDatosAPI() => ObtnerDatosAPI();

        public static string ObtnerInsulto()
        {
            var datosAPI = obtenerDatosAPI();
            return datosAPI.insult;
        }

    }
}


