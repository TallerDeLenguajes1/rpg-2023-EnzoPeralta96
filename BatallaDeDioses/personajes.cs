using System.Text.Json;
namespace GeneracionPersonajes
{
    public enum TipoPersonaje{
        Dios=1,
        Humano=2
    }
    public class Personaje
    {
        private TipoPersonaje team;
        private string nombre;
        private string apodo;
        //private DateTime fechaNac;
        private int edad;
        private int velocidad;
        private int destreza;
        private int fuerza;
        private int nivel;
        private int armadura;
        private int salud;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Apodo { get => apodo; set => apodo = value; }
        //public DateTime FechaNac { get => fechaNac; set => fechaNac = value; }
        public int Edad { get => edad; set => edad = value; }
        public int Velocidad { get => velocidad; set => velocidad = value; }
        public int Destreza { get => destreza; set => destreza = value; }
        public int Fuerza { get => fuerza; set => fuerza = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Armadura { get => armadura; set => armadura = value; }
        public int Salud { get => salud; set => salud = value; }
        public TipoPersonaje Team { get => team; set => team = value; }

        public Personaje(TipoPersonaje team, string nombre, string apodo, /*DateTime fechaNac*/ int edad, int velocidad, int destreza, int fuerza, int nivel, int armadura, int salud)
        {
            Team = team;
            Nombre = nombre;
            Apodo = apodo;
            //FechaNac = fechaNac;
            Edad = edad;
            Velocidad = velocidad;
            Destreza = destreza;
            Fuerza = fuerza;
            Nivel = nivel;
            Armadura = armadura;
            Salud = salud;
        }
    }
    public class FabricaPersonajes
    {
        static public Personaje CrearPersonaje(TipoPersonaje team, string nombre, String apodo){

           /* int yearNac = ValorAleatorio(1,1900);   
            int  mesNac = ValorAleatorio(1,13);
            int DiaMax = DateTime.DaysInMonth(yearNac,mesNac);  
            int diaNac = ValorAleatorio(1,DiaMax+1);
            DateTime fechaNac = new DateTime(yearNac,mesNac,diaNac);
            DateTime FechaActual = DateTime.Now;
            int edadPersonaje = FechaActual.Year - fechaNac.Year;*/
            int edadPersonaje = ValorAleatorio(0,300);
            int velocidad = ValorAleatorio(1,11);
            int destreza = ValorAleatorio(1,6);
            int fuerza = ValorAleatorio(1,11);
            int nivel = ValorAleatorio(1,11);
            int armadura = ValorAleatorio(1,10);
            int salud = 100;
            Personaje Nuevo = new Personaje(team,nombre,apodo,/*fechaNac,*/edadPersonaje,velocidad,destreza,fuerza,nivel,armadura,salud);

            return Nuevo;
        }

        static int ValorAleatorio(int a, int b){
            Random NumeroAletorio = new Random();
            return NumeroAletorio.Next(a,b);
        }

    }

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
                return true;
            }else
            {
                return false;
            }
        }
    }

}
