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

        private ArrayList ins_buildings;
        private GameObject ins_mary;
        private GameObject ins_spider;

        private int buildingFootprint = 3;
        // Start is called before the first frame update
        void Start()
        {
            ins_spider = new GameObject();
            ins_mary = new GameObject();
            ins_buildings = new ArrayList();
        }
        
        public City generateMap(int length, int width)
        {
            if (ins_buildings.Count != 0)
            {
                foreach (GameObject building in ins_buildings)
                {
                    Destroy(building);
                }
                ins_buildings.Clear();
            }


            City map = new City(length, width);

            //float seed = Random.Range(0, 100);

            for (int h = 0; h < map.height; h++)
            {
                for (int w = 0; w < map.width; w++)
                {
                    //int result = (int)(Mathf.PerlinNoise(w / 10.0f + seed, h / 10.0f + seed) * 10);
                    Vector3 pos = new Vector3(w * buildingFootprint, 0, h * buildingFootprint);
                    int n = Random.Range(0, buildings.Length);
                    GameObject building = Instantiate(buildings[n], pos, Quaternion.identity);
                    ins_buildings.Add(building);
                    if (n == 4)
                    {
                        map.obstructed.Add(new Point(w, h));
                    }
                }
            }

            return map;
        }

        public Point generateOrigin(City map)
        {
            if (ins_spider != null) Destroy(ins_spider);

            Point origin = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            while (map.obstructed.Contains(origin))
            {
                origin = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            }

            Vector3 posSpiderman = new Vector3(origin.X * buildingFootprint, 5, origin.Y * buildingFootprint);
            ins_spider = Instantiate(people[1], posSpiderman, Quaternion.identity);

            return origin;
        }

        public Point generateGoal(City map)
        {
            if (ins_mary != null) Destroy(ins_mary);

            Point goal = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            while (map.obstructed.Contains(goal))
            {
                goal = new Point(Random.Range(0, map.height - 1), Random.Range(0, map.width - 1));
            }

            Vector3 posWoman = new Vector3((float) (goal.X + 0.25) * buildingFootprint, 4, goal.Y * buildingFootprint);
            ins_mary = Instantiate(people[0], posWoman, Quaternion.identity);

            return goal;
        }
        
        public void SetSpiderLocation(Point location)
        {
            if (ins_spider != null) Destroy(ins_spider);
            Vector3 posSpiderman = new Vector3(location.X * buildingFootprint, 5, location.Y * buildingFootprint);
            ins_spider = Instantiate(ins_spider, posSpiderman, Quaternion.identity);
        }

        public void SetMaryPosition(Point location)
        {
            if (ins_mary != null) Destroy(ins_mary);
            Vector3 posMary = new Vector3(location.X * buildingFootprint, 5, location.Y * buildingFootprint);
            ins_mary = Instantiate(ins_mary, posMary, Quaternion.identity);
        }

        public void moveSpiderman(Point oldPosition, Point newPosition)
        {
            ins_spider.transform.Translate(System.Math.Abs(oldPosition.X - newPosition.X) * buildingFootprint, 5, System.Math.Abs(oldPosition.Y - newPosition.Y) * buildingFootprint);   
            
        }
        public void moveWoman(Point oldPosition, Point newPosition)
        {
            ins_mary.transform.Translate(System.Math.Abs(oldPosition.X - newPosition.X) * buildingFootprint, 5, System.Math.Abs(oldPosition.Y - newPosition.Y) * buildingFootprint);
            
        }

        // Update is called once per frame
        void Update()
        {

        }

    }
}