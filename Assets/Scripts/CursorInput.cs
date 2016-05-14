﻿using UnityEngine;
using System;

public class CursorInput : MonoBehaviour {

    public GameObject selectedOption;
    public GameObject[] menuObjects;

    private GameObject cursor;
    private GameObject currentMenu ;
    private int counter;
    private float scaleOfCanvas;
	// Use this for initialization
	void Awake () {
        cursor = GameObject.Find("Cursor");
        string parent = cursor.transform.parent.name;
        currentMenu = GameObject.Find(parent);
        scaleOfCanvas = GameObject.Find("MainMenu").transform.localScale.x;
        counter = 0;

    }
	
	// Update is called once per frame
	void Update () {
        //check y positions of the previous, current and next elements in menuobjects array
        //if previous and current have different y, allow only vertical
        //if previous and current have same y, allow only horizontal
        //if current and next have different y, allow only vertical
        //if current and next have same y, allow only horizontal

        if (Input.GetButtonDown("Vertical")) {
            var direction = getDirection();
            //-1 is up
            // 1 is down
            float current = menuObjects[counter].transform.position.y;
            float next = 0.0f;
            try
            {
                counter = counter + direction;
                next = menuObjects[counter].transform.position.y;
                if (!sameYPositions(next, current)) {
                    
                    cursor.transform.position = new Vector3(GetCursorXPosition(menuObjects[counter]), next);
                }
            }
            catch (IndexOutOfRangeException)
            {
                //stay in current position, no valid input
                next = current;

                if (counter < 0)
                {
                    counter = 0;
                }
                else {
                    counter = menuObjects.Length - 1;
                }
                return;
            }
            selectedOption = menuObjects[counter];

            
        }

        if (Input.GetButtonDown("Fire1")) {
            MenuLink ml = selectedOption.GetComponent<MenuLink>();
            string type = ml.GetState().ToString();
            if (type == "menu") {
                GameObject enterMenu = ml.GetMenuItem();
                currentMenu.SetActive(false);
                enterMenu.SetActive(true);
            }
            if (type == "inputfield") {

            }

        }

	}

    private bool sameYPositions(float pos1, float pos2)
    {
        if (Mathf.Approximately(pos1, pos2)) {
            return true;
        }
        return false;
    }

    private float GetCursorXPosition(GameObject selected)
    {
        RectTransform rt = selected.GetComponent<RectTransform>();
        float x = selected.transform.position.x/scaleOfCanvas;
        Debug.Log("0. " + x);
        float cursorPos = x +( rt.rect.width / 2.0f);
        Debug.Log("1. " + cursorPos);
        cursorPos = cursorPos + 40.0f;
        Debug.Log("2. " + cursorPos);
        cursorPos = cursorPos * -1;
        //have to multiply by the scale set in canvas
        cursorPos = cursorPos * scaleOfCanvas;
        
        return cursorPos;
    }

    private int getDirection()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            return -1;
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            return 1;
        }
        return 0;
    }
}