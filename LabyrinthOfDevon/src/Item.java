 
/**
 * Item handles a single item and what it is able to do in the game.
 * Items may have very unique abilities and are stored in rooms initially. 
 * 
 * @author Devon X. Dalrymple
 * @version 2019.11.27-04
 */
public class Item
{
    private String type; //Is it stationary, is it a weapon, is it armor, is it a key for a door?
    private String description;  //Flavor text describing the item, holds no game changing purpose, just cool to have.
    private boolean isEssential; //Must this item stay with the player to complete a certain goal?
    private boolean useAble; //Can this item be used?
    private String name; //Name of the item.
    private int weight; //-1 Means can not be carried, weight is in kilograms.
    private Room storedRoom; //What room if any does this item have stored.
    private boolean roomStored; //Does this item have a room stored?

    /**
     * Constructor for objects of class Item.
     * 
     * @param name String, name of the item.
     * @param description String, flavor-text of this item.
     * @param type String, what type of item is it, is it armor.
     * @param weight int, how much in kilograms does this item weigh. If it is -1, the item can not be picked up. 
     */
    public Item(String name, String description, String type, int weight)
    {
        // initialise instance variables
        this.description = description;
        this.name = name;
        this.weight = weight;
        this.type = type;
        roomStored = false;
    }
    
    /**
     * Getter for name.
     * 
     * @return name String, what is the item called.
     */
    public String getName()
    {
        return name;
    }
    
    /**
     * Getter for description.
     * 
     * @return description String, flavor-text for this item.
     */
    public String getDescription()
    {
        return description;
    }
    
    /**
     * Getter for weight.
     * 
     * @return weight int, weight of the object in kilograms
     */
    public int getWeight()
    {
        return weight;
    }
    
    /**
     * Getter for the long description.
     * 
     * @return A_name_description String, a long description that includes the name and description.
     */
    public String getLongDescription()
    {
        return (name + ": " + description);
    }
    
    /**
     * Getter for the item's type.
     * 
     * @return type String, type of item
     */
    public String getType()
    {
        return type;
    }
    
    /*
     * Checks the type of item to see if it matches one of the available types.
     * 
     * @param type String, set the type of object.
     */
    private void setType(String type)
    {
        if (type.equals("consumable")) {
            this.type = type;
        }
        else if (type.equals("weapon")) {
            this.type = type;
        }
        else if (type.equals("armor")) {
            this.type = type;
        }
        else if (type.equals("stationary")) {
            this.type = type;
        }
        else {
            this.type = "ERROR";
        }
    }
    
    /**
     * Setter for stored room.
     * 
     * @param room Room, room to store in the item.
     */
    public void setStoredRoom(Room room)
    {
        storedRoom = room;
        roomStored = true;
    }
    
    /**
     * Tests to see if a room has been stored.
     * 
     * @return roomStored boolean, is a room stored yet?
     */
    public boolean isRoomStored()
    {
        return roomStored;
    }
    
    /**
     * Getter for stored room.
     * 
     * @return storedRoom Room, room stored in the item.
     */
    public Room getStoredRoom()
    {
        return storedRoom;
    }
    
}

