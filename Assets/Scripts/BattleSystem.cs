using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST,RUN }

public class BattleSystem : MonoBehaviour
{
	public MonstronData pmd;
	public MonstronData emd;
	public TMP_Text mv1;
	public TMP_Text mv2;
	public Image player;
	public Image enemy;
	public GameObject atkPanel;
	public Dictionary<MonType, Dictionary<MonType, float>> typeChart = new Dictionary<MonType, Dictionary<MonType, float>>
	{
		{ MonType.Fire,new Dictionary<MonType, float>{ { MonType.Fire,1f}, { MonType.Water, 0.5f }, { MonType.Dark, 2f }, { MonType.Energy, 0.5f }, { MonType.Poison, 2f }, { MonType.Rock, 2f } } },
		{ MonType.Water,new Dictionary<MonType, float>{ { MonType.Fire,2f}, { MonType.Water, 1f }, { MonType.Dark, 1f }, { MonType.Energy, 0.5f }, { MonType.Poison, 0.5f }, { MonType.Rock, 2f } } },
		{ MonType.Dark,new Dictionary<MonType, float>{ { MonType.Fire,0.5f}, { MonType.Water, 1f }, { MonType.Dark, 1f }, { MonType.Energy, 0.5f }, { MonType.Poison, 1f }, { MonType.Rock, 1f } } },
		{ MonType.Energy,new Dictionary<MonType, float>{ { MonType.Fire,1f}, { MonType.Water, 1f }, { MonType.Dark, 2f }, { MonType.Energy, 2f }, { MonType.Poison, 2f }, { MonType.Rock, 2f } } },
		{ MonType.Rock,new Dictionary<MonType, float>{ { MonType.Fire,2f}, { MonType.Water, 1f }, { MonType.Dark, 1f }, { MonType.Energy, 0.5f }, { MonType.Poison, 1f }, { MonType.Rock, 1f } } },
		{ MonType.Poison,new Dictionary<MonType, float>{ { MonType.Fire,1f}, { MonType.Water, 0.5f }, { MonType.Dark, 1f }, { MonType.Energy, 1f }, { MonType.Poison, 0.5f }, { MonType.Rock, 0.5f } } }
	};
	public Unit playerUnit;
	public Unit enemyUnit;

	public int playerh;
	public int players;
	public int playerd;
	public int playera;
	//public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
		


			state = BattleState.START;
			StartCoroutine(SetupBattle());
		
    }

	IEnumerator SetupBattle()
	{


		pmd = DataObj.myMonstrons[DataObj.activeMon];
		emd = NewMonstronSpawner.spawn();
		mv1.text = DataObj.inst.moves[pmd.move1].movename;
		mv2.text = DataObj.inst.moves[pmd.move2].movename;
		playerh = pmd.healthbuff + DataObj.inst.monstrons[pmd.id].health;
		players = pmd.speedbuff + DataObj.inst.monstrons[pmd.id].speed;
		playerd = pmd.defensebuff + DataObj.inst.monstrons[pmd.id].defense;
		playera = pmd.attackbuff + DataObj.inst.monstrons[pmd.id].attack;
		player.sprite = DataObj.inst.monstrons[pmd.id].back;
		playerUnit.init(DataObj.inst.monstrons[pmd.id].monster_name, pmd.level, playerh);
		enemy.sprite = DataObj.inst.monstrons[emd.id].front;
		enemyUnit.init(DataObj.inst.monstrons[emd.id].monster_name, 1, DataObj.inst.monstrons[emd.id].health);
		DialoguePanel.addDialogue("A wild " + enemyUnit.unitName + " approaches...");
		
		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);
		atkPanel.SetActive(false);
		yield return new WaitForSeconds(1f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack(int moveNo)
	{
		MoveData moveUsed = (moveNo == 0) ? DataObj.inst.moves[pmd.move1] : DataObj.inst.moves[pmd.move2];
		float damage = (Random.Range(moveUsed.mindamage, moveUsed.maxdamage) * ((100 + playera) / 100) * typeChart[moveUsed.type][DataObj.inst.monstrons[emd.id].monster_type]*pmd.level)/ (DataObj.inst.monstrons[emd.id].defense*emd.level);
		float heal = (Random.Range(moveUsed.minHeal, moveUsed.maxHeal)+((moveUsed.isLifedrain)?(0.1f*damage):0));
		bool isDead = enemyUnit.TakeDamage(Mathf.CeilToInt(damage));
		playerUnit.Heal(Mathf.CeilToInt(heal));
		enemyHUD.SetHP(enemyUnit.currentHP);
		DialoguePanel.addDialogue($"{playerUnit.unitName} used {moveUsed.movename}!");
		DialoguePanel.addDialogue($"The attack is Successful!");
		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.WON;
			EndBattle();
		} else
		{
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
	}

	IEnumerator EnemyTurn()
	{
		MoveData moveUsed = (Random.Range(0,2)==0) ? DataObj.inst.moves[emd.move1] : DataObj.inst.moves[emd.move2];
		float damage = (Random.Range(moveUsed.mindamage, moveUsed.maxdamage) * ((100 + DataObj.inst.monstrons[emd.id].attack) / 100) * typeChart[moveUsed.type][DataObj.inst.monstrons[pmd.id].monster_type] * emd.level) / (playerd * pmd.level);
		float heal = (Random.Range(moveUsed.minHeal, moveUsed.maxHeal) + ((moveUsed.isLifedrain) ? (0.1f * damage) : 0));

		DialoguePanel.addDialogue($"{enemyUnit.unitName} used {moveUsed.movename}!");
		DialoguePanel.addDialogue($"The Attack Hits!");
		yield return new WaitForSeconds(1f);
		bool isDead = playerUnit.TakeDamage(Mathf.CeilToInt(damage));
		enemyUnit.Heal(Mathf.CeilToInt(heal));

		playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		} else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			DialoguePanel.addDialogue("You won the battle!");
		} else if (state == BattleState.LOST)
		{
			DialoguePanel.addDialogue("You were defeated.");
		}
		else if (state == BattleState.RUN)
		{
			DialoguePanel.addDialogue("You ran away.");
		}
		StartCoroutine(end());
	}

	IEnumerator end() {
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("MainScene");
	}

	void PlayerTurn()
	{
		//dialogueText.text = "Choose an action:";
	}


	public void OnAttackButton(int i)
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack(i));
	}

	public void OnRun() {
		state = BattleState.RUN;
		EndBattle();
	}
	public void OnStall()
	{
		DialoguePanel.addDialogue("You stall.");
		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

}
