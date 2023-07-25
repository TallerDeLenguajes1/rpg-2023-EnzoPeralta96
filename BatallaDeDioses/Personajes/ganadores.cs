namespace Personajes
{
    public class Ganadores
    {
        private string nombreUsuario;
       // private string nickName;
        private double puntaje;
        private  int ranking;

        public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
       // public string NickName { get => nickName; set => nickName = value; }
        public double Puntaje { get => puntaje; set => puntaje = value; }
        public int Ranking { get => ranking; set => ranking = value; }

        public Ganadores(string nombreUsuario, /*tring nickName,*/ double puntaje, int ranking)
        {
            NombreUsuario = nombreUsuario;
           // NickName = nickName;
            Puntaje = puntaje;
            Ranking = ranking;
        }
        private static List<Ganadores> ordenarListaGanadores(List<Ganadores> ListadoGanadores)
        {
           return ListadoGanadores.OrderByDescending(p => p.puntaje).ToList();
        }
        public static List<Ganadores> OrdenarListaGanadores(List<Ganadores> ListadoGanadores) => ordenarListaGanadores(ListadoGanadores);

        


    }

}