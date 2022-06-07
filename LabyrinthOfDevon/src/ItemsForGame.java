import java.util.ArrayList;
import java.util.Random;

/**
 * Generates ArrayLists for items that can be placed in the rooms
 *
 * @author Devon X. Dalrymple
 * @version 2019.12.9-04
 */
public class ItemsForGame
{
    // instance variables - replace the example below with your own
    private ArrayList<Item> itemsFloor1;
    private ArrayList<Item> itemsFloor2;
    private ArrayList<Item> itemsFloor3;
    private Random rand;
    
    /**
     * Constructor the class to hold itemLists.
     */
    public ItemsForGame()
    {
        itemsFloor1 = new ArrayList<>();
        itemsFloor2 = new ArrayList<>();
        itemsFloor3 = new ArrayList<>();
        rand = new Random();
        popItemLists(); //Populate Item Lists
    }
    
    /*
     * Adds items into the lists
     */
    private void popItemLists()
    {
        //itemsFloor1.add(new Item("bottled_sewage", "disgusting but gives a random potion effect", "consumable", 2));
        itemsFloor1.add(new Item("lesser_health_potion", "provides a small amount of health, for minor wounds only", "consumable", 1));
        itemsFloor1.add(new Item("lesser_health_potion", "provides a small amount of health, for minor wounds only", "consumable", 1));
       // itemsFloor1.add(new Item("lesser_magicka_potion", "provides a minor amount of magicka, perfect for a beginner though", "consumable", 1));
       // itemsFloor1.add(new Item("lesser_magicka_potion", "provides a minor amount of magicka, perfect for a beginner though", "consumable", 1));
       // itemsFloor1.add(new Item("lesser_stamina_potion","provides a good amount of stamina, great for a start", "consumable", 1));
       // itemsFloor1.add(new Item("lesser_stamina_potion","provides a good amount of stamina, great for a start", "consumable", 1));
        itemsFloor1.add(new Item("wool_armor", "lowest tier of armor, destroyed when burning, 1 point of armor", "armor", 5));
        itemsFloor1.add(new Item("leather_armor", "2nd tier of armor, destroyed when burning, 2 points of armor", "armor", 5));
        itemsFloor1.add(new Item("chain_armor", "3rd tier of armor, well made, 3 points of armor", "armor", 7));
        //itemsFloor1.add(new Item("magic_staircase", "returns the user to the staircase", "consumable", 3));
       // itemsFloor1.add(new Item("strength_potion", "gives the user more damage for the turn", "consumable", 2));
        itemsFloor1.add(new Item("flint_sword", "lowest tier sword, 2-4 more damage per hit", "weapon", 4));
        itemsFloor1.add(new Item("feather_feet_potion", "gives the user all their actions back", "consumable", 3));
        itemsFloor1.add(new Item("magic_cookie", "gives the user 5 more units of carry weight (kilograms)", "consumable", 5));
        //itemsFloor1.add(new Item("weakness_potion", "gives the user a massive debuff, they lose one movement permanently", "consumable", 2));
        itemsFloor1.add(new Item("flint_dagger", "lowest tier dagger, 1-3 more damage per hit", "weapon", 1));
        itemsFloor1.add(new Item("flint_shield", "lowest tier shield, 20%: block ranged, 10%: block melee", "shield", 8));
        itemsFloor1.add(new Item("flint_axe", "lowest tier blunt weapon, 6-8 more damage per hit", "weapon", 8));
        itemsFloor1.add(new Item("tin_sword", "2nd tier sword, 3-5 more damage per hit", "weapon", 4));
        itemsFloor1.add(new Item("tin_dagger", "2nd tier dagger, 2-3 more damage per hit", "weapon", 1));
        itemsFloor1.add(new Item("tin_shield", "2nd tier shield, 25%: block ranged, 15%: block melee", "shield", 8));
        itemsFloor1.add(new Item("tin_axe", "2nd tier blunt weapon, 7-11 more damage per hit", "weapon", 8));
        //itemsFloor1.add(new Item("bottled_sewage_water", "digusting but it puts out fires, it will hurt you a bit", "consumable", 1));
        itemsFloor1.add(new Item("Zuul's_hammer", "legendary tier 1 blunt weapon, 9-12 more damage per hit", "weapon", 9));
        itemsFloor1.add(new Item("guard_armor", "legendary tier 1 armor, 4 points of armor", "armor", 6));
        //itemsFloor1.add(new Item("bottled_sewage", "disgusting but gives a random potion effect", "consumable", 2));
        itemsFloor1.add(new Item("wool_armor", "lowest tier of armor, destroyed when burning, 1 point of armor", "armor", 5));
        itemsFloor1.add(new Item("flint_dagger", "lowest tier dagger, 1-3 more damage per hit", "weapon", 1));
        
        for (Item item : itemsFloor1) {
            itemsFloor2.add(item);
        }
        
        itemsFloor2.add(new Item("copper_sword", "3rd tier sword, 4-7 more damage per hit", "weapon", 5));
        itemsFloor2.add(new Item("copper_axe", "3rd tier axe, 9-13 more damage per hit", "weapon", 9));
        itemsFloor2.add(new Item("copper_dagger", "3rd tier dagger, 3-6 more damage per hit", "weapon", 2));
        itemsFloor2.add(new Item("copper_shield", "3rd tier shield, 30% block ranged, 15% block melee", "shield", 8));
        itemsFloor2.add(new Item("copper_armor", "4th tier armor, 4 points of armor", "armor", 8));
        itemsFloor2.add(new Item("bronze_sword", "4th tier sword, 7-12 more damage per hit", "weapon", 5));
        itemsFloor2.add(new Item("bronze_axe", "4th tier axe, 13-17 more damage per hit", "weapon", 8));
        itemsFloor2.add(new Item("bronze_dagger", "4th tier dagger, 5-10 more damage per hit", "weapon", 1));
        itemsFloor2.add(new Item("bronze_shield", "4th tier shield, 30% block ranged, 20% melee", "shield", 7));
        itemsFloor2.add(new Item("bronze_armor", "5th tier armor, 6 points of armor", "armor", 7));
        //itemsFloor2.add(new Item("weak_spell_tome", "one free spell with a cost of 20 or less", "consumable", 2));
        //itemsFloor2.add(new Item("cave_spider_venom", "trade 20 health for magicka", "consumable", 1));
        //itemsFloor2.add(new Item("chalice_of_life", "trade health a few times and increase max health", "chalice", 3));
        //itemsFloor2.add(new Item("chalice_of_blood", "trade health a few times and increase damage", "chalice", 3));
        //itemsFloor2.add(new Item("chalice_of_movement", "trade health a few times and increase the number of actions you an take", "chalice", 3));
        //itemsFloor2.add(new Item("boom_device", "new technology that explodes after being thrown, 150 damage", "consumable", 5));
        
        for (Item item : itemsFloor2) {
            itemsFloor3.add(item);
        }
        
        itemsFloor3.add(new Item("iron_sword", "5th tier weapon, 12-17 more damage per hit", "weapon", 5));
        itemsFloor3.add(new Item("iron_axe", "5th tier weapon, 13-27 more damage per hit", "weapon", 8));
        itemsFloor3.add(new Item("iron_dagger", "5th tier weapon, 11-14 more damage per hit", "weapon", 1));
        itemsFloor3.add(new Item("iron_shield", "5th tier shield, 40% block ranged, 30% melee", "shield", 7));
        itemsFloor3.add(new Item("iron_armor", "6th tier armor, 8 points of armor", "armor", 7));
        itemsFloor3.add(new Item("steel_sword", "6th tier weapon, 15-25 more damage per hit", "weapon", 5));
        itemsFloor3.add(new Item("steel_axe", "6th tier weapon, 16-33 more damage per hit", "weapon", 8));
        itemsFloor3.add(new Item("steel_dagger", "6th tier weapon, 14-22 more damage per hit", "weapon", 1));
        itemsFloor3.add(new Item("steel_shield", "6th tier shield, 45% to block ranged, 30% melee", "shield", 7));
        itemsFloor3.add(new Item("steel_armor", "7th tier armor, 10 points of armor", "armor", 7));
        itemsFloor3.add(new Item("decent_health_potion", "provides a decent amount of health", "consumable", 2));
        //itemsFloor3.add(new Item("decent_stamina_potion", "provides a decent amount of stamina", "consumable", 2));
        //itemsFloor3.add(new Item("decent_magicka_potion", "provides a decent amount of magicka", "consumable", 2));
        itemsFloor3.add(new Item("decent_health_potion", "provides a decent amount of health", "consumable", 2));
       // itemsFloor3.add(new Item("decent_stamina_potion", "provides a decent amount of stamina", "consumable", 2));
       // itemsFloor3.add(new Item("decent_magicka_potion", "provides a decent amount of magicka", "consumable", 2));
        itemsFloor3.add(new Item("flaming_sword", "5th tier legendary weapon, 20-30 more damage per hit", "weapon", 6));
        //itemsFloor3.add(new Item("flaming_shield", "5th tier legendary shield, 40% block ranged, 30% melee and does damage to melee", "shield", 8));
        //itemsFloor3.add(new Item("crystal_water", "Heals a small amount of everything", "consumable", 3));
    }
    
    /**
     * Generates an item for the first floor.  Returns a item and removes it from its list.
     * 
     * @return item Item, item generated randomly.
     */
    public Item floor1Room()
    {
        int i = rand.nextInt(itemsFloor1.size());
        Item toReturn = itemsFloor1.get(i);
        itemsFloor1.remove(i);
        return toReturn;
    }
    
    /**
     * Generates an item for the second floor.  Returns a item and removes it from its list.
     * 
     * @return item Item, item generated randomly.
     */
    public Item floor2Room()
    {
        int i = rand.nextInt(itemsFloor2.size());
        Item toReturn = itemsFloor2.get(i);
        itemsFloor2.remove(i);
        return toReturn;
    }
    
    /**
     * Generates an item for the second floor.  Returns a item and removes it from its list.
     * 
     * @return item Item, item generated randomly.
     */
    public Item floor3Room()
    {
        int i = rand.nextInt(itemsFloor3.size());
        Item toReturn = itemsFloor3.get(i);
        itemsFloor3.remove(i);
        return toReturn;
    }
}

