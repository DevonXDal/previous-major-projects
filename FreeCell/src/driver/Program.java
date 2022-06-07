package driver;

import controller.Controller;

/**
 * The driver class for the Free Cell project. It has a single goal: start up the program with the main method and have
 * the controller begin running the game
 *
 * @author Devon X. Dalrymple
 * @version 2020.11.18
 */
public class Program
{

    public static void main(String[] args)
    {

        Controller gameController = Controller.getController();
        gameController.runGameLoop();
    }
}

