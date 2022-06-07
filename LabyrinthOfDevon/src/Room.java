import java.util.ArrayList;
import java.util.HashMap;
import java.util.Set;

/**
 * Class Room - a room in an adventure game.
 *
 * A "Room" represents one location in the scenery of the game.  It is 
 * connected to other rooms via exits.  The exits are labelled north, 
 * east, south, west.  For each direction, the room stores a reference
 * to the neighboring room, or null if there is no exit in that direction.
 * 
 * @author  Michael Kölling and David J. Barnes
 * @author Devon X. Dalrymple
 * @version 2020.05.01
 */
public class Room 
{
    private String description; //Brief description of the room
    private HashMap<String, Room> exits; //Exits to other rooms
    private boolean hasItem; //Does the room have any items?
    private boolean hasMonster; //Does the room contain monsters?
    private boolean hasPlayer; //Does the room have the player?
    private ArrayList<Item> itemList; //List of all the items in the room
    private ArrayList<Monster> monsterList; //Monsters this room contains
    private int floor; //What floor is this room on?  The first floor is actually the highest up.
    private String type; //Type of room, is it normal or does it cause something special to happen?
    
    /**
     * Create a room described "description". Initially, it has
     * no exits. "description" is something like "a kitchen" or
     * "an open court yard".
     * 
     * @param description String, the room's description.
     * @param floor int, the floor the room is on.
     * @param type String, is the room special in any way?
     */
    public Room(String description, int floor, String type) 
    {
        this.description = description;
        exits = new HashMap<String, Room>();
        itemList = new ArrayList<Item>();
        monsterList = new ArrayList<>();
        hasItem = false;
        this.floor = floor; //Top is floor 1
        this.type = type;
    }
    
    /**
     * Checks to see if the room has any items.
     * 
     * @return hasItem boolean, does this room contain items?
     */
    public boolean items()
    {
        return hasItem;
    }
    
    /**
     * Returns and ArrayList of items and their descriptions.
     * 
     * @return itemStrings ArrayList<String>, collects the items and their descriptions to print to the screen.
     */
    public ArrayList<String> itemDescriptions()
    {
        ArrayList<String> itemStrings = new ArrayList<>();
        for (Item item : itemList)
        {
            itemStrings.add(item.getLongDescription());
        }
        return itemStrings;
    }
    
    /**
     * Getter for the list of items the room has.
     * 
     * @return justList ArrayList<Item>, creates a temporary new list to return instead of the actual itemList.
     */
    public ArrayList<Item> getItemList()
    {
        ArrayList<Item> justList = new ArrayList<>();
        for (Item item : itemList) {
            justList.add(item);
        }
        return justList;
    }
    
    /**
     * Getter for the list of monsters the room has.
     * 
     * @return justList ArrayList<Monster>, creates a temporary new list to return instead of the actual itemList.
     */
    public ArrayList<Monster> getMonsterList()
    {
        ArrayList<Monster> justList = new ArrayList<>();
        for (Monster monster : monsterList) {
            justList.add(monster);
        }
        return justList;
    }
    
    /**
     * Define an exit of this room.  
     * 
     * @param direction String, the direction of the exit.
     * @param neighbor Room, the room in the given direction.
     */
    public void setExit(String direction, Room neighbor) 
    {
        exits.put(direction, neighbor);
    }
    
    /**
     * Add an item to the room.
     * 
     * @param name String, name of the item.
     * @param description String, description of the item.
     * @param type String, what type of item is it.
     * @param weight int, how much does it weigh in kilograms.
     */
    public void addItem(String name, String description, String type, int weight)
    {
        itemList.add(new Item(name, description, type, weight));
        hasItem = true;
    }
    
    /**
     * Removes and item from the room if found
     * @param String name of item
     */
    public void removeItem(String name)
    {
        int i = 0; 
        boolean found = false;
        while (i < itemList.size() && (!found)) {
            if (itemList.get(i).getName().equals(name)) {
                itemList.remove(i);
                found = true;
            }
            ++i;
        }
        
        
        if (itemList.size() == 0) {
            hasItem = false;
        }
    }
    
    /**
     * Adds a monster into the room
     * 
     * @param monster Monster, monster to add to this room
     */
    public void addMonster(Monster monster) 
    {
        monsterList.add(monster);
        hasMonster = true;
    }
    
    /**
     * Removes a monster from this room and sets hasMonster to false if no monsters remain
     * 
     * @param monster Monster, monster to remove from this room
     */
    public void removeMonster(Monster monster)
    {
        monsterList.remove(monster);
        if (monsterList.size() == 0) {
            hasMonster = false;
        }
    }
    
    /**
     * Does this room contain monsters?
     * 
     * @return boolean, does this room have monsters
     */
    public boolean hasMonster()
    {
        return hasMonster;
    }
    
    /**
     * Get monsters in the room
     * 
     * @return monsterNames String, names of monster in the room
     */
    public String getMonsters()
    {
        ArrayList<String> tempList = new ArrayList<>();
        for (Monster monster : monsterList) {
            tempList.add(monster.getName());
        }
        return "Monsters: " + String.join(", ", tempList);
    }
    
    /**
     * Place an item in the room
     * @param Item item
     */
    public void placeItem(Item item)
    {
        itemList.add(item);
        hasItem = true;
    }
    
    /**
     * Returns the exits for a room from a given direction
     * @param direction of the exit to search for
     */
    public Room getExit(String direction)
    {
        return exits.get(direction);
    }
    
    /**
     * Returns the ArrayList of exit directions that you can use
     * 
     * @return arrayList ArrayList<String>, list of directions possible.
     */
    public ArrayList<String> getExits()
    {
        return new ArrayList<String>(exits.keySet());
    }

    /**
     * Return a description of the room's exits,
     * for example, "Exits: north west".
     * @return A description of available exits.
     */
    public String getExitString()
    {
        String returnString = "Exits:";
        ArrayList<String> stringList = new ArrayList<>(exits.keySet());

        returnString = ("Exits: " + String.join(", ", stringList));
        return returnString.replaceAll("_"," ");
    }
    
    /**
     * @return The description of the room.
     */
    public String getDescription()
    {
        return description;
    }
    
    /**
     * Return a long descrption of this room, of the form:
     * You are in the kitchen.
     * Exits: north west
     * 
     * @return A description of the room, including exits.
     */
    public String getLongDescription()
    {
        return description + "\n" + getExitString();
    }
    
    /**
     * Getter for the type of room
     * 
     * @return type String, type of room
     */
    public String getType()
    {
        return type;
    }
    
    /**
     * Getter for the floor of the room.
     * 
     * @return floor int, floor of the room.  1 is the closest to the surface.
     */
    public int getFloor()
    {
        return floor;
    }
    
    /**
     * Checks to see if the room has the player
     * 
     * @return boolean, does it have the player?
     */
    public boolean hasPlayer()
    {
        return hasPlayer;
    }
    
    /**
     * Setter for whether or not the room has the player.
     * 
     * @param here boolean, is the player in this room?
     */
    public void setPlayerHere(boolean here)
    {
        hasPlayer = here;
    }
    
    /**
     * Checks to see if a monster is in this room by name, returns boolean
     * 
     * @param name String, name of monster to find
     * @return found boolean, was the monster found by name?
     */
    public boolean findMonster(String name)
    {
       for (Monster monster : monsterList) {
           if (monster.getName().equals(name)) {
               return true;
           }
       }
       return false;
    }
    
    /**
     * Hits a monster in the room for damage
     * 
     * @param name String, name of monster to attack
     * @param damage int, amount of damage to deal
     */
    public void attackMonster(String name, int damage) 
    {
        boolean found = false;
        int i = 0;
        while (!(found)) {
            if (monsterList.get(i).getName().equals(name)) {
                monsterList.get(i).dealDamage(damage);
                found = true;
            }
            i++;
        }
    }

}

