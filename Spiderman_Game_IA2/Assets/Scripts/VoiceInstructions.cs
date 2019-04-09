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
        private DictationRecognizer dictationRecognizer;
        private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();

        List<UnityEngine.Vector3> pathPositions = new List<UnityEngine.Vector3>();

        string[] modesArray = { "start", "progress", "finalized" };

        string[] tokens;

        private System.Boolean diagonal = false;

        public int m, n, a; // These atributes set the size to the city.

        //buildCity city = new buildCity();

        GameObject Spiderman;
        GameObject Maryjane;
        private int length = 20;
        private int width = 20;

        public buildCity city;

        private City map;
        private Point origin;
        private Point goal;

        Point SpiderManPos { get; set; }
        Point MaryJanePos { get; set; }

        // Start is called before the first frame update
        public void Start()
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
            map = city.generateMap(length, width);
            origin = city.generateOrigin(map);
            goal = city.generateGoal(map);

            // start
            actions.Add("spider", spiderCommands);
            actions.Add("controls", controlsCommands);
            actions.Add("help", helpCommmands);

            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
            dictationRecognizer.Start();
            Debug.Log("Starting");
        }
        private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
        {
            Debug.Log(text);
            tokens = text.Split(' ');
            if (actions.ContainsKey(tokens[0]))
            {
                actions[tokens[0]].Invoke();
            }
        }

        private void spiderCommands()
        {
            if (tokens.Length > 1)
            {
                switch (tokens[1].ToLower())
                {
                    case "help":
                        Debug.Log("in spider help");
                        //speaker.SpeakAsync("To toggle diagonal say spider diagonal");
                        //speaker.SpeakAsync("To start the animation say spider find jane");
                        //speaker.SpeakAsync("To quit say good day");
                        break;
                    case "do":
                        if (tokens.Length >= 4)
                        {
                            if (tokens[2].ToLower() == "you" && tokens[3].ToLower() == "copy")
                            {
                                Debug.Log("affirm");
                                //speaker.SpeakAsync("Affirm, Spider");
                            }
                        }
                        break;
                    case "find":
                        if (tokens.Length >= 3)
                        {
                            if (tokens[2].ToLower() == "jane")
                            {
                                //speaker.SpeakAsync("wilco, Spider");
                                Debug.Log("wilco");
                                // Navigate Astar solution path if it exists
                                SaveHer();
                            }
                        }
                        break;
                    case "diagonal":
                        if (diagonal == true)
                        {
                            diagonal = false;
                            //speaker.SpeakAsync("diagonal set to off, spider");
                            Debug.Log("diagonal off");
                        }
                        else
                        {
                            diagonal = true;
                            //speaker.SpeakAsync("diagonal set to on, spider");
                            Debug.Log("diagonal on");
                        }
                        break;
                }
            }
        }

        private void controlsCommands()
        {
            if (tokens.Length > 1)
            {
                switch (tokens[1].ToLower())
                {
                    case "help":
                        Debug.Log("in controls help");
                        //speaker.SpeakAsync("To reset spider say control reset");
                        //speaker.SpeakAsync("To randomize everything say controls random");
                        //speaker.SpeakAsync("To randomize obstacles say controls random obstacles");
                        //speaker.SpeakAsync("To randomize spider say controls random spider");
                        //speaker.SpeakAsync("To randomize jane say controls random jane");
                        //speaker.SpeakAsync("To set obstacle say controls set obstacle location");
                        //speaker.SpeakAsync("To set spider say controls set spider location");
                        //speaker.SpeakAsync("To set jane say controls set jane location");
                        //speaker.SpeakAsync("To set width say controls set mike value");
                        //speaker.SpeakAsync("To set length say controls set november value");
                        break;
                    case "do":
                        if (tokens.Length >= 4)
                        {
                            if (tokens[2].ToLower() == "you" && tokens[3].ToLower() == "copy")
                            {
                                Debug.Log("loud and clear");
                                //speaker.SpeakAsync("loud and clear, control");
                            }
                        }
                        break;
                    case "reset":
                        Debug.Log("reseting to starting position");
                        //speaker.SpeakAsync("Reseting to starting position, control");
                        break;
                    case "mike":
                        if (tokens.Length > 2)
                        {
                            Debug.LogFormat("Set grid width to {0}. Redraw map to update.", tokens[2]);
                            System.Int32.TryParse(tokens[2], out width);
                        }
                        break;
                    case "november":
                        if (tokens.Length > 2)
                        {
                            Debug.LogFormat("Set grid length to {0}. Redraw map to update.", tokens[2]);
                            System.Int32.TryParse(tokens[2], out length);
                        }
                        break;
                }
            }
        }

        private void helpCommmands()
        {
            Debug.Log("Preface your command with spider or control");
        }

        private void ShowWeb()
        {
            // Call whatever the function it is to show the web - hope it exists
        }
            
        private void FindPath()
        {
            AStar astar = new AStar(map, origin, goal, diagonal);
            if (astar.cameFrom.ContainsKey(goal))
            {
                //Debug.LogFormat("Found path from ({0},{1}) to ({2},{3}) = \n", origin.X.ToString(), origin.Y.ToString(), goal.X.ToString(), goal.Y.ToString());
                Point current = goal;
                while (current != origin)
                {
                    //Debug.LogFormat("({0},{1}) <- ", current.X.ToString(), current.Y.ToString());
                    current = astar.cameFrom[current];
                }
            }
            else
            {
                Debug.LogFormat("No path found from ({0},{1}) to ({2},{3}) = \n", origin.X.ToString(), origin.Y.ToString(), goal.X.ToString(), goal.Y.ToString());
            }
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
            map = city.generateMap(length, width);
            origin = city.generateOrigin(map);
            goal = city.generateGoal(map);
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
            
        }
    }
}
