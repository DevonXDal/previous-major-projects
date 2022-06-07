package controller;

import model.*;
import org.jetbrains.annotations.Nullable;
import view.DisplayGUI;

import java.util.HashMap;
import java.util.concurrent.TimeUnit;

/**
 * The class Controller is a class that represents the controller part of MVC. It has coupling issues between itself and
 * the DisplayGUI, however, it completely seperates the view from interacting with the model. It also starts and runs
 * the solitaire game. It maintains the state of the solitaire game and receives incoming user input from the view.
 *
 * @author Devon X. Dalrymple
 * @version 2020-12-10
 */
public class Controller
{
    private static Controller controller; //Singleton
    private DisplayGUI display; //Display and user input elements
    private GraphicalDeck deck; //The deck with all the cards
    private Table table; //The table model that allows access to the free cell set up
    private ConfigurationHandler configurationHandler; //Used to grab configuration information
    private HashMap<String, Integer> currentConfig; //Current Configuration
    private int move; //The move the player is on
    private long startTimeMillis; //The number of milliseconds that it was at when the game started
    volatile private boolean playing; //Is the game still being played?
    private CardCollection pointerToSource; //Pointer to the selected card collection
    private boolean handlingCard; //Is a selected card being handled right now?
    volatile private boolean restartGame; //Does the user wish to restart the game?
    private boolean loopFinished; //Has the loop finished execution?


    /*
    Hidden constructor that sets up values for the singleton
     */
    private Controller()
    {
        handlingCard = false;
        configurationHandler = new ConfigurationHandler();
        currentConfig = new HashMap<>(); //Defaylt Settings
        currentConfig.put("Resolution", 0); //Default Resolution Settings
        currentConfig.put("Background", 0); //Default Background
        currentConfig.put("Difficulty", 0); //Normal Difficulty
    }

    /**
     * Creates a controller singleton if it does not currently exist. Returns the singleton for use.
     *
     * @return Controller singleton
     */
    public static Controller getController()
    {
        if (controller == null)
        {
            controller = new Controller();
        }
        return controller;
    }

    /**
     * Runs the game after setting up the necessary variables. Will allow user input to be made and will handle
     * the turns, time and the like. The backbone heart of the game it will keep everything updated as necessary to
     * provide a good experience for the user.
     */
    public void runGameLoop()
    {
        if (configurationHandler.readConfiguration()) //Try to read it
        {
            currentConfig = configurationHandler.getConfig();
        }
        else //Use default and to write default settings to a file
        {
            if (!configurationHandler.writeConfiguration(currentConfig))
            {
            }
        }
        if (display != null)
        {
            display.trashDisplay();
        }
        pointerToSource = null;
        display = new DisplayGUI(this);
        loopFinished = false;
        deck = new GraphicalDeck();
        table = new Table();
        restartGame = false;
        move = 0;

        if (readConfig("Difficulty") > 0)
        {
            Card[] cards;
            if (readConfig("Difficulty") == 1)
            {
                cards = deck.retrieveAces(2);
            }
            else
            {
                cards = deck.retrieveAces(4);
            }

            for (Card card : cards)
            {
                for (int i = 1; i < 5; i++)
                {
                    if (table.getFoundation(i).checkLegal(card))
                    {
                        table.getFoundation(i).addCard(card);
                        break;
                    }
                }
            }
        }

        { //Scope to clean up i after use
            int i = 1;
            while (deck.getSize() > 0)
            {
                table.getTableau().getCardColumn(i++).addCard(deck.drawTop());
                if (i > 8)
                {
                    i = 1;
                }
            }
        }

        playing = true;
        startTimeMillis = System.currentTimeMillis();
        updateDisplay();
        updateDisplayStatistics("N/A");
        while (playing)
        {

            updateDisplayStatistics(null);
            try //This try-catch block allows the display to update the data displayed
            {
                TimeUnit.MILLISECONDS.sleep(4);
            }
            catch(InterruptedException e)
            {
                Thread.currentThread().interrupt();
            }
        }

        if (restartGame)
        {
            runGameLoop();
        }
        loopFinished = true;
    }

    /**
     * It does one of three things depending on the given circumstances:
     * <ul>
     *     <li>
     *         If no card column is selected, it selects it
     *     </li>
     *     <li>
     *         If the card column that is selected is the same as the card column specified, it deselects it
     *     </li>
     *     <li>
     *         If a card column is selected but the one chosen is different, the card is moved from the source to the destination.
     *     </li>
     * </ul>
     *
     * The user will notice if the move was not legal or otherwise could not be done.
     *
     * There is a number of options:
     * <ul>
     *     <li>
     *         The four free cells can be selected with 1-4 with one being the leftmost and four the rightmost.
     *     </li>
     *     <li>
     *         The four foundations can be selected with 5-8 with five being the leftmost and eight the rightmost.
     *     </li>
     *     <li>
     *         The eight card columns can be selected with 9-16 with nine being the leftmost and sixteen the rightmost.
     *     </li>
     * </ul>
     *
     * If a move is made and all works well, it will update the display and process the move.
     *
     * @param option The option from the options listed above (a value between 1-16)
     */
    public void selectCard(int option)
    {
        Runnable runnable =
                () -> {
                    boolean updateTheDisplay = false;

                    if (handlingCard)
                    {
                        return;
                    }
                    handlingCard = true;
                    if (pointerToSource == null)
                    {
                        pointerToSource = switchForPointer(option);
                        if (pointerToSource.getCard() == null)
                        {
                            updateDisplayStatistics("Card Selected: None, Sorry no Card to Select There");
                            pointerToSource = null;
                        }
                        else
                        {
                            updateDisplayStatistics("Card Selected: " + pointerToSource.getCard().toString());
                        }
                    }
                    else
                    {
                        if (pointerToSource == switchForPointer(option))
                        {
                            pointerToSource = null;
                            updateDisplayStatistics("Card Selected: None");
                        }
                        else
                        {
                            CardCollection pointerToDestination = switchForPointer(option);
                            if (pointerToDestination.checkLegal(pointerToSource.getCard()))
                            {
                                pointerToDestination.addCard(pointerToSource.removeCard());
                                move++;
                                updateDisplayStatistics("Card Selected: None, Card Moved Successfully");
                                updateTheDisplay = true;
                                pointerToSource = null;
                            }
                            else
                            {
                                updateDisplayStatistics("Card Selected: " + pointerToSource.getCard().toString() + ", Illegal Move Attempted");
                            }
                        }
                    }
                    handlingCard = false;
                    if (updateTheDisplay)
                    {
                        updateDisplay();
                        checkForEndOfGame();
                    }
                };
        Thread thread = new Thread(runnable);
        thread.setPriority(Thread.MAX_PRIORITY);
        thread.start();

    }

    /**
     * Reads the configuration information and returns it
     *
     * @param key The key to find the value
     * @return The value assigned to the key
     */
    public int readConfig(String key)
    {
        return currentConfig.get(key);
    }

    /**
     * Tells the controller to change configuration settings
     *
     * @param dataToAlter The keys with the values they should be changed to (No vertical pipes or semicolons)
     * @return true if the config was altered
     */
    public boolean alterConfig(HashMap<String, Integer> dataToAlter)
    {
        return configurationHandler.alterConfiguration(dataToAlter);
    }

    /**
     * Calls the game to end by the player.
     */
    public void endGame()
    {
        playing = false;
    }

    /**
     * Calls the game to be restarted by the player
     */
    public void restartGame()
    {
        if (loopFinished)
        {
            runGameLoop();
        }
        else
        {
            playing = false;
            restartGame = true;
        }
    }

    /*
    Updates the general display but not the statistics on the display
     */
    private void updateDisplay()
    {
        String[] freeCellImages = new String[4];
        String[] foundationImages = new String[4];
        Card currentCard;
        for (int i = 1; i < 5; i++) //Grab the images from the cards or assign null if no card exists
        {
            currentCard = table.getFoundation(i).getCard();
            if (currentCard != null)
            {
                foundationImages[i-1] = currentCard.getImage();
            }
            currentCard = table.getFreeCell(i).getCard();
            if (currentCard != null)
            {
                freeCellImages[i-1] = currentCard.getImage();
            }
        }
        display.updateTable(table.getTableau().getCardImages(), freeCellImages, foundationImages);
    }

    /*
    Updates the statistics but not the general display, ran frequently
     */
    private void updateDisplayStatistics(@Nullable String extraInformation)
    {
        double seconds = ((System.currentTimeMillis() - startTimeMillis) / 1000d); //How many seconds has this round been played?
        display.updateExtraInformation(seconds, move, extraInformation);

    }

    /*
    Check to see whether the game has been won.
     */
    private void checkForEndOfGame()
    {
        boolean gameWon = true;
        for (int i = 1; i < 5; i++)
        {
            if (table.getFoundation(i).checkSize() != 13)
            {
                gameWon = false;
                break;
            }
        }
        if (gameWon)
        {
            playing = false;
            updateDisplayStatistics("Good Job!!");
            display.gameWon();
        }
    }

    /*
    Returns a CardCollection pointer to use
     */
    private CardCollection switchForPointer(int option)
    {
        switch (option)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                return table.getFreeCell(option);
            case 5:
            case 6:
            case 7:
            case 8:
                return table.getFoundation(option - 4);
            default:
                return table.getTableau().getCardColumn(option - 8);
        }
    }
}
