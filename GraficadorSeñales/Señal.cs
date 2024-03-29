﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GraficadorSeñales
{
    abstract class Señal
    {
        public List<Muestra> Muestras { get; set; }
        public double TiempoInicial { get; set; }
        public double TiempoFinal { get; set; }
        public double FrecuenciaMuestreo { get; set; }

        public double AmplitudMaxima { get; set; }

        public abstract double evaluar(double tiempo);

        public void construirSeñal()
        {
            double periodoMuestreo =
                1 / FrecuenciaMuestreo;

            Muestras.Clear();

            for(double i = TiempoInicial;
                i <= TiempoFinal;
                i += periodoMuestreo)
            {
                double muestra = evaluar(i);

                Muestras.Add(
                    new Muestra(i, muestra));

                if (Math.Abs(muestra) >
                    AmplitudMaxima)
                {
                    AmplitudMaxima =
                        Math.Abs(muestra);
                }

            }
        }

        public static Señal escalaExponencial(
            Señal señalOrginal, double exponente)
        {
            SeñalResultante resultado =
                new SeñalResultante();
            resultado.TiempoInicial =
                señalOrginal.TiempoInicial;
            resultado.TiempoFinal =
                señalOrginal.TiempoFinal;
            resultado.FrecuenciaMuestreo =
                señalOrginal.FrecuenciaMuestreo;

            foreach(var muestra in señalOrginal.Muestras)
            {
                double nuevoValor =
                    Math.Pow(muestra.Y,
                    exponente);
                resultado.Muestras.Add(
                    new Muestra(muestra.X,
                    nuevoValor));
                if (Math.Abs(nuevoValor) > 
                    resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima =
                        Math.Abs(nuevoValor);
                }
            }
            return resultado;
        }

        public static Señal transformadaFourier(Señal señal)
        {
            SeñalResultante resultado =
                new SeñalResultante();

            resultado.TiempoInicial = señal.TiempoInicial;
            resultado.TiempoFinal = señal.TiempoInicial;
            resultado.FrecuenciaMuestreo = señal.FrecuenciaMuestreo;

            for(int k=0; k< señal.Muestras.Count; k++)
            {
                Complex muestra = 0; //0 + 01
                for (int n=0; n<señal.Muestras.Count; n++)
                {
                    muestra +=
                        señal.Muestras[n].Y *
                        Complex.Exp((-2 * Math.PI *
                        Complex.ImaginaryOne * k * n) /
                        señal.Muestras.Count);
                }
                resultado.Muestras.Add(new Muestra(
                    señal.Muestras[k].X,
                    muestra.Magnitude
                    ));
                if(Math.Abs(muestra.Magnitude) >
                    señal.AmplitudMaxima)
                {
                    señal.AmplitudMaxima =
                        Math.Abs(muestra.Magnitude);
                }
            }
            return resultado;
        }
        public static Señal escalarAmplitud(
            Señal señalOriginal, double factorEscala)
        {

            SeñalResultante resultado =
                new SeñalResultante();
            resultado.TiempoInicial = señalOriginal.TiempoInicial;
            resultado.TiempoFinal = señalOriginal.TiempoFinal;
            resultado.FrecuenciaMuestreo =
                señalOriginal.FrecuenciaMuestreo;

            foreach(var muestra in señalOriginal.Muestras)
            {
                double nuevoValor = muestra.Y * factorEscala;
                resultado.Muestras.Add(
                    new Muestra(
                        muestra.X,
                        nuevoValor)
                    );
                if (Math.Abs(nuevoValor) > resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima =
                        Math.Abs(nuevoValor);
                }
            }

            return resultado;
        }

        public static Señal desplazarAmplitud(
            Señal señalOriginal, double cantidadDesplazamiento)
        {

            SeñalResultante resultado =
                new SeñalResultante();
            resultado.TiempoInicial = señalOriginal.TiempoInicial;
            resultado.TiempoFinal = señalOriginal.TiempoFinal;
            resultado.FrecuenciaMuestreo =
                señalOriginal.FrecuenciaMuestreo;

            foreach (var muestra in señalOriginal.Muestras)
            {
                double nuevoValor = muestra.Y + cantidadDesplazamiento;
                resultado.Muestras.Add(
                    new Muestra(
                        muestra.X,
                        nuevoValor)
                    );
                if (Math.Abs(nuevoValor) > resultado.AmplitudMaxima)
                {
                    resultado.AmplitudMaxima =
                        Math.Abs(nuevoValor);
                }
            }

            return resultado;
        }
    }

}
