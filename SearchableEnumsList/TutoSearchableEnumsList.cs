 using System.Collections;
 using Unity
 
 public class TutoSearchableEnumsList : MonoBehavior {
 
    [Searchable]
    public Item Item;
 
    void Start(){
    
    }
 
    void Update(){
    
    }
 }
 
 public enum Item
 {
     [InspectorName("Weapon/Melee/Sword")]
     Sword,
     [InspectorName("Weapon/Melee/Axe")]
     Axe,
     [InspectorName("Weapon/Ranged/Bow")]
     Bow,
     [InspectorName("Weapon/Magic/Wand")]
     Wand,
     [InspectorName("Weapon/Magic/Staff")]
     Staff,
     [InspectorName("Equipment/Helmet")]
     Helmet,
     [InspectorName("Equipment/Armor")]
     Armor,
     [InspectorName("Consumable/HealthPotion")]
     HealthPotion,
     [InspectorName("Consumable/ManaPotion")]
     ManaPotion,
     [InspectorName("Consumable/Food")]
     Food,
     Coin
 }
