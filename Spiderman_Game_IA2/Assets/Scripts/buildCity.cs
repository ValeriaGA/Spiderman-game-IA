using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

namespace IA_Proyecto_1
{
    public class buildCity : MonoBehaviour
    {
        public GameObject[] buildings;
        public GameObject[] people;
        private Grid map;
        private int buildingFootprint = 3;
		private System.Boolean diagonal = false;
        private int n = 20;
        private int m = 20;

        // Start is called before the first frame update
        void Start()
        {
            map = new Grid(n, m);
            //float seed = Random.Range(0, 100);

            for (int h = 0; h < map.height; h++)
            {
                for (int w = 0; w < map.width; w++)
                {
                    //int result = (int)(Mathf.PerlinNoise(w / 10.0f + seed, h / 10.0f + seed) * 10);
                    Vector3 pos = new Vector3(w * buildingFootprint, 0, h * buildingFootprint); 
                    int n = Random.Range(0, buildings.Length);
                    Instantiate(buildings[n], pos, Quaternion.identity);
                    if (n == 5)
                    {
                        map.obstructed.Add(new Point(w, h));
                    }
                }
            }
            Point origin = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            while (map.obstructed.Contains(origin))
            {
                origin = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            }
                
            Point goal = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            while (map.obstructed.Contains(goal))
            {
                goal = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            }

            Vector3 posWoman = new Vector3(goal.X+1 * buildingFootprint, 4, goal.Y * buildingFootprint);
            Instantiate(people[0], posWoman, Quaternion.identity);

            Vector3 posSpiderman = new Vector3(origin.X * buildingFootprint, 5, origin.Y * buildingFootprint);
            Instantiate(people[1],posSpiderman , Quaternion.identity);


            AStar astar = new AStar(map, origin, goal, diagonal);
            if (astar.cameFrom.ContainsKey(goal))
            {
                Debug.LogFormat("Found path from ({0},{1}) to ({2},{3}) = \n", origin.X.ToString(), origin.Y.ToString(), goal.X.ToString(), goal.Y.ToString());
                Point current = goal;
                while (current != origin)
                {
                    Debug.LogFormat("({0},{1}) <- ", current.X.ToString(), current.Y.ToString());
                    current = astar.cameFrom[current];
                }
            }
            else
            {
                Debug.LogFormat("No path found from ({0},{1}) to ({2},{3}) = \n", origin.X.ToString(), origin.Y.ToString(), goal.X.ToString(), goal.Y.ToString());
            }

        
        }

        public void set_city_size(int n, int m)
        {
            this.n = n;
            this.m = m;
            
        }
        public void moveSpiderman(Point newPosition)
        {
            
            
        }
        public void moveWoman(Point newPosition)
        {
            
            
        }

        // Update is called once per frame
        void Update()
        {

        }



        //funciones por hacer
        //funcion para recibir el tamaño de la ciudad set_city_size(int n, int m)
        //funcion para mover a spiderman y la mujer de un edificio a otro  moveSpiderman(Point newPosition) moveWoman(Point newPosition)
    }
}