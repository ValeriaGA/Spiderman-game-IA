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

        private int length = 20;
        private int width = 20;

        public buildCity city;

        private City map;
        private Point origin;
        private Point goal;

        // Start is called before the first frame update
        public void Start()
        {
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
                        //speaker.SpeakAsync("To reset say control reset");
                        //speaker.SpeakAsync("To randomize and reset say control random");
                        //speaker.SpeakAsync("To set spider \'say controls set spider width value length value\'");
                        //speaker.SpeakAsync("To set jane \'say controls set jane width value length value\'");
                        //speaker.SpeakAsync("To set width say controls set width value");
                        //speaker.SpeakAsync("To set length say controls set length value");
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
                    case "set":
                        if (tokens.Length > 3)
                        {
                            switch (tokens[2])
                            {
                                case "width":
                                    Debug.LogFormat("Set grid width to {0}. Redraw map to update.", tokens[2]);
                                    System.Int32.TryParse(tokens[3], out width);
                                    break;
                                case "length":
                                    Debug.LogFormat("Set grid length to {0}. Redraw map to update.", tokens[2]);
                                    System.Int32.TryParse(tokens[3], out length);
                                    break;
                                case "spider":
                                    if (tokens.Length > 6)
                                    {
                                        Debug.LogFormat("Set spider position to {0}, {1}", tokens[4], tokens[6]);
                                        int x, y;
                                        System.Int32.TryParse(tokens[4], out x);
                                        origin.X = x;
                                        System.Int32.TryParse(tokens[6], out y);
                                        origin.Y = y;
                                    }
                                    break;
                                case "jane":
                                    if (tokens.Length > 6)
                                    {
                                        Debug.LogFormat("Set jane position to {0}, {1}", tokens[4], tokens[6]);
                                        int x, y;
                                        System.Int32.TryParse(tokens[4], out x);
                                        goal.X = x;
                                        System.Int32.TryParse(tokens[6], out y);
                                        goal.Y = y;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "tell":
                        if (tokens.Length > 3)
                        {
                            switch (tokens[2])
                            {
                                case "spider":
                                    Debug.LogFormat("Spider location is: {0}, {1}", origin.X, origin.Y);
                                    break;
                                case "jane":
                                    Debug.LogFormat("Jane location is: {0}, {1}", goal.X, goal.Y);
                                    break;
                                case "width":
                                    Debug.LogFormat("Current width is: {0}", width);
                                    break;
                                case "length":
                                    Debug.LogFormat("Current length is: {0}", length);
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
            }
            else
            {
                Debug.LogFormat("No path found from ({0},{1}) to ({2},{3}) = \n", origin.X.ToString(), origin.Y.ToString(), goal.X.ToString(), goal.Y.ToString());
            }

            solution.Reverse();

            foreach (Point position in solution)
            {
                //transform.Translate(position);
                city.moveSpiderman(position);
                Debug.LogFormat("Moving Spider to ({0},{1})", position.X.ToString(), position.Y.ToString());
            }
        }

        private void CreateCity()
        {
            map = city.generateMap(length, width);
            // Move spider to X Y PlaceSpiderMan() ?
            // Move jane to X Y PlaceMaryJane() ?
        }

        private void RandomCity()
        {
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
