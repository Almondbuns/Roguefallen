using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum VisualActionType
{
    MELEE_ATTACK,
    MOVEMENT,
    SHOW,
    HIDE,
    TELEPORT,
    KILL,
    FLOATINGINFO,
}

public class VisualAction
{
    public VisualActionType type;
    public (int x, int y) target_tile;
    public string text = "";
}

public class Actor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ActorData actor_data;
    public GameObject prepare_action_panel;
    public GameObject start_action_panel;

    public GameObject prepare_action_panel_prefab;
    public GameObject start_action_panel_prefab;

    public GameObject floating_number_prefab;
    public GameObject health_prefab;

    public float floating_info_time = 0;
    public  List<string> floating_info_queue;

    public bool is_visible = true;
    public  float visual_action_time = 0;
    public  List<VisualAction> visual_action_queue;
    public  VisualAction current_visual_action;

    public float shift_sprite_position_x;
    public float shift_sprite_position_y;

    int health = 0;
    public GameObject health_bar;

    // Start is called before the first frame update

    void Awake()
    {
        floating_info_queue = new();
        visual_action_queue = new();
    }

    public void Create(ActorData actor_data)
    {
        this.actor_data = actor_data;
        
        if (actor_data is DynamicObjectData)
        {
            Texture2D texture = Resources.Load<Texture2D>(actor_data.prototype.icon);
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        if (actor_data is ProjectileData)
        {
            Texture2D texture = Resources.Load<Texture2D>(actor_data.prototype.icon);
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            transform.Find("Shadow").GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        name = actor_data.prototype.name;

        shift_sprite_position_x = actor_data.prototype.tile_width / 2.0f;
        shift_sprite_position_y = actor_data.prototype.tile_height / 2.0f;
        transform.position = new Vector3(actor_data.X + shift_sprite_position_x, actor_data.Y + shift_sprite_position_y, 0);
        
        actor_data.HandlePrepareActionTick += HandlePrepareActionTick;
        actor_data.HandleStartActionTick += HandleStartActionTick;
        actor_data.HandleRecoverActionTick += HandleRecoverActionTick;
        actor_data.HandleHasFinishedActionTick += HandleHasFinishedActionTick;
        actor_data.HandleAbsorbDamage += HandleAbsorbDamage;
        actor_data.HandleDodge += HandleDodge;
        actor_data.HandleResist += HandleResist;
        actor_data.HandleMeleeAttack += HandleMeleeAttack;
        actor_data.HandleHeal += HandleHeal;
        actor_data.HandleDamage += HandleDamage;
        actor_data.HandleExperienceGain += HandleExperienceGain;
        actor_data.HandleLevelUp += HandleLevelUp;
        actor_data.HandleMovement += HandleMovement;
        actor_data.HandleTeleport += HandleTeleport;
        actor_data.HandleEffect+= HandleEffect;
        actor_data.HandleParry+= HandleParry;
        actor_data.HandleBlock+= HandleBlock;

        if (actor_data is PlayerData)
        {
            GameObject.Find("Camera").transform.SetParent(transform);
            GameObject.Find("Camera").transform.localPosition = new Vector3(0, 0, -10);
        }
        else
        {
            Hide(); //Hide monsters first to avoid creation flicker before visibility update
        }
        
        UpdateVisibility(GameObject.Find("GameData").GetComponent<GameData>().current_map);
    }

    public void HandleMovement()
    {
        VisualAction v = new VisualAction {type = VisualActionType.MOVEMENT, target_tile = (actor_data.X, actor_data.Y)};
        visual_action_queue.Add(v);
        UpdateVisibility(GameObject.Find("GameData").GetComponent<GameData>().current_map);
    }

    public void HandleTeleport()
    {
        VisualAction v = new VisualAction {type = VisualActionType.TELEPORT, target_tile = (actor_data.X, actor_data.Y)};
        visual_action_queue.Add(v);
        UpdateVisibility(GameObject.Find("GameData").GetComponent<GameData>().current_map);
    }

    public void UpdateVisibility(MapData map)
    {
        if (this == null)
            return;

        if (actor_data is PlayerData)
            return;
            
        if (actor_data.is_currently_hidden == false && map.tiles[actor_data.X, actor_data.Y].visibility == Visibility.Active)
        {
            VisualAction v = new VisualAction {type = VisualActionType.SHOW, target_tile = (actor_data.X, actor_data.Y)};
            visual_action_queue.Add(v); 
            gameObject.SetActive(true); //if not set true immediately script will not work
        }
        else
        {
            VisualAction v = new VisualAction {type = VisualActionType.HIDE, target_tile = (actor_data.X, actor_data.Y)};
            visual_action_queue.Add(v);
        }
    }

    void AddFloatingInfo(string text)
    {
        floating_info_queue.Add(text);
    }

    void HandleLevelUp()
    {
        string text = "<color=#3333FF>" + "+" + "Level Up!" + "</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleExperienceGain(int amount)
    {
        string text = "<color=#3333FF>" + "+" + amount.ToString() + " XP</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleHeal(int amount, string type)
    {
        string text = "";
        if (type == "Health")
            text = "<color=#aa0000>" + "+" + amount.ToString()  + "</color>";
        if (type == "Stamina")
            text = "<color=#00aa00>" + "+" + amount.ToString() + "</color>";
        if (type == "Mana")
            text = "<color=#006666>" + "+" + amount.ToString() + "</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleDamage(int amount, string type)
    {
        string text = "";
        if (type == "Health")
            text = "<color=#aa0000>" + "-" + amount.ToString()  + "</color>";
        if (type == "Stamina")
            text = "<color=#00aa00>" + "-" + amount.ToString() + "</color>";
        if (type == "Mana")
            text = "<color=#006666>" + "-" + amount.ToString() + "</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleAbsorbDamage(int damage, int armor_absorb)
    {
        string text = "<color=#ff0000>-" + damage.ToString();
        if (armor_absorb > 0)
            text += "</color><color=#CCCCCC>" + " (" + armor_absorb.ToString() + ")</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleDodge()
    {
        string text = "<color=#aaaa00>Dodged</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleParry()
    {
        string text = "<color=#ffff00>Parried!</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleBlock()
    {
        string text = "<color=#ffff00>Blocked</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }
    void HandleResist()
    {
        string text = "<color=#aaaa00>Resisted</color>";
        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void HandleEffect(bool gain, EffectData effect)
    {
        string text;
        if (gain == true)
        {
            text = "<color=#00aaaa>+ " + effect.name;
        }
        else
        {
            text = "<color=#00aaaa>- " + effect.name;
        }

        if (effect.show_amount_info == true)
            text +=  ": " + effect.amount;
        
        text += "</color>";

        VisualAction v = new VisualAction {type = VisualActionType.FLOATINGINFO, text = text};
        visual_action_queue.Add(v);
    }

    void OnDestroy()
    {
        if (actor_data.is_boss == true)
            GameObject.Find("UI").GetComponent<UI>().DeactivateBossPanel();
            
        actor_data.HandlePrepareActionTick -= HandlePrepareActionTick;
        actor_data.HandleStartActionTick -= HandleStartActionTick;
        actor_data.HandleRecoverActionTick -= HandleRecoverActionTick;
        actor_data.HandleHasFinishedActionTick -= HandleHasFinishedActionTick;
        actor_data.HandleAbsorbDamage -= HandleAbsorbDamage;
        actor_data.HandleDodge -= HandleDodge;
        actor_data.HandleResist -= HandleResist;
        actor_data.HandleMeleeAttack -= HandleMeleeAttack;
        actor_data.HandleHeal -= HandleHeal;
        actor_data.HandleDamage -= HandleDamage;
        actor_data.HandleExperienceGain -= HandleExperienceGain;
        actor_data.HandleLevelUp -= HandleLevelUp;
        actor_data.HandleMovement -= HandleMovement;
        actor_data.HandleTeleport -= HandleTeleport;
        actor_data.HandleEffect -= HandleEffect;
        actor_data.HandleParry -= HandleParry;
        actor_data.HandleParry -= HandleBlock;
    }

    void HandleMeleeAttack(List<AttackedTileData> tiles)
    {
        foreach(AttackedTileData tile in tiles)
        {
            VisualAction v = new VisualAction {type = VisualActionType.MELEE_ATTACK, target_tile = (tile.x, tile.y)};
            visual_action_queue.Add(v);
        }

    }

    void HandlePrepareActionTick(ActorData actor_data)
    {
        if (prepare_action_panel == null)
        {
            prepare_action_panel = GameObject.Instantiate(prepare_action_panel_prefab, transform.Find("Canvas").transform, false);
            Texture2D texture = Resources.Load<Texture2D>(actor_data.current_action.icon);
            prepare_action_panel.transform.Find("ActionIcon").GetComponent<Image>().sprite = 
                Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    void HandleStartActionTick(ActorData actor_data)
    {
        Destroy(prepare_action_panel);     

        if (actor_data.current_action.show_on_map == true)
        {
            start_action_panel = GameObject.Instantiate(start_action_panel_prefab, transform.Find("Canvas").transform, false);
            Texture2D texture = Resources.Load<Texture2D>(actor_data.current_action.icon);
            start_action_panel.transform.Find("ActionIcon").GetComponent<Image>().sprite =
            Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    void HandleRecoverActionTick(ActorData actor_data)
    {
        Destroy(prepare_action_panel);
    }

    void HandleHasFinishedActionTick(ActorData actor_data)
    {
        Destroy(prepare_action_panel);
    }

    void RefreshHealth()
    {
        health = actor_data.Health_current;

        //Only show health bar if damaged
        if (health_bar == null && health < actor_data.GetHealthMax())
        {
            /*PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
            int monster_stats = player_data.GetCurrentAdditiveEffectAmount<EffectMonsterStats>();

            if (monster_stats > 0 || actor_data is PlayerData)*/
            health_bar = GameObject.Instantiate(health_prefab, transform.Find("Canvas").transform, false);
        }
        
        if (health_bar != null && health >= actor_data.GetHealthMax())
        {
            Destroy(health_bar);
        }

        if (health_bar != null)
            health_bar.GetComponent<ActorHealthBar>().SetValues(actor_data.Health_current, actor_data.GetHealthMax());
    }

    void FixedUpdate()
    {
        if (actor_data.Health_current != health)
            RefreshHealth();

        //Movement
        Vector3 position_old = transform.position;
                
        //FloatingInfo Sorting
        floating_info_time -= Time.deltaTime;
        if (floating_info_time <= 0 && floating_info_queue.Count > 0)
        {
            CreateFloatingInfo(floating_info_queue[0]);
            floating_info_queue.Remove(floating_info_queue[0]);
            floating_info_time = 0.5f;
        }

        //MeleeAttack Sorting
        bool check_direction = true;

        visual_action_time -= Time.deltaTime;
        if (visual_action_time <= 0 || current_visual_action == null) 
        {
            current_visual_action = null;

            if (visual_action_queue.Count > 0)
            {
                current_visual_action = visual_action_queue[0];
                visual_action_queue.Remove(visual_action_queue[0]);

                if (is_visible == true)
                {
                    if (current_visual_action.type == VisualActionType.MELEE_ATTACK)
                        visual_action_time = 0.5f;
                    else if (current_visual_action.type == VisualActionType.MOVEMENT)
                    {
                        if (actor_data.prototype.projectile != null)
                            visual_action_time = 0.10f;
                        else
                            visual_action_time = 0.15f;
                    }
                    else
                        visual_action_time = 0.5f;
                }
                else
                        visual_action_time = 0.01f;
            }
        }

        if (current_visual_action != null)
        {
            if (current_visual_action.type == VisualActionType.MELEE_ATTACK)
            {
                if (is_visible == true)
                {
                    if (visual_action_time >= 0.25f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(current_visual_action.target_tile.x + shift_sprite_position_x, current_visual_action.target_tile.y + shift_sprite_position_y, 0), 10f * Time.deltaTime);
                    }
                    else if (visual_action_time >= 0.0f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(actor_data.X + shift_sprite_position_x, actor_data.Y + shift_sprite_position_y, 0), 10f * Time.deltaTime);
                        check_direction = false;
                    }
                }
                else
                {
                    current_visual_action = null;
                }
            }
            else if (current_visual_action.type == VisualActionType.MOVEMENT)
            {
                if (is_visible == true)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(current_visual_action.target_tile.x + shift_sprite_position_x, current_visual_action.target_tile.y + shift_sprite_position_y, 0), 10f * Time.deltaTime);
                }
                else
                {
                    transform.position = new Vector3(current_visual_action.target_tile.x + shift_sprite_position_x, current_visual_action.target_tile.y + shift_sprite_position_y, 0);
                    current_visual_action = null;
                }
            }
            else if (current_visual_action.type == VisualActionType.SHOW)
            {
                if (is_visible == false)
                    Show(current_visual_action.target_tile);

                current_visual_action = null;
            }
            else if (current_visual_action.type == VisualActionType.HIDE)
            {
                current_visual_action = null;
                if (is_visible == true)
                    Hide();
            }
            else if (current_visual_action.type == VisualActionType.TELEPORT)
            {             
                transform.position = new Vector3(current_visual_action.target_tile.x + shift_sprite_position_x, current_visual_action.target_tile.y + shift_sprite_position_y, 0);
                GameObject.Find("Map").GetComponent<Map>().AddVisualEffectToTile(VisualEffect.Teleport, current_visual_action.target_tile);
                current_visual_action = null;              
            }
            else if (current_visual_action.type == VisualActionType.KILL)
            {             
                Kill();             
            }
            else if (current_visual_action.type == VisualActionType.FLOATINGINFO)
            {             
                AddFloatingInfo(current_visual_action.text);
                current_visual_action = null;
            }
        }
      
        if (check_direction == true && !(actor_data is ProjectileData))
        {
            //Sprite Direction
            if (transform.position.x < position_old.x)
                GetComponent<SpriteRenderer>().flipX = true;
            else if (transform.position.x > position_old.x)
                GetComponent<SpriteRenderer>().flipX = false;
        }

        if (actor_data is ProjectileData && (transform.position.x != actor_data.X + shift_sprite_position_x || transform.position.y != actor_data.Y + shift_sprite_position_y))
        {
            float angle = Vector2.Angle(Vector2.up, new Vector2(actor_data.X + shift_sprite_position_x - transform.position.x, actor_data.Y + shift_sprite_position_y - transform.position.y));
            if (actor_data.X + shift_sprite_position_x - transform.position.x > 0)
                angle = -angle;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
    }

    void CreateFloatingInfo(string text)
    {
        GameObject floating_number = GameObject.Instantiate(floating_number_prefab, transform.Find("Canvas").transform, false);
        floating_number.transform.Find("Number").GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (actor_data is not PlayerData)
            GameObject.Find("MousePointer").GetComponent<MousePointer>().AddInfoPanel(actor_data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (actor_data is not PlayerData)
            GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(actor_data);
    }

    public void Show((int x, int y) tile)
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.Find("Canvas").gameObject.SetActive(true);
        transform.position = new Vector3(tile.x + shift_sprite_position_x, tile.y + shift_sprite_position_y, 0);
        is_visible = true;

        if (actor_data.is_boss == true)
        {
            GameObject.Find("UI").GetComponent<UI>().ActivateBossPanel(actor_data);
        }
    }

    public void Hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Canvas").gameObject.SetActive(false);
        is_visible = false;
    }

    public void SpeedUpVisualActions()
    {
        //Reminder: there might be an action in current_visual_action not in the list

        if (current_visual_action != null)
            visual_action_queue.Insert(0,current_visual_action); // just for convinience
        
        foreach(VisualAction v in visual_action_queue)
        {
            if (v.type == VisualActionType.TELEPORT)
            {
                transform.position = new Vector3(v.target_tile.x + shift_sprite_position_x, v.target_tile.y + shift_sprite_position_y, 0);
                GameObject.Find("Map").GetComponent<Map>().AddVisualEffectToTile(VisualEffect.Teleport, current_visual_action.target_tile);
            }
            else if (v.type == VisualActionType.SHOW)
            {
                if (is_visible == false)
                    Show(v.target_tile);
            }
            else if (v.type == VisualActionType.HIDE)
            {
                if (is_visible == true)
                    Hide();
            }
            else if (v.type == VisualActionType.KILL)
            {
                Kill();
            }
            else if (v.type == VisualActionType.FLOATINGINFO)
            {             
                AddFloatingInfo(v.text);
            }
        }

        visual_action_queue.Clear();
        current_visual_action = null;

        transform.position = new Vector3(actor_data.X + shift_sprite_position_x, actor_data.Y + shift_sprite_position_y, 0); // Always end at the current tile
    }

    public void HandleKill()
    {
        VisualAction v = new VisualAction {type = VisualActionType.KILL, target_tile = (actor_data.X, actor_data.Y)};
        visual_action_queue.Add(v);
    }

    public void Kill()
    {
        if (actor_data is ProjectileData)
            Destroy(gameObject, 1);
        else if (actor_data is DynamicObjectData)
            Destroy(gameObject, 0);
        else
        {
            gameObject.AddComponent<ActorDeathScript>();
            Destroy(gameObject, 5);
        }
    }
}


