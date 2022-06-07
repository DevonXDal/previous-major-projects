import java.util.ArrayList;
import java.util.Random;

/**
 * Holds rooms that can be selected randomly to be returned for creation.
 *
 * @author Devon X. Dalrymple
 * @version 2019.12.8-2
 */
public class RoomList
{
    // instance variables - replace the example below with your own
    private ArrayList<Room> roomsLeft1;
    private ArrayList<Room> roomsLeft2;
    private ArrayList<Room> roomsLeft3;
    private ArrayList<Room> roomsLeft4;
    private ArrayList<Room> roomsLeft5;
    private Random rand;
    

    /**
     * Constructor for objects of class RoomList
     */
    public RoomList()
    {
        roomsLeft1 = new ArrayList<Room>();
        roomsLeft2 = new ArrayList<Room>();
        roomsLeft3 = new ArrayList<Room>();
        roomsLeft4 = new ArrayList<Room>();
        roomsLeft5 = new ArrayList<Room>();
        rand = new Random();
        popRooms(); //Create rooms and store them in the ArrayList.
    }
    
    /*
     * Fills the ArrayLists with rooms.
     */
    private void popRooms()
    {
        roomsLeft1.add(new Room("You find yourself in a room, where a sewage drain releases sewage onto the ground.\nA small stream pours from the drain into another wall.", 1, "normal"));
        roomsLeft1.add(new Room("You find yourself in a room with a rat's nest.\nShattered eggs lay on the ground.", 1, "normal"));
        roomsLeft1.add(new Room("You find yourself in a room with an abandoned guard post.\nThe skeletons of multiple sewer guards rest at its doorway.", 1, "normal"));
        roomsLeft1.add(new Room("You find yourself in a room where a garden has developed around the sewage.\nA small amount of sunlight leaks through from the sewage grate above.", 1, "normal"));
        roomsLeft1.add(new Room("You find yourself in an area where a sewer thief likely lives.\nRandom junk, from the hapless victims, sits on a stand.", 1, "normal"));
        roomsLeft1.add(new Room("Immediately upon looking a retched smell from the spider webs catches you off guard.", 1, "normal"));
        roomsLeft1.add(new Room("You find yourself in a room with a pool of sludge.\nThe smell is horrendous and almost forces you out of the room.", 1, "normal"));
        roomsLeft1.add(new Room("You find yourself in a room with a water filtration system.\nThe parts are not moving and are rusted.", 1, "normal"));
        
        roomsLeft2.add(new Room("You find yourself in a goblin nest full of children.\nYou decide to leave them alone.", 2, "normal"));
        roomsLeft2.add(new Room("You find yourself in the old iron mines.\nThe rotting support beams have started to snap, and it looks like the area will cave in at any moment.", 2, "normal"));
        roomsLeft2.add(new Room("You find yourself in the abandoned prison.\nThe smell of rotting bodies flow through the air.\nThe room is densly packed with webs.", 2, "normal"));
        roomsLeft2.add(new Room("You find yourself standing in front of a ritual area, shattered skulls of children form an omega shape on the ground.\nGoat blood connects the skulls." +
                                "\nYou heart races as you feel something far more sinister glaring at you." +
                                "\nWhile not moving at all, the ground feels as though it is shaking and you find that you can barely keep your footing.", 2, "normal"));
        roomsLeft2.add(new Room("You find yourself in the remains of a devestated underground village.\nThe area has been raided and you find no bodies remain.", 2, "normal"));
        roomsLeft2.add(new Room("You find yourself at the old mine's convenience store.  Nothing is left on the shelves.", 2, "normal"));
        roomsLeft2.add(new Room("You find yourself in a room where a lake of sewage has formed.\nA small waterfall trinkles the sewage farther down.\nA sewage grate pours the waste in from above"
                                , 2, "normal"));
        roomsLeft2.add(new Room("You find yourself in a underground mushroom forest.\nYou recall that every single one these mushrooms are poisonous.", 2, "normal"));
        roomsLeft2.add(new Room("You find this area...to be another area for the cave...\nThere is not much to look at.", 2, "normal"));
        roomsLeft2.add(new Room("You find a thieves den.\nYou find 3 beds, an old couch, 3 shattered dishes, a fence, and a picture of an ugly cat.", 2, "normal"));
        roomsLeft2.add(new Room("Scorpian eggs surrond the ceiling, but not on it or anywhere else.", 2, "normal"));
        roomsLeft2.add(new Room("You find yourself in a small area with a thatch floor.\nIt doesn't burn or else you would of died when you tried to light it.", 2, "normal"));
        
        roomsLeft3.add(new Room("You find yourself at the entrance of the black market.\nYou can not enter due to its 12 guards.", 3, "normal"));
        roomsLeft3.add(new Room("You find yourself inside a rogue hideout.\nYou try to force open the adamantite safe to no avail.", 3, "normal"));
        roomsLeft3.add(new Room("You find yourself inside a room with a single large spider web.\n The web itself is as tough as copper.", 3, "normal"));
        roomsLeft3.add(new Room("You find yourself inside a decent sized library.\nThe library is bigger than the one in the city above.", 3, "normal"));
        roomsLeft3.add(new Room("You find yourself at a training grounds.\nTen of the targets have been cut in half.", 3, "normal"));
        roomsLeft3.add(new Room("You find yourself in temple, the walls have dried blood smeared across them.\nConstant laughter echos through the temple", 3, "normal"));
        roomsLeft3.add(new Room("You find yourself at the recently uncovered caustic slime pond.\nThe toxins are strong enough to melt people within a minute.", 3, "normal"));
        roomsLeft3.add(new Room("As you look, you feel dizzy, the room is spinning, and sounds of screams echo around you.\n" +
                                "Your heartrate is increasing quickly and you feel as you might collapse.\n" +
                                "An omega symbol drawn in bloodstone destroys the light around it and shines as bright as the sun.\n" +
                                "The ritual area shakes the ground and slowly sucks away the air left in the room.", 3, "normal"));
    }
    
    /**
     * Generates rooms for the first floor.  Returns a room and removes it from its list.
     * 
     * @return room Room, room generated randomly.
     */
    public Room floor1Room()
    {
        int i = rand.nextInt(roomsLeft1.size());
        Room toReturn = roomsLeft1.get(i);
        roomsLeft1.remove(i);
        return toReturn;
    }
    
    /**
     * Generates rooms for the second floor.  Returns a room and removes it from its list.
     * 
     * @return room Room, room generated randomly.
     */
    public Room floor2Room()
    {
        int i = rand.nextInt(roomsLeft2.size());
        Room toReturn = roomsLeft2.get(i);
        roomsLeft2.remove(i);
        return toReturn;
    }
    
    /**
     * Generates rooms for the third floor.  Returns a room and removes it from its list.
     * 
     * @return room Room, room generated randomly.
     */
    public Room floor3Room()
    {
        int i = rand.nextInt(roomsLeft3.size());
        Room toReturn = roomsLeft3.get(i);
        roomsLeft3.remove(i);
        return toReturn;
    }
    
}

