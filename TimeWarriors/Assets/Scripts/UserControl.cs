/*
UserControl controls the user input and handles all the input to classes where the functionality is implemented.
For example UserControl calls Move() of the PlayerController class.
UserControl checks for jump and attack inputs in Update() and handles them in FixedUpdate()
This relies on the TeamUtilities InputManager
*/
using UnityEngine;

namespace PlayerCharacter
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(Attack))]

    public class UserControl : MonoBehaviour
    {
        [SerializeField]
        private PlayerID playerID = PlayerID.P1;                // PlayerID for GameManager to know
        [SerializeField]
        private TeamUtility.IO.PlayerID playerID_InputManager;  // Id for InputManager. Requires own playerID from TeamUtility Namespace. To know which Configuration to use

        private PlayerController m_Character;                   // Reference to the PlayerController
        private Attack m_Attack;                                // Attack Class
        private bool m_Enabled;                                 // Are the controls enabled?
        private bool m_Jump;                                    // Does the player jump?
        private bool m_JumpButtonDown;
        private bool m_isAttacking1;                            // Does the player use firstAttack?
        private bool m_isAttacking2;                            // Does the player use secondAttack?

        // Input Axis Names
        private string key_axis = "Horizontal";
        private string key_jump = "Jump";
        private string key_crouch = "Crouch";
        private string key_firstAttack = "FirstAttack";
        private string key_secondAttack = "SecondAttack";
        private string key_extra = "Extra";


        private void Awake() {
            
            m_Character = GetComponent<PlayerController>();
            m_Attack = GetComponent<Attack>();

            SetPlayerID(playerID);
            

            m_Enabled = true;
        
        }


        // Read the jump and attack input in Update so button presses aren't missed.
        private void Update() {
            if (m_Enabled) {
                if (!m_Jump) {
                    m_Jump = TeamUtility.IO.InputManager.GetButton(key_jump, playerID_InputManager);
                    m_JumpButtonDown = TeamUtility.IO.InputManager.GetButtonDown(key_jump, playerID_InputManager);
                }
                if (!m_isAttacking1 && !m_isAttacking2) {
                    m_isAttacking1 = TeamUtility.IO.InputManager.GetButtonDown(key_firstAttack, playerID_InputManager);
                }
                if (!m_isAttacking2 && !m_isAttacking1) {
                    m_isAttacking2 = TeamUtility.IO.InputManager.GetButtonDown(key_secondAttack, playerID_InputManager);
                }
            }
        }
        

        private void FixedUpdate() {
            if (m_Enabled) {
                // Read the inputs.
                bool crouch = TeamUtility.IO.InputManager.GetButton(key_crouch, playerID_InputManager);
                float h = TeamUtility.IO.InputManager.GetAxis(key_axis, playerID_InputManager);

                // Pass all states to the character control script and the attack script.
                m_Character.Move(h, crouch, m_Jump, m_JumpButtonDown);
                m_Attack.FirstAttack(m_isAttacking1);
                m_Attack.SecondAttack(m_isAttacking2);

                // Reset states
                m_Jump = false;
                m_JumpButtonDown = false;
                m_isAttacking1 = false;
                m_isAttacking2 = false;
            }
        }


        // Control on/off switches
        public void DisableControl() { m_Enabled = false; }
        public void EnableControl() { m_Enabled = true; }


        // Delegate PlayerID from gameManager to InputManager
        public void SetPlayerID(PlayerID pID) {
            playerID = pID;
            if (playerID == PlayerID.P1){
                gameObject.tag = "Player";
                playerID_InputManager = TeamUtility.IO.PlayerID.One;
                foreach (Transform t in transform)
                {
                    t.gameObject.tag = "Player";
                }
            } else{
                gameObject.tag = "Player2";
                playerID_InputManager = TeamUtility.IO.PlayerID.Two;
                foreach (Transform t in transform)
                {
                    t.gameObject.tag = "Player2";
                }
            }  
        }
        public PlayerID GetPlayerID(){ return playerID; }
    }
}
