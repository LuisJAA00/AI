using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Ciudades
{

    internal class Genetic
    {
        public Dictionary<int, List<(int, int, int)>> adjList = new Dictionary<int, List<(int, int, int)>>() {
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
        public Random random = new Random((int)DateTime.Now.Ticks);
        
        public List<List<int>> survivors;
        public int totalPobs = 0;
        public int NumSurv = 0;
        public int fit = 0;
        public List<List<int>> generatePopulation(int TotalPop, int currentVertex)
        {
             List<List<int>> poblacionC = new List<List<int>>();
        
            this.totalPobs = TotalPop;
            // class to generate the random number and int to store it


           
            // store visisted vertex
            bool Allvisited = false;
            bool[] visited = new bool[12];

            for (int j = 0; j < 12; j++)
            {

                visited[j] = false;

            }
            int avg = 0;
            int number = 0;
            Console.WriteLine("Generating individuals...");
            for (int i = 0; i < TotalPop; i++)
            {
                int count2 = 0;
                List<int> path = new List<int>();
                poblacionC.Add(path);
                while (Allvisited == false)
                {
                    // generate a random number with the number of elements at the tdictionary containing the elementes of the graph
                    number = random.Next(adjList[currentVertex].Count);

                    // add to this individual the random selected vertex that is accsesible from the current vertex
                    poblacionC[i].Add(adjList[currentVertex][number].Item1);

                    visited[adjList[currentVertex][number].Item1] = true;
                    currentVertex = adjList[currentVertex][number].Item1;
                    int count = 0;

                    // check what cities have been visited
                    foreach (bool k in visited)
                    {
                        if (k == true)
                        {
                            count++;

                        }

                    }
                    // when all is visited...
                    if (count == 12)
                    {
                        Allvisited = true;
                    }
                    count2++;
                }
                 //Print generated
           //       foreach (int h in poblacionC[i])
           //      {
            //          Console.WriteLine("Los elementos del array" + i + " son: " + h + " obtained in " + count2) ;
          //       }
                Allvisited = false;
                for (int j = 0; j < 12; j++)
                {

                    visited[j] = false;

                }
                avg = avg + count2;
            }
            Console.WriteLine(" AVG steps (travels) " + avg / TotalPop);
            return poblacionC;
        }


        public List<List<int>> surviveTime(List<List<int>> poblacion, int PoblacionTotal)
        {
            Console.WriteLine("Searching survivors for time requirement...");
            survivors = new List<List<int>>();

            int count = 0;

            for (int i = 0; i < PoblacionTotal; i++)
            {

                int totalCost = 0;

                for (int j = 0; j < poblacion[i].Count - 1; j++)
                {
                    int current = poblacion[i][j];
                    int next = poblacion[i][j + 1];

                    // Look up the edges between the current and next vertices
                    List<(int, int, int)> edges = adjList[current];
                    (int, int, int) edge = edges.Find(e => e.Item1 == next);

                    // Add the weight of the edge to the total cost
                    totalCost += edge.Item2;
                }
                if (totalCost < 4600 && count < 10)
                {

                    survivors.Add(poblacion[i]);
                    count++;


                }
                NumSurv = count;
                count = 0;
            }



            return survivors;
        }
        public List<List<int>> GetElite(List<List<int>> poblacion, int EliteNumber)
        {
            // Calculate the total cost for each list
            List<int> costs = new List<int>();
            for (int i = 0; i < poblacion.Count; i++)
            {
                int totalCost = 0;
                for (int j = 0; j < poblacion[i].Count - 1; j++)
                {
                    int current = poblacion[i][j];
                    int next = poblacion[i][j + 1];

                    // Look up the edges between the current and next vertices
                    List<(int, int, int)> edges = adjList[current];
                    (int, int, int) edge = edges.Find(e => e.Item1 == next);

                    // Add the weight of the edge to the total cost
                    totalCost += edge.Item3;
                }
                costs.Add(totalCost);
            }

            // Sort the list of costs in ascending order
            costs.Sort();



            // Create a new list to store the result
            List<List<int>> result = new List<List<int>>();

            // Loop through the first numListsToReturn lists and add them to the result list
            for (int i = 0; i < EliteNumber; i++)
            {
                int index = costs.IndexOf(costs[i]);
                result.Add(poblacion[index]);
            }

            // Return the result list
            return result;
        }

        public double AvgTotalCost(List<List<int>> pobs, int totalpobs)
        {
            double totalCost = 0;
            foreach (List<int> pob in pobs)
            {

                totalCost = totalCost + this.Cost(pob);
            }
            return totalCost / totalpobs;
        }

        public List<List<int>> fitnes(List<List<int>> pobs, int totalpobs, double limite)
        {
            double totalCostAVG = this.AvgTotalCost(pobs, totalpobs);

            double fit = 0;

            pobs = this.surviveTime(pobs, totalpobs);
            Console.Write("Obtaining survivors...");
            Console.WriteLine();
            NumSurv = 0;
            List<List<int>> NewPops = new List<List<int>>();
            
            foreach (List<int> p in pobs)
            {
                fit = totalCostAVG / this.Cost(p);

                // if fit is close to 1 all pobs are doing well compared to their avg, but the avg of 100 mistakes is one mistake, si esta 
                // muy cerca de solucion final, ya no se progresa aun con esta funcion...
                if ( Math.Abs(totalCostAVG -this.Cost(p)) < 0.03 && this.Cost(p) <= totalCostAVG ) // Math.Abs(fit-limite) < 0.02
                {
                    // mata dinosaurios estancados
                    int numbf = pobs.Count;
                    double a = 0.6 * pobs.Count;
                    pobs.RemoveRange(0,(int)a);

                    // repobla para mayor cambio
                    int start = random.Next(0, 11);
                    foreach (List<int> po in this.generatePopulation(numbf-pobs.Count, start)) 
                    {
                        pobs.Add(po);
                    }
                    
                   return pobs;
                }

                // if fit of individual is more than the limit, then this pop
                if (fit > limite && this.Cost(p)< totalCostAVG)
                {
                    NewPops.Add(p);
                    Console.WriteLine("Superviviente costo: " + this.Cost(p) + " total cost(avg): " + totalCostAVG + " fit(avg/cost)" + fit);
                    NumSurv++;
                }
               

            }
            Console.WriteLine(" Numero de sobrevivientes: " + NumSurv);
            return NewPops;
        }

        //input is the fitnes result:
        public List<List<int>> breed(List<List<int>> pobs)
        {
            List<List<int>> childs = new List<List<int>>();
            List<int> path = new List<int>();
      

            // ensure the best pass to next gen
            Console.WriteLine("Getting elite...");
            for (int i = 0; i < pobs.Count; i++)
            {
                childs.Add(pobs[i]);
            }
            List<int> parent1 = new List<int>();
            List<int> parent2 = new List<int>();
 
            Console.WriteLine("Selecting parents...");
            int Breed = 100;
            for (int i = 0; i < Breed; i++)
            {

                List<int> pathG = new List<int>();
                parent1 = pobs[random.Next(0, pobs.Count)];
                parent2 = pobs[random.Next(0, pobs.Count-1)];


                //chose one (best more likely) parent, add its "desicion"
                 pathG = crossover(parent1, parent2);

                //pathG = Mutate(pathG);

                childs.Add(pathG);
            }
            if (totalPobs - Breed - NumSurv < 0)
            {
                return childs;
            }
            else
            {
                
                foreach(List<int> n in this.generatePopulation(1000 - Breed - NumSurv, 0))
                { childs.Add(n); 
                }
                return childs;
            }

        }

        public List<int> crossover(List<int> parent1, List<int> parent2)
        {
            List<int> child = new List<int>();

            // Determine which parent is smaller
            int smallerParentSize = Math.Min(parent1.Count, parent2.Count);


            // child is the smaller parent...
            if (smallerParentSize == parent1.Count)
            {
                child = parent1;
            }
            else
            {
                child = parent2;
            }

            return child;
        }





        public double Cost(List<int> pob)
        {

            double totalCost = 0;
            for (int j = 0; j < pob.Count - 1; j++)
            {
                int current = pob[j];
                int next = pob[j + 1];


                // Look for the edges between the current and next vertices
                List<(int, int, int)> edges = adjList[current];
                (int, int, int) edge = edges.Find(e => e.Item1 == next);

                // Add the weight of the edge to the total cost
                totalCost += edge.Item3;
            }
            // return the cost for one path
            return totalCost;
        }






    }
}
