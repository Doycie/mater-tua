﻿using System.Linq;
using Microsoft.Xna.Framework;
using System;

partial class Level
{
    int count = 0;
    private int _buildingID;

    public void update(GameTime gameTime)
    {
        count++;

        if (count % 60 == 0)
        {
            foreach (CombatUnit e in entities.OfType<CombatUnit>())
            {
                foreach (BuildingAndUnit g in entities)
                {
                    if (g.Faction != e.Faction && g.Faction != BuildingAndUnit.faction.Neutral)
                    {
                        e.Defend(g);
                    }
                }
            }
        }
        //  foreach (Unit e in entities.OfType<Unit>())
        //  {
        //      if (e.Faction == Unit.faction.Human)
        //      {
        //         foreach (CombatUnit g in entities.OfType<CombatUnit>())
        //          {
        //              if (g.Faction == CombatUnit.faction.Orc)
        //              {
        //                  g.Defend(e);
        //              }
        //          }
        //      }

        //}

        if (count % 5400 == 0 && count > 0)
        {
            foreach (Barracks b in entities.OfType<Barracks>())
            {
                if (b.Faction == BuildingAndUnit.faction.Orc)
                {
                    b.produceUnit(new Grunt(this, new Vector2(b.Position.X + 2 * data.tSize(), b.Position.Y + 1 * data.tSize())));
                }
            }
        }
        if (count > 0 && count % 8100 == 0)
        {
            Vector2 attackLocation = Vector2.Zero;
            foreach (Townhall t in entities.OfType<Townhall>())
            {
                if (t.Faction == BuildingAndUnit.faction.Human)
                {
                    attackLocation = t.Position - new Vector2(data.tSize(), -3 * data.tSize());
                }
            }
            foreach (CombatUnit c in entities.OfType<CombatUnit>())
            {
                if (c.Faction == BuildingAndUnit.faction.Orc)
                {
                    c.orderMove(new Point((int)attackLocation.X / data.tSize(), (int)attackLocation.Y / data.tSize()));
                }
            }
        }


        //Update all the entities in the level list
        for (int i = entities.Count() - 1; i >= 0; i--)
        {
            if (typeof(BuildingAndUnit).IsAssignableFrom(entities[i].GetType()))
            {
                entities[i].Update(gameTime);
                //if(count%60==0 && entities[i] is Barracks && entities[i].Faction == BuildingAndUnit.faction.Orc)
                //{
                //    (entities[i] as Barracks).produceUnit(new Grunt(this, new Vector2(entities[i].Position.X + 2* data.tSize(), entities[i].Position.Y + 1 * data.tSize())));
                //}
                if (typeof(Unit).IsAssignableFrom(entities[i].GetType()) && entities[i].HitPoints < 1)
                {
                    if (entities[i].Faction == BuildingAndUnit.faction.Human)
                        Player.UseFood(-(entities[i] as Unit).FoodCost);
                    GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/DieSound");
                }
                if (typeof(StaticBuilding).IsAssignableFrom(entities[i].GetType()) && entities[i].HitPoints < 1)
                {
                    GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/BuildingDemolitionSound");
                    if (entities[i].Faction == BuildingAndUnit.faction.Human && typeof(Townhall).IsAssignableFrom(entities[i].GetType()))
                    {
                        GameEnvironment.gameStateManager.State = GameStateManager.state.Defeat;
                    }
                    else if (entities[i].Faction == BuildingAndUnit.faction.Orc && typeof(Townhall).IsAssignableFrom(entities[i].GetType()))
                    {
                        GameEnvironment.gameStateManager.State = GameStateManager.state.Victory;
                    }
                }
                if ((entities[i] as BuildingAndUnit).HitPoints < 1)
                {
                    if (typeof(Tree).IsAssignableFrom(entities[i].GetType()))
                    {
                        specialFX.Add(new Spritesheet("Sprites/Misc/sparkle", entities[i].Position, entities[i].Size , 8,32,32,60));
                        entities.RemoveAt(i);
                    }
                    else if(typeof(TreasureChest).IsAssignableFrom(entities[i].GetType()))
                    {
                        GameEnvironment.getAssetManager().PlaySoundEffect("Sounds/Soundeffects/OpenChest");
                        specialFX.Add(new Spritesheet("Sprites/Misc/sparkle", entities[i].Position, entities[i].Size, 8, 32, 32, 60));
                        entities.RemoveAt(i);
                    }
                    else if (typeof(Unit).IsAssignableFrom(entities[i].GetType()))
                    {
                        specialFX.Add(new Spritesheet("Sprites/Misc/BloodSplatter", entities[i].Position, entities[i].Size, 4, 128, 16, 180));
                        entities.RemoveAt(i);
                    }
                    else if (typeof(StaticBuilding).IsAssignableFrom(entities[i].GetType()))
                    {
                        specialFX.Add(new Spritesheet("Sprites/Misc/explosionSpriteSheet", entities[i].Position, entities[i].Size, 5, 96, 15, 180));
                        entities.RemoveAt(i);
                    }

                }
            }
        }

        for (int i = specialFX.Count() - 1; i >= 0; i--)
        {
            specialFX[i].Update(gameTime);
            if (specialFX[i] is Spritesheet)
            {
                if ((specialFX[i] as Spritesheet).remove())
                {
                    {
                        specialFX.RemoveAt(i);
                    }
                }
            }
        }
        for (int i = Projectiles.Count() - 1; i >= 0; i--)
        {
            Projectiles[i].Update(gameTime);
        }

        Player.Update();
    }

    public void dragBuilding(int buildingID)
    {
        _buildingID = buildingID;
        if (_buildingID == 1)
        {
            _tempBuilding = new Farm(this, Vector2.Zero, BuildingAndUnit.faction.Human);
        }
        else if (_buildingID == 2)
        {
            _tempBuilding = new Barracks(this, Vector2.Zero, BuildingAndUnit.faction.Human);
        }

        
        else
        {
            _tempBuilding = null;
        }


    }


    public void moveUnits()
    {
        movingUnits = true;

    }

    public void attackMoveUnits()
    {
        _attackMoveUnits = true;
    }
}