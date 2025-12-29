using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

namespace SonicBloom.Koreo.Demos
{
    [AddComponentMenu("Rhythm Fighting Game/Attack Controller Server")]
    public class AttackControllerServer : MonoBehaviour
    {
        #region Fields

        [Tooltip("The amount of time in seconds to provide before playback of the audio begins.  Changes to this value are not immediately handled during the lead-in phase while playing in the Editor.")]
        public float leadInTime;

        float leadInTimeLeft;
        float timeLeftToPlay;
        public AudioSource audioCom;

        private Dictionary<string, string> playerResults = new Dictionary<string, string>();

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            InitializeLeadIn();

            // register two player's event
            LaneController1P player1;
            LaneController2P player2;
            player1 = LaneController1P.Instance.GetComponent<LaneController1P>(); // get player script
            player2 = LaneController2P.Instance.GetComponent<LaneController2P>(); 

            player1.NoteJudged += OnNoteResultReceived; //registered
            player2.NoteJudged += OnNoteResultReceived;
        }

        void InitializeLeadIn()
        {
            // Initialize the lead-in-time only if one is specified.
            if (leadInTime > 0f)
            {
                // Set us up to delay the beginning of playback.
                leadInTimeLeft = leadInTime;
                timeLeftToPlay = leadInTime - Koreographer.Instance.EventDelayInSeconds;
            }
            else
            {
                // Play immediately and handle offsetting into the song.  Negative zero is the same as
                // zero so this is not an issue.
                audioCom.time = -leadInTime;
                audioCom.Play();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (leadInTimeLeft > 0f)
            {
                leadInTimeLeft = Mathf.Max(leadInTimeLeft - Time.unscaledDeltaTime, 0f);

            }

            // Count down the time left to play, if necessary.
            if (timeLeftToPlay > 0f)
            {
                timeLeftToPlay -= Time.unscaledDeltaTime;

                // Check if it is time to begin playback.
                if (timeLeftToPlay <= 0f)
                {
                    audioCom.time = -timeLeftToPlay;
                    audioCom.Play();

                    timeLeftToPlay = 0f;
                }
            }

            //if (timeLeftToPlay == 0f)
                //if (!audioCom.isPlaying)
                    //GameOverManager.Instance.TimeoutGameOver();
        }

        private void OnNoteResultReceived(String playerID, String judgment)
        {
            // store registered result in dictionary
            playerResults[playerID] = judgment;

            CompareNoteResults();
        }

        private void CompareNoteResults()
        {
            if (playerResults.ContainsKey("Player1") && playerResults.ContainsKey("Player2"))
            {
                string resultPlayer1 = playerResults["Player1"];
                string resultPlayer2 = playerResults["Player2"];

                // compare player rhythm result
                if (resultPlayer1 == "Perfect" || resultPlayer1 == "Good") {
                    P1Movement.Instance.Anime1P.SetTrigger("ATTACK");
                }

                if (resultPlayer2 == "Perfect" || resultPlayer2 == "Good") {
                    P2Movement.Instance.Anime2P.SetTrigger("ATTACK");
                }

                if (P2Movement.Instance.transform.position.x - P1Movement.Instance.transform.position.x <= 1.5f)
                {
                    switch (resultPlayer1)
                    {
                        case "Perfect":
                            switch (resultPlayer2)
                            {
                                case "Perfect": PowerController2P.Instance.GetPower(2); break;
                                case "Good": P2Movement.Instance.Anime2P.SetTrigger("BLOCK"); HPController2P.Instance.TakeDamage(2); PowerController2P.Instance.GetPower(1); StartCoroutine(P2Movement.Instance.RepulsedWithAnimation(0.003f)); break;
                                case "Miss": P2Movement.Instance.Anime2P.SetTrigger("LARGERBLOCK"); HPController2P.Instance.TakeDamage(3); StartCoroutine(P2Movement.Instance.RepulsedWithAnimation(0.005f)); break;
                            }
                            PowerController1P.Instance.GetPower(2);
                            break;

                        case "Good":
                            switch (resultPlayer2)
                            {
                                case "Perfect": P1Movement.Instance.Anime1P.SetTrigger("BLOCK"); HPController1P.Instance.TakeDamage(2); PowerController2P.Instance.GetPower(2); StartCoroutine(P1Movement.Instance.RepulsedWithAnimation(0.003f)); break;
                                case "Good": PowerController2P.Instance.GetPower(1); break;
                                case "Miss": P2Movement.Instance.Anime2P.SetTrigger("BLOCK"); HPController2P.Instance.TakeDamage(2); StartCoroutine(P2Movement.Instance.RepulsedWithAnimation(0.003f)); break;
                            }
                            PowerController1P.Instance.GetPower(1);
                            break;

                        case "Miss":
                            switch (resultPlayer2)
                            {
                                case "Perfect": P1Movement.Instance.Anime1P.SetTrigger("LARGERBLOCK"); HPController1P.Instance.TakeDamage(3); PowerController2P.Instance.GetPower(2); StartCoroutine(P1Movement.Instance.RepulsedWithAnimation(0.005f)); break;
                                case "Good": P1Movement.Instance.Anime1P.SetTrigger("BLOCK"); HPController1P.Instance.TakeDamage(2); PowerController2P.Instance.GetPower(1); StartCoroutine(P1Movement.Instance.RepulsedWithAnimation(0.003f)); break;
                                case "Miss": break;
                            }
                            break;
                    }
                    playerResults.Clear();
                }
                else{
                    playerResults.Clear();
                }
            }
        }

    }
}
