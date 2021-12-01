using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatus : MonoBehaviour
{
    public Player player;

    public Text ui_Level;
    public Text ui_Exp;
    public Text ui_Health;
    public Text ui_Mana;
    public Text ui_Armor;
    public Text ui_Damage;
    public Text ui_CriticalChance;
    public Text ui_CriticalDamage;
    public Text ui_AttackSpeed;
    public Text ui_MoveSpeed;
    public Text ui_Cooldown;
    public Text ui_AbilityPower;
    public Text ui_Shield;
    public Text ui_Test;

    public Image ui_player_sprite;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePlayerStatus();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStatus();
    }

    public void UpdatePlayerStatus()
    {
        if (!player) player = Player.player;
        if (player)
        {
            if (ui_Level) ui_Level.text = player.level.ToString() + " (" + player.SkillpointsSpendable() + ")";
            if (ui_Exp) ui_Exp.text = player.experience.ToString() + " / " + player.experienceMax.ToString();
            if (ui_Health) ui_Health.text = player.health.ToString("0.0") + " / " + player.healthMax.ToString() + " (" + player.healthRegeneration.ToString() + ")";
            if (ui_Mana) ui_Mana.text = player.mana.ToString("0.0") + " / " + player.manaMax.ToString("0.0") + " (" + player.manaRegeneration.ToString("0.0") + ")";
            if (ui_Armor) ui_Armor.text = player.armor.ToString();
            if (ui_Damage) ui_Damage.text = player.attackDamage.ToString();
            if (ui_AbilityPower) ui_AbilityPower.text = player.abilityPower.ToString();
            if (ui_CriticalChance) 
            {
                var critical = player.criticalChance * 100f;
                if (critical >= 100f)
                    critical = 100f;
                ui_CriticalChance.text = (critical).ToString() + "%"; 
            }
            if (ui_CriticalDamage) ui_CriticalDamage.text = "+" + ((player.criticalDamage - 1f) * 100f).ToString() + "%";
            if (ui_AttackSpeed && player.skills.Count > 0) ui_AttackSpeed.text = player.attackSpeed.ToString();// + "(" + player.skills[0].CooldownRemaining() + ")";
            if (ui_MoveSpeed) ui_MoveSpeed.text = player.moveSpeed.ToString();
            if (ui_Cooldown)
            {
                var cooldown = player.cooldown * 100f;
                if (cooldown > 80f)
                    cooldown = 80f;
                ui_Cooldown.text = (cooldown).ToString() + "%";
            }
            if (ui_Shield) ui_Shield.text = player.shield.ToString();
            if (ui_Test) ui_Test.text = player.agent.velocity.magnitude.ToString();

            if (ui_player_sprite)
                if (ui_player_sprite.sprite == null)
                    ui_player_sprite.sprite = player.sprite;
                
        }
    }
}
