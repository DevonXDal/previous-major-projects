import java.util.ArrayList;
import java.util.Random;

/**
 * Holds everything a player does, has and interacts with
 * Permadeath, autosaves delete on death (TODO).
 * Magicka (TODO).
 * Stamina (TODO).
 *
 * @author Devon X. Dalrymple
 * @version 2020.05.01
 */
public class Player
{
    // instance variables - replace the example below with your own
    private Room currentRoom; //Current room the player is in
    private int hitPoints, magickaPoints, staminaPoints; //Current status of the player
    private int hitPointsMax, magickaPointsMax, staminaPointsMax; //Maximum value for the player's status
    private Item weapon;
    private Item armor;
    private Item shield;
    private ArrayList<Item> inventory; //Inventory of the player
    private int carryWeight; //In kilograms (pound / 2.2 = kilogram)
    private int actionCount; //Number of actions the player may take
    private int actions; //Number of actions the player has taken
    private int experience, level; //Values pertaining to
    private final int[] BASE_ATTACK = {2, 4};
    private PlayerClass myClass; //Class the player chooses to play as {Warrior, Ranger, Mage, Peasant}
    private String name; //Name of player, Game handles the checks
    private Random rand;
    //private String divine;
    //private String intent; {Malicous, Neutral}
    
    /**
     * Constructor for objects of class Player.
     * @param myclass PlayerClass, handle what type of class the player is.
     * @param name String, holds the name of the player.
     * @param level int, level of the player.
     */
    public Player(PlayerClass myClass, String name, int level)
    {
        this.name = name;
        this.level = level;
        this.myClass = myClass;
        
        inventory = new ArrayList<Item>();
        rand = new Random();
        
        setUpPlayer(); //Handles most of the initialization
        
        if (!(level == 1)) {
            quickLevel();
        }
    }
    
    /*
     * Method for preparing the player for the game.
     */
    private void setUpPlayer()
    {
        if (myClass == PlayerClass.WARRIOR) {
            makeWarrior();
        }
        
        if (myClass == PlayerClass.MAGE) {
            makeMage();
        }
        
        if (myClass == PlayerClass.ROGUE) {
            makeRogue();
        }
        
        if (myClass == PlayerClass.PEASANT) {
            makePeasant();
        }
    }
    
    public int getHitPointsMax() {
		return hitPointsMax;
	}

	public String getWeapon() {
		if (weapon == null) {
			return null;
		}
		return weapon.getName();
	}

	public String getArmor() {
		if (armor == null) {
			return null;
		}
		return armor.getName();
	}

	public String getShield() {
		if (shield == null) {
			return null;
		}
		return shield.getName();
	}

	public int getActionCount() {
		return actionCount;
	}

	public void setHitPointsMax(int hitPointsMax) {
		this.hitPointsMax = hitPointsMax;
	}

	public void setWeapon(Item weapon) {
		this.weapon = weapon;
	}

	public void setArmor(Item armor) {
		this.armor = armor;
	}

	public void setShield(Item shield) {
		this.shield = shield;
	}

	public void setInventory(ArrayList<Item> inventory) {
		this.inventory = inventory;
	}

	public void setActionCount(int actionCount) {
		this.actionCount = actionCount;
	}

	public void setLevel(int level) {
		this.level = level;
	}

	/*
     * What to do for a warrior.
     * Brute-force can achieve many things.
     */
    private void makeWarrior()
    {
        hitPoints = 40;
        hitPointsMax = 40;
        
        //Magicka is not understood by the brawn
        //magickaPoints = 0;
        //magickaPointsMax = 0;
        
        staminaPoints = 40;
        staminaPointsMax = 40;
        
        actions = actionCount = 2;
        
        carryWeight = 40;
        
    }
    
    /*
     * What to do for a rogue.
     * Well-rounded and reliable choice.
     */
    private void makeRogue()
    {
        hitPoints = 30;
        hitPointsMax = 30;
        
        magickaPoints = 20;
        magickaPointsMax = 20;
        
        //Extremely fast, many actions allowed
        staminaPoints = 60;
        staminaPointsMax = 60;
        
        actions = actionCount = 3;
        
        carryWeight = 20;
        
    }
    
    /*
     * What to do for a mage.
     * High in magicka.
     */
    private void makeMage()
    {
        hitPoints = 20;
        hitPointsMax = 20;
        
        //Magicka is well understood
        magickaPoints = 80;
        magickaPointsMax = 80;
        
        staminaPoints = 20;
        staminaPointsMax = 20;
        
        actions = actionCount = 2;
        
        carryWeight = 30;
        
    }
    
    /*
     * What to do for a peasant.
     * Underdog of the game.
     * Only one action per round in the beginning.
     */
    private void makePeasant()
    {
        hitPoints = 15;
        hitPointsMax = 15;
        
        magickaPoints = 15;
        magickaPointsMax = 15;
        
        staminaPoints = 15;
        staminaPointsMax = 15;
        
        actions = actionCount = 1;
        
        carryWeight = 30;
        
    }
    
    /**
     * Handles the level and experience of the player.
     * 
     * @param xp int, experience to gain.
     */
    public void levelSystem(int xp)
    {
        experience += xp;
        while (experience >= (13*level)) {
            level++;
            levelUp();
            experience = 0;
        }
    }
    
    /*
     * Levels the player and their stats.
     */
    private void levelUp()
    {
        if (myClass == PlayerClass.WARRIOR) {
            hitPointsMax *= 1.4;
            magickaPointsMax *= 1.2;
            staminaPointsMax *= 1.2;
            actionCount++;
        }
        else if (myClass == PlayerClass.MAGE) {
            hitPointsMax *= 1.3;
            magickaPointsMax *= 1.3;
            staminaPointsMax *= 1.2;
            actionCount++;
        }
        else if (myClass == PlayerClass.ROGUE) {
            hitPointsMax *= 1.3;
            magickaPointsMax *= 1.2;
            staminaPointsMax *= 1.3;
            actionCount++;
        }
        else { //PEASANT which is able to quickly grow stronger
            hitPointsMax *= 1.63;
            magickaPointsMax *= 1.53;
            staminaPointsMax *= 1.53;
            actionCount++;
        }
        
        carryWeight *= 1.1;
        
        hitPoints = hitPointsMax;
        
        GameMain.game.printText("Level up!! You are now level " + level);
        GameMain.game.printText("Your health has been restored");
    }
    
    /*
     * Levels the player before the game starts.
     */
    private void quickLevel()
    {
        for (int i = 1; i != level; i++)
        {
            levelUp();
        }
    }
    
    /**
     * Sets the currentRoom of the player.
     * 
     * @param room Room, for the player to be assigned.
     */
    public void setCurrentRoom(Room room)
    {
        currentRoom.setPlayerHere(false);
        currentRoom = room;
        currentRoom.setPlayerHere(true);
        uniqueRoom(room);
    }
    
    /*
     * Deals with special rooms.
     * @param room Room, to test uniqueness.
     */
    private void uniqueRoom(Room room)
    {

    }
    
    /**
     * Sets the currentRoom of the player via teleportation and clears the history of rooms the player can go back to.
     * @param room Room, for the player to be assigned.
     */
    public void teleportPlayer(Room room)
    {
        GameMain.game.printText("You've been teleported"); 
        currentRoom.setPlayerHere(false);
        currentRoom = room;
        currentRoom.setPlayerHere(true);
    }
    
    /**
     * Sets the currentRoom of the player for the first time only.
     * 
     * @param room Room, for the player to be assigned.
     */
    public void setInitialRoom(Room room)
    {
        currentRoom = room;
        currentRoom.setPlayerHere(true);
    }
    
    /**
     * Gets the currentRoom of the player.
     * 
     * @return currentRoom Room, current room assigned to the player.
     */
    public Room getCurrentRoom()
    {
        return currentRoom;
    }
    
    /**
     * Gets the inventory list of the player.
     * 
     * @return inventory ArrayList<Item>, inventory of the player.
     */
    public ArrayList<Item> getInventory()
    {
        ArrayList<Item> tempList = new ArrayList<>();
        for (Item item : inventory)
        {
            tempList.add(item);
        }
        return tempList;
    }
    
    /**
     * Sets the player's carry weight, increases with a positive number, decreases with a negative number.
     * 
     * @param weight int, amount to change by in kilograms.
     */
    public void setCarryWeight(int weight)
    {
        carryWeight += weight;
    }
    
    /**
     * Adds an item to the inventory of the player.
     * 
     * @param item Item, to add.
     * @return boolean, did the item get stored successfully?
     */
    public boolean takeItem(Item item)
    {
        if ((invWeight() + item.getWeight()) <= carryWeight)
        {
            inventory.add(item);
            return true;
        }
        return false;
    }
    
    /**
     * Removes an item from the inventory of the player and drops it to the ground.
     * 
     * @param name String, item to remove.
     * @return item Item, found from name.
     */
    public Item dropItem(String name)
    {
        int i = 0;
        while (i < inventory.size())
        {
            if (inventory.get(i).getName().equals(name))
            {
                Item j = inventory.get(i);
                if (j == weapon) {
                    weapon = null;
                }
                if (j == armor) {
                    armor = null;
                }
                if (j == shield) {
                    shield = null;
                }
                inventory.remove(i);
                return j;
            }
            ++i;
        }
        return null;
    }
    
    /**
     * Removes an item from the inventory of the player and use (consumable).
     * 
     * @param name String, name of item to remove.
     */
    public void useItem(String name)
    {
        int i = 0;
        while (i < inventory.size())
        {
            if (inventory.get(i).getName().equals(name))
            {
                inventory.remove(i);
            }
            ++i;
        }
    }
    
    /**
     * Searches for an item from the inventory of the player
     * 
     * @param name String, name of item to remove
     * @return boolean, did the item get found successfully?
     */
    public boolean findItem(String name)
    {
        int i = 0;
        while (i < inventory.size())
        {
            if (inventory.get(i).getName().equals(name))
            {
                return true;
            }
            ++i;
        }
        return false;
    }
    
    /*
     * Weight of all inventory items.
     * 
     * @return currentWeight int, weight of everything in the inventory.
     */
    public int invWeight()
    {
        int currentWeight = 0;
        for (Item item : inventory)
        {
            currentWeight += item.getWeight();
        }
        return currentWeight;
    }
    
    /**
     * Lists the items in the inventory, and their total weight.
     * 
     * @return aString String, a string listing the items and the sum of their weight.
     */
    public String seeInv()
    {
        ArrayList<String> inventoryList = new ArrayList<>();
        for (Item item : inventory)
        {
            inventoryList.add(item.getName());
        }
        
        return ("in your inventory is: " + String.join(", ", inventoryList)) + "\n and the total weight is: " + invWeight() + " kilograms \n and you can carry: " + carryWeight + " kilograms";
    }
    
    /**
     * Getter for hitPoints
     * 
     * @return hitPoints int, amount of health remaining
     */
    public int getHitPoints()
    {
        return hitPoints;
    }
    
    /**
     * Getter for staminaPoints
     * 
     * @return staminaPoints int, amount of stamina remaining
     */
    public int getStaminaPoints()
    {
        return staminaPoints;
    }
    
    /**
     * Getter for magickaPoints
     * 
     * @return magickaPoints int, amount of magicka remaining
     */
    public int getMagickaPoints()
    {
        return magickaPoints;
    }
    
    /**
     * Getter for carryWeight
     * 
     * @return carryWeight int, amount of weight the player can carry in kilograms
     */
    public int getCarryWeight()
    {
        return carryWeight;
    }
    
    /**
     * Getter for name
     * 
     * @return name String, name of the player
     */
    public String getName()
    {
        return name;
    }
    
    /**
     * Getter for level
     * 
     * @return level int, level of the player
     */
    public int getLevel()
    {
        return level;
    }
    
    /**
     * Resets the actions counter
     */
    public void resetActions()
    {
        actions = actionCount; //Max number left
    }
    
    /**
     * Decreases the actions left by 1
     */
    public void action()
    {
        --actions;
    }
    
    /**
     * Getter for the number of actions left
     * 
     * @return actions int, number of actions the player can do still
     */
    public int getActions()
    {
        return actions;
    }
    
    /**
     * Setter for the number of actions left
     * 
     * @param actions int, the number of actions the player has left
     */
    public void setActions(int actions)
    {
        this.actions = actions;
    }
    
    /**
     * Tells the player how much damage they take (negative gives the player life)
     * 
     * @param damage int, amount of damage to take
     * @return actualDamage int, amount of damage taken
     */
    public int dealDamage(int damage)
    {
        int actualDamage = damage;
        if (testArmor() > 0 && damage > testArmor()) { //Armor blocks some of the damage
            damage -= testArmor();
            actualDamage = damage;
        }
        if (testArmor() > 0 && damage < testArmor()) { //Armor blocked everything
            damage = 0;
        }
        if ((hitPoints - damage) < 1) { //Player dies
            hitPoints = 0;
        }
        
        else if ((hitPoints - damage) > hitPointsMax) { //Healed completely
            hitPoints = hitPointsMax;
        }
        else { //Player takes damage
            hitPoints -= damage;
        }
        return actualDamage;
    }
    
    /**
     * Tells the player to equip something, returns boolean false if unsuccessfull
     * 
     * @param itemName String, name of item to equip
     * @return success boolean, did you successfully equip the item?
     */
    public boolean equipItem(String itemName)
    {
        int i = 0;
        boolean found = false;
        boolean inInventory = false;
        Item itemToEquip = null;
        while (i < inventory.size() && (!(found)))
        {
            if (inventory.get(i).getName().equals(itemName))
            {
                inInventory = true;
                itemToEquip = inventory.get(i);
                found = true;
            }
            ++i;
        }
        
        if (found) {
            if (itemToEquip.getType().equals("weapon")) {
                weapon = itemToEquip;
                GameMain.game.printText("You equip the weapon");
                if (weapon.getName().contains("hammer") || weapon.getName().contains("axe") && shield != null) {
                    shield = null;
                }
                return true;
            }
            else if (itemToEquip.getType().equals("armor")) {
                armor = itemToEquip;
                GameMain.game.printText("You equip the armor");
                return true;
            }
            else if (itemToEquip.getType().equals("shield")) {
                shield = itemToEquip;
                GameMain.game.printText("You equip the shield");
                if (!(weapon == null)) {
                    if (weapon.getName().contains("hammer") || weapon.getName().contains("axe") && shield != null) {
                        weapon = null;
                   }
                }
                return true;
            }
            else {
                GameMain.game.printText("This item is not able to be equiped");
                return true;
            }
        }
        
        return false;
    }
    
    /*
     * Tests the armor to see how much damage is reduced.
     * 
     * Returns 0 if no armor is on
     */
    private int testArmor()
    {
        if (armor == null) {
            return 0;
        }
        if (armor.getName().equals("wool_armor")) { //Tier 1
            return 1;
        }
        if (armor.getName().equals("leather_armor")) { //Tier 2
            return 2;
        }
        if (armor.getName().equals("chain_armor")) { //Tier 3
            return 3;
        }
        if (armor.getName().equals("copper_armor")) { //Tier 4
            return 4;
        }
        if (armor.getName().equals("bronze_armor")) { //Tier 5
            return 6;
        }
        if (armor.getName().equals("iron_armor")) { //Tier 6
            return 8;
        }
        if (armor.getName().equals("steel_armor")) { //Tier 7
            return 10;
        }
        if (armor.getName().equals("guard_armor")) {
            return 4;
        }
        
        return 0; //In case something that is not armor ends up being worn
    }
    
    /*
     * Tests the weapon to see how much more damage is done
     * 
     * Returns 0 for both parts of the array if there are not any weapons equiped
     */
    private int[] testWeapon()
    {
        int[] damageValues = new int[2]; //[0]: Minimum, [1]: Maximum
        if (weapon == null) {
            return damageValues;
        }
        
        if (weapon.getName().equals("flint_sword")) {
            damageValues[0] = 2;
            damageValues[1] = 4;
        }
        if (weapon.getName().equals("flint_dagger")) {
            damageValues[0] = 1;
            damageValues[1] = 3;
        }
        if (weapon.getName().equals("flint_axe")) {
            damageValues[0] = 6;
            damageValues[1] = 8;
        }
        if (weapon.getName().equals("tin_sword")) {
            damageValues[0] = 3;
            damageValues[1] = 5;
        }
        if (weapon.getName().equals("tin_dagger")) {
            damageValues[0] = 2;
            damageValues[1] = 3;
        }
        if (weapon.getName().equals("tin_axe")) {
            damageValues[0] = 7;
            damageValues[1] = 11;
        }
        if (weapon.getName().equals("Zuul's_hammer")) {
            damageValues[0] = 9;
            damageValues[1] = 12;
        }
        if (weapon.getName().equals("copper_sword")) {
            damageValues[0] = 4;
            damageValues[1] = 7;
        }
        if (weapon.getName().equals("copper_axe")) {
            damageValues[0] = 9;
            damageValues[1] = 13;
        }
        if (weapon.getName().equals("copper_dagger")) {
            damageValues[0] = 3;
            damageValues[1] = 6;
        }
        if (weapon.getName().equals("bronze_sword")) {
            damageValues[0] = 7;
            damageValues[1] = 12;
        }
        if (weapon.getName().equals("bronze_axe")) {
            damageValues[0] = 13;
            damageValues[1] = 17;
        }
        if (weapon.getName().equals("bronze_dagger")) {
            damageValues[0] = 5;
            damageValues[1] = 10;
        }
        if (weapon.getName().equals("iron_sword")) {
            damageValues[0] = 12;
            damageValues[1] = 17;
        }
        if (weapon.getName().equals("iron_axe")) {
            damageValues[0] = 13;
            damageValues[1] = 27;
        }
        if (weapon.getName().equals("iron_dagger")) {
            damageValues[0] = 11;
            damageValues[1] = 14;
        }
        if (weapon.getName().equals("steel_sword")) {
            damageValues[0] = 15;
            damageValues[1] = 25;
        }
        if (weapon.getName().equals("steel_axe")) {
            damageValues[0] = 16;
            damageValues[1] = 33;
        }
        if (weapon.getName().equals("steel_dagger")) {
            damageValues[0] = 14;
            damageValues[1] = 22;
        }
        if (weapon.getName().equals("flaming_sword")) {
            damageValues[0] = 20; 
            damageValues[1] = 30;
        }
        
        return damageValues; //In case a accidental weapon is equiped
    }
    
    /*
     * Tests how likely the shield is to block the damage
     * 
     * Returns the chance for eac divided by 5
     */
    private int[] testShield()
    {
        int[] blockChance = {0, 1}; //[0]: ranged, [1]: block melee (Currently everything uses a melee form of combat)
        if (shield == null) {
            return blockChance;
        }
        if (shield.getName().equals("flint_shield")) {
            blockChance[0] = 4;
            blockChance[1] = 2;
        }
        if (shield.getName().equals("tin_shield")) {
            blockChance[0] = 5;
            blockChance[1] = 3;
        }
        if (shield.getName().equals("copper_shield")) {
            blockChance[0] = 6;
            blockChance[1] = 3;
        }
        if (shield.getName().equals("bronze_shield")) {
            blockChance[0] = 6;
            blockChance[1] = 4;
        }
        if (shield.getName().equals("iron_shield")) {
            blockChance[0] = 8;
            blockChance[1] = 6;
        }
        if (shield.getName().equals("steel_shield")) {
            blockChance[0] = 9;
            blockChance[1] = 6;
        }
        
        return blockChance; //In case a accidental shield is equiped
    }
    
    /**
     * Attack!!
     * 
     * Returns an amount of damage to do to a monster.  May result in a miss! (10% chance)
     * 
     * @return damage int, amount of damage to do to a monster
     */
    public int attack()
    {
        if (rand.nextInt(20) < 2) {
            GameMain.game.printText("Your attack misses");
            return 0;
        }
        
        int warriorBonus = (myClass == PlayerClass.WARRIOR) ? 1 : 0;
        int[] profiecency = new int[2];
        int[] weaponBonus = testWeapon();
        if (!(weapon == null)) { 
            if ((myClass == PlayerClass.WARRIOR && weapon.getName().contains("sword") || weapon.getName().contains("hammer") || weapon.getName().contains("axe")) ||
                (myClass == PlayerClass.ROGUE && weapon.getName().contains("sword") || weapon.getName().contains("dagger"))) {
                profiecency[0] = 1;
                profiecency[1] = 2;
            }
        }
        int toRandom = ((level + profiecency[1] + warriorBonus + weaponBonus[1] + BASE_ATTACK[1]) - (profiecency[0] + warriorBonus + weaponBonus[0] + BASE_ATTACK[0]));
        int damage = rand.nextInt(toRandom) + (1 + profiecency[0] + warriorBonus + weaponBonus[0] + BASE_ATTACK[0]);
        return damage;
    }
    
    /**
     * Block!!
     * 
     * Tells game whether or not a block was successful or not
     * 
     * @return success boolean, was the block successful?
     */
    public boolean block()
    {
        int[] blockChance = testShield();
        if (rand.nextInt(20) > blockChance[1]) {
            return false;
        }
        else {
            return true;
        }
    }
    
    /**
     * Dodge!!
     * 
     * Tells game whether or not a dodge was successful or not (33% chance)
     * 
     * @return success boolean, was the dodge successful?
     */
    public boolean dodge()
    {
        return (rand.nextInt(3) == 2);
    }
    
    /**
     * Prints out any stats related to the player with no charge to their action points.
     */
    public void requestStats()
    {
        GameMain.game.printText("Your stats are:");
        GameMain.game.printText(hitPoints + " health out of " + hitPointsMax); //Health
        GameMain.game.printText("You are level " + level + " and have " + experience + " xp"); //Level and xp
        GameMain.game.printText("You can hold " + carryWeight + " kilograms"); //Maximum carry weight
        GameMain.game.printText("You have " + actions + " actions left"); //Actions left
        if (!(weapon == null)) {
            String weaponName = weapon.getName();
            GameMain.game.printText("Your weapon is a " + weaponName.replaceAll("_", " ")); //Equiped weapon
        }
        if (!(shield == null)) {
            String shieldName = shield.getName();
            GameMain.game.printText("Your shield is a " + shieldName.replaceAll("_", " ")); //Equiped shield
        }
        if (!(armor == null)) {
            String armorName = armor.getName();
            GameMain.game.printText("You are wearing " + armorName.replaceAll("_", " ")); //Equiped armor
        }
        
        GameMain.game.printText("");
    }
}

