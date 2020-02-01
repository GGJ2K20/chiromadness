using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class PlayerController : MonoBehaviour
{
    //prefabs
    public Canvas HUD;
    public GameObject patient;
    public int timeLimitPerPatient;

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private float startTime;
    public Canvas instantiatedHud;
    private GameObject instantiatedPatient;
    private hudController hudCtl;
    private patientController patientCtl;
    private string[] currentCombination;
    private int currentCombinationIndex;
    private float remainingTime;
    public float secondsPerButton = 1.5f;
    private bool gameEnded;

    
    // Start is called before the first frame update
    void Start()
    {

        gameEnded = false;
        startTime = Time.time;
        instantiatedHud = Instantiate(HUD,new Vector3(0,0,0),Quaternion.identity);
        hudCtl = HUD.gameObject.GetComponent<hudController>();
        nextPatient();

        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                playerIndex = testPlayerIndex;
                playerIndexSet = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        prevState = state;
        state = GamePad.GetState(playerIndex);
       
        if (gameEnded == false && remainingTime > 0)
        {
            controllerActions();
            //keyboardActions();
            remainingTime = remainingTime - Time.deltaTime;
            hudCtl.setTimer(Mathf.FloorToInt(remainingTime), instantiatedHud.transform);
        }
        else {
            Debug.Log("GAME OVER!");
        }

        
    }

    void keyboardActions() {
        /*
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(Vector3.up, Vector3.up * -1 * moveSpeedX, 30 * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(Vector3.up, Vector3.up * 1* moveSpeedX, 30 * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = Vector3.Lerp(playerPosition, playerPosition + new Vector3(0, 1, 0) * 1 * moveSpeedY, (Time.time - startTime));

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = Vector3.Lerp(playerPosition, playerPosition + new Vector3(0, 1, 0) * -1* moveSpeedY, (Time.time - startTime));

        }

        if (Input.GetKey(KeyCode.Space))
        {
            GameObject laser_instance = Instantiate(laser_beam, gun.position, laser_beam.GetComponent<Transform>().rotation);
            laser_instance.GetComponent<Rigidbody>().AddRelativeForce(gun.GetComponent<Transform>().forward * 100);
        }
        */

    }

    void controllerActions()
    {
        bool noButtonsPressed = true;
        bool[] idle_state = { false, false, false, false, false, false, false, false, };
        bool[] btns_state = getButtonsState();

        for (int i = 0; i < btns_state.Length; ++i) {
            if (btns_state[i] == true) { noButtonsPressed = false; break; }
        }

        string currentCombinationButtonString = currentCombination[currentCombinationIndex];
       
        if (!noButtonsPressed && !checkIfOneControllerButtonPressed(currentCombinationButtonString))
        {
            hudCtl.setCombinationButton(currentCombinationIndex, "failed");
            currentCombinationIndex++;
            //endGame();
        }
        else if(checkIfOneControllerButtonPressed(currentCombinationButtonString))
        {
            hudCtl.setCombinationButton(currentCombinationIndex, "pressed");
            currentCombinationIndex++;
        }

        
        if (currentCombinationIndex >= currentCombination.Length) nextPatient();


        //Debug.Log(currentCombination[currentCombinationIndex]);
        /*
        playerTransform.position = playerPosition + new Vector3(0, state.ThumbSticks.Left.Y , 0);

        float LeftStickYDirection = state.ThumbSticks.Left.Y != 0 ? (state.ThumbSticks.Left.Y > 0 ? 1f : -1f) : 0;
        float LeftStickXDirection = state.ThumbSticks.Left.X != 0 ? (state.ThumbSticks.Left.X > 0 ? 1f : -1f) : 0;

        //Debug.Log(LeftStickXDirection);

        //mover en eje y (arriba/abajo)
        transform.position = Vector3.Lerp(playerPosition, playerPosition + new Vector3(0,1,0) * LeftStickYDirection * moveSpeedY, (Time.time - startTime));
        transform.RotateAround(Vector3.up, Vector3.up * LeftStickXDirection * moveSpeedX, 30 * Time.deltaTime);

        //disparar
        if (state.Triggers.Right > 0.5)
        {
            GameObject laser_instance = Instantiate(laser_beam, gun.position, laser_beam.GetComponent<Transform>().rotation);
            laser_instance.GetComponent<Rigidbody>().AddRelativeForce(gun.GetComponent<Transform>().forward * 100);
        }
        */


    }

    bool checkIfOneControllerButtonPressed(string btnName)
    {
        bool[] a_state = { true, false, false, false, false, false, false, false };
        bool[] b_state = { false, true, false, false, false, false, false, false };
        bool[] x_state = { false, false, true, false, false, false, false, false };
        bool[] y_state = { false, false, false, true, false, false, false, false };
        bool[] u_state = { false, false, false, false, true, false, false, false };
        bool[] d_state = { false, false, false, false, false, true, false, false };
        bool[] l_state = { false, false, false, false, false, false, true, false };
        bool[] r_state = { false, false, false, false, false, false, false, true };

        bool [] compare_state = a_state;
        bool[] btn_states = getButtonsState();

        switch (btnName) {
            case "A":
                compare_state = a_state;
                break;
            case "B":
                compare_state = b_state;
                break;
            case "X":
                compare_state = x_state;
                break;
            case "Y":
                compare_state = y_state;
                break;
            case "U":
                compare_state = u_state;
                break;
            case "D":
                compare_state = d_state;
                break;
            case "L":
                compare_state = l_state;
                break;
            case "R":
                compare_state = r_state;
                break;
            default:
                return false;
        }

        for (int i = 0;i<compare_state.Length;++i) {
            if (compare_state[i] != btn_states[i]) { return false; break; }
        }
        return true;
    }

    bool[] getButtonsState() {
        bool[] buttonsState = { false, false, false, false, false, false, false, false};
        buttonsState[0] = prevState.Buttons.A  == ButtonState.Released && state.Buttons.A  == ButtonState.Pressed;
        buttonsState[1] = prevState.Buttons.B  == ButtonState.Released && state.Buttons.B  == ButtonState.Pressed;
        buttonsState[2] = prevState.Buttons.X  == ButtonState.Released && state.Buttons.X  == ButtonState.Pressed;
        buttonsState[3] = prevState.Buttons.Y  == ButtonState.Released && state.Buttons.Y  == ButtonState.Pressed;
        buttonsState[4] = prevState.DPad.Up    == ButtonState.Released && state.DPad.Up    == ButtonState.Pressed;
        buttonsState[5] = prevState.DPad.Down  == ButtonState.Released && state.DPad.Down  == ButtonState.Pressed;
        buttonsState[6] = prevState.DPad.Left  == ButtonState.Released && state.DPad.Left  == ButtonState.Pressed;
        buttonsState[7] = prevState.DPad.Right == ButtonState.Released && state.DPad.Right == ButtonState.Pressed;
        return buttonsState;
    }

    void endGame() {
        gameEnded = true;
    }

    void nextPatient() {
        if (instantiatedPatient) {
            Destroy(instantiatedPatient);
        }
        instantiatedPatient = Instantiate(patient, Vector3.zero, Quaternion.identity);
        currentCombinationIndex = 0;
        patientCtl = instantiatedPatient.GetComponent<patientController>();
        patientCtl.init();
        currentCombination = patientCtl.getCombination();
        hudCtl.setCombination(currentCombination,instantiatedHud);
        remainingTime = (secondsPerButton * currentCombination.Length);
    }
}
