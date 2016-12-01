using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityStandardAssets.Vehicles.Car;

public class RaceManager : MonoBehaviour 
{


	[SerializeField]
	private GameObject _carContainer;

	[SerializeField]
	private GUIText _announcement;

	[SerializeField]
	private int _timeToStart;

	[SerializeField]
	private int _endCountdown;

    public GameObject joueur;

    public GUIText scoreText;

    private int score;

	// Use this for initialization
	void Awake () 
	{
		CarActivation(false, false);

	}
	
	void Start()
	{
        score = 0;
       // turnSignal = "Left";
        updateScore();
       // updateTurnSignal();
        StartCoroutine(StartCountdown());
	}

	IEnumerator StartCountdown()
	{
		int count = _timeToStart;
		do 
		{
			_announcement.text = count.ToString();
			yield return new WaitForSeconds(1.0f);
			count--;
		}
		while (count > 0);
		_announcement.text = "Partez!";
		CarActivation(true, false);
		yield return new WaitForSeconds(1.0f);
		_announcement.text = "";
	}

	public void EndRace(string winner)
	{
		StartCoroutine(EndRaceImpl(winner));
	}

	IEnumerator EndRaceImpl(string winner)
	{
        //ICI METTRE ROUTINE IA A LA PLACE
		CarActivation(false, true);
        joueur.gameObject.SendMessage("SwitchCam");
		_announcement.fontSize = 20;
		int count = _endCountdown;
		do 
		{
			_announcement.text = "Victoire: " + winner + " en premiere place. Retour au titre dans " + count.ToString();
			yield return new WaitForSeconds(1.0f);
			count--;
		}
		while (count > 0);

		Application.LoadLevel("boot");
	}

	public void Announce(string announcement, float duration = 2.0f)
	{
		StartCoroutine(AnnounceImpl(announcement,duration));
	}

	IEnumerator AnnounceImpl(string announcement, float duration)
	{
		_announcement.text = announcement;
		yield return new WaitForSeconds(duration);
		_announcement.text = "";
	}

	public void CarActivation(bool activate, bool raceEnd)
	{
		foreach (CarAIControl car in _carContainer.GetComponentsInChildren<CarAIControl>(true))
		{
			car.enabled = activate;
		}
		
		foreach (CarUserControl car in _carContainer.GetComponentsInChildren<CarUserControl>(true))
		{
			car.enabled = activate;
            car.GetComponentInParent<CarAIControl>().enabled = false;
            if (raceEnd == true) car.GetComponentInParent<CarAIControl>().enabled = raceEnd;
        }
        foreach (shellSpawn shell in _carContainer.GetComponentsInChildren<shellSpawn>(true))
        {
            shell.enabled = activate;
        }

    }

    public void updateScore()
    {
        scoreText.text = "score : " + score;
    }

    public void updateTurnSignal()
    {
        //nextTurnSignal.text = "next turn : " + turnSignal;
    }

    public void addScore (int newScore)
    {
        score += newScore;
        updateScore();
    }

    public void changeTurnSignal (string newTurnSignal)
    {
        //turnSignal = newTurnSignal;
       // updateTurnSignal();
    }

}
