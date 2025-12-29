using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SonicBloom.Koreo.Demos {
    public class PaceControllerServer : MonoBehaviour
    {

        public bool canMove;
        private Dictionary<string, string> playerCommands = new Dictionary<string, string>();

        // Start is called before the first frame update
        void Start()
        {
            PaceLaneController1P pace1;
            PaceLaneController2P pace2;
            
            pace1 = PaceLaneController1P.Instance.GetComponent<PaceLaneController1P>();
            pace2 = PaceLaneController2P.Instance.GetComponent<PaceLaneController2P>();

            pace1.moveCommand += OnMoveCommandReceived;
            pace2.moveCommand += OnMoveCommandReceived;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMoveCommandReceived(String playerID, String direction) 
        {
            playerCommands[playerID] = direction;
            checkCanMove();
        }

        private void checkCanMove()
        {
            if (playerCommands.ContainsKey("Player1") && playerCommands.ContainsKey("Player2"))
            {

                string directPlayer1 = playerCommands["Player1"];
                string directPlayer2 = playerCommands["Player2"];

                if (directPlayer1 == "Forward" && directPlayer2 == "Forward") {
                    float distance = P2Movement.Instance.transform.position.x - P1Movement.Instance.transform.position.x;
                    if (distance <= 1) {
                        canMove = false;
                    }
                }
            }
        }
    }
}
