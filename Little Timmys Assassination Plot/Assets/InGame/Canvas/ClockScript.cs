using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ClockScript : MonoBehaviour
{

	public Image minuteTen;
	public Image minuteOne;
	public Image secondTen;
	public Image secondOne;

    public SpriteLibrary library;
	public PlayerInput input;

    public int minuteTenData;
	public int minuteOneData;
	public int secondTenData;
	public int secondOneData;

	public float time;

    // Start is called before the first frame update
    void Awake()
    {
        
	    minuteTen = GameObject.Find("MinuteTen").GetComponent<Image>();
	    minuteOne = GameObject.Find("MinuteOne").GetComponent<Image>();
	    secondTen = GameObject.Find("SecondTen").GetComponent<Image>();
	    secondOne = GameObject.Find("SecondOne").GetComponent<Image>();

	    minuteTenData = 0;
	    minuteOneData = 9;
	    secondTenData = 3;
	    secondOneData = 0;

	    library = GetComponent<SpriteLibrary>();

		{
            input = new PlayerInput();
        }

	    time = 0;

    }

    private void OnEnable()
    {

        input.Enable();
		input.InGame.RealTime.performed += (InputAction.CallbackContext context) => { float i = context.ReadValue<float>(); if (i == 1) { realTime = true; } else { realTime = false; } UpdateClockUI(); };

    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Start()
	{

		UpdateClockUI();

	}

    // Update is called once per frame
    void Update()
    {

	    time += Time.deltaTime;

		UpdateClockData();

    }

    void UpdateClockData()
    {

		bool clockUpdate = false;

	    if(time > 5)
	    {

		    time = 0;
		    secondOneData++;

			clockUpdate = true;

        }

	    if(CycleClock(secondOneData, 9))
	    {

		    secondTenData ++;
		    secondOneData = 0;

	    }

	    if(CycleClock(secondTenData, 5))
	    {

		    minuteOneData ++;
		    secondTenData = 0;

	    }

	    if(CycleClock(minuteOneData, 9))
	    {

		    minuteTenData++;
		    minuteOneData = 0;

	    }

		if(clockUpdate)
		{

			UpdateClockUI();

		}

    }

	public bool realTime = false;

	void UpdateClockUI()
	{


		string spritePrefix = "Clock_Gui-Recovered_";

        if (realTime)
		{

            minuteTen.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + System.DateTime.Now.GetDateTimeFormats().GetValue(35).ToString().Substring(0, 1));
            minuteOne.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + System.DateTime.Now.GetDateTimeFormats().GetValue(35).ToString().Substring(1, 1));
            secondTen.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + System.DateTime.Now.GetDateTimeFormats().GetValue(35).ToString().Substring(3, 1));
            secondOne.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + System.DateTime.Now.GetDateTimeFormats().GetValue(35).ToString().Substring(4, 1));

        }
		else
		{

            minuteTen.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + minuteTenData.ToString());
            minuteOne.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + minuteOneData.ToString());
            secondTen.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + secondTenData.ToString());
            secondOne.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + secondOneData.ToString());

			DisplayClock(new string[4] { "", "", "", "" });
			
        }

    }

	void DisplayClock(string[] data)
	{

		foreach (var item in data)
		{

            secondOne.sprite = library.GetSprite("ClockUI", "Clock_Gui-Recovered_" + secondOneData.ToString());

        }

	}

    bool CycleClock(int current, int max)
    {

	    if(current > max)
	    {
	    
		    return true;
	    
	    }
	    else
	    {
	    
		    return false;
	    
	    }

    }
}
