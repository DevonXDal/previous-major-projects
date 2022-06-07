import java.util.ArrayList;

/**
 * This class holds an enumeration of all command words known to the game.
 * It is used to recognise commands as they are typed in.
 *
 * @author Michael Kölling and David J. Barnes
 * @author Devon X. Dalrymple 
 * @version 2019.12.11-03
 */

public class CommandWords
{
    // a constant array that holds all valid command words
    private static final String[] validCommands = {
        "go", //Go to another room 
        "quit", //Quit the game
        "help", //Ask for the commands
        "look", //Look around the room
        "take", //Pick up an item if it is able to be held
        "drop", //Drop an item from the player's inventory
        "use", //Use an item, in the inventory or or on the ground if it can not be lifted
        "items", //List the items and their descriptions in the inventory
        "attack", //Attacks an enemy in the room
        "equip", //Equip a weapon, shield or armor
        "stats", //Gives the player their information (Does not count as an action)
        "end" //Ends the turn early and gives dodging moves
        //"interact", "attack", "screech", "trade"
    };

    /**
     * Constructor - initialise the command words.
     */
    public CommandWords()
    {
        // nothing to do at the moment...
    }
    
    /**
     * Print all valid commands to System.out.
     */
    public String getCommandList()
    {
        ArrayList list = new ArrayList();
        for (String command : validCommands) {
            list.add(command);
        }
        return String.join(", ", list);
    }

    /**
     * Check whether a given String is a valid command word. 
     * @return true if a given string is a valid command,
     * false if it isn't.
     */
    public boolean isCommand(String aString)
    {
        for(int i = 0; i < validCommands.length; i++) {
            if(validCommands[i].equals(aString))
                return true;
        }
        // if we get here, the string was not found in the commands
        return false;
    }
}

