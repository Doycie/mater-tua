﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    private int _Wood;
    private int _Gold;
    private int _Food;
    private int _usedFood;
    static private int _AmountFarms;
    static private int _TempFarms;
    

    private Level _level;

    public Player(Level level)
    {
        _level = level;

        foreach (Farm e in _level.entities.OfType<Farm>())
        {
            if (e.Faction == BuildingAndUnit.faction.Human)
            {
                _AmountFarms += 1;
            }
        }

        _Gold = 400;
        _usedFood = 11;
        _Wood = 400;
    }

    public void Update()
    {
        FoodUpdate();
        _Food = (_AmountFarms * 6 + 10);
    }

     public int Wood
    {
        get { return _Wood; }
    }

     public int Gold
    {
        get { return _Gold; }
    }

     public int Food
    {
        get { return _Food; }
    }

    public int UsedFood
    {
        get { return _usedFood; }
    }

    public void UseFood(int Amount)
    {
        _usedFood = _usedFood + Amount;
    }

    public void AddWood(int Amount)
    {
        _Wood = _Wood + Amount;
    }

    public void AddGold(int Amount)
    {
        _Gold = _Gold + Amount;
    }

    public void AddFood(int Amount)
    {
        _Food = _Food + Amount;
    }

    public void SubtractWood(int Amount)
    {
        _Wood = _Wood - Amount;
    }

    public void SubtractGold(int Amount)
    {
        _Gold = _Gold - Amount;
    }

    public void SubtractFood(int Amount)
    {
        _Food = _Food - Amount;
    }

    private void FoodUpdate()
    {
        foreach (Farm e in _level.entities.OfType<Farm>())
        {
            if (e.Faction == BuildingAndUnit.faction.Human)
            {
                _TempFarms += 1;
            }
        }

        if (_TempFarms < _AmountFarms)
        {
            while (_TempFarms < _AmountFarms)
            {
                _AmountFarms -= 1;
            }

        }
        else if (_TempFarms > _AmountFarms)
        {
            while (_TempFarms > _AmountFarms)
            {
                _AmountFarms += 1;
            }
        }
        _TempFarms = 0;
    }
}