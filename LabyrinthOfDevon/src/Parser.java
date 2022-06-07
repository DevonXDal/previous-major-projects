import java.util.Scanner;

/**
 * This parser reads user input and tries to interpret it as an "Adventure"
 * command. Every time it is called it reads a line from the terminal and
 * tries to interpret the line as a two word command. It returns the command
 * as an object of class Command.
 *
 * The parser has a set of known command words. It checks user input against
 * the known commands, and if the input is not one of the known commands, it
 * returns a command object that is marked as an unknown command.
 * 
 * @author Michael Kölling and David J. Barnes
 * @author Devon X. Dalrymple 
 * @version 2019.12.9-03
 */
public class Parser 
{
    private CommandWords commands;  // holds all valid command words

    /**
     * Create a parser to read from the terminal window.
     */
    public Parser() 
    {
        commands = new CommandWords();
    }
    
    /**
     * Print out a list of valid command words.
     */
    public String showCommands()
    {
        return commands.getCommandList();
    }

    /**
     * Turns an input into a command
     * 
     * @param input The input from the user
     * @return The next command from the user.
     */
    public Command getCommand(String input) 
    {
    	
        String word1 = null;
        String word2 = null;
        Scanner reader = new Scanner(input);
     

        // Find up to two words on the line.
        if(reader.hasNext()) {
            word1 = reader.next();      // Get first word
            if(reader.hasNext()) {
                word2 = reader.next();      // Get second word
                if(reader.hasNext()) {
                    word2 = word2 + "_" + reader.next();  // Get third and combine the two
                    if(reader.hasNext()) {
                        word2 = word2 + "_" + reader.next();      // Get fourth word and combine the three
                    }
                }
            }
        }

        // Now check whether this word is known. If so, create a command
        // with it. If not, create a "null" command (for unknown command).
        if(commands.isCommand(word1)) {
            return new Command(word1, word2);
        }
        else {
            return new Command(null, word2); 
        }
    }
}

