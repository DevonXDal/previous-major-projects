import java.util.Scanner;


/**
 * Kick starts the game, creates a public, static game object that will be used to greatly reduce coupling.
 * 
 * By request of my mom, some ghostbuster references have been added.
 * 
 * Holds the main method for the game
 *
 * @author Devon X. Dalrymple
 * @version 2020.05.01
 */
public class GameMain
{
    /**
     * game is public and static object of the Game class.
     * This is public and static to allow all classes to recognize it and tell it what needs to happen.  
     * This also allows for coupling to be reduced.
     */
    public static Game game; //Main controller of the game
 
    /**
     * Starts the game by creating the game object and calling its play method.
     * 
     * Handles the player's class selection.
     */
    public static void main(String[] args)
    {
    	LabyrinthGUI gui = new LabyrinthGUI();
        game = new Game(gui.displayClassDialog(), gui);
        game.play();
    }
}

