import java.util.Random;
import java.util.ArrayList;
/**
 * Handles the monsters for the random generation.  Keeps the monsters on their inital floors.
 *
 * @author Devon X. Dalrymple
 * @version 2019.12.10-02
 */
public class MonsterList
{
    private ArrayList<Monster> floor1Monsters;
    private ArrayList<Monster> floor2Monsters;
    private ArrayList<Monster> floor3Monsters;
    private ArrayList<Monster> floorBossMonsters;
    
    private ArrayList<Monster> floor1UsedMonsters;
    private ArrayList<Monster> floor2UsedMonsters;
    private ArrayList<Monster> floor3UsedMonsters;
    private ArrayList<Monster> floorBossMonster;
    
    private Random rand;
    /**
     * Constructor for objects of class MonsterList, creates the lists for the floors and calls the lists to be populated.
     */
    public MonsterList()
    {
        floor1Monsters = new ArrayList<>();
        floor2Monsters = new ArrayList<>();
        floor3Monsters = new ArrayList<>();
        floorBossMonsters = new ArrayList<>();
        
        floor1UsedMonsters = new ArrayList<>();
        floor2UsedMonsters = new ArrayList<>();
        floor3UsedMonsters = new ArrayList<>();
        floorBossMonster = new ArrayList<>();
        
        rand = new Random();
        
        popLists();
    }

    /*
     * Populate the lists with monsters to be randomly generated.
     */
    private void popLists()
    {
        floor1Monsters.add(new Monster(null, 1, 7, 1, "rat", 3, 4, 4));
        floor1Monsters.add(new Monster(null, 1, 7, 1, "rat", 3, 4, 4));
        floor1Monsters.add(new Monster(null, 1, 7, 1, "rat", 3, 4, 4));
        floor1Monsters.add(new Monster(null, 1, 7, 1, "rat", 3, 4, 4));
        floor1Monsters.add(new Monster(null, 1, 10, 1, "sewer_thief", 3, 5, 6));
        floor1Monsters.add(new Monster(null, 1, 10, 1, "sewer_thief", 3, 5, 6));
        floor1Monsters.add(new Monster(null, 1, 10, 1, "sewer_thief", 3, 5, 6));
        floor1Monsters.add(new Monster(null, 1, 10, 1, "sewer_thief", 3, 5, 6));
        floor1Monsters.add(new Monster(null, 1, 17, 2, "sewer_spider", 5, 9, 8));
        floor1Monsters.add(new Monster(null, 1, 17, 2, "sewer_spider", 5, 9, 8));
        floor1Monsters.add(new Monster(null, 1, 17, 2, "sewer_spider", 5, 9, 8));
        floor1Monsters.add(new Monster(null, 1, 19, 2, "lurking_sludge", 3, 10, 9));
        floor1Monsters.add(new Monster(null, 1, 19, 2, "lurking_sludge", 3, 10, 9));
        floor1Monsters.add(new Monster(null, 1, 19, 2, "lurking_sludge", 3, 10, 9));
        
        floor2Monsters.add(new Monster(null, 2, 14, 3, "goblin", 9, 15, 16));
        floor2Monsters.add(new Monster(null, 2, 14, 3, "goblin", 9, 15, 16));
        floor2Monsters.add(new Monster(null, 2, 14, 3, "goblin", 9, 15, 16));
        floor2Monsters.add(new Monster(null, 2, 14, 3, "goblin", 9, 15, 16));
        floor2Monsters.add(new Monster(null, 2, 39, 3, "slime", 7, 9, 20));
        floor2Monsters.add(new Monster(null, 2, 39, 3, "slime", 7, 9, 20));
        floor2Monsters.add(new Monster(null, 2, 39, 3, "slime", 7, 9, 20));
        floor2Monsters.add(new Monster(null, 2, 39, 3, "slime", 7, 9, 20));
        floor2Monsters.add(new Monster(null, 2, 24, 3, "cave_spider", 8, 14, 18));
        floor2Monsters.add(new Monster(null, 2, 24, 3, "cave_spider", 8, 14, 18));
        floor2Monsters.add(new Monster(null, 2, 24, 3, "cave_spider", 8, 14, 18));
        floor2Monsters.add(new Monster(null, 2, 24, 3, "cave_spider", 8, 14, 18));
        floor2Monsters.add(new Monster(null, 2, 31, 4, "crazed_thief", 10, 16, 26));
        floor2Monsters.add(new Monster(null, 2, 31, 4, "crazed_thief", 10, 16, 26));
        floor2Monsters.add(new Monster(null, 2, 31, 4, "crazed_thief", 10, 16, 26));
        floor2Monsters.add(new Monster(null, 2, 31, 4, "crazed_thief", 10, 16, 26));
        floor2Monsters.add(new Monster(null, 2, 29, 4, "cave_scorpion", 9, 16, 28));
        floor2Monsters.add(new Monster(null, 2, 29, 4, "cave_scorpion", 9, 16, 28));
        floor2Monsters.add(new Monster(null, 2, 29, 4, "cave_scorpion", 9, 16, 28));
        floor2Monsters.add(new Monster(null, 2, 29, 4, "cave_scorpion", 9, 16, 28));
        
        floor3Monsters.add(new Monster(null, 3, 51, 5, "lunatic_rogue", 19, 29, 34));
        floor3Monsters.add(new Monster(null, 3, 51, 5, "lunatic_rogue", 19, 29, 34));
        floor3Monsters.add(new Monster(null, 3, 51, 5, "lunatic_rogue", 19, 29, 34));
        floor3Monsters.add(new Monster(null, 3, 51, 5, "lunatic_rogue", 19, 29, 34));
        floor3Monsters.add(new Monster(null, 3, 26, 5, "lunatic_mage", 29, 39, 34));
        floor3Monsters.add(new Monster(null, 3, 26, 5, "lunatic_mage", 29, 39, 34));
        floor3Monsters.add(new Monster(null, 3, 26, 5, "lunatic_mage", 29, 39, 34));
        floor3Monsters.add(new Monster(null, 3, 26, 5, "lunatic_mage", 29, 39, 34));
        floor3Monsters.add(new Monster(null, 3, 86, 5, "lunatic_warrior", 11, 34, 34));
        floor3Monsters.add(new Monster(null, 3, 86, 5, "lunatic_warrior", 11, 34, 34));
        floor3Monsters.add(new Monster(null, 3, 86, 5, "lunatic_warrior", 11, 34, 34));
        floor3Monsters.add(new Monster(null, 3, 86, 5, "lunatic_warrior", 11, 34, 34));
        floor3Monsters.add(new Monster(null, 3, 36, 5, "demon_lord_worshiper", 22, 30, 40));
        floor3Monsters.add(new Monster(null, 3, 36, 5, "demon_lord_worshiper", 22, 30, 40));
        floor3Monsters.add(new Monster(null, 3, 36, 5, "demon_lord_worshiper", 22, 30, 40));
        floor3Monsters.add(new Monster(null, 3, 36, 5, "demon_lord_worshiper", 22, 30, 40));
        floor3Monsters.add(new Monster(null, 3, 120, 6, "caustic_slime", 10, 30, 46));
        floor3Monsters.add(new Monster(null, 3, 120, 6, "caustic_slime", 10, 30, 46));
        floor3Monsters.add(new Monster(null, 3, 120, 6, "caustic_slime", 10, 30, 46));
        floor3Monsters.add(new Monster(null, 3, 96, 6, "ogre", 18, 42, 48));
        floor3Monsters.add(new Monster(null, 3, 96, 6, "ogre", 18, 42, 48));
        floor3Monsters.add(new Monster(null, 3, 96, 6, "ogre", 18, 42, 48));
        floor3Monsters.add(new Monster(null, 3, 46, 6, "great_legged_spider", 22, 32, 50));
        floor3Monsters.add(new Monster(null, 3, 46, 6, "great_legged_spider", 22, 32, 50));
        floor3Monsters.add(new Monster(null, 3, 46, 6, "great_legged_spider", 22, 32, 50));
        
        floorBossMonsters.add(new Monster(null, 30, 500, 8, "king_slime", 25, 35, 100));
        floorBossMonsters.add(new Monster(null, 30, 212, 8, "spider_queen", 32, 40, 100));
        floorBossMonsters.add(new Monster(null, 30, 200, 8, "abomination", 35, 48, 100));
    }
    
    /**
     * Calls a monster to be created into a room to oppose the player on the first floor
     * 
     * @param room Room, initial room of the monster
     */
    public void firstFloorMonster(Room room)
    {
        int i = rand.nextInt(floor1Monsters.size());
        Monster toAssign = floor1Monsters.get(i);
        toAssign.setSpawn(room);
        room.addMonster(toAssign);
        floor1Monsters.remove(i);
        floor1UsedMonsters.add(toAssign);
        
    }
    
    /**
     * Calls a monster to be created into a room to oppose the player on the first floor
     * 
     * @param room Room, initial room of the monster
     */
    public void secondFloorMonster(Room room)
    {
        int i = rand.nextInt(floor2Monsters.size());
        Monster toAssign = floor2Monsters.get(i);
        toAssign.setSpawn(room);
        room.addMonster(toAssign);
        floor2Monsters.remove(i);
        floor2UsedMonsters.add(toAssign);
        
    }
    
    /**
     * Calls a monster to be created into a room to oppose the player on the first floor
     * 
     * @param room Room, initial room of the monster
     */
    public void thirdFloorMonster(Room room)
    {
        int i = rand.nextInt(floor3Monsters.size());
        Monster toAssign = floor3Monsters.get(i);
        toAssign.setSpawn(room);
        room.addMonster(toAssign);
        floor3Monsters.remove(i);
        floor3UsedMonsters.add(toAssign);
        
    }
    
    /**
     * Calls a monster to be created into a room to oppose the player on the first floor
     * 
     * @param room Room, initial room of the monster
     */
    public void bossFloorMonster(Room room)
    {
        int i = rand.nextInt(floorBossMonsters.size());
        Monster toAssign = floorBossMonsters.get(i);
        toAssign.setSpawn(room);
        room.addMonster(toAssign);
        floorBossMonsters.remove(i);
        floorBossMonster.add(toAssign);
        
    }
    
    /**
     * Tells a particular floor to have its monsters move around
     * @param floor int, what floor to tell it to move
     */
    public void enemyTurn(int floor)
    {
        if (floor == 1) {
            for (Monster monster : floor1UsedMonsters) {
                monster.doAction();
            }
        }
        if (floor == 2) {
            for (Monster monster : floor2UsedMonsters) {
                monster.doAction();
            }
        }
        if (floor == 3) {
            for (Monster monster : floor3UsedMonsters) {
                monster.doAction();
            }
        }
        if (floor == 30) {
            floorBossMonster.get(0);
        }
    }
}

