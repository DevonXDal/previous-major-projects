import java.util.Random;
import java.util.ArrayList;

/**
 * Handles the monsters the player will face in combat.  This IS the enemy of the game.  This also holds bosses
 *
 * @author Devon X. Dalrymple
 * @version 2019.12.11-09
 */
public class Monster
{
    private Room currentRoom; //Current room the monster is in
    private int floor; //Floor the monster is on
    private int hitPoints; //Hitpoints of the monster
    private int hitPointsMax; //Max health of the monster
    private int level; //Level of the monster
    private int xpReward;
    private String name; //Name of the monster
    private int attackMin; //Damage dealt by attack, minimum
    private int attackMax; //Damage dealt by attack, maximum
    private String specialAbility; //Unique Ability
    private boolean boss; //Is this a boss
    private boolean devon; //Final boss boolean, changes this to a much more powerful form
    private boolean foundPlayer;
    private Random rand;
    private ArrayList<String> killText;
    
    /**
     * Constructor for a standard monster, not a boss
     */
    public Monster(Room spawn, int floor, int health, int level, String name, int attackMin, int attackMax, int xp)
    {
        currentRoom = spawn;
        this.floor = floor;
        hitPoints = hitPointsMax = health;
        foundPlayer = false; 
        boss = false;
        specialAbility = null;
        this.attackMin = attackMin;
        this.attackMax = attackMax;
        this.name = name;
        xpReward = xp;
        rand = new Random();
        isBoss();
        
        killText = new ArrayList<>();
        prepKillText();
    }
    
    /*
     * Sees if the monster is a boss
     */
    private void isBoss()
    {
        if (name == "king_slime" || name == "spider_queen" || name == "abomination") {
            boss = true;
        }
    }
    
    /**
     * Setter for the monster's initial room.
     * 
     * @param spawn Room, room to spawn the monster in.
     */
    public void setSpawn(Room room)
    {
        currentRoom = room;
    }
    
    /**
     * Getter for the monster's name
     * 
     * @return name String, name of the monster
     */
    public String getName()
    {
        return name;
    }
    
    /**
     * Getter for the amount of xp earned by defeating the monster
     * 
     * @return xp int, amount of experience earned
     */
    public int getReward()
    {
        return xpReward;
    }
    
    /**
     * What should the monster do during its turn?
     * 
     * Turn phase for the monster, movement or attack
     */
    public void doAction()
    {
        if (hitPoints > 0) {
            foundPlayer = currentRoom.hasPlayer(); //Locate player
            
            if (foundPlayer) {
                attackPhase();
                if (boss) {
                    attackPhase();
                    attackPhase();
                }
            }
            else if (!(boss)) {
                movePhase();
            }
        }
    }
    
    /*
     * Move the monster to another room
     */
    private void movePhase()
    {
       ArrayList<String> moveList = currentRoom.getExits(); 
       boolean searching = true;
       while (searching) {
           int v = rand.nextInt(moveList.size());
           if (!(moveList.get(v).equals("up") || moveList.get(v).equals("down"))) {
               currentRoom.removeMonster(this);
               currentRoom = currentRoom.getExit(moveList.get(v));
               currentRoom.addMonster(this);
               searching = false;
               if (currentRoom.hasPlayer()) {
                   GameMain.game.printText("Surprise Attack!!");
                   attackPhase();
               }
           }
       }
    }
    
    /*
     * Attack the player
     */
    private void attackPhase()
    {
        int damage = rand.nextInt((attackMax-attackMin)) + attackMin;
        GameMain.game.attackPlayer(name, damage);
    }
    
    /**
     * Deals damage to the monster, checks to see if it dies
     * 
     * @param damage int, damage to receive
     */
    public void dealDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints < 1) {
        	GameMain.game.printText("");
        	GameMain.game.printText("You have " + getKillWord() + " a(n) " + name);
            GameMain.game.rewardPlayer(xpReward);
            if (boss) {
                GameMain.game.gameWon();
            }
            currentRoom.removeMonster(this);
        }
    }
    
    /*
     * Adds the kill text variations  
     */
    private void prepKillText()
    {
    	killText.add("killed");
    	killText.add("assassinated");
    	killText.add("murdered");
    	killText.add("executed");
    	killText.add("massacred");
    	killText.add("slaughtered");
    	killText.add("slayed");
    	killText.add("annihilated");
    	killText.add("eradicated");
    	killText.add("erased");
    	killText.add("exterminated");
    	killText.add("neutralized");
    	killText.add("obliterated");
    	killText.add("wasted");
    	killText.add("destroyed");
    }
    
    /*
     * Gets a kill word at random
     */
    private String getKillWord()
    {
    	return killText.get(rand.nextInt(killText.size()));
    }
}

