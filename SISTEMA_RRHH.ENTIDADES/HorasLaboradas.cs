namespace SISTEMA_RRHH.ENTIDADES
{
    public class HorasLaboradas
    {
        public int HorasLaboradasID { get; set; }
        public int FuncionarioID { get; set; }
        public string NombreCompleto { get; set; }

        public decimal HorasNormales { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal HorasDobles { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal SalarioBruto { get; set; }
        public int PeriodoMes { get; set; }
        public int PeriodoAnno { get; set; }

    }
}

