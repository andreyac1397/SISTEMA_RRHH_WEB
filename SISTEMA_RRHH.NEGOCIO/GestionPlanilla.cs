using SISTEMA_RRHH.DATOS;
using SISTEMA_RRHH.DATOSSQL;
using SISTEMA_RRHH.DTO;
using SISTEMA_RRHH.ENTIDADES;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SISTEMA_RRHH.NEGOCIO
{
    public class GestionPlanilla
    {
        private readonly PlanillaDatos _planillaDatos;

        public GestionPlanilla()
        {
            _planillaDatos = new PlanillaDatos();
        }

        public List<Planilla> CalcularPlanilla(int mes, int anno, int usuarioCalculoID)
        {
            try
            {
                // Listar todos los datos que necesitamos
                var funcionarios = _planillaDatos.ListarFuncionarios();
                var horas = _planillaDatos.ListarHorasPorPeriodo(mes, anno);
                var cargasSociales = _planillaDatos.ListarCargas();
                var creditosFiscales = _planillaDatos.ListarCreditos();
                var tractosImpuestoRenta = _planillaDatos.ListarTractos();

                List<Planilla> planillaCalculada = new List<Planilla>();

                foreach (var funcionario in funcionarios)
                {
                    var horasFuncionario = horas.FirstOrDefault(h => h.FuncionarioID == funcionario.FuncionarioID);
                    if (horasFuncionario == null)
                        continue;

                    decimal salarioBruto = horasFuncionario.SalarioBruto;
                    decimal aportePension = funcionario.AportePensionComplementaria;

                    decimal cargas = CalcularCargasSociales(cargasSociales, salarioBruto);
                    decimal impuestoCalculado = CalcularImpuestoRenta(tractosImpuestoRenta, salarioBruto, aportePension);
                    decimal creditos = CalcularCreditosFiscales(creditosFiscales, funcionario);

                    // Aplicar créditos fiscales al impuesto
                    decimal impuestoRenta = Math.Max(0, impuestoCalculado - creditos);

                    // Calcular salario neto correctamente
                    decimal salarioNeto = salarioBruto - aportePension - cargas - impuestoRenta;

                    Planilla planilla = new Planilla
                    {
                        FuncionarioID = funcionario.FuncionarioID,
                        PeriodoMes = mes,
                        PeriodoAnno = anno,
                        SalarioBaseMensual = funcionario.SalarioBase,
                        SalarioBruto = salarioBruto,
                        AportePensionComplementaria = aportePension,
                        TotalCargasSociales = cargas,
                        TotalCreditosFiscales = creditos,
                        ImpuestoRenta = impuestoRenta,
                        SalarioNeto = salarioNeto,
                        UsuarioCalculoID = usuarioCalculoID,
                        NombreCompleto = funcionario.NombreCompleto
                    };

                    planillaCalculada.Add(planilla);
                }

                InsertarResultadosPlanilla(planillaCalculada, mes, anno);
                return planillaCalculada;
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al calcular la planilla", ex);
            }
        }

        private decimal CalcularCargasSociales(List<CargaSocial> cargasSociales, decimal salarioBruto)
        {
            decimal totalCargas = 0;
            foreach (var carga in cargasSociales)
            {
                totalCargas += carga.PorcentajeTrabajador * salarioBruto / 100;
            }
            return totalCargas;
        }

        private decimal CalcularImpuestoRenta(
        List<TractoImpuestoRenta> tractos,
        decimal salarioBruto,
        decimal aportePension)
        {
            decimal salarioGravable = Math.Max(0, salarioBruto - aportePension);

            decimal impuesto = 0;
            foreach (var tramo in tractos.OrderBy(t => t.SalarioDesde))
            {
                if (salarioGravable <= tramo.SalarioDesde) break;

                decimal limiteSuperior = Math.Min(salarioGravable, tramo.SalarioHasta);
                decimal baseTramo = limiteSuperior - tramo.SalarioDesde;

                impuesto += baseTramo * (tramo.PorcentajeAplicable / 100m);
            }

            return impuesto;
        }





        private decimal CalcularCreditosFiscales(List<CreditoFiscal> creditosFiscales, FuncionarioPlanilla funcionario)
        {
            decimal totalCreditos = 0;

            foreach (var credito in creditosFiscales)
            {
                string tipo = RemoverTildes(credito.Tipo.Trim().ToLower());

                if (tipo == "hijo" && funcionario.CantHijosAplicables > 0)
                {
                    totalCreditos += funcionario.CantHijosAplicables * credito.Monto;
                }
                else if (tipo == "conyuge" && funcionario.EstadoCivilID == 2)
                {
                    totalCreditos += credito.Monto;
                }
                else if (tipo != "hijo" && tipo != "conyuge")
                {
                    totalCreditos += credito.Monto;
                }
            }

            return totalCreditos;
        }


        private string RemoverTildes(string texto)
        {
            return texto
                .Normalize(System.Text.NormalizationForm.FormD)
                .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                .Aggregate("", (s, c) => s + c)
                .ToLower();
        }
        public Planilla ObtenerComprobanteGuardado(int funcionarioID, int mes, int anno)
        {
            return new PlanillaDatosSQL().ObtenerComprobanteFuncionario(funcionarioID, mes, anno);
        }


        private void InsertarResultadosPlanilla(List<Planilla> planillas, int mes, int anno)
        {
            foreach (var planilla in planillas)
            {
                _planillaDatos.InsertarResultado(planilla, mes, anno);
            }
        }
        public Planilla ObtenerComprobanteFuncionario(int funcionarioID, int mes, int anno)
        {
            var planillas = CalcularPlanilla(mes, anno, funcionarioID);
            return planillas.FirstOrDefault(p => p.FuncionarioID == funcionarioID);
        }

    }
}

