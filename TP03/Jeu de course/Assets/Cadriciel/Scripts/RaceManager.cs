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

    private string ranking;

    private bool raceEnd;

    public GUIText scoreText;

    private int score;

    private bool restart = false;

	// Use this for initialization
	void Awake () 
	{
		CarActivation(false);
        ShellActivation(false);
	}
	
	void Start()
	{
        score = 0;
       // turnSignal = "Left";
        updateScore();
       // updateTurnSignal();
        StartCoroutine(StartCountdown());
        //ranking = new string[8];
    }

    void Update()
    {
        if (raceEnd)
        {
            _announcement.fontSize = 16;
            _announcement.text = ranking;
        }
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
		CarActivation(true);
        ShellActivation(true);
		yield return new WaitForSeconds(1.0f);
		_announcement.text = "";
	}

	public void EndCarRace(CarController car, int position)
	{
        AddToRanking(car.gameObject.name, position);
        Debug.Log(car.gameObject.name);
        if (car.gameObject.name == "Joueur 1")
        {
            ShellActivation(false);
            car.GetComponentInParent<CarUserControl>().enabled = false;
            car.GetComponentInParent<CarAIControl>().enabled = true;
            car.gameObject.SendMessage("SwitchCam");
            raceEnd = true;
        }
    }

    public void AddToRanking(string carName, int position)
    {
        //ranking[rankingIndex] = carName + " - Position : " + position + " - Temps : " + (int)(Time.time/60) + ":" + Time.time%60;
        //rankingIndex++;
        ranking = ranking + carName + " - Position : " + position + " - Temps : " + (int)(Time.time / 60) + ":" + Time.time % 60 + "\n";
    }

    //IEnumerator EndRaceImpl(string winner)
    private void EndRaceImpl(string winner)
    {
		_announcement.fontSize = 20;
        //int count = _endCountdown;
        //do
        {
            _announcement.text = "Joueur : " + winner; // + " en premiere place."; Retour au titre dans "; + count.ToString();
            //count--;
        }
        //while (!Input.GetKeyDown(KeyCode.Return));//count > 0);

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

    public void CarActivation(bool activate)
	{
		foreach (CarAIControl car in _carContainer.GetComponentsInChildren<CarAIControl>(true))
		{
			car.enabled = activate;
        }
        foreach (CarUserControl car in _carContainer.GetComponentsInChildren<CarUserControl>(true))
        {
            car.enabled = activate;
            car.GetComponentInParent<CarAIControl>().enabled = false;
        }
    }

    public void ShellActivation(bool activate)
    {
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
