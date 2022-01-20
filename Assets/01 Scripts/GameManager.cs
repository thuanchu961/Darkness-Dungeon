using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(hud);
            Destroy(menu);
          //  Destroy(floatingTextManager.gameObject);
            return;
        }

        instance = this; 
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoad;
       
    }
    private void Start()
    {
        isPause = false;
    }

    //ressources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //references
    public PlayerController player;
    public Weapon weapon;

    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnim;
    public Animator winMenuAnim;
    public GameObject hud;
    public GameObject menu;
    public int pesos;
    public int experience;
    public GameObject winMenu;
    public bool isPause;

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);

    }

    public bool TryUpgradeWeapon()
    {
        if(weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;

        }

        if(pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;
    }
    
    public void OnHitPointChange()
    {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }
   
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;
        while(experience >= add)
        {
            add += xpTable[r];
            r++;
            if (r == xpTable.Count)// max level
                return r;
        }
        return r; 
    }
    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;
        while (r <level)
        {
            xp += xpTable[r];
            r++;

        }
        return xp;

    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if(currLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }
    public void OnLevelUp()
    {
        player.OnLevelUp();
        OnHitPointChange();
    }

    public void OnSceneLoad(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    //Death menu and respawn
    public void Respawn()
    {
        deathMenuAnim.SetTrigger("hide");
        SceneManager.LoadScene("MainScene");
        GameManager.instance.isPause = false;
        player.Respawn();

    }
    public void Restart()
    {
        winMenuAnim.SetTrigger("hide");
        //winMenu.SetActive(false);
        SceneManager.LoadScene("MainScene");
        player.Respawn();
    }
    public void SaveState()
    {
         string s = "";
         s += "0" + "|";
         s += pesos.ToString() + "|";
         s += experience.ToString() + "|";
         s += weapon.weaponLevel.ToString();

         PlayerPrefs.SetString("SaveState", s);
        
        Debug.Log("Save State");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        pesos = int.Parse(data[1]);
        //exp
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
        {
            player.SetLevel(GetCurrentLevel());
        }
        //weapon
        weapon.SetWeaponLevel(int.Parse(data[3]));


         SceneManager.sceneLoaded -= LoadState;
        // Debug.Log("Load State");
        //player.transform.position = GameObject.Find("SpawnPoint").transform.position;

    }
}
