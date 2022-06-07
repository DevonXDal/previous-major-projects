import java.util.ArrayList;
import java.util.Random;

/**
 * Generates psuedorandom game experiences using classes for each generation
 *
 * @author Devon X. Dalrymple
 * @version 2019.12.8-02
 */
public class RandGen
{
    private Random rand;
    private ArrayList<Room> floor1Rooms;
    private ArrayList<Room> floor2Rooms;
    private ArrayList<Room> floor3Rooms;
    private RoomList roomList;
    private ItemsForGame itemList;
    private MonsterList monsterList;
    
    private Room roomStart;
    private Room room1B;
    private Room room1C;
    private Room room1D;
    private Room room1E;
    private Room room1F;
    private Room room1G;
    private Room room1H;
    
    private Room room2A;
    private Room room2B;
    private Room room2C;
    private Room room2D;
    private Room room2E;
    private Room room2F;
    private Room room2G;
    private Room room2H;
    
    private Room room3A;
    private Room room3B;
    private Room room3C;
    private Room room3D;
    private Room room3E;
    private Room room3F;
    private Room room3G;
    private Room room3H;
    
    private Room room3Boss;
    
    //private Room room4A;
    //private Room room4B;
    //private Room room4C;
    //private Room room4D;
    //private Room room4E;
    //private Room room4F;
    //private Room room4G;
    //private Room room4H;
    
    //private Room room5A;
    //private Room room5B;
    //private Room room5C;
    //private Room room5D;
    //private Room room5E;
    //private Room room5F;
    //private Room room5G;
    //private Room room5H;
    
    //private Room room6A;
    //private Room room6Boss1;
    //private Room room6Boss2;
    //private Room room6Boss3;
    //private Room room6Boss4;
    //private Room room6Safe;
    //private Room roomFinalBoss;
    

    /**
     * Constructor for for the random generation of the dungeon structure
     */
    public RandGen()
    {
        rand = new Random();
        floor1Rooms = new ArrayList<Room>();
        floor2Rooms = new ArrayList<Room>();
        floor3Rooms = new ArrayList<Room>();
        roomList = new RoomList();
        itemList = new ItemsForGame();
        monsterList = new MonsterList();
        roomStart = new Room("A faint light leaks from the rusted sewer grate above.", 1, "normal");
        createFloor1();
        createFloor2();
        createFloor3();
    }

   /*
    * Creates floor 1 of the dungeon
    */
   private void createFloor1()
   {
       room1B = roomList.floor1Room();
       room1C = roomList.floor1Room();
       room1D = roomList.floor1Room();
       room1E = roomList.floor1Room();
       room1F = roomList.floor1Room();
       room1G = roomList.floor1Room();
       room1H = roomList.floor1Room();
       //Add the rooms of floor 1 to a list phase, besides roomStart to ensure the player is not spawn-killed.
       floor1Rooms.add(room1B);
       floor1Rooms.add(room1C);
       floor1Rooms.add(room1D);
       floor1Rooms.add(room1E);
       floor1Rooms.add(room1F);
       floor1Rooms.add(room1G);
       floor1Rooms.add(room1H);
       
       //Items phase
       int v;
       for (int i = 8; i > 0; --i) {
           v = rand.nextInt(floor1Rooms.size()-1);
           floor1Rooms.get(v).placeItem(itemList.floor1Room());
       }
       
       for (int i = 0; i < 6; i++) {
           v = rand.nextInt(floor1Rooms.size()-1);
           monsterList.firstFloorMonster(floor1Rooms.get(v));
       }
       
       //Layout phase
       switch (rand.nextInt(4)) {
           case 0:
           layout1A();
           break;
           
           case 1:
           layout1B();
           break;
           
           case 2:
           layout1C();
           break;
           
           case 3:
           layout1D();
           break;
     
       }
       
       //TODO Staircases
   }
   
   /**
    * Getter for the starting room of the game
    * 
    * @return roomStart Room, initial room of the player
    */
   public Room getStartingRoom()
   {
       return roomStart;
   }
   
   
   //Create the layouts for floor 1
   
   private void layout1A()
   {
       roomStart.setExit("east", room1C);
       roomStart.setExit("south-east", room1B);
       roomStart.setExit("south", room1H);
       room1B.setExit("north-west", roomStart);
       room1B.setExit("north-east", room1C);
       room1C.setExit("west", roomStart);
       room1C.setExit("south", room1B);
       room1C.setExit("north", room1D);
       room1D.setExit("south", room1C);
       room1D.setExit("west", room1E);
       room1E.setExit("east", room1D);
       room1E.setExit("north-west", room1F);
       room1E.setExit("south", room1G);
       room1F.setExit("east", room1E);
       room1G.setExit("north", room1E);
       room1G.setExit("west", room1F);
       room1G.setExit("west", room1H);
       room1H.setExit("north-west", room1G);
       room1H.setExit("north", roomStart);
   }
   
   private void layout1B()
   {
      roomStart.setExit("south", room1H);
      roomStart.setExit("east", room1B);
      roomStart.setExit("north", room1D);
      roomStart.setExit("west", room1F);
      room1B.setExit("north-west", roomStart);
      room1C.setExit("north", room1E);
      room1C.setExit("west", room1D);
      room1D.setExit("east", room1C);
      room1D.setExit("south", roomStart);
      room1E.setExit("east", room1F);
      room1F.setExit("north", room1E);
      room1F.setExit("east", roomStart);
      room1F.setExit("south", room1G);
      room1G.setExit("north", room1F);
      room1G.setExit("east", room1H);
      room1H.setExit("west", room1G);
      room1H.setExit("north", roomStart);
   }
   
   private void layout1C()
   {
       roomStart.setExit("north-west", room1B);
       roomStart.setExit("north", room1C);
       roomStart.setExit("east", room1G);
       room1B.setExit("north", room1H);
       room1B.setExit("south-east", roomStart);
       room1C.setExit("east", room1D);
       room1C.setExit("south", roomStart);
       room1D.setExit("west", room1C);
       room1D.setExit("north-east", room1E);
       room1D.setExit("south", room1F);
       room1E.setExit("south-west",room1D);
       room1F.setExit("north", room1D);
       room1F.setExit("west", room1G);
       room1F.setExit("south", room1H);
       room1G.setExit("west", roomStart);
       room1G.setExit("east", room1F);
       room1H.setExit("north", room1F);
   }
   
   private void layout1D()
   {
       roomStart.setExit("north", room1G);
       roomStart.setExit("east", room1H);
       roomStart.setExit("south-east", room1D);
       room1B.setExit("west", room1G);
       room1B.setExit("east", room1C);
       room1B.setExit("south", room1H);
       room1C.setExit("south", room1D);
       room1D.setExit("north-east", room1H);
       room1D.setExit("north-west", roomStart);
       room1D.setExit("west", room1E);
       room1D.setExit("east", room1C);
       room1E.setExit("east", room1D);
       room1E.setExit("north", room1F);
       room1F.setExit("north", room1G);
       room1F.setExit("east", room1E);
       room1G.setExit("west", room1F);
       room1G.setExit("east", room1B);
       room1G.setExit("south", roomStart);
       room1H.setExit("north", room1B);
       room1H.setExit("west", roomStart);
       room1H.setExit("south-west", room1D);
   }
   
   /*
    * Creates floor 2 of the dungeon
    */
   private void createFloor2()
   {
       room2A = roomList.floor2Room();
       room2B = roomList.floor2Room();
       room2C = roomList.floor2Room();
       room2D = roomList.floor2Room();
       room2E = roomList.floor2Room();
       room2F = roomList.floor2Room();
       room2G = roomList.floor2Room();
       room2H = roomList.floor2Room();
       
       //Add the rooms of floor 2 to a list phase
       floor2Rooms.add(room2A);
       floor2Rooms.add(room2B);
       floor2Rooms.add(room2C);
       floor2Rooms.add(room2D);
       floor2Rooms.add(room2E);
       floor2Rooms.add(room2F);
       floor2Rooms.add(room2G);
       floor2Rooms.add(room2H);
       
       //Items phase
       int v;
       int w;
       for (int i = 8; i > 0; --i) {
           v = rand.nextInt(floor2Rooms.size()-1);
           floor2Rooms.get(v).placeItem(itemList.floor2Room());
       }
       
       for (int i = 0; i < 8; i++) {
           v = rand.nextInt(floor2Rooms.size()-1);
           monsterList.secondFloorMonster(floor2Rooms.get(v));
       }
       
       //Stairs to between floors 1 & 2
       v = rand.nextInt(floor1Rooms.size()-1);
       w = rand.nextInt(floor2Rooms.size()-1);

       floor2Rooms.get(w).setExit("up", floor1Rooms.get(v));
       floor1Rooms.get(v).setExit("down", floor2Rooms.get(w));
       
       //Layout phase
       switch (rand.nextInt(4)) {
           case 0:
           layout2A();
           break;
           
           case 1:
           layout2B();
           break;
           
           case 2:
           layout2C();
           break;
           
           case 3:
           layout2D();
           break;
     
       }
       
       //TODO Staircases
   }
   
   private void layout2A()
   {
       room2A.setExit("east", room2C);
       room2A.setExit("south-east", room2B);
       room2A.setExit("south", room2H);
       room2B.setExit("north-west", room2A);
       room2B.setExit("north-east", room2C);
       room2C.setExit("west", room2A);
       room2C.setExit("south", room2B);
       room2C.setExit("north", room2D);
       room2D.setExit("south", room2C);
       room2D.setExit("west", room2E);
       room2E.setExit("east", room2D);
       room2E.setExit("north-west", room2F);
       room2E.setExit("south", room2G);
       room2F.setExit("east", room2E);
       room2G.setExit("north", room2E);
       room2G.setExit("west", room2F);
       room2G.setExit("west", room2H);
       room2H.setExit("north-west", room2G);
       room2H.setExit("north", room2A);
   }
   
   private void layout2B()
   {
      room2A.setExit("south", room2H);
      room2A.setExit("east", room2B);
      room2A.setExit("north", room2D);
      room2A.setExit("west", room2F);
      room2B.setExit("north-west", room2A);
      room2C.setExit("north", room2E);
      room2C.setExit("west", room2D);
      room2D.setExit("east", room2C);
      room2D.setExit("south", room2A);
      room2E.setExit("east", room2F);
      room2F.setExit("north", room2E);
      room2F.setExit("east", room2A);
      room2F.setExit("south", room2G);
      room2G.setExit("north", room2F);
      room2G.setExit("east", room2H);
      room2H.setExit("west", room2G);
      room2H.setExit("north", room2A);
   }
   
   private void layout2C()
   {
       room2A.setExit("north-west", room2B);
       room2A.setExit("north", room2C);
       room2A.setExit("east", room2G);
       room2B.setExit("north", room2H);
       room2B.setExit("south-east", room2A);
       room2C.setExit("east", room2D);
       room2C.setExit("south", room2A);
       room2D.setExit("west", room2C);
       room2D.setExit("north-east", room2E);
       room2D.setExit("south", room2F);
       room2E.setExit("south-west",room2D);
       room2F.setExit("north", room2D);
       room2F.setExit("west", room2G);
       room2F.setExit("south", room2H);
       room2G.setExit("west", room2A);
       room2G.setExit("east", room2F);
       room2H.setExit("north", room2F);
   }
   
   private void layout2D()
   {
       room2A.setExit("north", room2G);
       room2A.setExit("east", room2H);
       room2A.setExit("south-east", room2D);
       room2B.setExit("west", room2G);
       room2B.setExit("east", room2C);
       room2B.setExit("south", room2H);
       room2C.setExit("south", room2D);
       room2D.setExit("north-east", room2H);
       room2D.setExit("north-west", room2A);
       room2D.setExit("west", room2E);
       room2D.setExit("east", room2C);
       room2E.setExit("east", room2D);
       room2E.setExit("north", room2F);
       room2F.setExit("north", room2G);
       room2F.setExit("east", room2E);
       room2G.setExit("west", room2F);
       room2G.setExit("east", room2B);
       room2G.setExit("south", room2A);
       room2H.setExit("north", room2B);
       room2H.setExit("west", room2A);
       room2H.setExit("south-west", room2D);
   }
   
   /**
    * Tells the monsterList to handle the enemies' moves and provide player's floor
    * 
    * @param floor int, floor is the player on
    */
   public void enemyTurn(int floor)
   {
       monsterList.enemyTurn(floor);
   }
   
   /*
    * Creates floor 3 of the dungeon
    */
   private void createFloor3()
   {
       room3A = roomList.floor3Room();
       room3B = roomList.floor3Room();
       room3C = roomList.floor3Room();
       room3D = roomList.floor3Room();
       room3E = roomList.floor3Room();
       room3F = roomList.floor3Room();
       room3G = roomList.floor3Room();
       room3H = roomList.floor3Room();
       
       room3Boss = new Room("You find yourself locked in a room with a powerful monster", 30, "boss");
       
       //Add the rooms of floor 3 to a list phase
       floor3Rooms.add(room3A);
       floor3Rooms.add(room3B);
       floor3Rooms.add(room3C);
       floor3Rooms.add(room3D);
       floor3Rooms.add(room3E);
       floor3Rooms.add(room3F);
       floor3Rooms.add(room3G);
       floor3Rooms.add(room3H);
       
       //Items phase
       int v;
       int w;
       for (int i = 8; i > 0; --i) {
           v = rand.nextInt(floor3Rooms.size()-1);
           floor3Rooms.get(v).placeItem(itemList.floor3Room());
       }
       
       for (int i = 0; i < 10; i++) {
           v = rand.nextInt(floor3Rooms.size()-1);
           monsterList.thirdFloorMonster(floor3Rooms.get(v));
       }
       
       //Stairs to between floors 2 & 3
       v = rand.nextInt(floor2Rooms.size()-1);
       w = rand.nextInt(floor3Rooms.size()-1);

       floor3Rooms.get(w).setExit("up", floor2Rooms.get(v));
       floor2Rooms.get(v).setExit("down", floor3Rooms.get(w));
       
       w = rand.nextInt(floor3Rooms.size()-1);
       floor3Rooms.get(w).setExit("down", room3Boss);
       monsterList.bossFloorMonster(room3Boss);
       
       //Layout phase
       switch (rand.nextInt(4)) {
           case 0:
           layout3A();
           break;
           
           case 1:
           layout3B();
           break;
           
           case 2:
           layout3C();
           break;
           
           case 3:
           layout3D();
           break;
     
       }
       
       //TODO Staircases
   }
   
   private void layout3A()
   {
       room3A.setExit("east", room3C);
       room3A.setExit("south-east", room3B);
       room3A.setExit("south", room3H);
       room3B.setExit("north-west", room3A);
       room3B.setExit("north-east", room3C);
       room3C.setExit("west", room3A);
       room3C.setExit("south", room3B);
       room3C.setExit("north", room3D);
       room3D.setExit("south", room3C);
       room3D.setExit("west", room3E);
       room3E.setExit("east", room3D);
       room3E.setExit("north-west", room3F);
       room3E.setExit("south", room3G);
       room3F.setExit("east", room3E);
       room3G.setExit("north", room3E);
       room3G.setExit("west", room3F);
       room3G.setExit("west", room3H);
       room3H.setExit("north-west", room3G);
       room3H.setExit("north", room3A);
   }
   
   private void layout3B()
   {
      room3A.setExit("south", room3H);
      room3A.setExit("east", room3B);
      room3A.setExit("north", room3D);
      room3A.setExit("west", room3F);
      room3B.setExit("north-west", room3A);
      room3C.setExit("north", room3E);
      room3C.setExit("west", room3D);
      room3D.setExit("east", room3C);
      room3D.setExit("south", room3A);
      room3E.setExit("east", room3F);
      room3F.setExit("north", room3E);
      room3F.setExit("east", room3A);
      room3F.setExit("south", room3G);
      room3G.setExit("north", room3F);
      room3G.setExit("east", room3H);
      room3H.setExit("west", room3G);
      room3H.setExit("north", room3A);
   }
   
   private void layout3C()
   {
       room3A.setExit("north-west", room3B);
       room3A.setExit("north", room3C);
       room3A.setExit("east", room3G);
       room3B.setExit("north", room3H);
       room3B.setExit("south-east", room3A);
       room3C.setExit("east", room3D);
       room3C.setExit("south", room3A);
       room3D.setExit("west", room3C);
       room3D.setExit("north-east", room3E);
       room3D.setExit("south", room3F);
       room3E.setExit("south-west",room3D);
       room3F.setExit("north", room3D);
       room3F.setExit("west", room3G);
       room3F.setExit("south", room3H);
       room3G.setExit("west", room3A);
       room3G.setExit("east", room3F);
       room3H.setExit("north", room3F);
   }
   
   private void layout3D()
   {
       room3A.setExit("north", room3G);
       room3A.setExit("east", room3H);
       room3A.setExit("south-east", room3D);
       room3B.setExit("west", room3G);
       room3B.setExit("east", room3C);
       room3B.setExit("south", room3H);
       room3C.setExit("south", room3D);
       room3D.setExit("north-east", room3H);
       room3D.setExit("north-west", room3A);
       room3D.setExit("west", room3E);
       room3D.setExit("east", room3C);
       room3E.setExit("east", room3D);
       room3E.setExit("north", room3F);
       room3F.setExit("north", room3G);
       room3F.setExit("east", room3E);
       room3G.setExit("west", room3F);
       room3G.setExit("east", room3B);
       room3G.setExit("south", room3A);
       room3H.setExit("north", room3B);
       room3H.setExit("west", room3A);
       room3H.setExit("south-west", room3D);
   }
}

