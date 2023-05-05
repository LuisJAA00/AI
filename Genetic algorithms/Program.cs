using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;




namespace Ciudades
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Genetic genetic = new Genetic();


            Dictionary<int, List<(int, int, int)>> adjList = new Dictionary<int, List<(int, int, int)>>() {
            {0, new List<(int, int, int)>{(1, 225, 380), (11,150,98)}},               // Madrid
            {1, new List<(int, int, int)>{(0, 225, 380), (11, 390, 400),(10, 112,185),(6,480,345),(3,82,80),(2,136,98)}},              // Paris
            {2, new List<(int, int, int)>{(1,136,98)}},                  // London
            {3, new List<(int, int, int)>{(1, 82,80), (4,105,48)}},                // Brucelas
            {4, new List<(int, int, int)>{(3, 105,48),(7,120,40),(5,364,235)} },            // Amsterdan
            {5, new List<(int, int, int)>{(4,364,235),(6,232,125)}},                              // Berlin
            {6, new List<(int, int, int)>{(5,232,125),(7,120,40),(1,480,345),(8,454,240)}},                              // Frankfurt
            {7, new List<(int, int, int)>{(4,120,40),(6,120,40)}},                              // Cologne
            {8, new List<(int, int, int)>{(6,454,240),(9,168,125),(10,176,180)}},                              // Milan
            {9, new List<(int, int, int)>{(8,168,125)}},                     // Rome
            {10, new List<(int, int, int)>{(8,176,180),(1,112,185),(11,200,320)}},                             // Lyon
            {11, new List<(int, int, int)>{(0,150,98),(1,390,400),(10,200,320)}},                             // Barcelona
        };
            Random randy = new Random();

            // Inicializa array conteniendo poblacion
            int PoblacionTotal = 100000;


            // Generar aleatoriamente una collecion de numeros que representen las ciudades a visitar (en ese orden), 
            // cada array debe contar con todas las ciudades...

            List<List<int>> poblacion = genetic.generatePopulation(PoblacionTotal, randy.Next(0,11));

            // Get those paths that take less time than the max permited and that are above the tresshold (1) the higer the 
            // tresshold the lower the cost of the selected pops, but the less likely we are to get pops...
            poblacion = genetic.fitnes(poblacion, PoblacionTotal, 0.2);


            poblacion = genetic.breed(poblacion);
            int reps = 200;
            int count = 0;
            double mutRate = 0.4;
  
            for(int i = 0; i<reps;i++)
            {



                poblacion = genetic.breed(genetic.fitnes(poblacion,poblacion.Count,1));
                
                
            }

            poblacion = genetic.GetElite(poblacion,10);
            for(int i = 0; i<10; i++)
            {
                Console.WriteLine("the elemenst of the final arrays (path)");
                List<int> list = poblacion[i];
                foreach(int b in list)
                {
                    Console.WriteLine(" Path:"+ b);

                }
            }


        }
    }
}
