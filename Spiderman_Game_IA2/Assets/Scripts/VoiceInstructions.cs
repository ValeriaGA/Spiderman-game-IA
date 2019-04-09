using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System.Drawing;

namespace IA_Proyecto_1
{
    public class VoiceInstructions : MonoBehaviour
    {
        private KeywordRecognizer keywordRecognizer;
        private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();

        List<UnityEngine.Vector3> pathPositions = new List<UnityEngine.Vector3>();

        string[] modesArray = { "start", "progress", "finalized" };

        bool diagonal = false; // Tells if the grid support diagonals or not

        public int m, n, a; // These atributes set the size to the city.

        buildCity city = new buildCity();

        GameObject Spiderman;
        GameObject Maryjane;

        Point SpiderManPos { get; set; }
        Point MaryJanePos { get; set; }

        

        // Start is called before the first frame update
        void Start()
        {
            city = new buildCit();
            Spiderman = city.people[1];
            Maryjane = city.people[0];

            // start
            actions.Add("save her", SaveHer);
            actions.Add("show web", ShowWeb);
            actions.Add("create city", CreateCity);
            actions.Add("place spider man", PlaceSpiderMan);
            actions.Add("place mary jane", PlaceMaryJane);

            // Spiderman movements
            actions.Add("move spider man up", MoveSpiderManUp);
            actions.Add("move spider man down", MoveSpiderManDown);
            actions.Add("move spider man left", MoveSpiderManLeft);
            actions.Add("move spider man right", MoveSpiderManRight);

            // Mary Jane movements
            actions.Add("move mary jane up", MoveMaryJaneUp);
            actions.Add("move mary jane down", MoveMaryJaneDown);
            actions.Add("move mary jane left", MoveMaryJaneLeft);
            actions.Add("move mary jane right", MoveMaryJaneRight);

            // progress


            //Numbers
            actions.Add("zero", Back);
            actions.Add("one", Back);
            actions.Add("two", Back);
            actions.Add("three", Back);
            actions.Add("four", Back);
            actions.Add("five", Back);
            actions.Add("six", Back);
            actions.Add("seven", Back);
            actions.Add("eight", Back);
            actions.Add("nine", Back);

            // finalized
            actions.Add("restart", Restart);

            keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
            keywordRecognizer.Start();
        }

        private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
        {
            Debug.Log(speech.text);
            actions[speech.text].Invoke();
        }

        private void ShowWeb()
        {
            // Call whatever the function it is to show the web - hope it exists
            
        }

        private void SaveHer()
        {
            var MaryJane = GameObject.Find("Sphere");
            if (MaryJane)
            {
                var MaryJanePos = (Vector2)MaryJane.transform.position; //Vector2, z ignored
                //FollowPath(MaryJanePos);
            }
        }

        // Makes the object follow the path from positions in the pathPositions
        void FollowPath(Vector2 MaryJanePos)
        {
            // Grid cityGrid = new Grid(m, n);
            Dictionary<Point, Point> pathDict = new Dictionary<Point, Point>();
            AStar astar = city.astar;
            pathDict = astar.cameFrom;

            // From path to array
            Point pos;
            pos.X = (int)MaryJanePos.x;
            pos.Y = (int)MaryJanePos.y;

            ArrayList pathList = new ArrayList();
            pathList.Add(pos);
            for (int i = 0; i < pathDict.Count; i++)
            {
                pathList.Add(pathDict[pos]);
                pos = pathDict[pos];
            }
            pathList.Reverse();
            System.Console.Out(pathList);

            foreach (var position in pathList)
            {
                city.moveSpiderman(position);
            }
        }

        private void CreateCity()
        {
            // Calls the script that handles city creation

            // Spiderman position
            Point SpidermanPoint = getSpidermanPosition();

            // Maryjane position
            Point MaryjanePoint = getMaryjanePosition();

            city.m = this.m;
            city.n = this.n;
            city.init(m,n, SpidermanPoint, MaryjanePoint);
        }

        private Point getSpidermanPosition()
        {
            var SpiderMan = GameObject.Find("Spiderman");
            var SpidermanPos = (Vector2)SpiderMan.transform.position; //Vector2, z ignored
            Point SpidermanPoint;
            SpidermanPoint.X = (int)SpidermanPos.x;
            SpidermanPoint.Y = (int)SpidermanPos.y;
            return SpidermanPoint;
        }

        private Point getMaryjanePosition()
        {
            var MaryJane = GameObject.Find("Maryjane");
            var MaryjanePos = (Vector2)MaryJane.transform.position; //Vector2, z ignored
            Point MaryjanePoint;
            MaryjanePoint.X = (int)MaryjanePos.x;
            MaryjanePoint.Y = (int)MaryjanePos.y;
            return MaryjanePoint;
        }

        private void PlaceSpiderMan()
        {/*
            // Here it is just necesary to make a transform
            var Spiderman = GameObject.Find("Spiderman");
            Point spidermanPos = transform.position;
            city.moveSpiderman(spidermanNewPos);*/


            city.moveSpiderman(spidermanNewPos);
        }

        private void PlaceMaryJane()
        {
            /*var Maryjane = GameObject.Find("Maryjane");
            Point MaryjanePos = Maryjane.transform.position;
            city.moveMaryjane(NewPos);*/    
        }

        private void Restart()
        {
            // Here it is just necesary to make a transform and probably a new set size for the city

        }

        private void Back()
        {
            transform.Translate(-1, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            System.Console.WriteLine(keywordRecognizer);
        }
    }
}
