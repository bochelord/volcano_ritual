﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PokedexManager : MonoBehaviour {


    [Header("Movement values")]
    public Transform leftPosition;
    public Transform rightPosition;
    public Transform centerScreenPosition;
    public float timeForMoving;
    [Header("WorldSprites")]
    public GameObject[] worldSprites;

    [Header("Canvas")]
    public Text enemyName;
    public Text enemyWorld;


    [Header("Enemies Scriptable Object")]
    public XintanaEnemiesBestiary bestiaryList;

    // Variables for swipe gestures
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    private PokedexProfile _pokedex;
    public List<GameObject> _enemies = new List<GameObject>();
    public int pokedexIndex = 0;
    private PokedexStep _step;
    private Swipe _swipe;
    private enum Swipe
    {
        Left,
        Right
    }
    private enum PokedexStep
    {
        WaitingForInput,
        CheckAnimation,
        ShowEnemy
    }
    void Awake()
    {
        SpawnEnemies();

    }

    void Start()
    {
        _pokedex = Rad_SaveManager.pokedex;
        _enemies[0].transform.DOMove(centerScreenPosition.position, timeForMoving, false);
        _step = PokedexStep.ShowEnemy;
    }

    void Update()
    {
        switch (_step)
        {
            case PokedexStep.CheckAnimation:
                MoveEnemies();
                break;
            case PokedexStep.ShowEnemy:
                ShowEnemyStats();
                break;
            case PokedexStep.WaitingForInput:
                checkInput();
                break;
        }
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < bestiaryList.xintanaEnemies.Count; i++)
        {
            GameObject _obj = Instantiate(bestiaryList.xintanaEnemies[i].prefab);
            //_obj.GetComponent<EnemyController>().enabled = false;
            _obj.transform.position = leftPosition.position;
            _enemies.Add(_obj);
        }
    }
    #region Input
    public void checkInput()
    {
        checkSwipeTouch();
        checkSwipeClick();
    }
    void checkSwipeTouch()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();


                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    _swipe = Swipe.Left;
                    if(pokedexIndex >= _enemies.Count - 1)
                    {
                        pokedexIndex = 0;
                    }
                    else
                    {
                        pokedexIndex++;
                    }
                }

                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    _swipe = Swipe.Right;
                    if (pokedexIndex <= 0)
                    {
                        pokedexIndex = _enemies.Count-1;
                    }
                    else
                    {
                        pokedexIndex--;
                    }
                }
                _step = PokedexStep.CheckAnimation;
            }
        }
    }
    void checkSwipeClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                _swipe = Swipe.Left;
                if (pokedexIndex >= _enemies.Count - 1)
                {
                    pokedexIndex = 0;
                }
                else
                {
                    pokedexIndex++;
                }
            }

            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                _swipe = Swipe.Right;
                if (pokedexIndex <= 0)
                {
                    pokedexIndex = _enemies.Count - 1;
                }
                else
                {
                    pokedexIndex--;
                }
            }
            _step = PokedexStep.CheckAnimation;
        }


    }
    #endregion

    #region Animation

    private void MoveEnemies()
    {
        //we need to check 4 situations, swipe right and left, and then swipe right when you are in first position and
        //swipe left when you are in the last position
        if(pokedexIndex == 0 && _swipe == Swipe.Left)
        {
            _enemies[_enemies.Count-1].transform.DOMove(leftPosition.position, timeForMoving, false);
            _enemies[0].transform.DOMove(rightPosition.position, 0, false); 
            _enemies[0].transform.DOMove(centerScreenPosition.position, timeForMoving, false);
        }
        else if(pokedexIndex == _enemies.Count-1 && _swipe == Swipe.Right)
        {
            _enemies[_enemies.Count-1].transform.DOMove(leftPosition.position, 0, false);
            _enemies[_enemies.Count-1].transform.DOMove(centerScreenPosition.position, timeForMoving, false); 
            _enemies[0].transform.DOMove(rightPosition.position, timeForMoving, false);
        }
        else if(_swipe == Swipe.Right)
        {
            _enemies[pokedexIndex + 1].transform.DOMove(rightPosition.position, timeForMoving, false);
            _enemies[pokedexIndex].transform.DOMove(leftPosition.position, 0, false);
            _enemies[pokedexIndex].transform.DOMove(centerScreenPosition.position, timeForMoving, false);
        }
        else if(_swipe == Swipe.Left)
        {
            _enemies[pokedexIndex - 1].transform.DOMove(leftPosition.position, timeForMoving, false);
            _enemies[pokedexIndex].transform.DOMove(rightPosition.position, 0, false);
            _enemies[pokedexIndex].transform.DOMove(centerScreenPosition.position, timeForMoving, false);
        }

        _step = PokedexStep.ShowEnemy;
    }

    #endregion

    #region Stats

    private void ShowEnemyStats()
    {

        EnemyController _tempController;
        if (_tempController = _enemies[pokedexIndex].GetComponent<EnemyController>())
        {
            _tempController = _enemies[pokedexIndex].GetComponent<EnemyController>();
        }
        else
        {
            _tempController = _enemies[pokedexIndex].GetComponentInChildren<EnemyController>();
        }

        //check for this type in profile
        if (Rad_SaveManager.pokedex.enemiesKnown[_tempController.type])
        {
            //loop around scriptable object to get type stats
            for (int i = 0; i < bestiaryList.xintanaEnemies.Count; i++)
            {
                
                if(bestiaryList.xintanaEnemies[i].type == _tempController.type)
                {
                    enemyName.text = bestiaryList.xintanaEnemies[i].nameId;
                    enemyWorld.text = "World " + bestiaryList.xintanaEnemies[i].appearsInWorld;
                    break;
                }
            }
        }
        else
        {
            //still unknown enemy
            enemyName.text = "?????";
            enemyWorld.text = "World " + bestiaryList.xintanaEnemies[pokedexIndex].appearsInWorld;
            //get 2dfx component so we can change their color
            _2dxFX_HSV[] _sprites = _enemies[pokedexIndex].GetComponentsInChildren<_2dxFX_HSV>();
            if(_sprites != null)
            {
                for (int i = 0; i < _sprites.Length; i++)
                {
                    _sprites[i]._HueShift = 0;
                    _sprites[i]._Saturation = 0.87f;
                    _sprites[i]._ValueBrightness = 0;

                }
            }else
            {
                //no 2dfx like makula 
                SpriteRenderer[] _spritesR = _enemies[pokedexIndex].GetComponentsInChildren<SpriteRenderer>();
                for (int i = 0; i < _spritesR.Length; i++)
                {
                    _spritesR[i].color = new Color(0, 0, 0);
                }
            }

        }


        //change background with enemy type
        ChangeBackground();
        _step = PokedexStep.WaitingForInput;
    }

    private void ChangeBackground()
    {
        //select worldsprite 
        switch (bestiaryList.xintanaEnemies[pokedexIndex].appearsInWorld)
        {
            case 1:

                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 0)
                    {
                        worldSprites[i].SetActive(true);
                    }else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;

            case 2:
                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 1)
                    {
                        worldSprites[i].SetActive(true);
                    }
                    else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 2)
                    {
                        worldSprites[i].SetActive(true);
                    }
                    else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;
            case 4:
                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 3)
                    {
                        worldSprites[i].SetActive(true);
                    }
                    else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;
            case 5:
                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 4)
                    {
                        worldSprites[i].SetActive(true);
                    }
                    else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;
            case 6:
                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 5)
                    {
                        worldSprites[i].SetActive(true);
                    }
                    else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;
            case 7:
                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 6)
                    {
                        worldSprites[i].SetActive(true);
                    }
                    else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;
            case 8:
                for (int i = 0; i < worldSprites.Length; i++)
                {
                    if (i == 7)
                    {
                        worldSprites[i].SetActive(true);
                    }
                    else
                    {
                        worldSprites[i].SetActive(false);
                    }
                }
                break;

        }
    }
    #endregion

    public void BUTTON_ExitBestiary()
    {
        SceneManager.LoadScene("XintanaLegendsGo_Menu");
    }
}
