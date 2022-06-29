using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;

    private List<GameObject> friendlies = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> missiles = new List<GameObject>();

    public List<GameObject> Enemies
    {
        get { return enemies; }
    }
    public List<GameObject> Friendlies
    {
        get { return friendlies; }
    }
    public List<GameObject> Missiles
    {
        get { return missiles; }
    }

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

    // Adds game object to friendly list
    public void AddFriendly(GameObject friendly)
    {
        friendlies.Add(friendly);
    }

    // Adds game object to enemy list
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    // Adds game object to missiles list
    public void AddMissile(GameObject friendly)
    {
        missiles.Add(friendly);
    }

    // Removes game object from friendly list
    public void RemoveFriendly(GameObject friendly)
    {
        friendlies.Remove(friendly);
    }

    // Removes game object from enemy list
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    // Removes game object from missile list
    public void RemoveMissile(GameObject friendly)
    {
        missiles.Remove(friendly);
    }
}
