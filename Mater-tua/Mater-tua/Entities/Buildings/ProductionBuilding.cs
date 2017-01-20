﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class ProductionBuilding : StaticBuilding
{
    public bool _producingUnit;
    private Unit _tempUnit;
    public int _unitProductionTime;
    public int _unitProductionTimer;

    public ProductionBuilding(Level level) : base(level)
    {
    }

    public void produceUnit(Unit e)
    {
        
        if (_level.Player.Gold > e.GoldCost && _level.Player.AvailableFood > 1 && _producingUnit == false)
        {
            _tempUnit = e;
            _level.Player.AddGold(-e.GoldCost);
            _level.Player.availableFood(1);

            _producingUnit = true;
            _unitProductionTime = e.ProductionTime;
            _unitProductionTimer = 0;
        }


    }

 

    public override void Update()
    {
        base.Update();

        if (_producingUnit)
        {
            _unitProductionTimer++;
            if(_unitProductionTimer > _unitProductionTime)
            {
                _level.entities.Add(_tempUnit);
                _producingUnit = false;
            }
        }



    }
}  