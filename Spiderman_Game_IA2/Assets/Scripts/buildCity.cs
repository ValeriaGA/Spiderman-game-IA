using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

namespace IA_Proyecto_1
{
    public class buildCity : MonoBehaviour
    {
        public int m { get; set; } //Width of the city
        public int n { get; set; } //Lenght of the city

        public GameObject[] buildings;
        public GameObject[] people;

        private int buildingFootprint = 3;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Uninstantiate previous objects?
        public City generateMap(int length, int width)
        {
            City map = new City(length, width);

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

            return map;
        }

        public Point generateOrigin(City map)
        {
            Point origin = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            while (map.obstructed.Contains(origin))
            {
                origin = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            }

            Vector3 posSpiderman = new Vector3(origin.X * buildingFootprint, 5, origin.Y * buildingFootprint);
            Instantiate(people[1], posSpiderman, Quaternion.identity);

            return origin;
        }

        public Point generateGoal(City map)
        {
            Point goal = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            while (map.obstructed.Contains(goal))
            {
                goal = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            }

            Vector3 posWoman = new Vector3(goal.X + 1 * buildingFootprint, 4, goal.Y * buildingFootprint);
            Instantiate(people[0], posWoman, Quaternion.identity);

            return goal;
        }
        

        public void moveSpiderman(Point newPosition)
        {
            people[1].transform.Translate(newPosition.X * buildingFootprint, 4, newPosition.Y * buildingFootprint);   
            
        }
        public void moveWoman(Point newPosition)
        {
            people[0].transform.Translate(newPosition.X * buildingFootprint, 4, newPosition.Y * buildingFootprint);
            
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}