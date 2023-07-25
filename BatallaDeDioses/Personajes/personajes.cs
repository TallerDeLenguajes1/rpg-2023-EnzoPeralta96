namespace Personajes
{
    public enum TipoPersonaje
    {
        DiosNordico = 1,
        DiosGriego = 2,
        DiosEjipcio = 3,
        DiosIndu = 4,
        Demonio = 5,
        Humano = 6
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
        private double salud;
        private string frase;

        private ConsoleColor color;

        private double puntaje;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Apodo { get => apodo; set => apodo = value; }
        //public DateTime FechaNac { get => fechaNac; set => fechaNac = value; }
        public int Edad { get => edad; set => edad = value; }
        public int Velocidad { get => velocidad; set => velocidad = value; }
        public int Destreza { get => destreza; set => destreza = value; }
        public int Fuerza { get => fuerza; set => fuerza = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Armadura { get => armadura; set => armadura = value; }
        public double Salud { get => salud; set => salud = value; }
        public TipoPersonaje Team { get => team; set => team = value; }
        public string Frase { get => frase; set => frase = value; }
        public ConsoleColor Color { get => color; set => color = value; }
        public double Puntaje { get => puntaje; set => puntaje = value; }

        public Personaje(TipoPersonaje team, string nombre, string apodo, /*DateTime fechaNac*/ int edad, int velocidad, int destreza, int fuerza, int nivel, int armadura, double salud, string frase, ConsoleColor color, double puntaje)
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
            Frase = frase;
            Color = color;
            Puntaje = puntaje;
        }
    }
    public class FabricaPersonajes
    {
        static public Personaje CrearPersonaje(int nroTipo, string nombre, String apodo, string fraseCampeon, ConsoleColor colorPersonaje)
        {
            /* int yearNac = ValorAleatorio(1,1900);   
             int  mesNac = ValorAleatorio(1,13);
             int DiaMax = DateTime.DaysInMonth(yearNac,mesNac);  
             int diaNac = ValorAleatorio(1,DiaMax+1);
             DateTime fechaNac = new DateTime(yearNac,mesNac,diaNac);
             DateTime FechaActual = DateTime.Now;
             int edadPersonaje = FechaActual.Year - fechaNac.Year;*/
            int edadPersonaje = ValorAleatorio(0, 300);
            double salud = 100;
            int velocidad = 0;
            int destreza = 0;
            int fuerza = 0;
            int nivel = 0;
            int armadura = 0;
            TipoPersonaje tipo = (TipoPersonaje)nroTipo;

            switch (tipo)
            {
                case TipoPersonaje.DiosNordico:
                    velocidad = ValorAleatorio(3, 5);
                    destreza = ValorAleatorio(3, 5);
                    fuerza = ValorAleatorio(6, 10);
                    nivel = ValorAleatorio(3, 5);
                    armadura = ValorAleatorio(3, 5);
                    break;
                case TipoPersonaje.DiosGriego:
                    velocidad = ValorAleatorio(3, 5);
                    destreza = ValorAleatorio(3, 5);
                    fuerza = ValorAleatorio(3, 5);
                    nivel = ValorAleatorio(3, 5);
                    armadura = ValorAleatorio(6, 10);
                    break;
                case TipoPersonaje.DiosEjipcio:
                    velocidad = ValorAleatorio(3, 5);
                    destreza = ValorAleatorio(3, 10);
                    fuerza = ValorAleatorio(3, 5);
                    nivel = ValorAleatorio(3, 5);
                    armadura = ValorAleatorio(3, 5);
                    break;
                case TipoPersonaje.DiosIndu:
                    velocidad = ValorAleatorio(6, 10);
                    destreza = ValorAleatorio(3, 5);
                    fuerza = ValorAleatorio(3, 5);
                    nivel = ValorAleatorio(3, 5);
                    armadura = ValorAleatorio(3, 5);
                    break;
                case TipoPersonaje.Demonio:
                    velocidad = ValorAleatorio(3, 5);
                    destreza = ValorAleatorio(3, 5);
                    fuerza = ValorAleatorio(3, 5);
                    nivel = ValorAleatorio(6, 10);
                    armadura = ValorAleatorio(3, 5);
                    break;
                case TipoPersonaje.Humano:
                    velocidad = ValorAleatorio(6, 8);
                    destreza = ValorAleatorio(2, 5);
                    fuerza = ValorAleatorio(6, 8);
                    nivel = ValorAleatorio(2, 5);
                    armadura = ValorAleatorio(2, 5);
                    break;
            }

            Personaje Nuevo = new Personaje(tipo, nombre, apodo,/*fechaNac,*/edadPersonaje, velocidad, destreza, fuerza, nivel, armadura, salud, fraseCampeon, colorPersonaje, 0);
            return Nuevo;
        }

        public static void CargarListaPersonajes(List<Personaje> Lista, string NombresPersonajes) // dentro de fabrica personajes
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

        public static void MejorarHabilidad(Personaje player1)
        {
            player1.Salud = 100;
            int habilidadMejora = FabricaPersonajes.ValorAleatorio(1, 6);

            switch (habilidadMejora)
            {
                case 1:
                    if (player1.Velocidad < 10)
                    {
                        player1.Velocidad += 1;
                        Console.WriteLine("Habilidad mejorada: +1 en Velocidad\n");
                    }
                    break;
                case 2:
                    if (player1.Destreza < 5)
                    {
                        player1.Destreza += 1;
                        Console.WriteLine("Habilidad mejorada: +1 en Destreza\n");
                    }
                    break;
                case 3:
                    if (player1.Fuerza < 10)
                    {
                        player1.Fuerza += 1;
                        Console.WriteLine("Habilidad mejorada: +1 en Fuerza\n");
                    }
                    break;
                case 4:
                    if (player1.Nivel < 10)
                    {
                        player1.Nivel += 1;
                        Console.WriteLine("Habilidad mejorada: +1 en Nivel\n");
                    }
                    break;
                case 5:
                    if (player1.Armadura < 10)
                    {
                        player1.Armadura += 1;
                        Console.WriteLine("Habilidad mejorada: +1 en Armadura\n");
                    }
                    break;
            }

        }

        static public int ValorAleatorio(int a, int b)
        {
            Random NumeroAletorio = new Random();
            return NumeroAletorio.Next(a, b);
        }

    }
}
