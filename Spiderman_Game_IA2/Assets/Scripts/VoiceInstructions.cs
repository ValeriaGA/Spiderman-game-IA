using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;
using System.Drawing;
using System.Speech.Synthesis;

namespace IA_Proyecto_1
{
    public class VoiceInstructions : MonoBehaviour
    {
        private DictationRecognizer dictationRecognizer;
        private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();

        private ArrayList ins_web;

        List<UnityEngine.Vector3> pathPositions = new List<UnityEngine.Vector3>();

        //SpeechSynthesizer speaker;

        string[] modesArray = { "start", "progress", "finalized" };

        string[] tokens;

        private System.Boolean diagonal = false;

        public int m, n, a; // These atributes set the size to the city.

        private int length = 10;
        private int width = 10;

        public buildCity city;
        public GameObject web;

        private City map;
        private Point origin;
        private Point goal;

        // Start is called before the first frame update
        public void Start()
        {
            ins_web = new ArrayList();
            map = city.generateMap(length, width);

            //Debug.Log("Obstructed");
            //foreach (Point p in map.obstructed)
            //{
            //    Debug.LogFormat("{0},{1}", p.X, p.Y);
            //}

            origin = city.generateOrigin(map);
            goal = city.generateGoal(map);

            // start
            actions.Add("spider", spiderCommands);
            actions.Add("controls", controlsCommands);
            actions.Add("help", helpCommmands);

            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
            dictationRecognizer.Start();

            //speaker = new SpeechSynthesizer();
            //speaker.SetOutputToDefaultAudioDevice();
            //speaker.SelectVoice("Microsoft David Desktop");
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
                        //speaker.SpeakAsync("to toggle diagonal say spider diagonal");
                        //speaker.SpeakAsync("to start the animation say spider find jane");
                        //speaker.SpeakAsync("to quit say good day");
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
                            if (tokens[2].ToLower() == "mary")
                            {
                                //speaker.SpeakAsync("wilco, Spider");
                                Debug.Log("wilco");
                                FollowPath();
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
                        //speaker.SpeakAsync("to reset say control reset");
                        //speaker.SpeakAsync("to randomize and reset say control random");
                        //speaker.SpeakAsync("to set spider say controls set spider width value length value");
                        //speaker.SpeakAsync("to set jane say controls set jane width value length value");
                        //speaker.SpeakAsync("to set width say controls set width value");
                        //speaker.SpeakAsync("to set length say controls set length value");
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
                        Debug.Log("reseting with current values");
                        CreateCity();
                        break;
                    case "random":
                        Debug.Log("reseting with current values");
                        RandomCity();
                        break;
                    case "set":
                        if (tokens.Length > 3)
                        {
                            switch (tokens[2])
                            {
                                case "whiskey":
                                    Debug.LogFormat("Set grid width to {0}. Redraw map to update.", tokens[2]);
                                    //speaker.SpeakAsync("seting grid width to: " + tokens[2]);
                                    System.Int32.TryParse(tokens[3], out width);
                                    break;
                                case "lima":
                                    Debug.LogFormat("Set grid length to {0}. Redraw map to update.", tokens[2]);
                                    //speaker.SpeakAsync("seting grid legnth to: " + tokens[2]);
                                    System.Int32.TryParse(tokens[3], out length);
                                    break;
                                case "spider":
                                    if (tokens.Length > 6)
                                    {
                                        Debug.LogFormat("Set spider position to {0}, {1}", tokens[4], tokens[6]);
                                        //speaker.SpeakAsync("seting spiders poslition to: " + tokens[4] + ", " + tokens[6]);
                                        Point prev = origin;
                                        int x, y;
                                        System.Int32.TryParse(tokens[4], out x);
                                        origin.X = x;
                                        System.Int32.TryParse(tokens[6], out y);
                                        origin.Y = y;
                                        city.moveSpiderman(prev, origin);
                                    }
                                    break;
                                case "marie":
                                case "merry":
                                case "mary":
                                    if (tokens.Length > 6)
                                    {
                                        Debug.LogFormat("Set mary position to {0}, {1}", tokens[4], tokens[6]);
                                        //speaker.SpeakAsync("seting marys poslition to: " + tokens[4] + ", " + tokens[6]);
                                        Point prev = goal;
                                        int x, y;
                                        System.Int32.TryParse(tokens[4], out x);
                                        goal.X = x;
                                        System.Int32.TryParse(tokens[6], out y);
                                        goal.Y = y;
                                        city.moveWoman(prev, goal);
                                    }
                                    break;
                            }
                        }
                        break;
                    case "tell":
                        if (tokens.Length > 2)
                        {
                            switch (tokens[2])
                            {
                                case "spider":
                                    Debug.LogFormat("Spider location is: {0}, {1}", origin.X, origin.Y);
                                    //speaker.SpeakAsync("spider's location is: " + origin.X.ToString() + ", " + origin.Y.ToString());
                                    break;
                                case "mary":
                                    Debug.LogFormat("mary location is: {0}, {1}", goal.X, goal.Y);
                                    //speaker.SpeakAsync("mary's location is: " + goal.X.ToString() + ", " + goal.Y.ToString());
                                    break;
                                case "whiskey":
                                    Debug.LogFormat("Current width is: {0}", width);
                                    //speaker.SpeakAsync("Current width is " + width.ToString());
                                    break;
                                case "lima":
                                    Debug.LogFormat("Current length is: {0}", length);
                                    //speaker.SpeakAsync("Current length is: " + length.ToString());
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private void helpCommmands()
        {
            Debug.Log("Preface your command with spider or control");
            //speaker.SpeakAsync("Preface your command with spider or control");
        }

        // Makes the object follow the path from positions in the pathPositions
        void FollowPath()
        {
            Dictionary<Point, Point> pathDict = new Dictionary<Point, Point>();
            AStar astar = new AStar(map, origin, goal, diagonal);
            pathDict = astar.cameFrom;

            // From path to array
            Point current = goal;
            ArrayList solution = new ArrayList();

            if (astar.cameFrom.ContainsKey(goal))
            {
                while (current != origin)
                {
                    solution.Add(current);
                    current = astar.cameFrom[current];
                }
                solution.Reverse();
                Point prev = origin;
                foreach (Point position in solution)
                {
                    //transform.Translate(position);
                    city.moveSpiderman(prev, position);
                    //city.SetSpiderLocation(position);
                    Debug.LogFormat("Moving Spider to ({0},{1})", position.X.ToString(), position.Y.ToString());

                    //Instantiate web
                    Vector3 pos = new Vector3(position.X * 3, 9, position.Y * 3);
                    GameObject web2 = Instantiate(web, pos, Quaternion.identity);
                    web2.transform.Rotate(new Vector3(90, 0, 0));
                    ins_web.Add(web2);

                    prev = position;
                }
            }
            else
            {
                Debug.LogFormat("No path found from ({0},{1}) to ({2},{3}) = \n", origin.X.ToString(), origin.Y.ToString(), goal.X.ToString(), goal.Y.ToString());
            }
            
            
        }

        private void CreateCity()
        {
            map = city.generateMap(length, width);
            city.moveWoman(goal, goal);
            city.moveSpiderman(origin, origin);
        }

        private void RandomCity()
        {
            foreach (GameObject web_o in ins_web)
            {
                Destroy(web_o);
            }

            map = city.generateMap(Random.Range(0, 35), Random.Range(0, 35));
            origin = city.generateOrigin(map);
            goal = city.generateGoal(map);
        }

        private void PlaceSpiderMan()
        {
            // Here it is just necesary to make a transform
            //Point spidermanPos = transform.position;
            //city.moveSpiderman(spidermanNewPos);
        }

        private void PlaceMaryJane()
        {
            //var MaryJane = GameObject.Find("Sphere");
            //Point spidermanPos = transform.position;
            //city.moveSpiderman(spidermanNewPos);
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
