using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Avion
    {
        private int _matricula;
        private string _marca;
        private string _modelo;
        private int _capacidad;
        private int _codigoVuelo;


        public int Matricula
        {
            get { return this._matricula; }
            set { this._matricula = value; }
        }

        public string Marca
        {
            get { return this._marca; }
            set { this._marca = value; }
        }

        public string Modelo
        {
            get { return this._modelo; }
            set { this._modelo = value; }
        }


        public int Capacidad
        {
            get { return this._capacidad; }
            set { this._capacidad = value; }
        }

        public int Codigo
        {
            get { return this._codigoVuelo; }
            set { this._codigoVuelo = value; }
        }

        public Avion()
        {

        }

        public Avion(int unaMatricula, string unaMarca, string unModelo, int unaCapacidad, int unCodigo)
            : this()
        {
            this._matricula = unaMatricula;
            this._marca = unaMarca;
            this._modelo = unModelo;
            this._capacidad = unaCapacidad;
            this._codigoVuelo = unCodigo;
        }

    }
}
