using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;

    [SerializeField] private List<GameObject> enemies;

    [SerializeField] private List<GameObject> friendlies;

    private void Awake()
    {
        // Create Singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public List<GameObject> Enemies
    {
        get{ return enemies; }      
    }

    public List<GameObject> Friendlies
    {
        get { return friendlies; }
    }

    // Adds game object to enemy list
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    // Adds game object to friendly list
    public void AddFriendly(GameObject friendly)
    {
        friendlies.Add(friendly);
    }

    // Removes game object from enemy list
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    // Removes game object from friendly list
    public void RemoveFriendly(GameObject friendly)
    {
        friendlies.Remove(friendly);
    }
}
