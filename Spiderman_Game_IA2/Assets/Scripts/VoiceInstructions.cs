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

        Point SpiderManPos { get; set; }
        Point MaryJanePos { get; set; }

        buildCity city = new buildCity();

        // Start is called before the first frame update
        void Start()
        {
            // start
            actions.Add("save her", SaveHer);
            actions.Add("create city", CreateCity);
            actions.Add("place spider man", PlaceSpiderMan);

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
            //AStar astar = new AStar(cityGrid, );
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
            city.init(m,n);
        }

        private void PlaceSpiderMan()
        {
            // Here it is just necesary to make a transform
            Point spidermanPos = transform.position;
            city.moveSpiderman(spidermanNewPos);
        }

        private void PlaceMaryJane()
        {
            var MaryJane = GameObject.Find("Sphere");
            Point spidermanPos = transform.position;
            city.moveSpiderman(spidermanNewPos);
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
