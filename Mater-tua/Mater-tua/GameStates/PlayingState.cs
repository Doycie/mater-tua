﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

internal class PlayingState : GameState
{
    private Vector2 _camSpeed = new Vector2(4.0f, 4.0f);
    private int _previousScrollValue;
    private MouseState _mouseState;
    private Level level;
    private FogOfWar fog;
    private List<BuildingAndUnit> _selectedEntities = new List<BuildingAndUnit>();
    private CustomCursor _customCursor;
    private HUD _hud;
    private Vector2 _lastMousePos;
    private Vector2 _currentMousePos;
    private bool _mouseReleased;
    private bool canBuild = true;
    private bool _mine;
    private bool _chop;

    private bool PlayedBattleCry2 = true;
    private bool PlayedBattleCry1 = false;
    private bool PlayedConfirmation1 = false;
    private bool PlayedConfirmation2 = true;

    //Construct a new state and set the level and all the needed variables
    public PlayingState()
    {
        _mouseState = Mouse.GetState();
        level = new Level();
        _hud = new PlayingHud(level, _selectedEntities, this);
        level.init("lvl2.txt", "lvlentities.txt");
        fog = new FogOfWar(level);
        level.setFog(fog);
        _customCursor = new CustomCursor(level);
    }

    //Update the level
    public void update(GameTime gameTime)
    {
        // GameEnvironment.getCamera().getScreenSize();
        // Console.WriteLine(mousePos);
        level.update(gameTime);
        fog.Update();

        if (level._tempBuilding != null)
        {
            if (level.Player.Gold < level._tempBuilding.GoldCost && level.Player.Wood < level._tempBuilding.LumberCost)
            {
                canBuild = false;
            }
        }
    }

    //Special function to draw the HUD

    public void drawHUD(SpriteBatch spriteBatch)
    {
        _hud.draw(spriteBatch);
        _customCursor.draw(spriteBatch);
    }

    //Draw the level then the cursor and the slected entities
    public void draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        level.draw(spriteBatch);
        fog.Draw(spriteBatch);
        if (_mouseReleased)
            DrawingHelper.DrawRectangle(new Rectangle((int)_lastMousePos.X, (int)_lastMousePos.Y, (int)(_currentMousePos.X - _lastMousePos.X), (int)(_currentMousePos.Y - _lastMousePos.Y)), spriteBatch, Color.Red);

        //spriteBatch.Draw(_selectTex, new Rectangle((int)_lastMousePos.X, (int)_lastMousePos.Y, (int)(_currentMousePos.X - _lastMousePos.X), (int)(_currentMousePos.Y - _lastMousePos.Y)), Color.White);
        if (_selectedEntities.Count > 0)
        {
            foreach (SpriteEntity e in _selectedEntities)
            {
                DrawingHelper.DrawRectangle(new Rectangle((int)e.Position.X, (int)e.Position.Y, e.Size * data.tSize(), e.Size * data.tSize()), spriteBatch, Color.Red);
                //spriteBatch.Draw(_selectTex, new Rectangle((int)e.Position.X, (int)e.Position.Y, 64, 64), Color.White);
            }
        }

        if (level._tempBuilding != null)
        {
            if (canBuild)
                level._tempBuilding.DrawGreen(spriteBatch);
            else
                level._tempBuilding.DrawRed(spriteBatch);
        }
        foreach (StaticBuilding e in level._tempBuildings)
        {
            spriteBatch.Draw(e.ConstructionSprite, new Rectangle((int)e.Position.X, (int)e.Position.Y, e.Size * data.tSize(), e.Size * data.tSize()), Color.White); ;
        }
    }

    //Handle the camera movement and the selecting units
    public void handleInput(InputHelper inputHelper)
    {
        _customCursor.updateCursorPosition(inputHelper);
        _mouseState = Mouse.GetState();
        _currentMousePos = _customCursor.getMousePos();

        (_hud as PlayingHud).update(inputHelper, _selectedEntities, level);

        if (!_hud.HUDSize().Contains(inputHelper.realMousePosition) && !(new Rectangle((int)GameEnvironment.getCamera().getScreenSize().X - 365, (int)GameEnvironment.getCamera().getScreenSize().Y - 256, 365, 256).Contains(inputHelper.realMousePosition)) && !(new Rectangle(0, (int)GameEnvironment.getCamera().getScreenSize().Y - 256, 256, 256).Contains(inputHelper.realMousePosition)))
        {
            //Make an order on the selected units
            if (inputHelper.MouseRightButtonPressed() && _selectedEntities.Count > 0)
            {
                bool PlayedBattleCry = false;
                bool PlayedConfirmation = false;
                foreach (Unit e in _selectedEntities.OfType<Unit>())
                {
                    if (e.Faction == BuildingAndUnit.faction.Human)
                    {
                        //if (PlayedConfirmation == false && PlayedConfirmation1 == false)
                        //{ GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/Yes"); PlayedConfirmation = true; PlayedConfirmation1 = true; PlayedConfirmation2 = false; }
                        //if (PlayedConfirmation == false && PlayedConfirmation2 == false)
                        //{ GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/Allright"); PlayedConfirmation = true; PlayedConfirmation2 = true; PlayedConfirmation1 = false; }
                        Point pos = new Point((int)_currentMousePos.X, (int)_currentMousePos.Y);
                        bool attack = false;
                        if (e is CombatUnit)
                        {
                            foreach (BuildingAndUnit g in level.entities.OfType<BuildingAndUnit>())
                            {
                                if (g.Faction == BuildingAndUnit.faction.Orc)
                                {
                                    if ((new Rectangle((int)g.Position.X, (int)g.Position.Y, g.Size * data.tSize(), g.Size * data.tSize()).Contains(pos)))
                                    {
                                        if (PlayedBattleCry == false && PlayedBattleCry1 == false)
                                        { GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/BattleCry"); PlayedBattleCry = true; PlayedBattleCry1 = true; PlayedBattleCry2 = false; }
                                        if (PlayedBattleCry == false && PlayedBattleCry2 == false)
                                        { GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/BattleCry2"); PlayedBattleCry = true; PlayedBattleCry2 = true; PlayedBattleCry1 = false; }
                                        Console.WriteLine("CHAARARRGGEEE   ");
                                        attack = true;
                                        (e as CombatUnit).orderAttack(g);
                                        //if(g is CombatUnit)
                                        //(g as CombatUnit).Defend(e);
                                        break;
                                    }
                                }
                            }
                        }
                        if (!attack)
                        {
                            e.removeTarget();

                            e.orderMove(new Point((int)_currentMousePos.X / data.tSize(), (int)_currentMousePos.Y / data.tSize()));
                        }
                    }
                }

                //make an order to a WorkerUnit
                foreach (WorkerUnit q in _selectedEntities.OfType<WorkerUnit>())
                {
                    if (q.Faction == BuildingAndUnit.faction.Human)
                    {
                        Point pos1 = new Point((int)_currentMousePos.X, (int)_currentMousePos.Y);
                        //if (_mine == true)
                        // {
                        foreach (Mine w in level.entities.OfType<Mine>())
                        {
                            foreach (Townhall r in level.entities.OfType<Townhall>())
                            {
                                if (r.Faction == BuildingAndUnit.faction.Human)
                                {
                                    if ((new Rectangle((int)w.Position.X, (int)w.Position.Y, w.Size * data.tSize(), w.Size * data.tSize()).Contains(pos1)))
                                    {
                                        q.OrderReset();
                                        q.MineOrder(w, new Vector2(w.Position.X, w.Position.Y + data.tSize()), r.Position + new Vector2(r.Size * data.tSize() - data.tSize(), r.Size * data.tSize() - data.tSize()));
                                        break;
                                    }
                                }
                            }
                        }
                        //}
                        //if (_chop == true)
                        //{
                        foreach (Tree n in level.entities.OfType<Tree>())
                        {
                            foreach (Townhall r in level.entities.OfType<Townhall>())
                            {
                                if (r.Faction == BuildingAndUnit.faction.Human)
                                {
                                    if ((new Rectangle((int)n.Position.X, (int)n.Position.Y, n.Size * data.tSize(), n.Size * data.tSize()).Contains(pos1)))
                                    {
                                        q.OrderReset();
                                        q.CutWoodOrder(n, n.Position, r.Position + new Vector2(r.Size * data.tSize() - data.tSize(), r.Size * data.tSize() - data.tSize()));
                                        break;
                                    }
                                }
                            }
                        }
                        //}
                        foreach (TreasureChest n in level.entities.OfType<TreasureChest>())
                        {
                            foreach (Townhall r in level.entities.OfType<Townhall>())
                            {
                                if (r.Faction == BuildingAndUnit.faction.Human)
                                {
                                    if ((new Rectangle((int)n.Position.X, (int)n.Position.Y, n.Size * data.tSize(), n.Size * data.tSize()).Contains(pos1)))
                                    {
                                        q.OrderReset();
                                        q.TreasureOrder(n, n.Position, r.Position + new Vector2(r.Size * data.tSize() - data.tSize(), r.Size * data.tSize() - data.tSize()));
                                        break;
                                    }
                                }
                            }
                        }
                        for(int i = level._tempBuildings.Count - 1; i >= 0; i--) 
                        {
                            if(new Rectangle((int)level._tempBuildings[i].Position.X,(int)level._tempBuildings[i].Position.Y, (int)level._tempBuildings[i].Size * data.tSize(), (int)level._tempBuildings[i].Size * data.tSize()).Contains(pos1))
                            {
                                q.OrderReset();
                                q.BuildOrder(level._tempBuildings[i].Position,i);
                            }
                        }
                    }
                }
                //_chop = false;
                //_mine = false;
            }

            //Order a stop on the selected entities
            if (inputHelper.KeyPressed(Keys.S))
            {
                if (_selectedEntities.Count > 0)
                {
                    foreach (Unit e in _selectedEntities.OfType<Unit>())
                    {
                        e.StopMove();
                    }
                    foreach (WorkerUnit q in level.entities.OfType<WorkerUnit>())
                    {
                        q.OrderReset();
                    }
                }
            }

            //Drag the selection box to include multiple entities
            if (!inputHelper.MouseLeftButtonDown() && !level.movingUnits && !level._attackMoveUnits)
            {
                if (_mouseReleased)
                {
                    Rectangle r = new Rectangle((int)_lastMousePos.X, (int)_lastMousePos.Y, (int)(_currentMousePos.X - _lastMousePos.X), (int)(_currentMousePos.Y - _lastMousePos.Y));
                    foreach (Unit e in level.entities.OfType<Unit>())
                        if (e.Faction == BuildingAndUnit.faction.Human)
                            if ((r.Contains(e.Center)) /*&& _selectedEntities[0] != e*/)
                            {
                                _selectedEntities.Add((e as Unit));
                            }
                }
                if (_selectedEntities.Count > 1)
                {
                    for (int i = 1; i < _selectedEntities.Count; i++)
                    {
                        if (_selectedEntities[0] == _selectedEntities[i])
                        {
                            _selectedEntities.Remove(_selectedEntities[i]);
                        }
                    }
                }
                _mouseReleased = false;
            }

            //Check if the mouse is pressed for the selection
            if (inputHelper.MouseLeftButtonDown() && !level.movingUnits && !level._attackMoveUnits)
            {
                if (_mouseReleased == false)
                {
                    _mouseReleased = true;
                    _lastMousePos = _customCursor.getMousePos();
                }
            }

            //One click on a unit to select/deselect them
            if (inputHelper.MouseLeftButtonPressed() && !level.movingUnits && !level._attackMoveUnits && level._tempBuilding == null)
            {
                Vector2 pos = _customCursor.getMousePos();

                bool clickedOnEntity = false;
                foreach (BuildingAndUnit e in level.entities)
                {
                    if ((new Rectangle((int)e.Position.X, (int)e.Position.Y, e.Size * data.tSize(), e.Size * data.tSize()).Contains(pos)) && e.Faction == BuildingAndUnit.faction.Human)
                    {
                        //if (PlayedHello == false && Hello1 == false )
                        //{ GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/Yes Sir"); PlayedHello = true; Hello1 = true; Hello2 = false; }
                        //if (PlayedHello == false && Hello2 == false )
                        //{ GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/what"); PlayedHello = true; Hello2 = true; Hello1 = false; }

                        clickedOnEntity = true;
                        if (inputHelper.IsKeyDown(Keys.LeftControl))
                        {
                            if (!_selectedEntities.Contains(e))
                            {
                                _selectedEntities.Add((e as BuildingAndUnit));
                            }
                        }
                        else
                        {
                            _selectedEntities.Clear();
                            _selectedEntities.Add((e as BuildingAndUnit));
                        }
                    }
                }
                if (!clickedOnEntity)
                {
                    _selectedEntities.Clear();
                }
            }
        }
        else if ((new Rectangle(0, (int)GameEnvironment.getCamera().getScreenSize().Y - 256, 256, 256).Contains(inputHelper.realMousePosition)))
        {
            //Clicked inside minimap

            if (inputHelper.MouseLeftButtonDown())
            {
                GameEnvironment.getCamera().setPos(new Vector2(inputHelper.realMousePosition.X * 16 - GameEnvironment.getCamera().getScreenSize().X / 2, ((inputHelper.realMousePosition.Y - GameEnvironment.getCamera().getScreenSize().Y) + 256) * 16 - GameEnvironment.getCamera().getScreenSize().Y / 2));
            }
        }

        if (level.movingUnits)
        {
            if (inputHelper.MouseLeftButtonPressed())
            {
                foreach (Unit e in _selectedEntities)
                {
                    e.orderMove(new Point((int)_currentMousePos.X / data.tSize(), (int)_currentMousePos.Y / data.tSize()));
                }
                level.movingUnits = false;
            }
        }

        if (level._attackMoveUnits)
        {
            if (inputHelper.MouseLeftButtonPressed())
            {
                foreach (CombatUnit e in _selectedEntities)
                {
                    e.orderMove(new Point((int)_currentMousePos.X / data.tSize(), (int)_currentMousePos.Y / data.tSize()));
                }
                level._attackMoveUnits = false;
            }
        }
        if (level._tempBuilding != null)
        {
            (level._tempBuilding as StaticBuilding).setPos(_currentMousePos);
            //if (inputHelper.MouseLeftButtonPressed())

            canBuild = true;
            for (int j = 0; j < level._tempBuilding.Size * level._tempBuilding.Size; j++)
            {
                int xxx = (int)(level._tempBuilding.Position.X / 64 + (int)(j / level._tempBuilding.Size));
                int yyy = (int)(level._tempBuilding.Position.Y / 64 + (int)(j % level._tempBuilding.Size));
                if (xxx >= 63) xxx = 63;
                if (xxx < 0) xxx = 0;
                if (yyy >= 63) yyy= 63;
                if (yyy < 0) yyy = 0;
                
                if (level._mapData[xxx,yyy] != 0)
                {
                    canBuild = false;
                    break;
                }
            }
            if (canBuild)
            {
                foreach (StaticBuilding e in level.entities.OfType<StaticBuilding>())
                {
                    for (int k = 0; k < e.Size * e.Size; k++)
                    {
                        for (int j = 0; j < level._tempBuilding.Size * level._tempBuilding.Size; j++)
                        {
                            if ((int)e.Position.X / 64 + (k / e.Size) == level._tempBuilding.Position.X / 64 + (j / level._tempBuilding.Size) && (int)e.Position.Y / 64 + (k % e.Size) == level._tempBuilding.Position.Y / 64 + (j % level._tempBuilding.Size))
                            {
                                canBuild = false;
                                break;
                            }
                        }
                    }
                }
                foreach (Tree e in level.entities.OfType<Tree>())
                {
                    for (int j = 0; j < level._tempBuilding.Size * level._tempBuilding.Size; j++)
                    {
                        if ((int)(e.Position.X / 64) == (int)level._tempBuilding.Position.X / 64 + (j / level._tempBuilding.Size) && (int)e.Position.Y / 64 == (int)level._tempBuilding.Position.Y / 64 + (j % level._tempBuilding.Size))
                        {
                            canBuild = false;
                            break;
                        }
                    }
                }
            }
            if (canBuild)
            {
                if (inputHelper.MouseLeftButtonPressed())
                {
                    
                        
                    
                    if (level.Player.Gold >= level._tempBuilding.GoldCost && level.Player.Wood >= level._tempBuilding.LumberCost)
                    {
                        level.Player.AddGold(-level._tempBuilding.GoldCost);
                        level.Player.AddWood(-level._tempBuilding.LumberCost);
                        level._tempBuildings.Add(level._tempBuilding as StaticBuilding);
                        foreach (WorkerUnit e in _selectedEntities.OfType<WorkerUnit>())
                        {
                            (e as WorkerUnit).BuildOrder(level._tempBuilding.Position, level._tempBuildings.Count - 1);
                        }
                        level._tempBuilding = null;
                    }

                }
                else if (inputHelper.MouseRightButtonPressed())
                {

                    level._tempBuilding = null;
                    canBuild = true;
                }
            }
            else if (!canBuild && inputHelper.MouseRightButtonPressed())
                level._tempBuilding = null;



        }

        int x = 0;
        int y = 0;

        if (inputHelper.IsKeyDown(Keys.Right))
        {
            x++;
        }
        if (inputHelper.IsKeyDown(Keys.Left))
        {
            x--;
        }
        if (inputHelper.IsKeyDown(Keys.Up))
        {
            y--;
        }
        if (inputHelper.IsKeyDown(Keys.Down))
        {
            y++;
        }

        if (inputHelper.KeyPressed(Keys.NumPad0))
            level.Player.AddGold(100);
        if (inputHelper.KeyPressed(Keys.NumPad1))
            level.Player.AddWood(100);

        // Mouse moves camera, +/- 20 for ease of use
        if (inputHelper.realMousePosition.Y <= 0 + 20)
        {
            y--;
        }
        if (inputHelper.realMousePosition.X <= 0 + 20 && inputHelper.realMousePosition.Y < GameEnvironment.getCamera().getScreenSize().Y - 256)
        {
            x--;
        }
        if (inputHelper.realMousePosition.Y >= GameEnvironment.getCamera().getScreenSize().Y - 20 && inputHelper.realMousePosition.X > 256)
        {
            y++;
        }
        if (inputHelper.realMousePosition.X >= GameEnvironment.getCamera().getScreenSize().X - 20)
        {
            x++;
        }
        Vector2 camspeedmultiplier = new Vector2(1.0f, 1.0f);
        if (inputHelper.IsKeyDown(Keys.LeftShift))
        {
            camspeedmultiplier = new Vector2(2.0f, 2.0f);
        }

        //Simple camera movement
        Vector2 mov = new Vector2(x, y);
        if (mov != Vector2.Zero)
        {
            mov.Normalize();
            mov *= _camSpeed;
            GameEnvironment.getCamera().move(Vector2.Normalize(mov) * _camSpeed * camspeedmultiplier);
        }

        //Zoomon scroll wheel disabled in the real game
        if (_mouseState.ScrollWheelValue < _previousScrollValue)
        {
            GameEnvironment.getCamera().zoom(-.04f);
        }
        else if (_mouseState.ScrollWheelValue > _previousScrollValue)
        {
            GameEnvironment.getCamera().zoom(0.04f);
        }
        _previousScrollValue = _mouseState.ScrollWheelValue;

        if (inputHelper.KeyPressed(Keys.Back))
        {
            _selectedEntities.Clear();
            GameEnvironment.gameStateManager.pauseState._playingHud = this._hud;
            GameEnvironment.gameStateManager.State = GameStateManager.state.Pause;
        }

        if (inputHelper.KeyPressed(Keys.Escape))
        {
            _selectedEntities.Clear();
            GameEnvironment.gameStateManager.pauseState._playingHud = this._hud;
            GameEnvironment.gameStateManager.State = GameStateManager.state.Pause;
        }

        if (inputHelper.KeyPressed(Keys.F1))
        {
            GameEnvironment.gameStateManager.State = GameStateManager.state.Menu;
        }
    }

    public bool Mine
    {
        get { return _mine; }
        set { _mine = value; }
    }

    public bool Chop
    {
        get { return _chop; }
        set { _chop = value; }
    }
}