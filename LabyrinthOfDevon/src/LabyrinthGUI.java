import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Container;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.Toolkit;
import java.awt.event.KeyEvent;
import java.util.concurrent.TimeUnit;

import javax.swing.JComboBox;
import javax.swing.JDialog;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;
import javax.swing.JTextField;
import javax.swing.KeyStroke;
import javax.swing.WindowConstants;

/**
 * The pretty side of the Labyrinth of Devon, bringing a nice GUI to an origonally console only game.  It makes use
 * 
 * @author Devon X. Dalrymple
 * @version 2020.05.01
 *
 */
public class LabyrinthGUI {

	private JFrame frame;
	
	private JMenu gameMenu;
	private JMenu helpMenu;
	private JMenu commandMenu;
	
	private JMenuItem quitGameMenuItem;
	private JMenuItem forceQuitMenuItem;
	
	private JMenuItem commandHelpMenuItem;
	private JMenuItem programInfoMenuItem;
	private JMenuItem gameMechanicsMenuItem;
	
	private JMenuItem goRoomMenuItem;
	private JMenuItem attackMonsterMenuItem;
	private JMenuItem takeItemMenuItem;
	private JMenuItem equipItemMenuItem;
	private JMenuItem useItemMenuItem;
	private JMenuItem dropItemMenuItem;
	private JMenuItem helpMenuItem;
	private JMenuItem lookMenuItem;
	private JMenuItem itemsMenuItem;
	private JMenuItem statsMenuItem;
	private JMenuItem endTurnMenuItem;
	private JMenuItem quitMenuItem;
	
	private JLabel playerHealthLabel;
	private JLabel playerLevelLabel;
	private JLabel playerInventoryLabel;
	private JLabel playerActionsLabel;
	private JLabel playerWeaponLabel;
	private JLabel playerShieldLabel;
	private JLabel playerArmorLabel;
	private JLabel roomExitsLabel;
	
	private JTextArea outputTextArea;
	
	private JTextField inputTextField;
	
	/**
	 * Creates and sets up the GUI of the game in order for the game to be used, without this nothing can be seen on the users screen
	 */
	public LabyrinthGUI()
	{
		makeFrame();
	}
	
	/**
	 * Creates the JFrame and the contentPane, along with what the contentPane contains
	 * 
	 */
	public void makeFrame()
	{
		frame = new JFrame("Labyrinth of Devon Alpha");
		makeMenuBar();
		
		Container contentPane = frame.getContentPane();
		BorderLayout contentLayout = new BorderLayout();
		contentPane.setLayout(contentLayout);
		
		JPanel rightPanel = new JPanel();
		JPanel leftPanel = new JPanel();
		
		BorderLayout rightLayout = new BorderLayout();
		GridLayout leftLayout = new GridLayout(8,1);
		
		
		rightPanel.setLayout(rightLayout);
		leftPanel.setLayout(leftLayout);
		rightPanel.setBackground(new Color(28, 18, 97));
		leftPanel.setBackground(new Color(28, 18, 97));
		
		contentPane.add(leftPanel, BorderLayout.WEST);
		contentPane.add(rightPanel, BorderLayout.CENTER);
		
		playerHealthLabel = new JLabel("Player's Health: N/A");
		playerHealthLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		playerHealthLabel.setForeground(new Color(245, 245, 245));
		playerLevelLabel = new JLabel("Player's Level: N/A");
		playerLevelLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		playerLevelLabel.setForeground(new Color(245, 245, 245));
		playerActionsLabel = new JLabel("Player's Actions: N/A");
		playerActionsLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		playerActionsLabel.setForeground(new Color(245, 245, 245));
		playerInventoryLabel = new JLabel("Player's Inventory: N/A");
		playerInventoryLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		playerInventoryLabel.setForeground(new Color(245, 245, 245));
		playerWeaponLabel = new JLabel("Player's Weapon: N/A");
		playerWeaponLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		playerWeaponLabel.setForeground(new Color(245, 245, 245));
		playerShieldLabel = new JLabel("Player's Shield: N/A");
		playerShieldLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		playerShieldLabel.setForeground(new Color(245, 245, 245));
		playerArmorLabel = new JLabel("Player's Armor: N/A");
		playerArmorLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		playerArmorLabel.setForeground(new Color(245, 245, 245));
		roomExitsLabel = new JLabel("No Current Room");
		roomExitsLabel.setFont(new Font("Lucida Bright", Font.BOLD, 14));
		roomExitsLabel.setForeground(new Color(245, 245, 245));
		
		leftPanel.add(playerHealthLabel);
		leftPanel.add(playerLevelLabel);
		leftPanel.add(playerActionsLabel);
		leftPanel.add(playerInventoryLabel);
		leftPanel.add(playerWeaponLabel);
		leftPanel.add(playerShieldLabel);
		leftPanel.add(playerArmorLabel);
		leftPanel.add(roomExitsLabel);
		leftPanel.setPreferredSize(new Dimension(300, contentPane.getHeight()));
		
		outputTextArea = new JTextArea(20, 65);
		outputTextArea.setFont(new Font("Lucida Bright", Font.PLAIN, 14));
		outputTextArea.setBackground(new Color(245, 245, 245));
		outputTextArea.setForeground(new Color(10, 10, 10));
		outputTextArea.setEditable(false);
		JScrollPane scrollableOutput = new JScrollPane (outputTextArea, 
				   JScrollPane.VERTICAL_SCROLLBAR_ALWAYS, JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
		
		inputTextField = new JTextField();
		inputTextField.setFont(new Font("Lucida Bright", Font.PLAIN, 18));
		inputTextField.setPreferredSize(new Dimension(rightPanel.getWidth(), 50));
		inputTextField.setBackground(new Color(245, 245, 245));
		inputTextField.setForeground(new Color(10, 10, 10));
		inputTextField.addActionListener(e -> sendInput(inputTextField.getText()));
		
		rightPanel.add(scrollableOutput, BorderLayout.CENTER);
		rightPanel.add(inputTextField, BorderLayout.SOUTH);
		
		
		frame.pack(); //Add everything in
        frame.setSize(1280, 720); //Resize it
        frame.setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE); //Set the correct closing method
        frame.setMinimumSize(new Dimension(1200, 675)); //Enforce a minimum size
        Dimension d = Toolkit.getDefaultToolkit().getScreenSize(); //Get computer's resolution
        frame.setLocation(d.width/2 - frame.getWidth()/2, d.height/2 - frame.getHeight()/2); //Center GUI on screen after creating
        frame.setVisible(true); //Show it to the user
		
	}
	
	/**
	 * Makes the menubar, along with its menus and the menus' items
	 */
	public void makeMenuBar()
	{
		final int SHORTCUT_MASK = Toolkit.getDefaultToolkit().getMenuShortcutKeyMask();

        JMenuBar menubar = new JMenuBar();
        frame.setJMenuBar(menubar);
	        
	    gameMenu = new JMenu("Game");
	    helpMenu = new JMenu("Help");
		commandMenu = new JMenu("Command");
	    
	    quitGameMenuItem = new JMenuItem("Quit Game");
        quitGameMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_Q, SHORTCUT_MASK));
        quitGameMenuItem.addActionListener(e -> doQuitGame());
    
        forceQuitMenuItem = new JMenuItem("Force Quit");
        forceQuitMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_Q, SHORTCUT_MASK | KeyEvent.SHIFT_MASK));
        forceQuitMenuItem.addActionListener(e -> doForceQuit());
        
        gameMenu.add(quitGameMenuItem);
        gameMenu.add(forceQuitMenuItem);
	    
        programInfoMenuItem = new JMenuItem("About Program");
        programInfoMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_I, SHORTCUT_MASK));
        programInfoMenuItem.addActionListener(e -> doProgramInfo());
    
        commandHelpMenuItem = new JMenuItem("Commands");
        commandHelpMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_H, SHORTCUT_MASK));
        commandHelpMenuItem.addActionListener(e -> doShowCommands());
        
        gameMechanicsMenuItem = new JMenuItem("Game Mechanics");
        gameMechanicsMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_M, SHORTCUT_MASK));
        gameMechanicsMenuItem.addActionListener(e -> doShowMechanics());
	    
	    helpMenu.add(programInfoMenuItem);
	    helpMenu.add(commandHelpMenuItem);
	    helpMenu.add(gameMechanicsMenuItem);
	    
	    goRoomMenuItem = new JMenuItem("Go to Room");
	    goRoomMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_1, SHORTCUT_MASK));
	    goRoomMenuItem.addActionListener(e -> doChangeRoom());
	    
	    attackMonsterMenuItem = new JMenuItem("Attack Monster");
	    attackMonsterMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_2, SHORTCUT_MASK));
	    attackMonsterMenuItem.addActionListener(e -> doAttackMonster());
	    
	    takeItemMenuItem = new JMenuItem("Pick up Item");
	    takeItemMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_3, SHORTCUT_MASK));
	    takeItemMenuItem.addActionListener(e -> doPickUp());
	    
	    equipItemMenuItem = new JMenuItem("Equip Item");
	    equipItemMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_4, SHORTCUT_MASK));
	    equipItemMenuItem.addActionListener(e -> doEquip());
	    
	    useItemMenuItem = new JMenuItem("Use Item");
	    useItemMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_5, SHORTCUT_MASK));
	    useItemMenuItem.addActionListener(e -> doUse());
	    
	    dropItemMenuItem = new JMenuItem("Drop Item");
	    dropItemMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_6, SHORTCUT_MASK));
	    dropItemMenuItem.addActionListener(e -> doDrop());
	    
	    helpMenuItem = new JMenuItem("List Commands");
	    helpMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_7, SHORTCUT_MASK));
	    helpMenuItem.addActionListener(e -> doOther("help"));
	    
		lookMenuItem = new JMenuItem("Look around Room");
		lookMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_8, SHORTCUT_MASK));
		lookMenuItem.addActionListener(e -> doOther("look"));
		
		itemsMenuItem = new JMenuItem("Display Items Held");
		itemsMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_9, SHORTCUT_MASK));
		itemsMenuItem.addActionListener(e -> doOther("items"));
		
		statsMenuItem = new JMenuItem("Player Stats");
		statsMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_0, SHORTCUT_MASK));
		statsMenuItem.addActionListener(e -> doOther("stats"));
		
	    endTurnMenuItem = new JMenuItem("End Turn");
	    endTurnMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_MINUS, SHORTCUT_MASK));
	    endTurnMenuItem.addActionListener(e -> doOther("end turn"));
	    
		quitMenuItem = new JMenuItem("Quit Game");
		quitMenuItem.setAccelerator(KeyStroke.getKeyStroke(KeyEvent.VK_EQUALS, SHORTCUT_MASK));
		quitMenuItem.addActionListener(e -> doOther("quit"));
	    
	    commandMenu.add(goRoomMenuItem);
	    commandMenu.add(attackMonsterMenuItem);
	    commandMenu.add(takeItemMenuItem);
	    commandMenu.add(equipItemMenuItem);
	    commandMenu.add(useItemMenuItem);
	    commandMenu.add(dropItemMenuItem);
	    commandMenu.add(helpMenuItem);
	    commandMenu.add(lookMenuItem);
	    commandMenu.add(itemsMenuItem);
	    commandMenu.add(statsMenuItem);
	    commandMenu.add(endTurnMenuItem);
	    commandMenu.add(quitMenuItem);
	    
	    menubar.add(gameMenu);
	    menubar.add(helpMenu);
	    menubar.add(commandMenu);
	}
	
	/**
	 * Shows the current statistics of the game the player would want to know on the left side of the GUI.
	 * 
	 * @param playerHealth The current health of the player
	 * @param maxPlayerHealth The maximum amount of health the player can have
	 * @param level The player's current level
	 * @param invNow The current amount of inventory taken up
	 * @param invMax The maximum amount of inventory the player can have
	 * @param actionsNow The number of actions left this turn, the player can perform
	 * @param actionsMax The maximum number of actions a player has each turn
	 * @param playerWeapon The weapon the player currently has equipped (None if no item has been equipped)
	 * @param playerShield The shield the player currently has equipped (None if no item has been equipped)
	 * @param playerArmor The armor the player currently has equipped (None if no item has been equipped)
	 * @param roomExits The exits of the player's current room
	 */
	public void updateStats(int playerHealth, int maxPlayerHealth, int level, int invNow, int invMax, int actionsNow, int actionsMax, String playerWeapon, String playerShield, String playerArmor, String roomExits)
	{
		playerHealthLabel.setText("Player's Health: " + playerHealth + "/" + maxPlayerHealth);
		playerLevelLabel.setText("Player's Level: " + level);
		playerInventoryLabel.setText("Player's Inventory: " + invNow + "/" + invMax + " kg");
		playerActionsLabel.setText("Player's Actions: " + actionsNow + "/" + actionsMax);
		if (playerWeapon == null) {
			playerWeaponLabel.setText("Player's Weapon: None");
		}
		else {
			playerWeaponLabel.setText("Player's Weapon: " + playerWeapon);
		}
		
		if (playerShield == null) {
			playerShieldLabel.setText("Player's Shield: None");
		}
		else {
			playerShieldLabel.setText("Player's Shield: " + playerShield);
		}
		
		if (playerArmor == null) {
			playerArmorLabel.setText("Player's Armor: None");
		}
		else {
			playerArmorLabel.setText("Player's Armor: " + playerArmor);
		}
		
		roomExitsLabel.setText(roomExits);
	}
	
	/**
	 * Displays output to the screen from the program
	 * 
	 * @param text The string of text to be displayed as the output
	 */
	public void displayOutputLine(String text)
	{
		outputTextArea.append(text + "\n");
		outputTextArea.setCaretPosition(outputTextArea.getDocument().getLength());
	}
	
	/**
	 * Displays output to the screen from the program without a new line
	 * 
	 * @param text The string of text to be displayed as the output
	 */
	public void displayOutput(String text)
	{
		outputTextArea.append(text);
		outputTextArea.setCaretPosition(outputTextArea.getDocument().getLength());
	}
	
	/*
	 * Sends the users input as a command to Game
	 * 
	 * @param text The text for the Game object to handle for commands.
	 */
	private void sendInput(String text)
	{
		GameMain.game.setProvidedInput(text);
		inputTextField.setText("");	
	}
	
	/**
	 * Displays a model dialog for the player to select their class for the game
	 * 
	 * http://www.java2s.com/Tutorials/Java/Swing_How_to/JOptionPane/Create_dialog_for_JComboBox_using_JOptionPane.htm - Combo for Dialog
	 * 
	 * @return String The chosen class for the player
	 */
	public String displayClassDialog()
	{
		String[] classes = { "Warrior", "Rogue", "Peasant"};
		String[] options = {"OK", "Cancel"};
		JComboBox<String> combo = new JComboBox<>(classes);

	    String title = "Title";
	    int selection = JOptionPane.showOptionDialog(frame, combo, "Class Selection", JOptionPane.DEFAULT_OPTION, JOptionPane.QUESTION_MESSAGE, null, options, options[0]);
	    
        
       
        return ((String) combo.getSelectedItem()).toLowerCase();
	}
	
	// ---- Actions ----
	
	private void doQuitGame()
	{
		JOptionPane.showMessageDialog(frame, "Thank you for trying out Labyrinth of Devon!");
        try
        {
            TimeUnit.SECONDS.sleep(1);
        } 
        catch(InterruptedException e) 
        { 
           Thread.currentThread().interrupt(); 
        }
		System.exit(0);
	}
	
	private void doForceQuit()
	{
		System.exit(0);
	}
	
	private void doProgramInfo()
	{
		JOptionPane pane = new JOptionPane("Labyrinth of Devon GUI Alpha Edition\nDeveloped by Devon X. Dalrymple\nVersion: 0.6.0\nMade with Java Awt and Swing");
        JDialog dialog = pane.createDialog(frame, "Program Information");
        dialog.setModal(false); // this says not to block background components
        dialog.show();
	}
	
	private void doShowCommands()
	{
		JOptionPane pane = new JOptionPane("go <room> - changes the room you are in (action)\n" +
                "attack <monster> - attempts to attack a monster (action)\n" +
                "take <item> - attempts to pick up an item in the room (action)\n" +
                "equip <item> - attempts to equip an item in your inventory and can be used to re-equip the same item [pointless] (action)\n" +
                "use <item> - uses a consumable item in your inventory (action)\n" +
                "drop <item> - drops an item from your inventory to the floor (action)\n" +
                "help - prints the list of commands you can use\n" +
                "look - examines the room for information (action)\n" +
                "items - lists the items in your inventory\n" +
                "stats - lists the statistics associated with the player\n" +
                "end turn - ends your turn immediately and gives you dodge moves (50% chance of dodging the next attack) for each action left\n" +
                "quit - ends the game\n\n" +
                "You do not need underscores to select an item or monster with the command line, they are optional! Dashes in a direction are not.");
		JDialog dialog = pane.createDialog(frame, "Commands");
		dialog.setModal(false); // this says not to block background components
		dialog.show();
	}
	
	private void doShowMechanics()
	{
		JOptionPane pane = new JOptionPane("Game Type: Randomly-Generated Dungeon-Crawler\n" +
                "Random Generation: Affects the combat system, rooms, layout, staircases, monsters, bosses, and items\n\n" +
                "Attacking: Damage is randomly generated from a high and low number.  Level, class, weapons, etc., determine the high and low.\n" +
                "Defense: Monsters have a 10% chance to dodge your attack.  You have calculated chance of blocking.  You can use armor to reduce damage.  You can also dodge attacks.\n" +
                "Ending Turn Early: Gives you a dodge move for every action you had left (50% to dodge an attack.)\n\n" +
                "Inventory: You can hold so many items at once determined by your maximum carrying weight in kilograms.\n\n" +
                "Monster Movement: Monsters get a single move after the end of your turn. They can move to another room or attack.  They \"surprise attack\" you if they move into your room.\n" +
                "Surprise Attack: A monster can move and attack in one turn if they enter the room of the player.  This will announce Surprise Attack to you.\n" +
                "Boss Fight - You will be locked in a room after the third floor with a powerful boss.  They all have 3 actions they can perform. Good luck!\n\n" +
                "Leveling: The next level requires 12 times your current level in xp.  XP is reset when you level.  Your damage, health and actions increase.  You also restore your health.\n\n" +
                "Warrior: Starts with 40 health and two moves, they receive bonuses to swords and heavy weapons, and get an extra 1 attack damage at the start of the game. \n" +
                "Rogue: Starts with 30 health and three moves, they receive bonuses to daggers and swords, their extra action makes them much faster to perform actions.\n" +
                "Peasant: Starts with 15 health and one moves, a challenging choice and requires luck to play. May the divines protect you, fool.\n\n" +
                "Equipment: Items must be equipped to help you.  You can have a one-handed weapon and shield, or a two-handed weapon equipped.  You can also equip armor.");
		JDialog dialog = pane.createDialog(frame, "Game Mechanics");
		dialog.setModal(false); // this says not to block background components
		dialog.show();
	}
	
	private void doChangeRoom()
	{
		String[] rooms = GameMain.game.findExits().toArray(new String[GameMain.game.findExits().size()]);
		
		if (rooms.length == 0) {
			displayOutputLine("You can not leave");
			GameMain.game.setProvidedInput("");
			return;
		}
		
		String[] options = {"OK", "Cancel"};
		JComboBox<String> combo = new JComboBox<>(rooms);

	    int selection = JOptionPane.showOptionDialog(frame, combo, "Go to which room?", JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, null, options, options[0]);
	    
	    if (selection == 0) {
	    	sendInput("go " + ((String) combo.getSelectedItem()));
	    }
	}
	
	private void doAttackMonster()
	{
		String[] monsters = GameMain.game.findMonsters().toArray(new String[GameMain.game.findMonsters().size()]);
		
		if (monsters.length == 0) {
			displayOutputLine("There are no monsters in the room");
			GameMain.game.setProvidedInput("");
			return;
		}
		
		String[] options = {"OK", "Cancel"};
		JComboBox<String> combo = new JComboBox<>(monsters);

	    int selection = JOptionPane.showOptionDialog(frame, combo, "Attack which monster?", JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, null, options, options[0]);
	    
	    if (selection == 0) {
	    	sendInput("attack " + ((String) combo.getSelectedItem()));
	    }
	}
	
	private void doPickUp()
	{
		String[] items = GameMain.game.findRoomItems().toArray(new String[GameMain.game.findRoomItems().size()]);
		
		if (items.length == 0) {
			displayOutputLine("There are no items in the room");
			GameMain.game.setProvidedInput("");
			return;
		}
		
		String[] options = {"OK", "Cancel"};
		JComboBox<String> combo = new JComboBox<>(items);

	    int selection = JOptionPane.showOptionDialog(frame, combo, "Pickup which item?", JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, null, options, options[0]);
	    
	    if (selection == 0) {
	    	sendInput("take " + ((String) combo.getSelectedItem()));
	    }
	}
	
	private void doEquip()
	{
		String[] items = GameMain.game.findPlayerItems().toArray(new String[GameMain.game.findPlayerItems().size()]);
		
		if (items.length == 0) {
			displayOutputLine("There are no items in the player's inventory");
			GameMain.game.setProvidedInput("");
			return;
		}
		
		String[] options = {"OK", "Cancel"};
		JComboBox<String> combo = new JComboBox<>(items);

	    int selection = JOptionPane.showOptionDialog(frame, combo, "Equip which item?", JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, null, options, options[0]);
	    
	    if (selection == 0) {
	    	sendInput("equip " + ((String) combo.getSelectedItem()));
	    }
	}
	
	private void doUse()
	{
		String[] items = GameMain.game.findPlayerItems().toArray(new String[GameMain.game.findPlayerItems().size()]);
		
		if (items.length == 0) {
			displayOutputLine("There are no items in the player's inventory");
			GameMain.game.setProvidedInput("");
			return;
		}
		
		String[] options = {"OK", "Cancel"};
		JComboBox<String> combo = new JComboBox<>(items);

	    int selection = JOptionPane.showOptionDialog(frame, combo, "Use which item?", JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, null, options, options[0]);
	    
	    if (selection == 0) {
	    	sendInput("use " + ((String) combo.getSelectedItem()));
	    }
	}
	
	private void doDrop()
	{
		String[] items = GameMain.game.findPlayerItems().toArray(new String[GameMain.game.findPlayerItems().size()]);
		
		if (items.length == 0) {
			displayOutputLine("There are no items in the player's inventory");
			GameMain.game.setProvidedInput("");
			return;
		}
		
		String[] options = {"OK", "Cancel"};
		JComboBox<String> combo = new JComboBox<>(items);

	    int selection = JOptionPane.showOptionDialog(frame, combo, "Drop which item?", JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE, null, options, options[0]);
	    
	    if (selection == 0) {
	    	sendInput("drop " + ((String) combo.getSelectedItem()));
	    }
	}
	
	/*
	 * Takes the command to be used as a parameter
	 * 
	 * @param command The command to be used
	 */
	private void doOther(String command) {
		sendInput(command);
	}
}

	