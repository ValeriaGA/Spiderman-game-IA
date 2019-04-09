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
        private DictationRecognizer dictationRecognizer;
        private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();

        List<UnityEngine.Vector3> pathPositions = new List<UnityEngine.Vector3>();

        string[] modesArray = { "start", "progress", "finalized" };

        string[] tokens;

        bool diagonal = false; // Tells if the grid support diagonals or not

        public int m, n, a; // These atributes set the size to the city.

        public buildCity city;

        Point SpiderManPos { get; set; }
        Point MaryJanePos { get; set; }

        // Start is called before the first frame update
        public void Start()
        {
            // start
            actions.Add("spider", spiderCommands);
            actions.Add("controls", controlsCommands);
            actions.Add("help", helpCommmands);

            //string[] c = { "good day", "help", "spider", "controls", "do you copy", "find", "diagonal", "reset", "random", "obstacles", "jane",
            //    "alpha", "bravo", "charlie", "delta", "echo",  "foxtrot", "golf", "hotel", "india", "juliet", "kilo", "lima", "mike",
            //    "november", "oscar", "papa", "quebec", "romeo", "sierra", "tango", "uniform", "viktor", "whiskey", "xray", "yankee", "zulu"};
            //List<string> commands = new List<string>(c);

            //for (var i = 0; i <= 9; i++)
            //    commands.Add(i.ToString());

            //keywordRecognizer = new KeywordRecognizer(commands.ToArray());
            //keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
            //keywordRecognizer.Start();

            dictationRecognizer = new DictationRecognizer();
            dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
            dictationRecognizer.Start();
            Debug.Log("Starting");
        }
        private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
        {
            Debug.Log(text);
            tokens = text.Split(' ');
            actions[tokens[0]].Invoke();
        }

        private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
        {
            Debug.Log(speech.text);
            tokens = speech.text.Split(' ');
            actions[tokens[0]].Invoke();
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
                        //speaker.SpeakAsync("To set m say controls set mike value");
                        //speaker.SpeakAsync("To set n say controls set november value");
                        //speaker.SpeakAsync("To toggle web say controls web");
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
                    case "random":
                        Debug.Log("in controls random");
                        break;
                }
            }
        }

        private void helpCommmands()
        {
            Debug.Log("Preface your command with spider or control");
        }


        private void SaveHer()
        {
            var MaryJane = GameObject.Find("Sphere");
            if (MaryJane)
            {
                var MaryJanePos = (Vector2)MaryJane.transform.position; //Vector2, z ignored
                FollowPath(MaryJanePos);
            }
        }

        // Makes the object follow the path from positions in the pathPositions
        void FollowPath(Vector2 MaryJanePos)
        {
            Grid cityGrid = new Grid(m, n);
            Dictionary<Point, Point> pathDict = new Dictionary<Point, Point>();
            AStar astar = new AStar(cityGrid, city.getOrigin(), city.getGoal(), diagonal);
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

            foreach (var position in pathList)
            {
                //transform.Translate(position);
                transform.position = (Vector3)position;
            }
        }

        private void CreateCity()
        {
            // Calls the script that handles city creation
            city.m = this.m;
            city.n = this.n;
            city.build_city(m,n);
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
            System.Console.WriteLine(keywordRecognizer);
        }
    }
}
