
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashSet;
import java.util.Stack;
import java.util.concurrent.TimeUnit;

/**
 *  This is an adaptation of the World of Zuul.
 *  The labyrinth of Devon is being made to be quite difficult to win.
 *  A simple mistake is all it will take.
 * 
 *  This main class creates and initialises all the others: it creates all
 *  rooms, creates the parser and starts the game.  It also evaluates and
 *  executes the commands that the parser returns.
 * 
 * @author Michael Kölling and David J. Barnes
 * @author Devon X. Dalrymple
 * @version 2020.05.01
 */

public class Game 
{
    private Parser parser; //Handles the commands
    private Player player1; //The main player of the game, may be built later to handle multiplayer
    private RandGen randGen; //Generates the map
    private LabyrinthGUI gui;
    private int dodgeMoves;
    private String providedInput; // Input from the GUI
    
    /**
     * Create the game and initialize its internal map.
     * Sets the number of turns left in the game.
     * Holds a list of NPCs who may need to do some actions.
     * Sets up parser to handle commands.
     * 
     * @param playerClass The class of the player
     * @param gui The GUI for the game
     */
    public Game(String playerClass, LabyrinthGUI gui) 
    {
        randGen = new RandGen();
        parser = new Parser();
        this.gui = gui;
        providedInput = null;
        
        if (playerClass.equals("warrior")) {
            player1 = new Player(PlayerClass.WARRIOR, "Timmy", 1); 
        }
        if (playerClass.equals("rogue")) {
            player1 = new Player(PlayerClass.ROGUE, "Timmy", 1); 
        }
        //if (playerClass.equals("mage")) {
        //    player1 = new Player(PlayerClass.MAGE, "Timmy", 1); 
        //}
        if (playerClass.equals("peasant")) {
            player1 = new Player(PlayerClass.PEASANT, "Timmy", 1); 
        }
    }

    /**
     *  Main play routine.  Loops until end of play.
     */
    public void play() 
    {            
        player1.setInitialRoom(randGen.getStartingRoom());
        printWelcome();
       
        // Enter the main command loop.  Here we repeatedly read commands and
        // Execute them until the game is over.
                
        boolean finished = false;
        gui.displayOutput("> ");
       
        while (! finished) {
        	if (providedInput != null) {
        		gui.displayOutputLine(providedInput);
	            Command command = parser.getCommand(providedInput);
	            providedInput = null;
	            finished = processCommand(command);
	            gui.displayOutput("> ");
	            
        	}
        	gui.updateStats(player1.getHitPoints(), player1.getHitPointsMax(), player1.getLevel(), player1.invWeight(), player1.getCarryWeight(), player1.getActions(), player1.getActionCount(), player1.getWeapon(), player1.getShield(), player1.getArmor(), player1.getCurrentRoom().getExitString());
	    	try //Try to allow everything to catch up during the while loop, allows GUI updates to happen
	        {
	            TimeUnit.MILLISECONDS.sleep(50);
	        } 
	        catch(InterruptedException e) 
	        { 
	           Thread.currentThread().interrupt(); 
	        }
        }
        gui.displayOutputLine("Thank you for playing.  Good bye.");
    }
    
    /**
     * Receive input from GUI
     * 
     * @param input The input from the GUI
     */
    public void setProvidedInput(String input)
    {
    	providedInput = input;
    }

    /*
     * Print out the opening message for the player.
     */
    private void printWelcome()
    {
    	gui.displayOutputLine("");
    	gui.displayOutputLine("Welcome to the alpha version of the Labyrinth of Devon!");
    	gui.displayOutputLine("In this game, you play as a greedy adventurer who ignored 6 signs and went into the labyrinth.");
    	gui.displayOutputLine("This edition only has 3 classes and very limited enjoyment, but showcases some of the main planned features.");
    	gui.displayOutputLine("Play this game again when it is finished and fight the Terrible Demon Lord Devon through 10 floors!");
    	gui.displayOutputLine("");
    	gui.displayOutputLine("This labyrinth features random generation as its main feature.");
    	gui.displayOutputLine("Oh! and type 'help' if you need help. Enjoy!");
    	gui.displayOutputLine("");
        look();
    }
    
    /*
     * Prints the items of a room and their descriptions 
     */
    private void itemsInRoom()
    {
        ArrayList<String> items = new ArrayList<>();
        items = player1.getCurrentRoom().itemDescriptions();
        for (String item : items)
        {
            String thisString = item.replaceAll("_"," ");
            gui.displayOutputLine(thisString);
        }
    }

    /*
     * Given a command, process (that is: execute) the command.
     * @param command The command to be processed.
     * @return true If the command ends the game, false otherwise.
     */
    public boolean processCommand(Command command) 
    {
        if (player1.getActions() == 0) {
            return false;
        }
        boolean wantToQuit = false;
        
        gui.displayOutputLine("");

        if(command.isUnknown()) {
        	gui.displayOutputLine("I don't know what you mean...");
            return false;
        }

        String commandWord = command.getCommandWord();
        if (commandWord.equals("help")) {
            printHelp();
        }
        else if (commandWord.equals("go")) {
            goRoom(command);
        }
        else if (commandWord.equals("look")) {
            look();
            player1.action();
        }
        else if (commandWord.equals("take")) {
            takeItem(command);
        }
        else if (commandWord.equals("drop")) {
            dropItem(command);
        }
        else if (commandWord.equals("use")) {
            useItem(command);
        }
        else if (commandWord.equals("items")) {
            printInv();
            player1.action();
        }
        else if (commandWord.equals("attack")) {
            attack(command);
        }
        else if (commandWord.equals("equip")) {
            equipItem(command);
        }
        else if (commandWord.equals("stats")) { 
            player1.requestStats();
        }
        else if (commandWord.equals("end")) {
        	if (command.getSecondWord().equals("turn")) {
        		dodgeMoves = player1.getActions();
                player1.setActions(0);
        	}
        	else {
        		gui.displayOutputLine("End what?");
        	}
        }
        else if (commandWord.equals("quit")) {
            wantToQuit = quit(command);
        }
        
        if (player1.getActions() == 0) {
            enemyTurn();
            if (player1.getHitPoints() == 0) {
                wantToQuit = true;
                gui.displayOutputLine("You have died");
            }
            dodgeMoves = 0;
        }
        
        return wantToQuit;
    }

    /*
     * Print out some help information.
     * Here we print some stupid, cryptic message and a list of the 
     * command words.
     */
    private void printHelp() 
    {
    	gui.displayOutputLine("Your insatiable greed has led you underneath Gozer.");
    	gui.displayOutputLine("You wander the sewers for the labyrinth filled with treasure underneath.");
    	gui.displayOutputLine("");
    	gui.displayOutputLine("Use an underscore to connect multiple words");
    	gui.displayOutputLine("");
    	gui.displayOutputLine("Your command words are:");
    	gui.displayOutputLine(parser.showCommands());
    }

    /* 
     * Try to go in one direction. If there is an exit, enter
     * the new room, otherwise print an error message.
     * 
     * @param command Command, command to be used
     */
    private void goRoom(Command command) 
    {
        if(!command.hasSecondWord()) {
            // if there is no second word, we don't know where to go...
        	gui.displayOutputLine("Go where?");
            return;
        }

        String direction = command.getSecondWord();

        // Try to leave current room.
        Room nextRoom = player1.getCurrentRoom().getExit(direction);
        

        if (nextRoom == null) {
        	gui.displayOutputLine("There is no door!");
        }
        else {
            player1.setCurrentRoom(nextRoom);
            look(); 
            player1.action();
        }
    }
    
    /* 
     * Try to take an item from the room
     * 
     * @param command Command, command to be used
     */
    private void takeItem(Command command) 
    {
        if(!command.hasSecondWord()) {
            // if there is no second word, we don't know where to go...
        	gui.displayOutputLine("Take what?");
            return;
        }
        
        boolean found = false;
        String itemName = command.getSecondWord();

        for (Item item : player1.getCurrentRoom().getItemList())
        {
            if (item.getName().equals(itemName))
            {
                if (!(item.getWeight() == -1))
                {
                    if (player1.takeItem(item))
                    {
                        player1.getCurrentRoom().removeItem(itemName);
                        found = true;
                        player1.action();
                        gui.displayOutputLine("You picked up the item");
                    }
                    else 
                    {
                    	gui.displayOutputLine("Player can not hold the item, carrying too much \nand the item weighs: " + item.getWeight() + " kilogram(s)");
                        found = true;
                        player1.action();
                    }
                }
                else
                {
                	gui.displayOutputLine("You fail to pick it up");
                    found = true;
                    player1.action();
                }
            }
        }
        if (!(found))
        {
        	gui.displayOutputLine("Item was not found in the room");
        }
    }
    
    /*
     * Try to take an item from the room
     * 
     * @param command Command, command to be used
     */
    private void dropItem(Command command) 
    {
        if(!command.hasSecondWord()) {
            // if there is no second word, we don't know where to go...
        	gui.displayOutputLine("Drop what?");
            return;
        }
        
        
        String itemName = command.getSecondWord();

        if (player1.findItem(itemName)) {
            player1.getCurrentRoom().placeItem(player1.dropItem(itemName));
            player1.action();
            gui.displayOutputLine("You dropped the item");
        }
        else {
        	gui.displayOutput("The player does not have that item");
        }
    }
    
    /*
     * Try to equip an item from your inventory
     * 
     * @param command Command, command to be used
     */
    private void equipItem(Command command) 
    {
        if(!command.hasSecondWord()) {
            // if there is no second word, we don't know where to go...
        	gui.displayOutputLine("Equip what?");
            return;
        }
        
        
        String itemName = command.getSecondWord();

        if (player1.equipItem(itemName)) {
            player1.action();
            
        }
        else {
        	gui.displayOutputLine("The player does not have that item");
        }
    }
    
    /*
     * Used when the look command is used 
     */
    private void look()
    {
    	gui.displayOutputLine(player1.getCurrentRoom().getLongDescription());
        if (player1.getCurrentRoom().items())
        {
        	gui.displayOutputLine("");
        	gui.displayOutputLine("The room contains:");
            itemsInRoom();
        }
        
        if (player1.getCurrentRoom().hasMonster()) {
            String monsters = player1.getCurrentRoom().getMonsters();
            gui.displayOutputLine("");
            gui.displayOutputLine(monsters.replaceAll("_", " "));
        }
    }
    
    /*
     * "Quit" was entered. Check the rest of the command to see
     * whether we really quit the game.
     * @return true, if this command quits the game, false otherwise.
     */
    private boolean quit(Command command) 
    {
        if(command.hasSecondWord()) {
        	gui.displayOutputLine("Quit what?");
            return false;
        }
        else {
            return true;  // signal that we want to quit
        }
    }
    
    /*
     * Use an item if it is usable
     * 
     * @param command Command, command to be used
     */
    private void useItem(Command command)
    {
        if(!command.hasSecondWord()) {
            // if there is no second word, we don't know where to go...
        	gui.displayOutputLine("Use what?");
            return;
        }
        
        boolean found = false;
        String itemName = command.getSecondWord();

        for (Item item : player1.getInventory()) {
            if (item.getName().equals(itemName)) {
                found = true;
                if (tryItem(itemName, item)) {
                    player1.useItem(itemName);
                    player1.action();
                }
                else {
                	gui.displayOutputLine("This item can not be used");
                }
            }
        }
        if (!(found)) {
        	gui.displayOutputLine("Item was not found in the room");
        }
    }
    
    /*
     * Checks for an item and its ability 
     * @param String itemName
     * @param Item item
     * @return boolean was the item used?
     */
    private boolean tryItem(String itemName, Item item)
    {
        if (itemName.equals("magic_cookie")) {
            player1.setCarryWeight(5);
            gui.displayOutputLine("You start to feel less and less, the weight on your back");
            return true;
        }
        if (itemName.equals("feather_feet_potion")) {
            player1.resetActions();
            gui.displayOutputLine("You suddenly feel a lot faster");
            return true;
        }
        if (itemName.equals("lesser_health_potion")) {
            player1.dealDamage(-25);
            gui.displayOutputLine("You gained 20 health");
            return true;
        }
        if (itemName.equals("decent_health_potion")) {
            player1.dealDamage(-100);
            gui.displayOutputLine("You gained 100 health");
            return true;
        }
        
        return false;
    }
    
    /*
     * Lists the things in the player's inventory
     */
    private void printInv()
    {
    	gui.displayOutputLine(player1.seeInv());
    }
    
    /**
     * Getter for the player object.
     * 
     * @return player1 Player, player 1 of the game.
     */
    public Player getPlayer1()
    {
        return player1;
    }
    
    /*
     * Handles the turn of the enemies
     */
    private void enemyTurn()
    {
        randGen.enemyTurn(player1.getCurrentRoom().getFloor());
        player1.resetActions();
    }
    
    /**
     * Tells game what to print to the screen.
     * 
     * @param text String, text to be printed to the screen
     */
    public void printText(String text)
    {
    	gui.displayOutputLine(text);
    }
    
    /**
     * Attempt to damage the player by an enemy attack
     * 
     * @param name String, name of the enemy attacking
     * @param damage int, amount of damage to try to deal to the player
     */
    public void attackPlayer(String name, int damage)
    {
        String attackCall = ("(A) " + name + " attacked you!");
        gui.displayOutputLine(attackCall.replaceAll("_", " "));
        boolean dodged = false;
        int takenDamage = 0;
        if (!(dodgeMoves == 0)) {
            --dodgeMoves;
            dodged = player1.dodge();
        }
        
        boolean damageDone = (!(dodged || player1.block()));
        
        if (!(damageDone) && dodged) {
        	gui.displayOutputLine("You dodged the attack!");
        }
        
        if (!(damageDone) && (!(dodged))) {
        	gui.displayOutputLine("You blocked the attack!");
        }

        if (damageDone) {
            takenDamage = player1.dealDamage(damage);
        }
        
        if (!(takenDamage == 0)  && damageDone) {
        	gui.displayOutputLine("You have taken " + takenDamage + " damage!");
        }
    }
    
    /**
     * Rewards the player with xp for killing an enemy
     * 
     * @param xp int, amount of experience for the player to gain
     */
    public void rewardPlayer(int xp)
    {
    	gui.displayOutputLine("You have gained " + xp + " xp");
        player1.levelSystem(xp);
    }
    
    /**
     * Attacks an enemy if it is found in the room.  
     * If two enemies have the same name, it will choose the first one to enter the room.
     * 
     * @param command Command, command to attack the enemy
     */
    public void attack(Command command)
    {
        if(!command.hasSecondWord()) {
            // if there is no second word, we don't know where to go...
        	gui.displayOutputLine("Attack what?");
            return;
        }
        
        boolean found = false;
        String enemyName = command.getSecondWord();
        
        if (player1.getCurrentRoom().findMonster(enemyName)) {
            found = true;
            int damage = player1.attack();
            String damageYouDid ="You did " + damage + " damage to the " + enemyName;
            gui.displayOutputLine(damageYouDid.replaceAll("_", " "));
            player1.getCurrentRoom().attackMonster(enemyName, damage);
            player1.action();
            
        }

        
        if (!(found))
        {
        	gui.displayOutputLine("The monster was not found in the room");
        }
    }
    
    /**
     * Signals a boss has been killed and the player has beat the dungeon
     */
    public void gameWon()
    {
    	gui.displayOutputLine("Congratulations, you beat the alpha for Labyrinth of Devon!!!");
        processCommand(new Command("quit", null));
    }
    
    // ---- Requests from GUI ----
    
    /**
     * Grabs the names of every monster in the room into a HashSet and returns it
     * 
     * @return HashSet<String> Lists the names of every monster
     */
    public HashSet<String> findMonsters()
    {
		HashSet<String> hs = new HashSet<>();
		ArrayList<Monster> ml = player1.getCurrentRoom().getMonsterList();
		for (Monster monster : ml) {
			hs.add(monster.getName());
		}
		return hs;
    }
    
    
    /**
     * Grabs the names of every monster in the room into a HashSet and returns it
     * 
     * @return HashSet<String> Lists the names of every exit
     */
    public HashSet<String> findExits()
    {
    	HashSet<String> hs = new HashSet<>(player1.getCurrentRoom().getExits());
    	return hs;
    }
    
    /**
     * Grabs the names of every item in the room into a HashSet and returns it
     * 
     * @return HashSet<String> Lists the names of every item
     */
    public HashSet<String> findRoomItems()
    {
    	HashSet<String> hs = new HashSet<>();
		ArrayList<Item> il = player1.getCurrentRoom().getItemList();
		for (Item item : il) {
			hs.add(item.getName());
		}
		return hs;
    }
    
    /**
     * Grabs the names of every item from the player into a HashSet and returns it
     * 
     * @return HashSet<String> Lists the names of every item
     */
    public HashSet<String> findPlayerItems()
    {
    	HashSet<String> hs = new HashSet<>();
		ArrayList<Item> il = player1.getInventory();
		for (Item item : il) {
			hs.add(item.getName());
		}
		return hs;
    }
    
}

